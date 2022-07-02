using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;

namespace TheInfiniteCrusade.Extensions
{
   static class SpellbookExtensions
    {
        public static IEnumerable<AbilityData> GetAllKnownManeuvers(this Spellbook s)
        {
            return s.GetAllKnownSpells().Where(x =>
            {
                var data = x.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                if (data == null)
                    return false;
                else
                    return data.ManeuverType != ManeuverType.Stance;

            });

        }

        public static IEnumerable<AbilityData> GetAllKnownStances(this Spellbook s)
        {
            return s.GetAllKnownSpells().Where(x =>
            {
                var data = x.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                if (data == null)
                    return false;
                else
                    return data.ManeuverType == ManeuverType.Stance;

            });

        }
    }
}
