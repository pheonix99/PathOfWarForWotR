using HarmonyLib;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;

namespace TheInfiniteCrusade.ModifiedComponents
{
    class PatchManeuverTooltips
    {
        /*
        [HarmonyPatch(typeof(UIUtilityItem), "GetSchoolName", new Type[] { typeof(string), typeof(BlueprintAbility) })]
        class DisplayDiscipline_UIUtilityItem_GetSchoolName
        {
            static void Postfix(ref string __result, BlueprintAbility blueprintAbility)
            {
                
                if (blueprintAbility.Components.OfType<ManeuverInformation>().Any())
                {
                    __result = blueprintAbility.Components.OfType<ManeuverInformation>().FirstOrDefault().GetManeuverSchoolString();
                }

            }
        }
        */
    }
}
