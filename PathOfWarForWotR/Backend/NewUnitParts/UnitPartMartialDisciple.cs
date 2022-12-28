using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.Backend.NewComponents.MartialAttackComponents;
using TheInfiniteCrusade.Backend.NewEvents;
using TheInfiniteCrusade.Backend.NewUnitDataClasses;
using TheInfiniteCrusade.Extensions;

namespace TheInfiniteCrusade.Backend.NewUnitParts
{
    class UnitPartMartialDisciple : OldStyleUnitPart, IUnitSubscriber, IUnitCompleteLevelUpHandler, ISubscriber, IInitiatorRulebookSubscriber, ICombatStartedWhileCooledDownHandler, IPostCombatCooldownHandler
    {

        #region Handle Maneuver Books

        private List<ManeuverBook> ManeuverBooks => Owner.ManeuverBooks().ToList();

        

        

      

        
        

       
        

        public override void OnPostLoad()
        {
            Main.Context.Logger.Log($"Loading Maneuver  Info: from Unit Part");
            foreach (var book in ManeuverBooks)
            {
                book.LoadBook();
            }
        }

        internal bool PlanIsValid()
        {
            List<BlueprintAbilityReference> prepped = new();
            foreach (var book in ManeuverBooks)
            {
                if (book.ManeuverSlots.Any(x => x.Planned == null))
                {
                    if (book.ManeuverSlots.Count <= book.KnownManeuversCount)
                    {
                        return false;
                    }
                }
            }

           foreach (var book in ManeuverBooks)
            {
               
                
                foreach(var slot in book.ManeuverSlots)
                {
                    if (!prepped.Any(x=>x.deserializedGuid.Equals(slot.Planned.deserializedGuid)))
                    {
                        prepped.Add(slot.Planned);
                    }
                    else
                    {
                        return false;
                    }
                        
                }


                


            }

            return true;

        }

        

        public override void OnPreSave()
        {
            Main.Context.Logger.Log($"Saving Maneuver  Info: from Unit Part");

            foreach (var book in ManeuverBooks)
            {
                Main.Context.Logger.Log($"Saving {book.Name} Book Info - stage : in Unit Part");
                book.SaveBook();
            }
            
        }
       

       

        


        

        public bool InCombatMode()
        {
            if (Owner.Unit.CombatState.IsInCombat)
                return true;
            if (Owner.HasFact(BlueprintTools.GetModBlueprint<BlueprintBuff>(Main.Context, "InCombatModeSystemBuff")))
                return true;


            else return false;
        }

      

       

        

        
        /*
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
        */


        /*
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

      */



        #endregion

        public bool KnowsManeuver(BlueprintAbilityReference maneuver)
        {
            return ManeuverBooks.Any(x => x.Knows(maneuver));


        }



        #region learn Maneuvers
        

        public static int ManeuverKnowledgeRequirementForLevel(int level)
        {
            return (level - 1) / 2;
        }

        public int ManeuverKnowledgeForDiscipline(string key)
        {
            int found = 0;
            var list = Owner.ManeuverBooks().SelectMany(x => x.GetKnownMartialTechniques());
            foreach (var move in list)
            {
                //Main.Context.Logger.Log($"Evaluating {key} against {move.Name}");
                var moveData = move.Components.OfType<ManeuverInformation>().FirstOrDefault();
                if (moveData != null)
                {
                    if (moveData.DisciplineKeys.Contains(key))
                    {
                        found++;
                        
                    }
                }

            }
            return found;
        }

        

        public int ArchetypeAllowedLevel(BlueprintProgressionReference grantingProgression)
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

        private int InitiatorLevelPermittedManeuverLevel(ManeuverBook book)
        {
            int initLevel = book.GetRawInitiatorLevel();

            return (initLevel - 1) / 2 + 1;

        }

        public bool DisciplineIsValidForClass(string disciplineKey, BlueprintCharacterClass currentClass, bool includePRCaccess)
        {
            if (GlobalUnlocks.Any(x => x.disciplineKey.Equals(disciplineKey)))
                return true;
            ClassData data = Owner.Progression.GetClassData(currentClass);

            foreach (var unlock in classSpecificUnlocks)
            {
                
                if (unlock.m_ClassRefs.Contains(currentClass.ToReference<BlueprintCharacterClassReference>()))
                {
                    //Main.Context.Logger.Log($"Assessing unlock for {currentClass.Name}");

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
            public BlueprintProgressionReference m_Reference => ManeuverBook.GrantingProgression;

            public BlueprintManeuverBook ManeuverBook => blueprintManeuverBookReference.Get();
            public readonly BlueprintManeuverBookReference blueprintManeuverBookReference;

            public BlueprintCharacterClassReference[] m_ClassRefs => ManeuverBook.ClassReference;
            public BlueprintArchetypeReference[] m_ArchRefs => ManeuverBook.ArchetypeReference;

            public UnitFact sourceFeature;
            public string discipline;

            public ProgressionSpecificUnlock(BlueprintManeuverBookReference blueprintManeuverBookReference, UnitFact sourceFeature, string discipline)
            {
                this.blueprintManeuverBookReference = blueprintManeuverBookReference;
                this.sourceFeature = sourceFeature;
                this.discipline = discipline;
            }

            public string Display()
            {
                return "Progression: " + m_Reference.Get().Name + ", Discipline: " + discipline + ", Source: " + sourceFeature.Name;
            }

        }
        private List<ProgressionSpecificUnlock> classSpecificUnlocks = new();

        
        internal void RegisterClassUnlock(UnitFact fact, BlueprintManeuverBookReference bookReference, string disciplineType)
        {
            //Main.Context.Logger.Log($"Registered Discipline {disciplineType} on {Owner.CharacterName} for {progresionRef.NameSafe()}");
            ProgressionSpecificUnlock classSpecificUnlock = new ProgressionSpecificUnlock(bookReference, fact, disciplineType);
            if (!classSpecificUnlocks.Any(x => x.sourceFeature.Equals(fact) && x.blueprintManeuverBookReference.Equals(bookReference) && x.discipline == disciplineType))
            {
                classSpecificUnlocks.Add(classSpecificUnlock);
            }
            

        }

        internal void UnregisterClassUnlock(UnitFact fact, BlueprintManeuverBookReference bookReference, string disciplineType)
        {
            //Main.Context.Logger.Log($"Unregisted Discipline {disciplineType} on {Owner.CharacterName} for {progresionRef.NameSafe()}");
            var rem = classSpecificUnlocks.RemoveAll(x => x.sourceFeature == fact && x.blueprintManeuverBookReference == bookReference && x.discipline.Equals(disciplineType));
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

        
        public void HandleUnitCompleteLevelup(UnitEntityData unit)
        {

            if (Owner.Unit != unit)
            {

            }
            else
            {
                Main.Context.Logger.Log($"Starting Post Level Up Martial Recording for {unit.CharacterName}");

                foreach (ManeuverBook book in ManeuverBooks)
                {
                    
                    
                    Main.Context.Logger.Log($"Starting Post Level Up Recording for {unit.CharacterName} with Maneuver Book {book.Name}");
                    book.DemandSlotsUpdate();
                    Main.Context.Logger.Log($"{book.ManeuverSlots.Count} slots in book");
                    if (book.ManeuverSlots.Any(x => x.Readied == null))
                    {
                        Main.Context.Logger.Log($"{unit.CharacterName} has {book.ManeuverSlots.Count(x => x.Readied == null)} empty slots for book {book.Name}, fixing");
                        var available = book.GetKnownManeuvers().Where(x => !book.ManeuverIsReadied(x)).ToList();
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
                                    slot.SetAsReadied(pick.ToReference<BlueprintAbilityReference>());
                                    available.RemoveAll(x=>x.AssetGuid.Equals(pick.AssetGuid));
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

        internal void RecoverManeuver(BlueprintAbilityReference blueprintAbilityReference)
        {
            ManeuverBooks.FirstOrDefault(x => x.CanRecover(blueprintAbilityReference))?.RecoverManeuver(blueprintAbilityReference);
        }

        public void OnCombatStartWhileCooledDown()
        {
            foreach(var book in ManeuverBooks)
            {
                book.OnCombatStartWhileCooledDown();
            }
        }

        public void OnPostCombatCooldown()
        {
            foreach (var book in ManeuverBooks)
            {
                book.OnPostCombatCooldown ();
            }
        }
    }
}
