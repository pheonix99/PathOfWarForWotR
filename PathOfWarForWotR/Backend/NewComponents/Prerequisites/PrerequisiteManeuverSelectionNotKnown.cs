using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using PathOfWarForWotR.Backend.NewUnitParts;

namespace PathOfWarForWotR.Backend.NewComponents.Prerequisites
{
    class PrerequisiteManeuverSelectionNotKnown : Prerequisite
    {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state)
        {
            if (selectionState.Selection is BlueprintFeatureSelection selection)//This is the one!
            {
                var abilityData = OwnerBlueprint.GetComponent<ManeuverSelectorPickComponent>()?.Maneuver;
                if (abilityData == null)
                {
                    return false;
                }
                else
                {
                    var part = unit.Get<UnitPartMartialDisciple>();
                    if (part != null)
                    {
                        return !part.KnowsManeuver(abilityData);
                    }
                    else
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

        public override string GetUITextInternal(UnitDescriptor unit)
        {
            return "Not Already Known";
        }
    }
}
