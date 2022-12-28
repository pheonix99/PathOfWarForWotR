using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace TheInfiniteCrusade.Backend.NewComponents.MartialAttackComponents
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
