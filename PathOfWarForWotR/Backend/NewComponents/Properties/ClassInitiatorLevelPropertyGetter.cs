using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System.Linq;

namespace TheInfiniteCrusade.Backend.NewComponents.Properties
{
    public class ClassInitiatorLevelPropertyGetter : PropertyValueGetter
    {
        public override int GetBaseValue(UnitEntityData unit)
        {




            int level = 0;
            int otherLevel = 0;
            foreach (var v in unit.Progression.Classes)
            {
                if (v.CharacterClass.IsMythic || v.CharacterClass.IsHigherMythic)
                    continue;
                else if (v.CharacterClass.ToReference<BlueprintCharacterClassReference>().Equals(m_Class))
                {
                    level += v.Level;
                }
                else if (v.CharacterClass.PrestigeClass && v.CharacterClass.Components.OfType<MartialPrestigeClass>().Any())
                {
                    level += v.Level;
                }
                else
                {
                    otherLevel += v.Level;
                }
            }
            level += (otherLevel / 2);

            if (m_ArbitraryLevelFact != null)
            {
                level += new FactRankGetter() { m_Fact = m_ArbitraryLevelFact }.GetBaseValue(unit);
            }

            return level;

        }

        public BlueprintCharacterClassReference m_Class;

        public BlueprintUnitFactReference m_ArbitraryLevelFact;


    }
}
