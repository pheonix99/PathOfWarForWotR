using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.Utility;
using System;
using TheInfiniteCrusade.Backend.NewComponents;
using TheInfiniteCrusade.Backend.NewUnitParts;
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
            if (classData.CharacterClass.IsMythic || classData.CharacterClass.IsHigherMythic)
                return;
            if (classData.ManeuverBook() != null)
            {
                unit.Ensure<UnitPartMartialDisciple>();
                unit.DemandManeuverBook(classData.ManeuverBook());
            }
            unit.ManeuverBooks().ForEach(x =>
            {
                if (!x.Blueprint.IsMartialTraining)
                {
                    x.AddLevelFromClass(classData);
                }
            });
            
            
            
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
