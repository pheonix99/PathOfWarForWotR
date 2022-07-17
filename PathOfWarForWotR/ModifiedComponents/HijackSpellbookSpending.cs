using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
using TheInfiniteCrusade.NewComponents.UnitParts;

namespace TheInfiniteCrusade.ModifiedComponents
{
    class HijackSpellbookSpending
    {
        /*
        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.GetAvailableForCastSpellCount))]
        static class Spellbook_GetAvailableForCastSpellCount_RerouteToCustomData
        {

            static bool Prefix(Spellbook __instance, ref int __result, AbilityData spell)
            {
                var bookdef = __instance.Blueprint.Components.OfType<AddManeuverBookComponent>().FirstOrDefault();

                if (bookdef != null)
                {
                    var comp = spell.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                    if (comp == null)
                    {
                        __result = 0;
                        return false;
                    }
                    else if (comp.ManeuverType == ManeuverType.Stance)
                    {
                        __result = 1;
                        return false;
                    }



                    var part = __instance.Owner.Ensure<UnitPartMartialDisciple>();
                    __result = part.GetCastsForManeuverFromBook(__instance, spell);

                    return false;
                }
                else
                    return true;
            }
        }

        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.SpendInternal), new Type[] { typeof(BlueprintAbility), typeof(AbilityData), typeof(bool), typeof(bool) })]
        static class Spellbook_SpendInternal_HijackAndRerouteToCustomData_Patch
        {
            static bool Prefix(Spellbook __instance, ref bool __result, [NotNull] BlueprintAbility blueprint, [CanBeNull] AbilityData spell, bool doSpend, bool excludeSpecial)
            {
                if (__instance.Blueprint.Components.OfType<AddManeuverBookComponent>().Any())
                {
                    var part = __instance.Owner.Get<UnitPartMartialDisciple>();
                    if (part == null)
                    {
                        __result = false;
                        return false;
                    }
                    else
                    {
                        var maneuverPart = blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                        if (maneuverPart.ManeuverType == ManeuverType.Stance)
                        {
                            __result = true;
                            return false;
                        }

                        bool canUse = part.ManeuverReadedAndUsable(__instance, blueprint);

                        if (canUse)
                        {
                            if (doSpend)
                            {
                                part.ExpendManeuverOnSelection(__instance, blueprint);
                            }

                            __result = true;
                            return false;

                        }
                        else
                        {
                            __result = false;
                            return false;
                        }

                    }


                }


                return true;
            }
        }



        //IsPossibleMemorize - false it to block interactions from outside my setup?
        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.CantSpendReason), new Type[] { typeof(BlueprintAbility) })]
        public static class CantSpendReasonPatch
        {

            static bool Prefix(Spellbook __instance, ref string __result, BlueprintAbility blueprint)
            {
                if (__instance.Blueprint.Components.OfType<AddManeuverBookComponent>().Any())
                {
                    var part = __instance.Owner.Ensure<UnitPartMartialDisciple>();
                    bool canUse = part.ManeuverReadedAndUsable(__instance, blueprint);
                    if (!canUse)
                    {
                        __result = "Manuever Not Readied";
                        return false;
                    }
                    else
                    {
                        __result = "";
                        return false;
                    }
                }
                else
                    return true;
            }
        }
        */
    }
}
