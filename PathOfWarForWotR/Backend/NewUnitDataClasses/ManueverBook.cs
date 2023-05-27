using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewComponents;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Backend.NewUnitParts;
using PathOfWarForWotR.Extensions;
using PathOfWarForWotR.Serialization;
using BlueprintCore.Utils;

namespace PathOfWarForWotR.Backend.NewUnitDataClasses
{
    public class ManeuverBook : IDisposable
    {
        public ManeuverBook(UnitDescriptor owner, BlueprintManeuverBook blueprint)
        {
            Owner = owner;
            Blueprint = blueprint;
            
            
        }
        #region properties
        public BlueprintManeuverBook.ManeuverBookType BookType => Blueprint.BookType;

        public UnitDescriptor Owner { get; private set; }


        public readonly BlueprintManeuverBook Blueprint;

        private bool HalfLevel = false;

        private Dictionary<BlueprintAbilityReference, Ability> Maneuvers = new();

        public int BaseLevel
        {
            get
            {
                

                return Math.Max(0, this.RawBaseLevel);
            }
        }
        

        public int RawBaseLevel
        {
            get
            {
               

                if (this.BookType == BlueprintManeuverBook.ManeuverBookType.MartialTraining && InitiatingStat.IsAttribute())
                {
                    return Math.Min(Owner.Progression.CharacterLevel, m_BaseLevelInternal + Owner.Stats.GetStat<ModifiableValueAttributeStat>(InitiatingStat).PermanentBonus);
                }
                return this.m_BaseLevelInternal;
            }
        }

        private int m_BaseLevelInternal;

        public int InitiatorLevel
        {
            get
            {
                //TODO ADD HAX
                return BaseLevel;
            }
        }

        public int EffectiveInitiatorLevel
        {
            get
            {
                int result;
                
                    int casterLevel = this.InitiatorLevel;
                result = casterLevel;
                   //TODO add cals
                return result;
            }
        }

        public void AddLevelFromClass(ClassData characterClass)
        {


            if (characterClass.CharacterClass.IsMythic)
            {
               
                return;
            }
            else if (BookType == BlueprintManeuverBook.ManeuverBookType.MartialTraining)
            {
                if (HalfLevel)
                {
                    AddBaseLevel();
                    HalfLevel = false;
                }
                else
                {
                    HalfLevel = true;
                }
            }
            else
            {
                if (characterClass.CharacterClass.PrestigeClass && characterClass.CharacterClass.HasComponent<MartialPrestigeClass>())
                { 
                    AddBaseLevel();
                    return;
                }
                else if (Blueprint.ClassReference.Any(x=>x.Equals(characterClass.CharacterClass.ToReference<BlueprintCharacterClassReference>())))
                {
                    if (!Blueprint.ArchetypeReference.Any())
                    {
                        AddBaseLevel();
                        return;
                    }
                    else if (Blueprint.ArchetypeReference.Any(x=>characterClass.Archetypes.Select(y=>y.ToReference<BlueprintArchetypeReference>()).Contains(x)))
                    {
                        AddBaseLevel();
                        return;
                    }
                }
                else
                {
                    if (HalfLevel)
                    {
                        AddBaseLevel();
                        HalfLevel = false;
                    }
                    else
                    {
                        HalfLevel = true;
                    }
                }
            }
        }

        public void AddBaseLevel()
        {
            
            
            this.m_BaseLevelInternal++;
            
        }


        private List<BlueprintAbilityReference> knownManeuvers = new();

        public int KnownManeuversCount => knownManeuvers.Count;

        

        private List<BlueprintAbilityReference> knownStances = new();

        

        public List<ManeuverSlot> ManeuverSlots = new();


        private StatType overrideMainStat;
        public bool HasStatOverride => overrideMainStat != StatType.Unknown;


        public StatType InitiatingStat => overrideMainStat.IsAttribute() ? overrideMainStat : Blueprint.DefaultMainStat;

        public bool IsGranted => Blueprint.IsGranted;

        public BlueprintUnitPropertyReference ManeuverSlotsPropertyReference => Blueprint.m_ManeuverSlotsReference;


        public string Name => Blueprint.DisplayName ?? Blueprint.name;


        #endregion




        internal void OnCombatStartWhileCooledDown()
        {
            if (!IsGranted)
            {
                foreach (var slot in ManeuverSlots)
                {
                    slot.ClearCombatHotswap();
                    slot.Recover();
                }
            }
        }

        

        internal void RechargeBookOnCombatEnd()
        {
            if (!IsGranted)
            {
                foreach (var slot in ManeuverSlots)
                {
                    slot.ClearCombatHotswap();
                    slot.Recover();
                    
                }
            }
        }


        



        public List<AbilityData> AllReadiedManeuvers(SlotLayer layer)
        {
            
            var abilities = ManeuverSlots.Select(x => x.Get(layer).Get());
            Main.Context.Logger.Log($"AllReadiedManuevers called on {Owner.CharacterName} for {Name} in layer: {layer}, count is {abilities.Count()}, Maneuver slot count is {ManeuverSlots.Count}");
            var dataList = new List<AbilityData>();

            foreach (var known in abilities)
            {
                Main.Context.Logger.Log($"Creating abilityData for {known?.name ?? "!NULL!"}");
                var comp = known.GetComponent<ManeuverInformation>();
                if (comp == null)
                    continue;
                var data = new AbilityData(known, Owner, Maneuvers[known.ToReference<BlueprintAbilityReference>()],null);



                data.OverrideSpellLevel = comp.ManeuverLevel;
                data.OverrideCasterLevel = EffectiveInitiatorLevel;
                
                dataList.Add(data);
            }


            return dataList;

        }

        internal IEnumerable<AbilityData> GetKnownManeuverDatas()
        {
            var list = new List<AbilityData>();
            foreach(var move in knownManeuvers)
            {
                list.Add(new AbilityData(move.Get(), Owner));
            }

            return list;
        }
        public IEnumerable<BlueprintAbility> GetKnownManeuvers()
        {
            return knownManeuvers.Select(x => x.Get());
        }

        internal IEnumerable<AbilityData> GetKnownStanceDatas()
        {
            var list = new List<AbilityData>();
            foreach (var move in knownStances)
            {
                list.Add(new AbilityData(move.Get(), Owner));
            }

            return list;
        }
        public IEnumerable<BlueprintAbility> GetKnownStances()
        {
            return knownStances.Select(x => x.Get());
        }

        public IEnumerable<BlueprintAbility> GetKnownMartialTechniques()
        {
            var result = knownStances.Select(x => x.Get()).ToList();
            result.AddRange(knownManeuvers.Select(x => x.Get()));
            return result;
        }


        public void PlannedToReadied()
        {
            if (Owner.Ensure<UnitPartMartialDisciple>().PlanIsValid())
            {
                foreach(var slot in ManeuverSlots)
                {
                    slot.PlannedToReadied();
                }
            }

        }

        

        internal void Rest()
        {
            PlannedToReadied();

            RechargeBookOnCombatEnd();

             
        }


        
        internal void DemandSlotsUpdate()
        {
            Main.Context.Logger.Log("Starting Demand Slots Update");
            Main.Context.Logger.Log($"Blueprint is {(Blueprint is not null ? "not":"")} null On ManueverBook");
            
         
            Main.Context.Logger.Log($"ManeuverSlotsPropertyReference is {(ManeuverSlotsPropertyReference is not null ? "not" : "")} null On ManueverBook");
            Main.Context.Logger.Log($"ManeuverSlotsPropertyReference.Get() is {(ManeuverSlotsPropertyReference.Get() is not null ? "not" : "")} null On ManueverBook");
            

            int correctSlots = ManeuverSlotsPropertyReference.Get().GetInt(Owner);
            Main.Context.Logger.Log($"Called demandSlotsUpdate on {Owner.CharacterName} for {Name}, value is {correctSlots}");
            if (correctSlots == ManeuverSlots.Count)
            {
                Main.Context.Logger.Log($"Slots Update Called On {Name}, slots are correct: {ManeuverSlots.Count}");
            }
            else if (correctSlots > ManeuverSlots.Count)
            {
                Main.Context.Logger.Log($"Slots Update Called On {Name}, slots are incorrect: {ManeuverSlots.Count}, should be {correctSlots}");
                while (correctSlots > ManeuverSlots.Count)
                {
                    Main.Context.Logger.Log($"Added Maneuver Slot!");
                    if (ManeuverSlots.Count == 0)
                    {
                        ManeuverSlots.Add(new ManeuverSlot(0, SlotType.Common));
                    }
                    else
                    {
                        ManeuverSlots.Add(new ManeuverSlot(ManeuverSlots.Max(x => x.Index) + 1, SlotType.Common));
                    }
                }
            }
            else
            {
                Main.Context.Logger.Log($"Slots Update Called On {Name}, slots are incorrect: {ManeuverSlots.Count}, should be {correctSlots}");
                while (correctSlots < ManeuverSlots.Count)
                {
                    ManeuverSlots.RemoveLast();
                }
            }

        }

        public bool CanSpend(AbilityData data)
        {
            if (data.Blueprint.GetComponent<ManeuverInformation>() != null)
            {
                var comp = data.Blueprint.GetComponent<ManeuverInformation>();
                if (comp.ManeuverType == ManeuverType.Stance)
                    return true;

                if (ManeuverSlots.Any(x=>x.Combat.Equals(data.Blueprint.ToReference<BlueprintAbilityReference>()) && x.Available))
                {
                    return true;
                }


                return false;

            }
            else
                return false;


        }

       

        public void SetStatOverride(StatType stat)
        {
            overrideMainStat = stat;
        }

        internal bool Knows(BlueprintAbilityReference maneuver)
        {
            return knownManeuvers.Contains(maneuver);
        }

        public bool Spend(AbilityData data)
        {
            if (data.Blueprint.GetComponent<ManeuverInformation>() != null)
            {
                var comp = data.Blueprint.GetComponent<ManeuverInformation>();
                if (comp.ManeuverType == ManeuverType.Stance)
                    return true;

                if (ManeuverSlots.Any(x => x.Combat.Equals(data.Blueprint.ToReference<BlueprintAbilityReference>()) && x.Available))
                {
                    ManeuverSlots.FirstOrDefault(x => x.Combat.Equals(data.Blueprint.ToReference<BlueprintAbilityReference>()) && x.Available).Expend();

                    return true;
                }


                return false;

            }
            else
                return false;

        }

        public void LearnManeuver(BlueprintAbilityReference maneuver)
        {
            
           
            var comp = maneuver.Get().GetComponent<ManeuverInformation>();

            if (comp != null)
            {
                Main.Context.Logger.Log($"{Owner.CharacterName} Learned {maneuver.NameSafe()} {(Owner.Unit.PreviewOf.IsEmpty ? "" : "in preview")}");
                if (comp.ManeuverType == ManeuverType.Stance && !knownStances.Contains(maneuver))
                {
                    knownStances.Add(maneuver);
                }
                else if (comp.ManeuverType != ManeuverType.Stance && !knownManeuvers.Contains(maneuver))
                {
                    var ability = new Ability(maneuver, Owner);//THIS IS NOT THE WAY TO DO IT - need active fact to hook it to as sourc
            
                    
                    knownManeuvers.Add(maneuver);
                    if (!Maneuvers.ContainsKey(maneuver))
                    {
                        Maneuvers.Add(maneuver, ability);
                    }
                   
                    
                }
            }
        }

        internal bool ManeuverIsReadied(BlueprintAbility blueprint)
        {
            return ManeuverSlots.Any(x => x.Readied != null && x.Readied.Equals(blueprint.ToReference<BlueprintAbilityReference>()));
        }

        internal bool ManueverIsAvailable(BlueprintAbility blueprint)
        {
            return ManeuverSlots.Any(x => x.Combat != null && x.Combat.Equals(blueprint.ToReference<BlueprintAbilityReference>()) && x.Available);
        }

        internal bool ExpendManeuver(BlueprintAbility blueprint)
        {
            var slot = ManeuverSlots.FirstOrDefault(x => x.Combat != null && x.Combat.Equals(blueprint.ToReference<BlueprintAbilityReference>()) && x.Available);
            if (slot != null)
            {
                return slot.Expend();
            }
            else
                return false;


        }

        

        

        public void Dispose()
        {
           
        }

#region serialization

        public void SaveBook()
        {
            Main.Context.Logger.Log($"Saving {Name} Book Info For {Owner.CharacterName}");
            var record = ManeuverBookStorage.Instance.ForCharacter(Owner).ForManeuverBook(this.Blueprint);
            record.HalfLevel = HalfLevel;
            record.ManeuverGuids.Clear();
            record.SlotRecords.Clear();
            record.BaseLevel = m_BaseLevelInternal;
            foreach (var move in knownManeuvers)
            {
                var guid = move.Guid.ToString();
                Main.Context.Logger.Log($"Saving {move.NameSafe()} with guid {guid} - {Name} Book Info For {Owner.CharacterName}");
                record.ManeuverGuids.Add(guid);
            }
            foreach (var move in knownStances)
            {
                record.ManeuverGuids.Add(move.guid);
            }
            foreach (var slot in ManeuverSlots)
            {

                record.SlotRecords.Add(new SlotRecord(slot));

            }

        }

        public void LoadBook()
        {
            Main.Context.Logger.Log($"Loading {Name} Book Info For {Owner.CharacterName}");
            var record = ManeuverBookStorage.Instance.ForCharacter(Owner).ForManeuverBook(this.Blueprint);
           
            ManeuverSlots.Clear();

            knownManeuvers.Clear();
            knownStances.Clear();
            HalfLevel = record.HalfLevel;
            m_BaseLevelInternal = record.BaseLevel;
            foreach (var move in record.ManeuverGuids)
            {
                Main.Context.Logger.Log($"Processing GUID {move}");
                var moveref = BlueprintTool.GetRef<BlueprintAbilityReference>(move);
                Main.Context.Logger.Log($"Loading {moveref.Get().name} - {Name} Book Info For {Owner.CharacterName}");
                LearnManeuver(moveref);
            }
            foreach (var slot in record.SlotRecords)
            {
                ManeuverSlots.Add(new ManeuverSlot(slot));
            }
            DemandSlotsUpdate();
            Main.LogDebug($"At end of LoadBook for {Name}, known maneuver count in list is {knownManeuvers.Count()}");
        }
#endregion

        internal bool CanRecover(BlueprintAbilityReference blueprintAbilityReference)
        {
            return ManeuverSlots.Any(x => x.Combat.Equals(blueprintAbilityReference) && x.State == SlotManeuverState.Exhausted);
        }

        internal void RecoverManeuver(BlueprintAbilityReference blueprintAbilityReference)
        {
            ManeuverSlots.FirstOrDefault(x => x.Combat.Equals(blueprintAbilityReference) && x.State == SlotManeuverState.Exhausted)?.Recover();
        }

        internal void OnPostCombatCooldown()
        {
            RechargeBookOnCombatEnd();
        }
    }

}
