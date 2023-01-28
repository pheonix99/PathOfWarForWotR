using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using PathOfWarForWotR.Backend.NewEvents;


namespace PathOfWarForWotR.Backend.NewComponents
{
    class OnCombatModeExpired : UnitBuffComponentDelegate
    {
        public override void OnRemoved()
        {
            if (!Owner.IsInCombat)
            {
                EventBus.RaiseEvent<IPostCombatCooldownHandler>(Owner, x =>
                {

                    x.OnPostCombatCooldown();
                });
            }

            
        }
    }
}
