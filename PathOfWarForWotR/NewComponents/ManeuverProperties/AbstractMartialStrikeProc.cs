using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.ManeuverProperties
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintAbility), false)]
    public abstract class AbstractMartialStrikeProc : BlueprintComponent
    {
        public abstract void DoProc(RuleAttackWithWeapon evt);

    }

    public class MartialStrikeProcActionsOnHit : AbstractMartialStrikeProc
    {
        public override void DoProc(RuleAttackWithWeapon evt)
        {
            using (ContextData<ContextAttackData>.Request().Setup(evt.AttackRoll, null))
            {
                using (evt.Reason.Context.GetDataScope(evt.Target))
                {
                    ActionList.Run();
                }
            }

            
        }
        public ActionList ActionList;
    }
}
