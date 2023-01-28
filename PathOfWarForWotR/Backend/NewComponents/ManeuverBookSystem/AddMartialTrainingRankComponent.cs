using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathOfWarForWotR.Backend.NewUnitParts;

namespace PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem
{
    class AddMartialTrainingRankComponent : UnitFactComponentDelegate, IGlobalSubscriber, ISubscriber
    {
       
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
            var part = base.Owner.Ensure<UnitPartMartialTraining>();
            part.RegisterFact(Fact);

        }

        public override void OnTurnOff()
        {
            var part = base.Owner.Get<UnitPartMartialTraining>();
            part?.UnregisterFact(Fact);
            
        }
    }
}
