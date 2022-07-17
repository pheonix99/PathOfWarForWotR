//using HarmonyLib;
//using Kingmaker;
//using Kingmaker.UI.MVVM._PCView.CharGen;
//using Kingmaker.UI.MVVM._PCView.CharGen.Phases;
//using Kingmaker.UI.MVVM._PCView.CharGen.Phases.Spells;
//using Kingmaker.UI.MVVM._VM.CharGen.Phases;
//using Kingmaker.UI.MVVM._VM.CharGen.Phases.Portrait;
//using Owlcat.Runtime.UI.Utility;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheInfiniteCrusade.CustomUI.MVVM._PCView.Chargen.Phases.Maneuvers;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.Chargen.Phases.Maneuvers;
//using UnityEngine;

//namespace TheInfiniteCrusade.Extensions
//{
//    static class CharGenRoadmapMenuViewExtensions
//    {
//        [HarmonyPatch(typeof(CharGenRoadmapMenuView), MethodType.Constructor)]
//        static class CharGenRoadmapMenuView_Constructor
//        {
//            static void Postfix(CharGenRoadmapMenuView __instance)
//            {
//                Main.Context.Logger.Log($"CharGenRoadmapMenuView Postfix");
//                GameObject testobj = new GameObject("CharGenManeuversPhaseRoadmapPCView", typeof(RectTransform));
//                ManeuversPhaseRoadmapPcView = testobj.AddComponent<CharGenManeuversPhaseRoadmapPCView>();

//                ManeuversPhaseRoadmapPcView.actuallyBuild();
//                if (__instance.RacePhaseRoadmapPcView != null)
//                {
//                    Main.Context.Logger.Log($"RacePhaseRoadmapPcView found");
//                }
//            }
//        }

//        private static CharGenManeuversPhaseRoadmapPCView ManeuversPhaseRoadmapPcView { get; set; }

        

//        [HarmonyPatch(typeof(CharGenRoadmapMenuView), nameof(CharGenRoadmapMenuView.GetRoadmapPhaseView))]
//        static class CharGenRoadmapMenuView_GetRoadmapPhaseView
//        {

            
//            static bool Prefix(ref ICharGenPhaseRoadmapView __result, CharGenRoadmapMenuView __instance, CharGenPhaseBaseVM phaseVM)
//            {
//                if (phaseVM != null && phaseVM is CharGenPortraitPhaseVM race)
//                {
                    

//                    Main.Context.Logger.Log($"m_ButtonBackground name on race is {__instance.RacePhaseRoadmapPcView.m_ButtonBackground.name}");
//                    Main.Context.Logger.Log($"m_button name on race is {__instance.RacePhaseRoadmapPcView.m_Button.name}");
//                    Main.Context.Logger.Log($"m_Label name on race is {__instance.RacePhaseRoadmapPcView.m_Label.name}");
//                    Main.Context.Logger.Log($"m_LabelLayoutElement name on race is {__instance.RacePhaseRoadmapPcView.m_LabelLayoutElement.name}");
                 
//                    Main.Context.Logger.Log($"m_LabelLayoutElement name on race is {__instance.RacePhaseRoadmapPcView.m_LabelLayoutElement.name}");
//                    Main.Context.Logger.Log($"m_LabelLayoutElement name on race is {__instance.RacePhaseRoadmapPcView.m_LabelLayoutElement.name}");
//                    if (__instance.RacePhaseRoadmapPcView.HasLabels)
//                    {
//                        Main.Context.Logger.Log($"m_ButtonLabel name on race is {__instance.RacePhaseRoadmapPcView.m_ButtonLabel.name}");
//                    }
//                    else
//                    {
//                        Main.Context.Logger.Log($"m_ButtonLabel name on race is isn't ready");
//                    }

//                }



//                if (phaseVM != null && phaseVM is CharGenManeuversPhaseVM maneuversPhaseVM)
//                {
//                    try
//                    {

//                        Main.Context.Logger.Log($"m_ButtonBackground name on race is {__instance.RacePhaseRoadmapPcView.m_ButtonBackground.name}");
//                        Main.Context.Logger.Log($"m_button name on race is {__instance.RacePhaseRoadmapPcView.m_Button.name}");
//                        if (__instance.RacePhaseRoadmapPcView.HasLabels)
//                        {
//                            Main.Context.Logger.Log($"m_ButtonLabel name on race is {__instance.RacePhaseRoadmapPcView.m_ButtonLabel.name}");
//                        }
//                        else
//                        {
//                            Main.Context.Logger.Log($"m_ButtonLabel name on race is isn't ready");
//                        }





//                        /*
//                         * names ChargenSpellsRoadmapView
//                         * 
//                         * 
//                         */
//                        try
//                        {
//                            if (ManeuversPhaseRoadmapPcView.transform == null)
//                            {

//                                Main.Context.Logger.Log("ManeuversPhaseRoadmapPcView transform is null before enterring ");
//                            }
//                        }
//                        catch
//                        {
//                            Main.Context.Logger.Log("ManeuversPhaseRoadmapPcView transform is null before enterring and nullreffing on .transform = null ");
//                        }
//                        foreach (var obj in ManeuversPhaseRoadmapPcView.GetComponentsInChildren<GameObject>())
//                        {
//                            Main.Context.Logger.Log($"Child named {obj.name} found in ManeuversPhaseRoadmapPcView");
//                        }


//                        CharGenManeuversPhaseRoadmapPCView maneuversPcView = WidgetFactory.GetWidget<CharGenManeuversPhaseRoadmapPCView>(ManeuversPhaseRoadmapPcView, true);
//                        Main.Context.Logger.Log("Constructor Clear");
//                        maneuversPcView.Initialize();
//                        Main.Context.Logger.Log("Initialize Clear");
//                        maneuversPcView.Bind(maneuversPhaseVM);
//                        Main.Context.Logger.Log("Bind Clear");
//                        maneuversPhaseVM.OnDispose += delegate ()
//                        {
//                            __instance.RetrieveRoadmapView(maneuversPcView);
//                        };
                        
//                        __result = (maneuversPcView as ICharGenPhaseRoadmapView);
//                        if (__result == null)
//                        {
//                            Main.Context.Logger.LogError($"result of custom getroadmapview is null");
//                        }
//                        else
//                        {
//                            Main.Context.Logger.Log($"Result of custom getroadmapview is not null");
//                            try
//                            {
//                                if (maneuversPcView.gameObject.transform != null)
//                                {
//                                    Main.Context.Logger.Log($"maneuversPcView.gameObject.transform is not null");
//                                    Main.Context.Logger.Log($"Sibling INdex: {maneuversPcView.gameObject.transform.GetSiblingIndex()}");
//                                }
//                                else
//                                {
//                                    Main.Context.Logger.Log($"maneuversPcView.gameObject.transform is null");
//                                }

//                            }
//                            catch (Exception e)
//                            {
//                                Main.Context.Logger.LogError($"Custom VM error in getroadmapview: {e}");
//                            }
//                        }
//                    }
//                    catch (Exception e)
//                    {
//                        Main.Context.Logger.LogError($"Custom VM error in getroadmapview: {e}");
//                    }
//                    Main.Context.Logger.Log($"GetRoadmapPhaseView result is {(__result != null ? "not" : "")} null immediately before prefix return");
//                    return false;
//                }

//                return true;

//            }
//        }
//    }
//}
