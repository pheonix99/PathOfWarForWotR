using Kingmaker.PubSubSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.Backend.NewEvents
{
    public interface IPostCombatCooldownHandler : IUnitSubscriber
    {
        void OnPostCombatCooldown();

    }
}
