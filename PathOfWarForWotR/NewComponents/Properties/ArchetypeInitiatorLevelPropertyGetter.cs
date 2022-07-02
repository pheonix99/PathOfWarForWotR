using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.Properties
{
    class ArchetypeInitiatorLevelPropertyGetter : PropertyValueGetter
    {
        public override int GetBaseValue(UnitEntityData unit)
        {




            int level = 0;
            int otherLevel = 0;
            foreach (var examinedClass in unit.Progression.Classes)
            {
                if (examinedClass.CharacterClass.IsMythic || examinedClass.CharacterClass.IsHigherMythic)
                    continue;
                else if (examinedClass.CharacterClass.PrestigeClass && examinedClass.CharacterClass.Components.OfType<MartialPrestigeClass>().Any())
                {
                    level += examinedClass.Level;
                }
                else if (m_Classes.Any(x => x.Equals(examinedClass.CharacterClass.ToReference<BlueprintCharacterClassReference>())))
                {
                    if (examinedClass.Archetypes.Any(x => m_Archetypes.Contains(x.ToReference<BlueprintArchetypeReference>())))
                    {
                        level += examinedClass.Level;
                    }
                    else
                    {
                        otherLevel += examinedClass.Level;
                    }



                }

                else
                {
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
