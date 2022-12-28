using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewUnitParts;

namespace TheInfiniteCrusade.Backend.NewComponents.ManeuverBookSystem
{
    public class DisciplineUnlockForInitiatorProgression : UnitFactComponentDelegate, IGlobalSubscriber, ISubscriber
    {
        public BlueprintProgression Progression => m_Progression.Get();

        public BlueprintProgressionReference m_Progression => bookRef.Get().GrantingProgression;

        public BlueprintManeuverBookReference bookRef;

        public string disciplineType;

        public override void OnActivate()
        {
            OnTurnOn();
        }

        public override void OnDeactivate()
        {
            OnTurnOff();
        }

        public override void OnTurnOn()
        {
           
            base.OnTurnOn();
            var part = base.Owner.Ensure<UnitPartMartialDisciple>();
            part.RegisterClassUnlock(base.Fact, bookRef, disciplineType);

        }

        public override void OnTurnOff()
        {
            var part = base.Owner.Get<UnitPartMartialDisciple>();
            part.UnregisterClassUnlock(base.Fact, bookRef, disciplineType);

        }
    }
}
