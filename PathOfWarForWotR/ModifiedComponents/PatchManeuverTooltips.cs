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
