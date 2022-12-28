using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System.Linq;

namespace TheInfiniteCrusade.Backend.NewComponents.Properties
{
    class ArchetypeInitiatorLevelPropertyGetter : PropertyValueGetter
    {
        public override int GetBaseValue(UnitEntityData unit)
        {

#if DEBUG
            //Main.Context.Logger.Log($"ArchetypeInitiatorLevelPropertyGetter {OwnerBlueprint.name}");
#endif


            int level = 0;
            int otherLevel = 0;
            foreach (var examinedClass in unit.Progression.Classes)
            {
#if DEBUG
                //Main.Context.Logger.Log($"ArchetypeInitiatorLevelPropertyGetter {OwnerBlueprint.name} on {examinedClass.CharacterClass.Name}");
#endif
                if (examinedClass.CharacterClass.IsMythic || examinedClass.CharacterClass.IsHigherMythic)
                    continue;
                else if (examinedClass.CharacterClass.PrestigeClass && examinedClass.CharacterClass.Components.OfType<MartialPrestigeClass>().Any())
                {
#if DEBUG
                    //Main.Context.Logger.Log($"ArchetypeInitiatorLevelPropertyGetter {OwnerBlueprint.name} on {examinedClass.CharacterClass.Name}, adding {examinedClass.Level}");
#endif

                    level += examinedClass.Level;
                }
                else if (m_Classes.Any(x => x.Equals(examinedClass.CharacterClass.ToReference<BlueprintCharacterClassReference>())))
                {
                    if (examinedClass.Archetypes.Any(x => m_Archetypes.Contains(x.ToReference<BlueprintArchetypeReference>())))
                    {
#if DEBUG
                        //Main.Context.Logger.Log($"ArchetypeInitiatorLevelPropertyGetter {OwnerBlueprint.name} on {examinedClass.CharacterClass.Name}, adding {examinedClass.Level}");
#endif
                        level += examinedClass.Level;
                    }
                    else
                    {
#if DEBUG
                       // Main.Context.Logger.Log($"ArchetypeInitiatorLevelPropertyGetter {OwnerBlueprint.name} on {examinedClass.CharacterClass.Name}, adding {examinedClass.Level} to otherlevel");
#endif
                        otherLevel += examinedClass.Level;
                    }



                }

                else
                {
#if DEBUG
                    //Main.Context.Logger.Log($"ArchetypeInitiatorLevelPropertyGetter {OwnerBlueprint.name} on {examinedClass.CharacterClass.Name}, adding {examinedClass.Level} to otherlevel");
#endif
                    otherLevel += examinedClass.Level;
                }
            }




            level += (otherLevel / 2);

            if (m_ArbitraryLevelFact != null)
            {
                level += new FactRankGetter() { m_Fact = m_ArbitraryLevelFact }.GetBaseValue(unit);
            }

            return level;

        }

        public BlueprintCharacterClassReference[] m_Classes;
        public BlueprintArchetypeReference[] m_Archetypes;

        public BlueprintUnitFactReference m_ArbitraryLevelFact;


    }

}
