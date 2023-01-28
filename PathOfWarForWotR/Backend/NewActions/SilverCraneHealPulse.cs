using Kingmaker;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewActions
{
    class SilverCraneHealPulse : ContextAction
    {
        public Feet Feet = new(30);

        public int dice = 1;
        public DiceType diceType = DiceType.D6;
        public bool useBonus = false;

        public override string GetCaption()
        {
            return "";
        }

        public override void RunAction()
        {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            var targets = GameHelper.GetTargetsAround(maybeCaster.Position, Feet).Where(unit => unit.IsAlly(maybeCaster));
            if (!targets.Any())
            {

                return;
            }
            targets = targets.Where(unit => !unit.Facts.HasComponent<SufferFromHealing>(fact => true));
            if (!targets.Any())
            {
                //Logger.Verbose("Skipped: Targets suffer from healing.");
                return;
            }
            DiceFormula die = new DiceFormula(dice, diceType);


            int bonus = 0;

            int max = dice * (int)diceType + bonus;

           
            foreach (var target in targets)
            {
                Rulebook.Trigger<RuleHealDamage>(new(maybeCaster, target, new DiceFormula(dice, diceType), bonus));
            }


           
        }
    }
}
