using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.Extensions
{
    public static class PoWBlueprintExtensions
    {
        public static bool HasComponent<T> (this BlueprintScriptableObject bp )
        {
            return bp.Components.OfType<T>().Any();
        }

        public static bool HasComponent<T>(this BlueprintScriptableObject bp, Predicate<T> func)
        {
            return bp.Components.OfType<T>().Any(x=> func.Invoke(x));
        }
    }
}
