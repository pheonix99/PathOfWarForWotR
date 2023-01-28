using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using System;
using PathOfWarForWotR.NewContent.MartialClasses;
using PathOfWarForWotR.NewContent.MartialSystem;
using PathOfWarForWotR.Utilities;

namespace PathOfWarForWotR.NewContent
{
    class ContentAdder
    {
        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch
        {
            static bool Initialized;

            [HarmonyPriority(Priority.First)]
            static void Postfix()
            {
                try
                {
                    ConstructionAssets.LoadGUIDS();
                    SystemLists.BuildSystemSpellLists();
                    SystemLists.BuildSystemSpellTables();

                    CommonBuffs.Build();

                    //Disciplines.BrokenBlade.BuildBrokenBlade();
                    //Disciplines.ElementalFlux.BuildElementalFlux();
                    //Disciplines.GoldenLion.BuildGoldenLion();
                    //Disciplines.PrimalFury.BuildPrimalFury();
                   // Disciplines.ScarletThrone.BuildScarletThrone();
                   // Disciplines.IronTortoise.Build();
                    Disciplines.SilverCrane.Build();
                    // MartialArchetypes.Myrmidon.BuildMyrmidon();
                    // MartialArchetypes.Polymath.MakePolymath();
                    // MartialArchetypes.PrimalDisciple.Make();
                    // Feats.MartialFeats.ExtraReadiedManeuver.MakeSelector();
                    Feats.MartialFeats.MartialTraining.Build();
                   // Mystic.Make();
                }
                catch (Exception ex)
                {
                    Main.Context.Logger.LogError(ex, "Error in early patch");
                }
            }
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch2
        {
            static bool Initialized;

            [HarmonyPriority(Priority.Last)]
            static void Postfix()
            {
                try
                {
                    ProcessProgressionDefinition.FinalRun();
                    Feats.MartialFeats.MartialTraining.Finish();
                    //Mystic.Finish();
                }
                catch (Exception ex)
                {
                    Main.Context.Logger.LogError(ex, "Error in final patch");
                }

            }
        }
    }
}
