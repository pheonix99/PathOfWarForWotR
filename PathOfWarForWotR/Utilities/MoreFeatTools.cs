using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.NewComponents;

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
