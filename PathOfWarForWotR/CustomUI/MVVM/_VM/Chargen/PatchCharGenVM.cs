//using HarmonyLib;
//using Kingmaker.UI.MVVM._VM.CharGen;
//using Kingmaker.UI.MVVM._VM.CharGen.Phases;
//using Kingmaker.UnitLogic.Abilities.Blueprints;
//using Owlcat.Runtime.UI.MVVM;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.Chargen.Phases.Maneuvers;
//using TheInfiniteCrusade.CustomUI.UnitLogic.Class.LevelUp;
//using TheInfiniteCrusade.Extensions;

//namespace TheInfiniteCrusade.CustomUI.MVVM._VM.Chargen
//{
//    public static class PatchCharGenVM
//    {
//        [HarmonyPatch(typeof(CharGenVM), nameof(CharGenVM.UpdateAllPhases))]
      
//        static class CharGenVM_UpdateAllPhases//This hijacking is for the spellbook display - just plain hijacking the level 1 display
//        {
//            static void Postfix(CharGenVM __instance)
//            {

//                __instance.UpdateManeuversPhases(__instance.m_LevelUpController.State.ManeuverSelections());

//            }
//        }

//        public static void UpdateManeuversPhases(this CharGenVM charGenVM, List<ManeuverSelectionData> maneuverSelections)
//        {
//            List<ManeuverSelectionData> spellsSelections2 = charGenVM.DefineVisibleManeuverSelections(maneuverSelections);
//            List<ManeuverSelectionData> selectionDataList = charGenVM.ClearFromExistingManeuverPhases(spellsSelections2);
//            charGenVM.CreateNewManeuverPhases(selectionDataList);
//        }

//        private static List<ManeuverSelectionData> DefineVisibleManeuverSelections(this CharGenVM charGenVM, List<ManeuverSelectionData> maneuverSelections)
//        {
//            List<ManeuverSelectionData> list = new List<ManeuverSelectionData>();
//            if (charGenVM.IsPregen)
//            {
               

//                foreach( var data in maneuverSelections)
//                {
//                    if (data.ExtraSelected.Any() && !CharGenManeuversPhaseVM.SelectionStateIsCompleted(charGenVM.m_LevelUpController.Unit, data))
//                    {
//                        list.Add(data);
//                    }
                    
//                }
//                return list;
//            }
//            else
//            {
//                foreach (var data in maneuverSelections)
//                {
//                    if (CharGenManeuversPhaseVM.SelectionStateHasSelections(data))
//                    {
//                        list.Add(data);
//                    }

//                }
//                return list;
//            }
//        }
    
//        private static List<ManeuverSelectionData> ClearFromExistingManeuverPhases(this CharGenVM charGenVM, List<ManeuverSelectionData> maneuverSelections)
//        {
//            if (!charGenVM.m_PhasesList.Any<CharGenPhaseBaseVM>())
//            {
//                return maneuverSelections;
//            }

//            List<CharGenManeuversPhaseVM> list = new List<CharGenManeuversPhaseVM>();
//            List<ManeuverSelectionData> list2 = new List<ManeuverSelectionData>(maneuverSelections);
//            foreach( var phase in charGenVM.m_PhasesList.OfType<CharGenManeuversPhaseVM>())
//            {
//                var relevantSelection = maneuverSelections.FirstOrDefault(x => phase.TryUpdateSamePhase(x));
//                if (relevantSelection == null)
//                {
//                    list.Add(phase);
//                }
//                else
//                {
//                    list2.Remove(relevantSelection);
//                }
//            }
//            foreach(var phase in list)
//            {
//                charGenVM.RemoveDisposable(phase);
//                phase.Dispose();
//                charGenVM.m_PhasesList.Remove(phase);
//            }
//            return list2;

//        }
    
//        private static void CreateNewManeuverPhases(this CharGenVM charGenVM, List<ManeuverSelectionData> maneuverSelectionData)
//        {
//            foreach(var data in maneuverSelectionData)
//            {
//                if (data.ExtraSelected.Any<BlueprintAbility>())
//                {
//                    CharGenManeuversPhaseVM phase = new CharGenManeuversPhaseVM(charGenVM.m_LevelUpController, data);
                   
//                    charGenVM.AddPhase(charGenVM.m_PhasesList, phase);
//                }
//                else
//                {
//                    CharGenManeuversPhaseVM phase3 = new CharGenManeuversPhaseVM(charGenVM.m_LevelUpController, data);
//                    charGenVM.AddPhase(charGenVM.m_PhasesList, phase3);
//                }
//            }
//        }
//    }
//}
