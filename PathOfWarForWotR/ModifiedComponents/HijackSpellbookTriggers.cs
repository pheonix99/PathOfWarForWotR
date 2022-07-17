using HarmonyLib;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.NewComponents.UnitParts;

namespace TheInfiniteCrusade.ModifiedComponents
{
    class HijackSpellbookTriggers
    {
        
        //Rest - hack to trigger refresh and reshuffle on 

        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.Rest))]
        public static class RestRecoversManeuversToo
        {
            static bool Prefix(Spellbook __instance)
            {

                if (__instance.Blueprint.Components.OfType<AddManeuverBookComponent>().Any())
                {
                    //Tossed this into the controller.

                    return false;
                }

                return true;
            }
        }
    }
}
