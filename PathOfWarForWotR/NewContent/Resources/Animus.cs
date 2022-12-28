using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent.Resources
{
    class Animus
    {
        public static void Make()
        {
            BlueprintAbilityResource MakeAnimusResource()
            {
                var config = ResourceTools.Make("AnimusResource", "Animus");
                config.SetMax(100);
                config.SetUseMax(true);
              


                return config.Configure();
            }

        }
    }
}
