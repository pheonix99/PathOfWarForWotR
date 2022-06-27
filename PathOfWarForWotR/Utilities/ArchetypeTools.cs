using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Utilities
{
    public static class ArchetypeTools
    {
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
    }
}
