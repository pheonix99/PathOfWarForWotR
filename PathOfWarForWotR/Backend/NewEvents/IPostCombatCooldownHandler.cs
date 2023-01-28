using Kingmaker.PubSubSystem;

namespace PathOfWarForWotR.Backend.NewEvents
{
    public interface IPostCombatCooldownHandler : IUnitSubscriber
    {
        void OnPostCombatCooldown();

    }
}
