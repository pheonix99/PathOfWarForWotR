using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using PathOfWarForWotR.Backend.NewComponents;
using PathOfWarForWotR.Extensions;

namespace PathOfWarForWotR.ModifiedComponents
{
    class ModifySelectFeature
    {
        [HarmonyPatch(typeof(SelectFeature), nameof(SelectFeature.Apply))]
        static class PatchSelectFeature_Apply
        {
            static bool Prefix(SelectFeature __instance, LevelUpState state, UnitDescriptor unit)
            {
                var selected = __instance.Item.Feature;
                if (selected == null)
                {
                    //Main.Context.Logger.Log($"__instance.Item.Feature is null on selectFeature");
                    return true;
                }
                var picked = selected.GetComponent<ManeuverSelectorPickComponent>()?.Maneuver;
                if (picked == null)
                {
                    //Main.Context.Logger.Log($"selected.GetComponent<ManeuverSelectorPickComponent>()?.Maneuver is null on selectFeature for selected {selected.Name}");
                    return true;
                }
                if (__instance.GetSelectionState(state)?.Selection != null && __instance.GetSelectionState(state).Selection is BlueprintFeatureSelection source)
                {
                   
                    var selectorComp = source.GetComponent<ManeuverSelectorMenuComponent>();
                    if (selectorComp == null)
                    {
                        Main.Context.Logger.Log($"__instance.GetSelectionState(state).SourceFeature.GetComponent<ManeuverSelectorMenuComponent>() is null on selectFeature for selected {selected.Name}");
                        Main.Context.Logger.Log($"SourceFeature is {source.Name}");
                        return true;
                    }
                    var book = selectorComp.targetBook;
                    if (book == null)
                    {
                        Main.Context.Logger.Log($"__instance.GetSelectionState(state).SourceFeature.GetComponent<ManeuverSelectorMenuComponent>().targetBook is null on selectFeature for selected {selected.Name}");
                        return true;
                    }
                    unit.DemandManeuverBook(book.Get()).LearnManeuver(picked);
                    if (unit.Unit.PreviewOf != null)
                    {
                        return !unit.Unit.PreviewOf.IsEmpty;
                    }
                    else
                        return false;
                    
                }
                return true;



             }

        }

    }
}
