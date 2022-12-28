using Kingmaker.RuleSystem.Rules.Damage;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.Backend.NewComponents.MartialAttackComponents
{
    class SilverCraneStrikeIncorporeal : AbstractMartialAttackWeaponModifier
    {

        public override void ModifyWeaponStats(RulePrepareDamage evt)
        {
            var condition = ManeuverTools.SilverCraneSpecialTarget().Build();

            if (evt.Target != null)
            {
                using (evt.Reason.Context.GetDataScope(evt.Target))
                {
                    if (condition.Check())
                    {
                        evt.DamageBundle.WeaponDamage.Reality |= Kingmaker.Enums.Damage.DamageRealityType.Ghost;
                       
                    }
                    evt.DamageBundle.WeaponDamage.AddAlignment(Kingmaker.Enums.Damage.DamageAlignment.Good);
                }
            }
        }
    }
}
