using HarmonyLib;
using Kingmaker;
using Kingmaker.UI.MVVM._PCView.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.Utility;
using PathOfWarForWotR.Backend.NewUnitParts;
using PathOfWarForWotR.CustomUI.ManeuverBookUI;
using PathOfWarForWotR.EnumHacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.CustomUI.InsertIntoExisting
{
    [HarmonyPatch(typeof(ActionBarGroupPCView))]
    class HackActionBarGroupPCView
    {
        [HarmonyPatch(nameof(ActionBarGroupPCView.SetVisible)), HarmonyPrefix]
        static void SetVisible(ActionBarGroupPCView __instance, bool state, bool force)
        {
            if (__instance.m_TogglableChildren == null)
                __instance.m_TogglableChildren = new();
        }

        [HarmonyPatch(nameof(ActionBarGroupPCView.SetVisible)), HarmonyPostfix]
        static void SetVisible2(ActionBarGroupPCView __instance, bool state, bool force)
        {
        }

        [HarmonyPatch(nameof(ActionBarGroupPCView.BindViewImplementation)), HarmonyPrefix]
        static void BindViewImplementation(ActionBarGroupPCView __instance)
        {
            Main.Context.Logger.Log($"BindViewImplementation called for ActionBarGroupPCView: {__instance.name}");
            


        }

        [HarmonyPatch(nameof(ActionBarGroupPCView.SetStatePosition)), HarmonyPrefix]
        static void SetStatePosition(ActionBarGroupPCView __instance, bool force)
        {
            Main.Context.Logger.Log($"SetStatePosition callled for {__instance.name}");
        }

        [HarmonyPatch(nameof(ActionBarGroupPCView.DrawSlots)), HarmonyPrefix]
        static void DrawSlots(ActionBarGroupPCView __instance, List<ActionBarSlotVM> slotVms)
        {
            Main.Context.Logger.Log($"DrawSlots Prefix called for {__instance.name}, activation is {__instance.gameObject.activeSelf}");
        }

        [HarmonyPatch(nameof(ActionBarGroupPCView.DrawSlots)), HarmonyPostfix]
        static void DrawSlots2(ActionBarGroupPCView __instance, List<ActionBarSlotVM> slotVms)
        {
           
            if (__instance is ActionBarManeuverGroupPCView && !slotVms.Empty())
            {
                __instance.gameObject.SetActive(true);//TODO make sure this isn't firing on non-initiators
               
            }
            else
            {
                
            }
        }

        [HarmonyPatch(nameof(ActionBarGroupPCView.AddEmptySlots)), HarmonyPostfix]
        static void AddEmptySlots(ActionBarGroupPCView __instance, List<ActionBarSlotVM> slotVms)
        {
          
            
            
        }

        [HarmonyPatch(nameof(ActionBarGroupPCView.GetGroup)), HarmonyPostfix]
        static void GetGroup(ActionBarGroupPCView __instance, ref List<ActionBarSlotVM> __result )
        {
            //probably need to 
         
            if (__instance.m_GroupType == ActionBarGroupType_EXT.Manuevers)
            {
                

                if (__instance.ViewModel.GroupManevers().Count == 0)
                {
                   
                    if (Game.Instance.SelectionCharacter.CurrentSelectedCharacter?.Get<UnitPartMartialDisciple>() != null)
                    {
                        __instance.ViewModel.CollectManeuvers(Game.Instance.SelectionCharacter.CurrentSelectedCharacter);
                        Main.Context.Logger.Log($"GetGroup called for maneuvers on {__instance.name}, manuever count was zero, patching to {__instance.ViewModel.GroupManevers().Count}");
                    }
                    else
                    {
                        Main.Context.Logger.Log($"GetGroup called for maneuvers on {__instance.name}, manuever count was zero, not patching");
                    }
                    

                }
                else
                {
                    Main.Context.Logger.Log($"GetGroup called for maneuvers on {__instance.name}, manuever counts is {__instance.ViewModel.GroupManevers().Count}");
                }

                __result.AddRange(__instance.ViewModel.GroupManevers());
            }
            else
            {
                Main.Context.Logger.Log($"GetGroup called for {__instance.m_GroupType} on {__instance.name}");

            }

        }

        [HarmonyPatch(nameof(ActionBarGroupPCView.Initialize)), HarmonyPrefix]
        static void Initialize(ActionBarGroupPCView __instance, ActionBarGroupType type, List<ActionBarGroupPCView> neighbors)
        {
            Main.Context.Logger.Log($"Initalize called for ActionBarGroupPCView: {__instance.name}");
            if (HackActionBarPCView.m_manuverView == null)
            {
                Main.Context.Logger.Log("HackActionBarPCView.m_manuverView is null at ActionBarGroupPCView.Initialize ");
            }
            else
            {
                if (!neighbors.Contains(HackActionBarPCView.m_manuverView) && __instance != HackActionBarPCView.m_manuverView)
                {

                    //neighbors.Add(HackActionBarPCView.m_manuverView);
                }
            }
        }
    }
}
