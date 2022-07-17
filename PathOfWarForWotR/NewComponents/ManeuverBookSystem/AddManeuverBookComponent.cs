using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewEvents;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.Extensions;

namespace TheInfiniteCrusade.NewComponents.ManeuverBookSystem
{
    [AllowedOn(typeof(BlueprintFeature), true)]
    public class AddManeuverBookComponent : UnitFactComponentDelegate, IUnitSubscriber, ISubscriber, IPostCombatCooldownHandler, ICombatStartedWhileCooledDownHandler
    {
        public BlueprintManeuverBook Book => m_Book.Get();
        public BlueprintManeuverBookReference m_Book;

        public void OnCombatStartWhileCooledDown()
        {
            Owner.DemandManeuverBook(Book).OnCombatStartWhileCooledDown();
        }

        public void OnPostCombatCooldown()
        {
           
            Owner.DemandManeuverBook(Book).RechargeBookOnCombatEnd();
        }
    }
}
