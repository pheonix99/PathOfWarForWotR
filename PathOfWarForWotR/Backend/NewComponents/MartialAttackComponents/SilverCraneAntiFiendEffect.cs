using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using PathOfWarForWotR.Utilities;

namespace PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents
{
    class SilverCraneAntiFiendEffect : AbstractMartialAttackWeaponModifier
    {

        private ConditionsChecker conditionChecker;
        public SilverCraneAntiFiendEffect()
        {
            conditionChecker = ManeuverConfigurator.SilverCraneSpecialTarget().Build();
        }

        public override void OnEventAboutToTrigger_RulePrepareDamage(RulePrepareDamage evt)
        {
            

            if (evt.Target != null)
            {
                using (evt.Reason.Context.GetDataScope(evt.Target))
                {
                    if (conditionChecker.Check())
                    {
                        evt.DamageBundle.WeaponDamage.Reality |= Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                       
                    }
                    evt.DamageBundle.WeaponDamage.AddAlignment(Kingmaker.Enums.Damage.DamageAlignment.Good);
                }
            }
        }
    }
}
