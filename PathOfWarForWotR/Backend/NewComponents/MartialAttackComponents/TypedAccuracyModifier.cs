using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents
{
    class TypedAccuracyModifier : AbstractMartialAttackWeaponModifier
    {
        public int bonus = 0;
        public ModifierDescriptor modifierDescriptor = ModifierDescriptor.None;

        public override void OnEventAboutToTrigger_RuleCalculateWeaponStats(RuleCalculateWeaponStats evt)
        {
            evt.AddModifier(bonus, modifierDescriptor);
        }
    }
}
