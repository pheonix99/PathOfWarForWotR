using Kingmaker.Blueprints.Classes;
using Kingmaker.RuleSystem.Rules.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.NewComponents.ManeuverProperties;
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
                }
            }
        }
    }
}
