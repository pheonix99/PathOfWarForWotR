using HarmonyLib;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;
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
