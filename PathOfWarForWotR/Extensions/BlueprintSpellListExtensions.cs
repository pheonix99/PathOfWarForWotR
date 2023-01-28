using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System.Collections.Generic;
using System.Linq;

namespace PathOfWarForWotR.Extensions
{
    public static class BlueprintSpellListExtensions
    {
        public static IEnumerable<BlueprintAbility> GetAllSpells(this BlueprintSpellList list)
        {
            return list.SpellsByLevel.SelectMany(x => x.Spells);
        }
    }
}
