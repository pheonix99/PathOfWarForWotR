using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Extensions;

namespace TheInfiniteCrusade.Backend.NewActions
{
    class SilverCraneSingleTargetHeal : ContextAction
    {
        public override string GetCaption()
        {
            return "";
        }
        public Feet Feet = new(30);

        public int dice = 1;
        public DiceType diceType = DiceType.D6;
        public bool useBonus = true;


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

            var effectTarget = targets.First();
            foreach (var target in targets)
            {
                if (effectTarget.Damage < max && effectTarget.Damage < target.Damage)
                {
                    // Change to the target which receives more healing
                    effectTarget = target;
                }
                else if (target.Damage > 0 && target.CurrentHP() < effectTarget.CurrentHP())
                {
                    // Change to the target with the lowest health
                    effectTarget = target;
                }
            }
            

            Rulebook.Trigger<RuleHealDamage>(new(maybeCaster, effectTarget, new DiceFormula(dice, diceType), bonus));
            
        }


    }
}
