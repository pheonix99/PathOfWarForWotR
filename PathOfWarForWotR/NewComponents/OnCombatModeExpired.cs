using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewEvents;
using TheInfiniteCrusade.NewComponents.UnitParts;

namespace TheInfiniteCrusade.NewComponents
{
    class OnCombatModeExpired : UnitBuffComponentDelegate
    {
        public override void OnRemoved()
        {
            if (!Owner.IsInCombat)
            {
                EventBus.RaiseEvent<IPostCombatCooldownHandler>(Owner, x =>
                {

                    x.OnPostCombatCooldown();
                });
            }

            
        }
    }
}
