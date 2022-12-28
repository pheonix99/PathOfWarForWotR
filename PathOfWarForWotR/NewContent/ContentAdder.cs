using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using TheInfiniteCrusade.NewContent.MartialSystem;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent
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
                ConstructionAssets.LoadGUIDS();
                SystemLists.BuildSystemSpellLists();
                SystemLists.BuildSystemSpellTables();

                CommonBuffs.Build();

                Disciplines.BrokenBlade.BuildBrokenBlade();
                Disciplines.ElementalFlux.BuildElementalFlux();
                Disciplines.GoldenLion.BuildGoldenLion();
                Disciplines.PrimalFury.BuildPrimalFury();
                Disciplines.ScarletThrone.BuildScarletThrone();
                Disciplines.IronTortoise.Build();
                Disciplines.SilverCrane.Build();
                MartialArchetypes.Myrmidon.BuildMyrmidon();
                MartialArchetypes.Polymath.MakePolymath();
                MartialArchetypes.PrimalDisciple.Make();
                Feats.MartialFeats.ExtraReadiedManeuver.MakeSelector();
            }
        }

        [HarmonyPatch(typeof(BlueprintsCache), "Init")]
        static class BlueprintsCache_Init_Patch2
        {
            static bool Initialized;

            [HarmonyPriority(Priority.Last)]
            static void Postfix()
            {
                
                ProcessProgressionDefinition.FinalRun();
                
            }
        }
    }
}
