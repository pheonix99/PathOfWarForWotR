using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace PathOfWarForWotR.Utilities
{
    class ResourceTools
    {
        public static AbilityResourceConfigurator Make(string sysname, string displayName )
        {
            var guid = Main.Context.Blueprints.GetGUID(sysname);
            var resource = AbilityResourceConfigurator.New(sysname, guid.ToString());
            resource.SetLocalizedName(LocalizationTool.CreateString(sysname + ".Name", displayName, false));

            return resource;
        }
    }
}
