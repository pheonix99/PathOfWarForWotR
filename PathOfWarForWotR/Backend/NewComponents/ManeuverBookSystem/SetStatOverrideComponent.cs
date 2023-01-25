using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Extensions;

namespace TheInfiniteCrusade.Backend.NewComponents.ManeuverBookSystem
{
    class SetStatOverrideComponent : UnitFactComponentDelegate, IGlobalSubscriber, ISubscriber
    {
        public StatType stat;
        public BlueprintManeuverBookReference m_book;

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
            var book = Owner.DemandManeuverBook(m_book);
            if (book != null)
            {
                book.SetStatOverride(stat);
            }



        }

        public override void OnTurnOff()
        {


        }
    }
}
