using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewUnitParts;
using PathOfWarForWotR.Extensions;

namespace PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem
{
    class AddMartialTrainingPartComponent : UnitFactComponentDelegate, IGlobalSubscriber, ISubscriber
    {
        public BlueprintManeuverBook ManeuverBook => m_ManeuverBook.Get();
        public BlueprintManeuverBookReference m_ManeuverBook;
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
            Owner.DemandManeuverBook(m_ManeuverBook);
            part.RegisterNonClassManeuverBook(m_ManeuverBook);

        }

        public override void OnTurnOff()
        {

            var part = base.Owner.Get<UnitPartMartialDisciple>();
            if (part != null)
                part.UnregisterNonClassManeuverBook(m_ManeuverBook);
        }
    }
}
