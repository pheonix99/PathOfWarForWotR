using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;

namespace PathOfWarForWotR.ModifiedComponents
{
    class HackAbilityData
    {
        [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.SpellLevel), MethodType.Getter)]
        static class HackGetSpellLevel//This hijacking is for the spellbook display - just plain hijacking the level 1 display
        {
            static bool Prefix(AbilityData __instance, ref int __result)
            {
                var part = __instance.Blueprint.GetComponent<ManeuverInformation>();
                if (part != null)
                {

                    __result = part.ManeuverLevel;
                    return false;
                }

                return true;

            }

        }
    }
}
