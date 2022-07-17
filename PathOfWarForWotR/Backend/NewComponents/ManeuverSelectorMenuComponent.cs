using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;

namespace TheInfiniteCrusade.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintFeatureSelection))]
    public class ManeuverSelectorMenuComponent : BlueprintComponent
    {
        public BlueprintManeuverBookReference targetBook;

        public ManeuverSelectionMode SelectionMode;

    }
}
