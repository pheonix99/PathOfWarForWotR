using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Class.LevelUp;
using TheInfiniteCrusade.Backend.NewComponents;
using TheInfiniteCrusade.Backend.NewUnitParts;

namespace TheInfiniteCrusade.ModifiedComponents
{
    class ModifyFeatureSelectionViewState
    {
        [HarmonyPatch(typeof(FeatureSelectionViewState), nameof(FeatureSelectionViewState.RefreshCanSelectState))]
        static class FeatureSelectionViewState_RefreshCanSelectState
        {
            static void Postfix(FeatureSelectionViewState __instance, FeatureSelectionState selectionState, IFeatureSelection selection, IFeatureSelectionItem item)
            {
                if (__instance.CanSelectState == FeatureSelectionViewState.SelectState.CanSelect)
                {
                    var move = __instance.Feature.GetComponent<ManeuverSelectorPickComponent>();
                    if  (move == null)
                    {
                        return;
                    }
                    else
                    {
                        var unit = Game.Instance.LevelUpController.Preview.Descriptor;
                        if (unit.Unit.PreviewOf.IsEmpty)
                            return;
                        var unit2 = unit.Unit.PreviewOf.Entity;
                        if (unit2 != null)
                        {
                            var part = unit2.Get<UnitPartMartialDisciple>();
                            if (part == null)
                                return;
                            else
                            {
                                if (part.KnowsManeuver(move.Maneuver))
                                {
                                    __instance.CanSelectState = FeatureSelectionViewState.SelectState.AlreadyHas;
                                }
                            }
                        }
                        
                    }
                }



            }

        }
    }
}
