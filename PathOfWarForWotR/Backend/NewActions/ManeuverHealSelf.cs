using Kingmaker;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewActions
{
    class ManeuverHealSelf : ContextAction, IHealDamageProvider
    {
		// Token: 0x0600C964 RID: 51556 RVA: 0x003445F3 File Offset: 0x003427F3
		public override string GetCaption()
		{
			return string.Format("Heal {0} of hit point damage", this.Value);
		}

		// Token: 0x0600C965 RID: 51557 RVA: 0x00344608 File Offset: 0x00342808
		public override void RunAction()
		{
			if (base.Target.Unit == null)
			{
				PFLog.Default.Error(this, "Invalid target for effect '{0}'", new object[]
				{
					base.GetType().Name
				});
				return;
			}
			if (base.Context.MaybeCaster == null)
			{
				PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
				return;
			}
			RuleHealDamage healRule = this.GetHealRule(base.Context.MaybeCaster, base.Target.Unit);
			base.Context.TriggerRule<RuleHealDamage>(healRule);
		}

		// Token: 0x0600C966 RID: 51558 RVA: 0x003446A0 File Offset: 0x003428A0
		public RuleHealDamage GetHealRule(UnitEntityData initiator)
		{
			RuleHealDamage ruleHealDamage = new RuleHealDamage(initiator, initiator, new DiceFormula(this.Value.DiceCountValue.Calculate(base.Context), this.Value.DiceType), this.Value.BonusValue.Calculate(base.Context));
			ruleHealDamage.SourceFact = ContextDataHelper.GetFact();
			
			return ruleHealDamage;
		}

		// Token: 0x04008616 RID: 34326
		public ContextDiceValue Value;
	}
}
