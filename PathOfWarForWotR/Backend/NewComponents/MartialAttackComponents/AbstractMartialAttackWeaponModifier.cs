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
        public virtual void OnEventDidTrigger_RuleCalculateWeaponStats(RuleCalculateWeaponStats evt)
        {

        }

        public virtual void OnEventAboutToTrigger_RulePrepareDamage(RulePrepareDamage evt)
        {

        }

        public virtual void OnEventAboutToTrigger_RuleCalculateWeaponStats(RuleCalculateWeaponStats evt)
        {

        }

    }
}
