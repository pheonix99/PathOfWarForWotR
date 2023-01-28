using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewComponents.AbilitySpecific
{
    class DWotICHealOnSpellResisted : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleSpellResistanceCheck>, IRulebookHandler<RuleSpellResistanceCheck>, ISubscriber, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleSpellResistanceCheck evt)
        {
            
        }

        public void OnEventDidTrigger(RuleSpellResistanceCheck evt)
        {
            if (evt.IsSpellResisted)
            {
               int? spellLevel = evt.Reason.Ability?.SpellLevel;
                if (spellLevel.HasValue)
                {
                    Rulebook.Trigger<RuleHealDamage>(new(Owner, Owner, spellLevel.Value * 2));
                }
            }    
        }
    }
}
