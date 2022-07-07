using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.NewComponents.UnitParts;

namespace TheInfiniteCrusade.NewComponents.ManeuverBookSystem
{
    public class DisciplineUnlockForInitiatorProgression : UnitFactComponentDelegate, IGlobalSubscriber, ISubscriber
    {
        public BlueprintProgression Progression => m_Progression.Get();

        public BlueprintProgressionReference m_Progression;

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
            part.RegisterClassUnlock(base.Fact, m_Progression, disciplineType);

        }

        public override void OnTurnOff()
        {
            var part = base.Owner.Get<UnitPartMartialDisciple>();
            part.UnregisterClassUnlock(base.Fact, m_Progression, disciplineType);

        }
    }
}
