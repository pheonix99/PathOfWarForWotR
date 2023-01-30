using HarmonyLib;
using Kingmaker;
using Kingmaker.UI.MVVM._PCView.ActionBar;
using Owlcat.Runtime.UI.Controls.Button;
using Owlcat.Runtime.UI.Controls.Other;
using Owlcat.Runtime.UI.Utility;
using PathOfWarForWotR.CustomUI.ManeuverBook;
using PathOfWarForWotR.EnumHacks;
using PathOfWarForWotR.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TMPro;
using UniRx;
using UnityEngine;

namespace PathOfWarForWotR.CustomUI.InsertIntoExisting
{




    [HarmonyPatch(typeof(ActionBarPCView))]
    static class HackActionBarPCView
    {
      
        public static ActionBarGroupPCView m_manuverView;

       

        public static ActionBarGroupPCView GetActionBarManeuverGroupPCView(this ActionBarPCView actionBarPCView)
        {
            return m_manuverView;
        }



        [HarmonyPatch("Initialize"), HarmonyPrefix]
        static void Initialize(ActionBarPCView __instance)
        {
            Main.Context.Logger.Log("Initalize Prefix called for ActionBarPCView");
            try
            {
                if (m_manuverView is not null)
                {
                    //__instance.m_SpellsGroup.m_Neighbours.Add(m_manuverView);
                    //__instance.m_AbilityGroup.m_Neighbours.Add(m_manuverView);
                    //__instance.m_ItemsGroup.m_Neighbours.Add(m_manuverView);
                }


            }
            catch (Exception ex)
            {
                Main.Context.Logger.LogError(ex, "Initialize maneuvers menu");
            }

        }

        [HarmonyPatch("Initialize"), HarmonyPostfix]
        static void Initialize2(ActionBarPCView __instance)
        {
            Main.Context.Logger.Log("Initalize Postfix called for ActionBarPCView");
            try
            {
                

                if (m_manuverView is not null)
                {
                    //__instance.m_SpellsGroup.m_Neighbours.Add(m_manuverView);
                    //__instance.m_AbilityGroup.m_Neighbours.Add(m_manuverView);
                    //__instance.m_ItemsGroup.m_Neighbours.Add(m_manuverView);
                }


            }
            catch (Exception ex)
            {
                Main.Context.Logger.LogError(ex, "Initialize maneuvers menu");
            }

        }


        [HarmonyPatch("BindViewImplementation"), HarmonyPrefix]
        static void BindViewImplementation(ActionBarPCView __instance)
        {
            try
            {
                Main.Context.Logger.Log("BindViewImplementation prefix rung on ActionBarPCView");
                var popouts = __instance.GetComponentsInChildren<ActionBarGroupPCView>();
                var abilityPopout = popouts.First(x => x is not ActionBarSpellGroupPCView);

                var groupsOBJ = abilityPopout.transform.parent;
                var otherButtons = groupsOBJ.GetComponentsInChildren<ActionBarGroupPCView>();
                var clone = GameObject.Instantiate(abilityPopout.gameObject, groupsOBJ);
                var extant = clone.GetComponent<ActionBarGroupPCView>();
                var extantslot = extant.m_Slot;
                var slotcontainter = extant.m_SlotContainer;
                var button = extant.m_SwitchButton;
                var label = extant.m_GroupNameLabel;
                clone.DestroyComponents<ActionBarGroupPCView>();
                clone.AddComponent<ActionBarManeuverGroupPCView>();
                m_manuverView = clone.GetComponent<ActionBarManeuverGroupPCView>();
                m_manuverView.m_Slot = extant.m_Slot;
                m_manuverView.m_SlotContainer = slotcontainter;
                m_manuverView.m_SwitchButton = button;
                m_manuverView.m_GroupNameLabel = label;

                //Do faux initiatialize
                m_manuverView.m_Neighbours = new();
                foreach (var v in otherButtons)
                {
                    Main.Context.Logger.Log($"About to hook {v.name} to maneuver popout");
                    if (v.m_Neighbours == null)
                    {
                        Main.Context.Logger.Log($"{v.name}.m_neighbors is null");
                    }
                    if (m_manuverView is null)
                    {
                        Main.Context.Logger.Log($"cloneview is null is null");
                    }
                    else if (m_manuverView.m_Neighbours is null)
                    {
                        m_manuverView.m_Neighbours = new();
                        Main.Context.Logger.Log($"cloneview.m_neighbors is null");
                    }
                    if (!v.m_Neighbours.Contains(m_manuverView))
                    {
                        v.m_Neighbours.Add(m_manuverView);
                    }
                    m_manuverView.m_Neighbours.Add(v);
                }
                m_manuverView.m_GroupType = ActionBarGroupType_EXT.Manuevers;
                //m_manuverView.m_GroupType = Kingmaker.UI.MVVM._VM.ActionBar.ActionBarGroupType.Ability;
                clone.transform.Find("Background/OpenCloseButton/Background/Letter").GetComponent<TextMeshProUGUI>().SetText("M");
                clone.transform.Find("Background/Header/HeaderText").GetComponent<TextMeshProUGUI>().SetText("Maneuvers");
                clone.transform.localPosition = new Vector3(-270, clone.transform.localPosition.y, clone.transform.localPosition.z);
                for (int i = 0; i < m_manuverView.GetMinSlotsCount(); i++)
                {
                    ActionBarBaseSlotPCView widget = WidgetFactory.GetWidget<ActionBarBaseSlotPCView>(m_manuverView.m_Slot, true, false);
                    widget.transform.SetParent(m_manuverView.m_SlotContainer, false);
                    widget.Initialize();
                    widget.CanvasGroup.alpha = 0f;
                    m_manuverView.m_SlotsList.Add(widget);
                }

                Main.Context.Logger.Log($"m_manuverView was set: {m_manuverView is not null}");
                
                clone.name = "ManueverGroupView";

                Main.Context.Logger.Log("Passed faux initialize");

                m_manuverView.Bind(__instance.ViewModel);

                m_manuverView.ClearSlots();
                //m_manuverView.BindViewImplementation();

                /*
                m_manuverView.SetGroup();
                m_manuverView.AddDisposable(m_manuverView.ViewModel.OnUnitUpdated.Subscribe(delegate (Unit _)
                {
                    m_manuverView.SetGroup();
                }));
                m_manuverView.AddDisposable(m_manuverView.m_SwitchButton.OnLeftClickAsObservable().Subscribe(delegate (Unit _)
                {
                    m_manuverView.SetVisible(!m_manuverView.VisibleState, false);
                }));
               (/


                //clone.gameObject.SetActive(Game.Instance.SelectionCharacter.SelectedUnit.Value.Value.HasMartialStuff());

                
                //spellview.m_Neighbours.Add(clone.GetComponent<ActionBarGroupPCView>());
                


            }
            catch (Exception ex)
            {
                Main.Context.Logger.LogError(ex, "injecting maneuvers menu");
            }

        }

        [HarmonyPatch("BindViewImplementation"), HarmonyPostfix]
        static void BindViewImplementation2(ActionBarPCView __instance)
        {
            try
            {
                /*
                var spellview = __instance.GetComponentInChildren<ActionBarSpellGroupPCView>();
                if (spellview == null)
                {
                    Main.Context.Logger.Log("Spellview is null in postfix");
                }
                spellview = __instance.m_SpellsGroup
                var groupsOBJ = spellview.transform.parent;
                if (groupsOBJ == null)
                {
                    Main.Context.Logger.Log("groupsOBJ is null in postfix");
                }
                var otherButtons = groupsOBJ.GetComponentsInChildren<ActionBarGroupPCView>();

                foreach (var v in otherButtons.Where(x => x.name != "ManueverGroupView"))
                {
                    Main.Context.Logger.Log($"About to hook {v.name} to maneuver popout");
                    if (v.m_Neighbours == null)
                    {
                        Main.Context.Logger.Log($"{v.name}.m_neighbors is null");
                    }
                    if (cloneview is null)
                    {
                        Main.Context.Logger.Log($"cloneview is null is null");
                    }
                    else if (cloneview.m_Neighbours is null)
                    {
                        Main.Context.Logger.Log($"cloneview.m_neighbors is null");
                    }
                    v.m_Neighbours.Add(cloneview);
                    cloneview.m_Neighbours.Add(v);
                }

                */
            }
            catch (Exception ex)
            {
                Main.Context.Logger.LogError(ex, "setting neighbors for maneuvers menu");
            }

        }
        
        
        
    }


}
