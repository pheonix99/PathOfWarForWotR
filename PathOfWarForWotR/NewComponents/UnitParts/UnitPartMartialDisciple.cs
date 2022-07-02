using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Extensions;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
using TheInfiniteCrusade.NewComponents.UnitParts.ManeuverBookSystem;

namespace TheInfiniteCrusade.NewComponents.UnitParts
{
    class UnitPartMartialDisciple : OldStyleUnitPart
    {

        #region Handle Maneuver Books

        public List<ManeuverBook> ManeuverBooks = new();
        public class ManeuverBook
        {







            public readonly UnitFact source;


            public List<ManeuverSlot> ManeuverSlots = new();

            public BlueprintSpellbookReference BlueprintSpellbookReference { get; }
            public bool GrantedType => ManeuverBookData.IsGranted;

            public BlueprintUnitPropertyReference ManeuverSlotsPropertyReference => ManeuverBookData.m_ManeuverSlotsReference;
            public ManeuverBookComponent ManeuverBookData => BlueprintSpellbookReference.Get().Components.OfType<ManeuverBookComponent>().First();
            public Spellbook Spellbook => source.Owner.DemandSpellbook(BlueprintSpellbookReference);

            public ManeuverBook(UnitFact fact, BlueprintSpellbookReference blueprintSpellbookReference)
            {
                source = fact;
                BlueprintSpellbookReference = blueprintSpellbookReference;
                Main.Context.Logger.Log($"Build Maneuver Book {blueprintSpellbookReference.NameSafe()} on  {fact.Owner.CharacterName}");

            }

            internal void DemandSlotsUpdate()
            {
                int correctSlots = ManeuverSlotsPropertyReference.Get().GetInt(source.Owner);
                if (correctSlots == ManeuverSlots.Count)
                {

                }
                else if (correctSlots > ManeuverSlots.Count)
                {
                    while (correctSlots > ManeuverSlots.Count)
                    {
                        ManeuverSlots.Add(new ManeuverSlot(ManeuverSlots.Max(x => x.Index) + 1, SlotType.Common));
                    }
                }
                else
                {
                    while (correctSlots < ManeuverSlots.Count)
                    {
                        ManeuverSlots.RemoveLast();
                    }
                }

            }
        }

        internal IEnumerable<SpellSlot> GetCurrentReadiedManeuversSlotDisplay(Spellbook instance, bool displayPrepped, int spellLevel)
        {
            var book = ManeuverBooks.FirstOrDefault(x => instance.Blueprint.ToReference<BlueprintSpellbookReference>().Equals(x));
            if (book == null)
                return new List<SpellSlot>();
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

        public void RegisterManeuverBook(UnitFact source, BlueprintSpellbookReference blueprintSpellbookReference)
        {
            if (!ManeuverBooks.Any(x => x.BlueprintSpellbookReference.Equals(blueprintSpellbookReference)))
            {
                ManeuverBooks.Add(new ManeuverBook(source, blueprintSpellbookReference));
            }
        }

        public void DemandSlotUpdate(BlueprintSpellbookReference reference)
        {
            var book = ManeuverBooks.FirstOrDefault(x => x.BlueprintSpellbookReference.Equals(reference));
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
            Main.Context.Logger.Log($"Registered Book {spellbook.Name} on {Owner.CharacterName}");

            var book = new ManeuverBook(fact, spellbook.ToReference<BlueprintSpellbookReference>());
            ManeuverBooks.Add(book);






        }

        internal void RegisterClassUnlock(UnitFact fact, BlueprintProgressionReference progresionRef, string disciplineType)
        {
            Main.Context.Logger.Log($"Registered Discipline {disciplineType} on {Owner.CharacterName} for {progresionRef.NameSafe()}");
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
            Main.Context.Logger.Log($"Unregisted Discipline {disciplineType} on {Owner.CharacterName} for {progresionRef.NameSafe()}");
            var rem = classSpecificUnlocks.RemoveAll(x => x.sourceFeature == fact && x.m_Reference == progresionRef && x.discipline.Equals(disciplineType));
            Main.Context.Logger.Log($"Removed {rem}");

        }



        #endregion
        internal void ReloadAndRecharge(Spellbook instance)
        {

        }
    }
}
