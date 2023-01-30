using HarmonyLib;
using Kingmaker.UI.MVVM._PCView.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
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
            if (__instance.VisibleState != state)
            {
                Main.Context.Logger.Log($"SetVisible interestingly called for {__instance.name}, state is {state}, force is {force}");
            }
            else
            {
                Main.Context.Logger.Log($"SetVisible called for {__instance.name}, state is {state}, force is {force}");
            }

           
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
