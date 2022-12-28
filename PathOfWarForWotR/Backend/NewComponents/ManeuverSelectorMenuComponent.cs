using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewComponents.ManeuverBookSystem;

namespace TheInfiniteCrusade.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintFeatureSelection))]
    public class ManeuverSelectorMenuComponent : BlueprintComponent
    {
        public BlueprintManeuverBookReference targetBook;

        public ManeuverSelectionMode SelectionMode;

    }
}
