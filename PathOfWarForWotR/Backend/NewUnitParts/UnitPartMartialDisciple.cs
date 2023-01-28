using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents;
using PathOfWarForWotR.Backend.NewEvents;
using PathOfWarForWotR.Backend.NewUnitDataClasses;
using PathOfWarForWotR.Extensions;

namespace PathOfWarForWotR.Backend.NewUnitParts
{
    class UnitPartMartialDisciple : OldStyleUnitPart, IUnitSubscriber, IUnitCompleteLevelUpHandler, ISubscriber, IInitiatorRulebookSubscriber, ICombatStartedWhileCooledDownHandler, IPostCombatCooldownHandler
    {

        #region Handle Maneuver Books

        private List<ManeuverBook> ManeuverBooks => Owner.ManeuverBooks().ToList();

        public bool KnowsManeuver(BlueprintAbilityReference maneuver)
        {
            return ManeuverBooks.Any(x => x.Knows(maneuver));
        }

        internal void UnregisterNonClassManeuverBook(BlueprintManeuverBookReference m_ManeuverBook)
        {
           
        }

        internal void RegisterNonClassManeuverBook(BlueprintManeuverBookReference reference)
        {
            if (!UnlocksForBooks.ContainsKey(reference))
                UnlocksForBooks.Add(reference, new());
        }

        internal void RegisterClassManueverBook(BlueprintCharacterClass blueprintCharacterClass, BlueprintManeuverBookReference blueprintManeuverBookReference)
        {
            if (!ClassToBookLinkage.ContainsKey(blueprintCharacterClass.ToReference<BlueprintCharacterClassReference>()))
                ClassToBookLinkage.Add(blueprintCharacterClass.ToReference<BlueprintCharacterClassReference>(), blueprintManeuverBookReference);
            if (!UnlocksForBooks.ContainsKey(blueprintManeuverBookReference))
                UnlocksForBooks.Add(blueprintManeuverBookReference, new());
        }

        internal void UnregisterClassManueverBook(BlueprintCharacterClass blueprintCharacterClass, BlueprintManeuverBookReference blueprintManeuverBookReference)
        {
            if (ClassToBookLinkage.ContainsKey(blueprintCharacterClass.ToReference<BlueprintCharacterClassReference>()))
                ClassToBookLinkage.Remove(blueprintCharacterClass.ToReference<BlueprintCharacterClassReference>());
            
        }
        #endregion

        #region handle unlocks
        internal void RegisterBookUnlock(UnitFact fact, BlueprintManeuverBookReference bookRef, string disciplineType)
        {
            
            if (UnlocksForBooks.TryGetValue(bookRef, out var list))
            {

                list.Add(new DisciplineUnlock() { sourceFeature = fact, discipline = disciplineType });
            }
            else
            {
                UnlocksForBooks.Add(bookRef, new());
                list.Add(new DisciplineUnlock() { sourceFeature = fact, discipline = disciplineType });
            }
            AllUnlocks.Add(new DisciplineUnlock() { sourceFeature = fact, discipline = disciplineType });
        }

        internal void UnregisterBookUnlock(UnitFact fact, BlueprintManeuverBookReference bookRef, string disciplineType)
        {
            if (UnlocksForBooks.TryGetValue(bookRef, out var list))
            {
                list.Remove(x => x.sourceFeature == fact && x.discipline.Equals(disciplineType));
            }
            AllUnlocks.Remove(x => x.sourceFeature == fact && x.discipline.Equals(disciplineType));
        }


        private class DisciplineUnlock
        {
            public UnitFact sourceFeature;
            public string discipline;
        }


        private Dictionary<BlueprintManeuverBookReference, List<DisciplineUnlock>> UnlocksForBooks = new();

        private Dictionary<BlueprintCharacterClassReference, BlueprintManeuverBookReference> ClassToBookLinkage = new();

        private class PRCAccessData
        {
            public List<DisciplineUnlock> disciplineUnlocks = new();
            public bool AllAvailable;
        }

        private Dictionary<BlueprintCharacterClassReference, PRCAccessData> PRCUnlockData = new();

        private List<DisciplineUnlock> AllUnlocks = new();

        public bool CanLearnDisciplineWithNonClassBook(string disciplineKey, BlueprintManeuverBookReference book)
        {
            if (UnlocksForBooks.TryGetValue(book, out var unlocks))
            {
                return unlocks.Any(x => x.discipline.Equals(disciplineKey));
            }
            return false;
        }

        public bool CanLearnDisciplineAsBaseClass(string disciplineKey, BlueprintCharacterClassReference currentClass)
        {
            if (ClassToBookLinkage.TryGetValue(currentClass, out var book))
            {
                if (UnlocksForBooks.TryGetValue(book, out var unlocks))
                {
                    return unlocks.Any(x => x.discipline.Equals(disciplineKey));
                }
            }
            return false;

        }

        public bool CanLearnDisciplineAsFreeStudy(string disciplineKey, BlueprintManeuverBookReference reference)
        {

            throw new NotImplementedException();
        }

        public bool CanLearnDisciplineAsPrestigeClass(string disciplineKey, BlueprintCharacterClassReference currentClass)
        {
            if (PRCUnlockData.TryGetValue(currentClass, out var data))
            {
                if (data.AllAvailable)
                    return AllUnlocks.Any(x => x.discipline.Equals(disciplineKey));
                else
                    return data.disciplineUnlocks.Any(x => x.discipline.Equals(disciplineKey));

            }
            return false;
        }



        #endregion

        #region serialization 
        public override void OnPostLoad()
        {
            Main.Context.Logger.Log($"Loading Maneuver  Info: from Unit Part");
            foreach (var book in ManeuverBooks)
            {
                book.LoadBook();
            }
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

        #endregion
        
       
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
            int initLevel = book.BaseLevel;

            return (initLevel - 1) / 2 + 1;

        }


        
        #endregion

        #region maneuver unlocks

     

        

        
        
        


        #endregion
        
        //TODO MOVE THIS ELSEWHERE
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

        

        Dictionary<BlueprintManeuverBookReference, StatType> InitiatorStatOverrides = new();



      

        
    }
}
