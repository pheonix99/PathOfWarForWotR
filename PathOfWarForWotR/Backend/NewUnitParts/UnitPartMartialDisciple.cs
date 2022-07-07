using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Backend.NewEvents;
using TheInfiniteCrusade.Extensions;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.NewComponents.ManeuverProperties;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
using TheInfiniteCrusade.NewComponents.UnitParts;
using TheInfiniteCrusade.NewComponents.UnitParts.ManeuverBookSystem;
using TheInfiniteCrusade.Serialization;

namespace TheInfiniteCrusade.Backend.NewUnitParts
{
    class UnitPartMartialDisciple : OldStyleUnitPart, IUnitSubscriber, IUnitCompleteLevelUpHandler, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {

        #region Handle Maneuver Books

        private List<ManeuverBook> ManeuverBooks = new();

        internal void RechargeBookOnCombatStart(BlueprintSpellbook spellbook)
        {
            var book = GetManeuverBook(spellbook);
            if (book == null)
                return;
            if (!book.GrantedType)
            {
                foreach(var slot in book.ManeuverSlots)
                {
                    slot.ClearCombatHotswap();
                    slot.Recover();
                }
            }
        }

        internal void RechargeBookOnCombatEnd(BlueprintSpellbook spellbook)
        {
            var book = GetManeuverBook(spellbook);
            if (book == null)
                return;
            if (!book.GrantedType)
            {
                foreach (var slot in book.ManeuverSlots)
                {
                    slot.ClearCombatHotswap();
                    slot.Recover();
                }
            }
        }

        internal void Rest()
        {
            foreach(Spellbook o in Owner.Spellbooks)
            {
                RechargeBookOnCombatEnd(o.Blueprint);
            }

            
        }

        public class ManeuverBook
        {







            public readonly UnitFact source;


            public List<ManeuverSlot> ManeuverSlots = new();

            private BlueprintSpellbookReference _blueprintSpellbookReference;
            public BlueprintSpellbookReference BlueprintSpellbookReference { get => _blueprintSpellbookReference; }
            public bool GrantedType => ManeuverBookData.IsGranted;

            public BlueprintUnitPropertyReference ManeuverSlotsPropertyReference => ManeuverBookData.m_ManeuverSlotsReference;
            public ManeuverBookComponent ManeuverBookData => BlueprintSpellbookReference.Get().Components.OfType<ManeuverBookComponent>().First();
            public Spellbook Spellbook => source.Owner.DemandSpellbook(BlueprintSpellbookReference);

            public BlueprintSpellbook BlueprintSpellbook => BlueprintSpellbookReference.Get();

            public ManeuverBook(UnitFact fact, BlueprintSpellbookReference blueprintSpellbookReference)
            {
                source = fact;
                _blueprintSpellbookReference = blueprintSpellbookReference;
                Main.Context.Logger.Log($"Build Maneuver Book {blueprintSpellbookReference.NameSafe()} on  {fact.Owner.CharacterName}");

            }

            internal void DemandSlotsUpdate()
            {
                
                int correctSlots = ManeuverSlotsPropertyReference.Get().GetInt(source.Owner);
                if (correctSlots == ManeuverSlots.Count)
                {
                    Main.Context.Logger.Log($"Slots Update Called On {Spellbook.Blueprint.Name}, slots are correct: {ManeuverSlots.Count}");
                }
                else if (correctSlots > ManeuverSlots.Count)
                {
                    Main.Context.Logger.Log($"Slots Update Called On {Spellbook.Blueprint.Name}, slots are incorrect: {ManeuverSlots.Count}, should be {correctSlots}");
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
                    Main.Context.Logger.Log($"Slots Update Called On {Spellbook.Blueprint.Name}, slots are incorrect: {ManeuverSlots.Count}, should be {correctSlots}");
                    while (correctSlots < ManeuverSlots.Count)
                    {
                        ManeuverSlots.RemoveLast();
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
        }

        private void LoadBook(ManeuverBook book)
        {
            Main.Context.Logger.Log($"Loading {book.BlueprintSpellbook.Name} Book Info - stage : in Unit Part");
            var record = ManeuverBookStorage.Instance.ForCharacter(Owner).ForSpellbook(book.BlueprintSpellbook);
            book.ManeuverSlots.Clear();
            foreach (var slot in record)
            {
                book.ManeuverSlots.Add(new ManeuverSlot(slot));
            }
            book.DemandSlotsUpdate();
        }

        public override void OnPostLoad()
        {
            Main.Context.Logger.Log($"Loading Maneuver  Info: from Unit Part");
            foreach (var book in ManeuverBooks)
            {
                LoadBook(book);
            }
        }

        public override void OnPreSave()
        {
            Main.Context.Logger.Log($"Saving Maneuver  Info: from Unit Part");

            foreach (var book in ManeuverBooks)
            {
                Main.Context.Logger.Log($"Saving {book.BlueprintSpellbook.Name} Book Info - stage : in Unit Part");
                var record = ManeuverBookStorage.Instance.ForCharacter(Owner).ForSpellbook(book.BlueprintSpellbook);
                record.Clear();
                foreach (var slot in book.ManeuverSlots)
                {
                    record.Add(new SlotRecord(slot));
                }
            }
            
        }

        public ManeuverBook GetManeuverBook(Spellbook spellbook)
        {
            return GetManeuverBook(spellbook.Blueprint.ToReference<BlueprintSpellbookReference>());
        }

        private ManeuverBook GetManeuverBook(BlueprintSpellbook spellbook)
        {
            return GetManeuverBook(spellbook.ToReference<BlueprintSpellbookReference>());
           
        }

        private ManeuverBook GetManeuverBook(BlueprintSpellbookReference spellbook)
        {
            var find = ManeuverBooks.FirstOrDefault(x => x.BlueprintSpellbookReference.Equals(spellbook));
            if (find == null)
            {
                Main.Context.Logger.Log($"Unable to find maneuver book {spellbook.NameSafe()} on {Owner.CharacterName}, recreating");
                var comp = spellbook.Get().Components.OfType<ManeuverBookComponent>().FirstOrDefault();
                RegisterBook(Owner.Progression.Features.GetFact(comp.GrantingFeature), spellbook, comp);
                find = ManeuverBooks.FirstOrDefault(x => x.BlueprintSpellbookReference.Equals(spellbook));
            }

            return find;

            
        }

        public bool InCombatMode()
        {
            if (Owner.Unit.CombatState.IsInCombat)
                return true;
            if (Owner.HasFact(BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.Context, "InCombatModeSystemBuff")))
                return true;


            else return false;
        }

        internal int GetTimesMemorized(BlueprintAbility blueprint, SlotLayer readied, Spellbook instance)
        {
            var book = GetManeuverBook(instance);
            int count = 0;
            if (book != null)
            {
                foreach (var slot in book.ManeuverSlots)
                {
                    if (slot.Get(readied) != null && slot.Get(readied).Equals(blueprint.ToReference<BlueprintAbilityReference>()))
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        internal int GetTimesMemorized(BlueprintAbility blueprint, SlotLayer readied)
        {
            int count = 0;
            foreach(var book in ManeuverBooks)
            {
                foreach(var slot in book.ManeuverSlots)
                {
                    if (slot.Get(readied) != null && slot.Get(readied).Equals(blueprint.ToReference<BlueprintAbilityReference>()))
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        internal bool CanMemorizeForBook(Spellbook instance)
        {
            if (InCombatMode())
                return false;


            return true;
        }

        internal bool ExpendManeuverOnSelection(Spellbook instance, BlueprintAbility blueprint)
        {
            var book = GetManeuverBook(instance);
            if (book == null)
                return false;
            else
            {
                //TODO INSERT FREEBIE LOGIC


                return book.ExpendManeuver(blueprint);

            }
        }

        internal bool ManeuverReadedAndUsable(Spellbook instance, BlueprintAbility blueprint)
        {
            var book = GetManeuverBook(instance);
            if (book != null)
            {
                return book.ManueverIsAvailable(blueprint);
            }
            else
                return false;
        }

        internal IEnumerable<SpellSlot> GetCurrentReadiedManeuversSlotDisplay(Spellbook instance, bool displayPrepped, int spellLevel)
        {
            var book = GetManeuverBook(instance);
            if (book == null)
            {
                Main.Context.Logger.Log($"ManeuverBook not found for spellbook {instance.Blueprint.Name} while getting readied list");
                return new List<SpellSlot>();
            }
            else
            {
                List<SpellSlot> spellSlots = new();
                var spells = instance.GetAllKnownManeuvers();


                for (int i = 0; i < book.ManeuverSlots.Count; i++)
                {
                    var slot = book.ManeuverSlots[i];

                    var newSlot = new SpellSlot(spellLevel, SpellSlotType.Common, i);
                    if (!displayPrepped)
                    {
                        if (slot.Combat != null)
                            newSlot.Spell = spells.FirstOrDefault(x => x.Blueprint.ToReference<BlueprintAbilityReference>().Equals(slot.Combat));
                    }
                    else
                    {
                        if (slot.Readied != null)
                            newSlot.Spell = spells.FirstOrDefault(x => x.Blueprint.ToReference<BlueprintAbilityReference>().Equals(slot.Readied));
                    }
                    newSlot.Available = slot.Available;
                    spellSlots.Add(newSlot);
                }

                return spellSlots;
            }

            
        }

        internal int GetCastsForManeuverFromBook(Spellbook instance, AbilityData spell)
        {
            var book = GetManeuverBook(instance);
            if (book != null)
            {
                int casts = 0;
                casts += book.ManeuverSlots.Count(x => x.Combat != null && spell.Blueprint.ToReference<BlueprintAbilityReference>().Equals(x.Combat));


                return casts;
            }
            else
                return 0;


        }

        public void DemandSlotUpdate(BlueprintSpellbookReference reference)
        {
            var book = GetManeuverBook(reference);
            if (book != null)
                book.DemandSlotsUpdate();


        }




        #endregion





        #region learn Maneuvers
        internal bool CanLearnManeuver(BlueprintAbilityReference manuever, ManeuverSelectionFeature.Mode mode, BlueprintSpellbookReference targetSpellbook, BlueprintCharacterClass currentClass)
        {
            var book = ManeuverBooks.FirstOrDefault(x => x.BlueprintSpellbookReference.Equals(targetSpellbook));
            var maenuverData = manuever.Get().GetComponent<ManeuverInformation>();

            if (maenuverData == null)
                return false;
            if (book == null)
                return false;
            if (maenuverData.isPrcAbility)
            {
                return false;
            }
            if (maenuverData.DisciplineKeys.Length != 1)
                return false;
            if (book.ManeuverBookData.BookType == ManeuverBookComponent.ManeuverBookType.MartialTraining)//If Martial Training, redirect (TODO convert this to using redirect components
            {


                var martialPart = Owner.Ensure<UnitPartMartialTraining>();
                return martialPart.CanLearnManeuver(manuever);
            }
            if (mode == ManeuverSelectionFeature.Mode.Standard && book.ManeuverBookData.BookType == ManeuverBookComponent.ManeuverBookType.Level6Archetype)
            {
                if (maenuverData.ManeuverLevel > ArchetypeAllowedLevel(book.ManeuverBookData.GrantingProgression))//do archetype check
                {
                    return false;
                }
            }
            else if (maenuverData.ManeuverLevel > InitiatorLevelPermittedManevueverLevle(targetSpellbook))//If ML too low, block - thus comes after MT because MT doesn't care about that - use if-else because if prior check applies and is passed, ML is absolutely high enough
                return false;
            if (mode == ManeuverSelectionFeature.Mode.Standard)//If normal learning for level up do normal ding things
            {
                if (!DisciplineIsValidForClass(maenuverData.DisciplineKeys[0], currentClass, false))//Check if this class can learn it (class discipline list plus weirdo global unlocks (battle templar
                {
                    return false;
                }


            }
            else if (mode == ManeuverSelectionFeature.Mode.AdvancedStudy)
            {
                if (!DisciplineIsValidForClass(maenuverData.DisciplineKeys[0], currentClass, true))
                {
                    return false;
                }
            }

            return ManeuverKnowledgeRequirementMet(maenuverData.ManeuverLevel, maenuverData.DisciplineKeys[0]);







        }

        private bool ManeuverKnowledgeRequirementMet(int maneuverLevel, string key)
        {
            int knownNeeded = (maneuverLevel - 1) / 2;

            int found = 0;

            if (knownNeeded == 0)
                return true;

            var list = Owner.Spellbooks.Where(x => x.Blueprint.GetComponent<ManeuverBookComponent>() != null).SelectMany(x => x.GetAllKnownSpells());
            foreach (var move in list)
            {
                var moveData = move.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                if (moveData != null)
                {
                    if (moveData.DisciplineKeys.Contains(key))
                    {
                        found++;
                        if (found >= knownNeeded)
                            return true;
                    }
                }

            }

            return false;


        }

        private int ArchetypeAllowedLevel(BlueprintProgressionReference grantingProgression)
        {
            int level = Owner.Progression.GetProgression(grantingProgression.Get()).Level;
            if (level <= 3)
                return 1;
            else if (level <= 6)
                return 2;
            else if (level <= 8)
                return 3;
            else if (level <= 10)
                return 4;
            else if (level <= 12)
                return 5;
            else
                return 6;

        }

        private int InitiatorLevelPermittedManevueverLevle(BlueprintSpellbookReference targetSpellbook)
        {
            int initLevel = Owner.DemandSpellbook(targetSpellbook.Get()).CasterLevel;

            return (initLevel - 1) / 2 + 1;

        }

        private bool DisciplineIsValidForClass(string disciplineKey, BlueprintCharacterClass currentClass, bool includePRCaccess)
        {
            if (GlobalUnlocks.Any(x => x.disciplineKey.Equals(disciplineKey)))
                return true;
            ClassData data = Owner.Progression.GetClassData(currentClass);

            foreach (var unlock in classSpecificUnlocks)
            {
                if (unlock.m_ClassRefs.Contains(currentClass.ToReference<BlueprintCharacterClassReference>()))
                {
                    if (unlock.m_ArchRefs.Length == 0 || unlock.m_ArchRefs.Any(x => data.Archetypes.Contains(x.Get())))
                    {
                        if (unlock.discipline.Equals(disciplineKey))
                            return true;
                    }
                    
                }
                if (includePRCaccess && unlock.m_ClassRefs.Length == 1 && unlock.m_ClassRefs[0].Get().PrestigeClass)
                {
                    if (unlock.discipline.Equals(disciplineKey))
                        return true;
                }
            }
            return false;
        }

        #endregion

        #region maneuver unlocks

        private class GlobalUnlock
        {

            public UnitFact sourceFeature;
            public string disciplineKey;

        }

        private List<GlobalUnlock> GlobalUnlocks = new();

        private class ProgressionSpecificUnlock
        {
            public BlueprintProgressionReference m_Reference;

            public AddManeuverBook AddManeuverBook => m_Reference.Get().LevelEntries.FirstOrDefault(x => x.Level == 1).Features.SelectMany(x => x.Components.OfType<AddManeuverBook>()).FirstOrDefault();

            public BlueprintCharacterClassReference[] m_ClassRefs => AddManeuverBook.ManeuverBookComponent.ClassReference;
            public BlueprintArchetypeReference[] m_ArchRefs => AddManeuverBook.ManeuverBookComponent.ArchetypeReference;

            public UnitFact sourceFeature;
            public string discipline;
            public string Display()
            {
                return "Progression: " + m_Reference.Get().Name + ", Discipline: " + discipline + ", Source: " + sourceFeature.Name;
            }

        }
        private List<ProgressionSpecificUnlock> classSpecificUnlocks = new();
        internal void RegisterBook(UnitFact fact, BlueprintSpellbook spellbook, ManeuverBookComponent maneuverBookComponent)
        {
           // Main.Context.Logger.Log($"Registered Book {spellbook.Name} on {Owner.CharacterName}");

            var book = new ManeuverBook(fact, spellbook.ToReference<BlueprintSpellbookReference>());
            ManeuverBooks.Add(book);
            LoadBook(book);





        }

        internal void RegisterClassUnlock(UnitFact fact, BlueprintProgressionReference progresionRef, string disciplineType)
        {
            //Main.Context.Logger.Log($"Registered Discipline {disciplineType} on {Owner.CharacterName} for {progresionRef.NameSafe()}");
            ProgressionSpecificUnlock classSpecificUnlock = new ProgressionSpecificUnlock
            {
                sourceFeature = fact,
                m_Reference = progresionRef,
                discipline = disciplineType
            };
            if (!classSpecificUnlocks.Any(x => x.sourceFeature.Equals(fact) && x.m_Reference.Equals(progresionRef) && x.discipline == disciplineType))
            {
                classSpecificUnlocks.Add(classSpecificUnlock);
            }
            foreach (var v in classSpecificUnlocks)
            {
                Main.Context.Logger.Log("Unlock Found: " + v.Display());
            }

        }

        internal void UnregisterClassUnlock(UnitFact fact, BlueprintProgressionReference progresionRef, string disciplineType)
        {
            //Main.Context.Logger.Log($"Unregisted Discipline {disciplineType} on {Owner.CharacterName} for {progresionRef.NameSafe()}");
            var rem = classSpecificUnlocks.RemoveAll(x => x.sourceFeature == fact && x.m_Reference == progresionRef && x.discipline.Equals(disciplineType));
            //Main.Context.Logger.Log($"Removed {rem}");

        }



        #endregion
        

        public void OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            var ability = evt.Reason?.Ability;
            if (ability != null)
            {
              

                if (ability.Blueprint.GetComponents<TICManeuverCMBBonus>().Any())
                {
                    foreach(var bonus in ability.Blueprint.GetComponents<TICManeuverCMBBonus>().Where(x=>x.combatManeuvers.Contains(evt.Type)))
                    {
                        evt.AddModifier(bonus.Bonus, bonus.Descriptor);
                    }
                }
            }
        }

        public void OnEventDidTrigger(RuleCombatManeuver evt)
        {
            
        }

        public void HandleUnitCompleteLevelup(UnitEntityData unit)
        {

            if (Owner.Unit != unit)
            {

            }
            else
            {
                Main.Context.Logger.Log($"Starting Post Level Up Martial Recording for {unit.CharacterName}");

                foreach (Spellbook s in unit.Spellbooks.Where(x => x.Blueprint.Components.OfType<ManeuverBookComponent>().Any()))
                {
                    
                    var book = ManeuverBooks.FirstOrDefault(x=>x.Spellbook.Equals(s));
                    Main.Context.Logger.Log($"Starting Post Level Up Recording for {unit.CharacterName} with Maneuver Book {s.Blueprint.Name}");
                    DemandSlotUpdate(s.Blueprint.ToReference<BlueprintSpellbookReference>());
                    Main.Context.Logger.Log($"{GetManeuverBook(s).ManeuverSlots.Count} slots in book");
                    if (book.ManeuverSlots.Any(x => x.Readied == null))
                    {
                        Main.Context.Logger.Log($"{unit.CharacterName} has {book.ManeuverSlots.Count(x => x.Readied == null)} empty slots for book {s.Blueprint.Name}, fixing");
                        List<AbilityData> available = s.GetAllKnownManeuvers().Where(x => !book.ManeuverIsReadied(x.Blueprint.ToReference<BlueprintAbilityReference>())).ToList();
                        int empty = 0;
                        foreach (var slot in book.ManeuverSlots.Where(x => x.Readied == null))//TODO improve for multiclassing
                        {
                            empty++;
                            Main.Context.Logger.Log($"Trying to fix empty slot: {empty}");
                            if (available.Any())
                            {
                                var pick = available.FirstOrDefault();
                                if (pick != null)
                                {
                                    Main.Context.Logger.Log($"Auto-adding {pick} to list");
                                    slot.SetAsReadied(pick.Blueprint.ToReference<BlueprintAbilityReference>());
                                    available.RemoveAll(x=>x.Blueprint.AssetGuid.Equals(pick.Blueprint.AssetGuid));
                                }
                            }
                            else
                            {
                                Main.Context.Logger.Log($"No Filler Found");
                            }
                        }



                    }
                    foreach (var slot in book.ManeuverSlots.Where(x => x.Readied != null && x.Combat == null))
                    {
                        slot.SetAsReadied(slot.Readied);
                    }
                }
            }

        }

        
    }
}
