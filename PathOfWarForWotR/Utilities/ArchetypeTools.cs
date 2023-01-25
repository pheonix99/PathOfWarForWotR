using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Localization;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.ModLogic;
using TheInfiniteCrusade.Backend.NewComponents.ModifyArchetypesSmartly;
using UnityEngine;

namespace TheInfiniteCrusade.Utilities
{
    public static class ArchetypeTools
    {
        public static ArchetypeConfigurator MakeArchetype(ModContextBase context, string systemName, string displayName, string description, BlueprintCharacterClass parentClass)
        {
            var guid = context.Blueprints.GetGUID(systemName);

            LocalizedString name = LocalizationTool.CreateString(systemName + ".Name", displayName);
            LocalizedString desc = LocalizationTool.CreateString(systemName + ".Desc", description);
            var config = ArchetypeConfigurator.New(systemName, guid.ToString());
            config.SetLocalizedName(name);
            config.SetLocalizedDescription(desc);
            config.SetLocalizedDescriptionShort(desc);


            config.SetParentClass(parentClass);
            return config;
        }

        public static BlueprintFeature MakeSpellsPerDayChangeFeature(ModContextBase contextBase, BlueprintArchetypeReference archetype, string sourceArchetypeDisplayName, string sourceClassDisplayName, string description, int change = -1, Sprite icon = null)
        {
            var classObj = archetype.Get().GetParentClass();
            string sysName = classObj.Name.Replace("Class", "") + archetype.NameSafe().Replace("Archetype", "") + "ReducedSpellcastingFeature";

            var feature = MoreFeatTools.MakeFeature(contextBase, sysName, $"{sourceArchetypeDisplayName} Reduced Spellcasting: {sourceClassDisplayName}", description, true, false, icon);
            feature.AddComponent<AllLevelsSpellSlotShift>(x =>
            {
                x.Amount = change;
                x.ClassReference = classObj.ToReference<BlueprintCharacterClassReference>();
            });
            var made = feature.Configure();

            //BuildContent.CastReducers.Add(made.ToReference<BlueprintFeatureReference>());

            return made;

        }

        public static void AddToRemoveFeatures(this BlueprintArchetype archetype, int level, BlueprintFeatureBaseReference feature )
        {
            var entry = archetype.RemoveFeatures.FirstOrDefault(x => x.Level == level);
            if (entry == null)
            {
                archetype.RemoveFeatures = archetype.RemoveFeatures.Append(new LevelEntry
                {
                    Level = level,
                    m_Features = new List<BlueprintFeatureBaseReference>() { feature }
                }).ToArray();
            }
            else
            {
                entry.m_Features.Add(feature);
            }
        }
        public static void RemoveFeatureFromAllLevels(this BlueprintArchetype archetype, BlueprintFeatureBaseReference reference)
        {
            foreach (LevelEntry l in archetype.GetParentClass().Progression.LevelEntries)
            {
                if (l.Features.Contains(reference))
                {
                    AddToRemoveFeatures(archetype, l.Level, reference);
                }
            }
        }
        public static void AddToAddFeatures(this BlueprintArchetype archetype, int level, BlueprintFeatureBaseReference feature)
        {
            var entry = archetype.AddFeatures.FirstOrDefault(x => x.Level == level);
            if (entry == null)
            {
                archetype.AddFeatures = archetype.AddFeatures.Append(new LevelEntry
                {
                    Level = level,
                    m_Features = new List<BlueprintFeatureBaseReference>() { feature }
                }).ToArray();
            }
            else
            {
                entry.m_Features.Add(feature);
            }
        }
    }
}
