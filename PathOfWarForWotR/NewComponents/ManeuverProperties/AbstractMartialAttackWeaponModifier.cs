using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.ManeuverProperties
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintAbility), false)]
    public abstract class AbstractMartialAttackWeaponModifier : BlueprintComponent
    {
        public virtual void ModifyWeaponStats(RuleCalculateWeaponStats evt)
        {

        }

        public virtual void ModifyWeaponStats(RulePrepareDamage evt)
        {

        }
        

    }
}
