using Kingmaker.PubSubSystem;

namespace TheInfiniteCrusade.Backend.NewEvents
{
    interface ICombatStartedWhileCooledDownHandler : IUnitSubscriber
    {
        void OnCombatStartWhileCooledDown();
    }
}
