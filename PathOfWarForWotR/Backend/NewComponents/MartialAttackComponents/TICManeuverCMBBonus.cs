using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System.Collections.Generic;

namespace PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents
{
    [AllowedOn(typeof(BlueprintAbility))]
    class TICManeuverCMBBonus : BlueprintComponent
    {
        public int Bonus = 0;
        public List<CombatManeuver> combatManeuvers = new();
        public ModifierDescriptor Descriptor;
    }
}
