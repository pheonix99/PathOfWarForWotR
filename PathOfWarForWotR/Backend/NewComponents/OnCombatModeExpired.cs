using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using TheInfiniteCrusade.Backend.NewEvents;


namespace TheInfiniteCrusade.Backend.NewComponents
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
