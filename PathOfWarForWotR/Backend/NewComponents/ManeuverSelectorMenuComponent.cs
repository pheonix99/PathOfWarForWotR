using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;

namespace PathOfWarForWotR.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintFeatureSelection))]
    public class ManeuverSelectorMenuComponent : BlueprintComponent
    {
        public BlueprintManeuverBookReference targetBook;

        public ManeuverSelectionMode SelectionMode;

    }
}
