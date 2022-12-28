using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using System;
using TheInfiniteCrusade.Extensions;

namespace TheInfiniteCrusade.CustomUI.UnitLogic.Class.LevelUp.Actions
{
    class ApplyManeuverBook : ILevelUpAction
    {
        public LevelUpActionPriority Priority
        {
            get
            {
                return LevelUpActionPriority.ApplySpellbook;
            }
        }

        public bool NeedUpdateUnitView => throw new NotImplementedException();

        public void Apply([NotNull] LevelUpState state, [NotNull] UnitDescriptor unit)
        {
            if (state.SelectedClass == null)
            {
                return;
            }
            //SkipLevelsForSpellProgression is inapplicable
            ClassData classData = unit.Progression.GetClassData(state.SelectedClass);
            if (classData == null)
            {
                return;
            }
            if (classData.ManeuverBook() != null)
            {
                var maneuverBook = unit.DemandManeuverBook(classData.ManeuverBook());

                //We don't actually need to do anything else because we handle level up spell gainz in ManeuverSelectionFeature, level up slot gains in ManeuverBook and casterLevel is a unitproperty
            }
        }

        public bool Check([NotNull] LevelUpState state, [NotNull] UnitDescriptor unit)
        {
            return true;
        }

        public void PostLoad()
        {
            
        }


        [HarmonyPatch(typeof(LevelUpController), nameof(LevelUpController.SelectDefaultClassBuild))]
        
        static class LevelUpController_SelectDefaultClassBuild
        {
            static void Postfix(LevelUpController __instance)
            {

                __instance.RemoveAction<ApplyManeuverBook>();//Try this before transpilin
            }
        }
    }
}
