using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Backend.NewComponents.Prerequisites;

namespace TheInfiniteCrusade.NewContent.Feats.MartialFeats
{
    class ExtraReadiedManeuver
    {
        public static void MakeSelector()
        {
            var selector = Helpers.CreateBlueprint<BlueprintFeatureSelection>(Main.Context, "ExtraReadiedManeuverSelector", x =>
            {
                x.SetNameDescription(Main.Context, "Extra Readied Maneuver", "Choose a martial disciple class you have levels in. Your maneuvers readied for that class increases by one. If that class has granted maneuvers (such as the mystic), the number of maneuvers you are granted at the beginning of an encounter and when you recover your maneuvers also increases by one.");
                x.IsClassFeature = true;
                x.AddPrerequisite<PrerequisiteInitiator>();
                x.Groups = new Kingmaker.Blueprints.Classes.FeatureGroup[] { Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat, Kingmaker.Blueprints.Classes.FeatureGroup.Feat };

            });

            FeatTools.AddAsFeat(selector);


        }

       

    }
}
