using Kingmaker.PubSubSystem;

namespace PathOfWarForWotR.Backend.NewEvents
{
    interface ICombatStartedWhileCooledDownHandler : IUnitSubscriber
    {
        void OnCombatStartWhileCooledDown();
    }
}
