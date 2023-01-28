using Kingmaker;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathOfWarForWotR.Backend.NewActions
{
    class ContextActionWhirlwind : ContextAction
    {
        public override string GetCaption()
        {
            return "Whirlwind Move";
        }

        public override void RunAction()
        {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            AbilityData ability = Context.SourceAbilityContext.Ability;
            if (ability == null)
            {
                PFLog.Default.Error("Ability is missing", Array.Empty<object>());
                return;
            }
            var reach = maybeCaster.Body.PrimaryHand.Weapon.AttackRange.Meters + maybeCaster.Corpulence;

            IEnumerable<UnitEntityData> source = GameHelper.GetTargetsAround(maybeCaster.EyePosition, new Kingmaker.Utility.Feet(reach), true, false).Where(new Func<UnitEntityData, bool>(Context.MaybeCaster.IsEnemy));
            foreach (var v in source)
            {

                using (Context.GetDataScope(v))
                {
                    WhirlwindThis.Run();
                }

            }

        }

        public ActionList WhirlwindThis;
    }
}
