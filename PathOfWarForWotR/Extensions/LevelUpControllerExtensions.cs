using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.CustomUI.UnitLogic.Class.LevelUp.Actions;

namespace TheInfiniteCrusade.Extensions
{
    public static class LevelUpControllerExtensions
    {
        public static void ApplyManeuverBook(this LevelUpController controller)
        {
            if (!controller.LevelUpActions.OfType<ApplyManeuverBook>().Any<ApplyManeuverBook>())
            {
                controller.AddAction(new ApplyManeuverBook(), false);
            }

        }

        

        [HarmonyPatch(typeof(LevelUpController), nameof(LevelUpController.ApplySpellbook))]
       
        static class LevelUpController_ApplySpellbook
        {
            static void Postfix(LevelUpController __instance)
            {

                __instance.ApplyManeuverBook();

            }
        }
    }
}
