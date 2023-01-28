using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;

namespace PathOfWarForWotR.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintFeature))]
    class ManeuverSelectorPickComponent : BlueprintComponent
    {
        public BlueprintAbilityReference Maneuver;


    }
}
