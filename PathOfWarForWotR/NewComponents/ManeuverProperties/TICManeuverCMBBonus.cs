using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.ManeuverProperties
{
    [AllowedOn(typeof(BlueprintAbility))]
    class TICManeuverCMBBonus : BlueprintComponent
    {
        public int Bonus = 0;
        public List<CombatManeuver> combatManeuvers = new();
        public ModifierDescriptor Descriptor;
    }
}
