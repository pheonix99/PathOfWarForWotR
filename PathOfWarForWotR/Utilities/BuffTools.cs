using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.Utilities;

namespace TheInfiniteCrusade.Utilities
{
    public static class BuffTools
    {
        public static BlueprintBuff MakeBuff(ModContextBase contextBase, string name, Action<BlueprintBuff> action)
        {
            return Helpers.CreateBlueprint<BlueprintBuff>(contextBase, name, x =>
            {
                x.FxOnStart = new();
                x.FxOnRemove = new();
                action.Invoke(x);

            });
        }
    }
}
