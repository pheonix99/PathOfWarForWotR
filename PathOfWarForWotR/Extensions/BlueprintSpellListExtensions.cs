using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.Extensions
{
    public static class BlueprintSpellListExtensions
    {
        public static IEnumerable<BlueprintAbility> GetAllSpells(this BlueprintSpellList list)
        {
            return list.SpellsByLevel.SelectMany(x => x.Spells);
        }
    }
}
