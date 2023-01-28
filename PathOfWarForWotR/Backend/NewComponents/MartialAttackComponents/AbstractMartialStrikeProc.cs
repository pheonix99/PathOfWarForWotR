using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.ContextData;

namespace PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintAbility), false)]
    public abstract class AbstractMartialStrikeProc : BlueprintComponent
    {
        public abstract void OnRuleAttackWithWeaponTrigger(RuleAttackWithWeapon evt);

    }

    public class MartialStrikeProcActionsOnHit : AbstractMartialStrikeProc
    {
        public override void OnRuleAttackWithWeaponTrigger(RuleAttackWithWeapon evt)
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
