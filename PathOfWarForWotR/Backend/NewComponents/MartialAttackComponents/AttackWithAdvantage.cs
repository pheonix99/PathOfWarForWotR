using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents
{
    class AttackWithAdvantage : AbstractMartialAttackWeaponModifier
    {
        public override void OnEventAboutToTrigger_RuleCalculateWeaponStats(RuleCalculateWeaponStats evt)
        {
            evt.AttackWithWeapon.AttackRoll.D20.AddReroll(1, true, evt.Reason.Ability.Fact);
        }
    }
}
