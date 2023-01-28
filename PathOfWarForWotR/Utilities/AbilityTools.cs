using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace PathOfWarForWotR.Utilities
{
    class AbilityTools
    {
        public static AbilityConfigurator MakeAbility(ModContextBase contextBase, string systemName, string displayName, string description, CommandType actionType, AbilityType type, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle, bool fullRound = false, Duration? duration = null)
        {
            var guid = contextBase.Blueprints.GetGUID(systemName);

            LocalizedString name = LocalizationTool.CreateString(systemName + ".Name", displayName, false);
            LocalizedString desc = LocalizationTool.CreateString(systemName + ".Desc", description, false);

            var config = AbilityConfigurator.New(systemName, guid.ToString()).SetDisplayName(name).SetDescription(desc);
            if (duration is not null)
            {
                config.SetLocalizedDuration(duration.Value);
            }
            config.SetActionType(actionType);
            if (actionType == CommandType.Standard && fullRound)
                config.SetIsFullRoundAction(true);

            config.SetType(type);
            config.SetAnimation(animationStyle);



            return config;


        }
    }
}
