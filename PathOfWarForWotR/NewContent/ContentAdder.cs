using HarmonyLib;
using Kingmaker.Blueprints.JsonSystem;
using PathOfWarForWotR.NewContent.MartialSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                SystemLists.BuildMasterSpellLists();



                Disciplines.BrokenBlade.BuildBrokenBlade();

                MartialArchetypes.Myrmidon.BuildMyrmidon();
            }
        }
    }
}
