using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.Extensions;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
using TheInfiniteCrusade.NewComponents.UnitParts.ManeuverBookSystem;
using TheInfiniteCrusade.Serialization;

namespace TheInfiniteCrusade.Backend.NewUnitDataClasses
{
    public class ManeuverBook : IDisposable
    {
        public ManeuverBook(UnitDescriptor owner, BlueprintManeuverBook blueprint)
        {
            Owner = owner;
            Blueprint = blueprint;
        }

        public BlueprintProgressionReference GrantingProgression => Blueprint.GrantingProgression;

        public BlueprintManeuverBook.ManeuverBookType BookType => Blueprint.BookType;

        public UnitDescriptor Owner { get; private set; }


        public readonly BlueprintManeuverBook Blueprint;

        private List<BlueprintAbilityReference> knownManeuvers = new();

        public int KnownManeuversCount => knownManeuvers.Count;

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

        private List<BlueprintAbilityReference> knownStances = new();

        public readonly UnitFact source;

        



        public List<AbilityData> AllReadiedManeuvers(SlotLayer layer)
        {
            var abilities = ManeuverSlots.Select(x => x.Get(layer).Get());
            var dataList = new List<AbilityData>();

            foreach (var known in abilities)
            {
                var comp = known.GetComponent<ManeuverInformation>();
                if (comp == null)
                    continue;
                var data = new AbilityData(known, source.Owner);



                data.OverrideSpellLevel = comp.ManeuverLevel;
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

        public List<ManeuverSlot> ManeuverSlots = new();


        private StatType overrideMainStat;

        public StatType InitiatingStat => overrideMainStat.IsAttribute() ? overrideMainStat : Blueprint.DefaultMainStat;

        public bool IsGranted => Blueprint.IsGranted;

        public BlueprintUnitPropertyReference ManeuverSlotsPropertyReference => Blueprint.m_ManeuverSlotsReference;
        public BlueprintUnitPropertyReference InitiatorLevelReference => Blueprint.m_InitiatorLevelReference;
        
        public string Name { get; internal set; }

        
        internal void DemandSlotsUpdate()
        {

            int correctSlots = ManeuverSlotsPropertyReference.Get().GetInt(source.Owner);
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
                    knownManeuvers.Add(maneuver);
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

        internal int GetRawInitiatorLevel()
        {
            return InitiatorLevelReference.Get().GetInt(source.Owner);
        }

        public int GetRealInitiatorLevel()
        {
            int raw = GetRawInitiatorLevel();

            //TODO ADD EVENT!

            return raw;
        }

        public void Dispose()
        {
           
        }

        #region serialization

        public void SaveBook()
        {
            var record = ManeuverBookStorage.Instance.ForCharacter(Owner).ForSpellbook(this.Blueprint);
            record.ManeuverGuids.Clear();
            record.SlotRecords.Clear();
            foreach (var move in knownManeuvers)
            {
                record.ManeuverGuids.Add(move.guid);
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
            Main.Context.Logger.Log($"Loading {Name} Book Info - stage : in Unit Part");
            var record = ManeuverBookStorage.Instance.ForCharacter(Owner).ForSpellbook(this.Blueprint);
            ManeuverSlots.Clear();
            knownManeuvers.Clear();
            knownStances.Clear();
            foreach (var move in record.ManeuverGuids)
            {
                LearnManeuver(BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(move));
            }
            foreach (var slot in record.SlotRecords)
            {
                ManeuverSlots.Add(new ManeuverSlot(slot));
            }
            DemandSlotsUpdate();
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
