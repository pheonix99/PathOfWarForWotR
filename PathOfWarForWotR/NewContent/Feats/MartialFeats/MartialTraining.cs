using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace TheInfiniteCrusade.NewContent.Feats.MartialFeats
{
    class MartialTraining
    {
        public static void Build()
        {
            var l1 = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MartialTraining1Feature", x =>
            {
                x.SetNameDescription(Main.Context, "Martial Training I", "Select a martial discipline. The associated skill for this discipline is now a class skill. Your initiation modifier is chosen from Intelligence, Wisdom, or Charisma. Your martial initiator level maneuvers granted by this feat (and subsequent Martial Training feats) is equal to half your character level + your initiation modifier. You may select any two maneuvers from the 1st level maneuvers from this discipline, and you may ready one of your maneuvers for use. You may recover one maneuver by expending a full round action to recover it./nSpecial: If you ever gain levels in a martial adept class or possess them previously, these maneuvers continue to use their own initiator level and recovery method, independent of your martial adept level. Those wishing to add new maneuvers from a discipline that is already available to their class should instead select the Advanced Study feat instead.");
                x.IsClassFeature = true;
                x.Groups = new FeatureGroup[] { FeatureGroup.Feat, FeatureGroup.CombatFeat };
                x.AddComponent<PrerequisiteStatValue>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus;
                    x.Group = Prerequisite.GroupType.Any;
                    x.Value = 3;

                });
                

            });



        }


    }
}
