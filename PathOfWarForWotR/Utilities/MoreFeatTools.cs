using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Localization;
using System.Linq;
using TabletopTweaks.Core.ModLogic;
using TheInfiniteCrusade.Backend.NewComponents;
using UnityEngine;

namespace TheInfiniteCrusade.Utilities
{
    public static class MoreFeatTools
    {
        public static AddFactOnlyParty CreateAddFactOnlyParty(BlueprintUnitFactReference fact, FeatureParam param = null)
        {
            var result = new AddFactOnlyParty();
            result.Feature = fact;
            result.Parameter = param;
            return result;
        }
        public static FeatureConfigurator MakeFeature(ModContextBase contextBase, string systemName, string displayName, string description, bool classFeature, bool hide = false, Sprite icon = null)
        {

            var guid = contextBase.Blueprints.GetGUID(systemName);

            LocalizedString name = LocalizationTool.CreateString(systemName + ".Name", displayName, false);
            LocalizedString desc = LocalizationTool.CreateString(systemName + ".Desc", description);

            var res = FeatureConfigurator.New(systemName, guid.ToString()).SetDisplayName(name).SetDescription(desc);
            if (icon != null)
            {
                res.SetIcon(icon);
            }
            res.SetIsClassFeature(classFeature);
            if (hide)
            {
                res.SetHideInCharacterSheetAndLevelUp(true);
                res.SetHideInUI(true);
            }
            return res;

        }

        public static void AddToProgressionLevels(this BlueprintProgression progression, int level, params BlueprintFeatureBaseReference[] refs)
        {
            var entry = progression.LevelEntries.FirstOrDefault(x => x.Level == level);
            if (entry == null)
            {
                progression.LevelEntries = progression.LevelEntries.Append(new LevelEntry { Level = level, m_Features = refs.ToList() }).ToArray();
            }
            else
            {
                entry.m_Features.AddRange(refs);
            }
        }

    }
}
