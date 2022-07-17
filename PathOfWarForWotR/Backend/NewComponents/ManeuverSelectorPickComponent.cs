using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintFeature))]
    class ManeuverSelectorPickComponent : BlueprintComponent
    {
        public BlueprintAbilityReference Maneuver;


    }
}
