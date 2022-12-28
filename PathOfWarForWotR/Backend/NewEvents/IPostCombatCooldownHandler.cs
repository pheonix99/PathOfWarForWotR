using Kingmaker.PubSubSystem;

namespace TheInfiniteCrusade.Backend.NewEvents
{
    public interface IPostCombatCooldownHandler : IUnitSubscriber
    {
        void OnPostCombatCooldown();

    }
}
