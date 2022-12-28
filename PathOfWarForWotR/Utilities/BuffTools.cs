using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.ModLogic;
using UnityEngine;

namespace TheInfiniteCrusade.Utilities
{
    public static class BuffTools
    {
       

        public static BuffConfigurator MakeBuff(ModContextBase contextBase, string systemName, string displayName, string description, Sprite icon = null)
        {



            var guid = contextBase.Blueprints.GetGUID(systemName);



            LocalizedString name = LocalizationTool.CreateString(systemName + ".Name", displayName);
            LocalizedString desc = LocalizationTool.CreateString(systemName + ".Desc", description);


            var buff = BuffConfigurator.New(systemName, guid.ToString()).SetDisplayName(name).SetDescription(desc);

            if (icon != null)
            {
                buff.SetIcon(icon);

            }
            else
            {
                buff.AddToFlags(BlueprintBuff.Flags.HiddenInUi);
            }
            buff.SetFxOnStart(new());
            buff.SetFxOnRemove(new());
            //buff.ModifyFxOnStart(x => { });
            //buff.ModifyFxOnRemove(x => { });
            return buff;
        }
    }
}
