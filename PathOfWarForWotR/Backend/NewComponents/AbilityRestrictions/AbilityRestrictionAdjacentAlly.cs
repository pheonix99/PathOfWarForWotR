using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewComponents.AbilityRestrictions
{
    class AbilityRestrictionAdjacentAlly : BlueprintComponent, IAbilityTargetRestriction
    {
        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
        {
            return "Must be next to ally";
        }

        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target)
        {
            
            var targets = GameHelper.GetTargetsAround(target.Point, new Feet(5f)).Where(unit => unit.IsAlly(caster));
            return targets.Any();
        }
    }
}
