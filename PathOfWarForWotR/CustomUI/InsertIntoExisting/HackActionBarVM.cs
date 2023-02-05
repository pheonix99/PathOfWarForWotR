using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Backend.NewUnitDataClasses;
using PathOfWarForWotR.Backend.NewUnitParts;
using PathOfWarForWotR.CustomUI.ManeuverBookUI;
using PathOfWarForWotR.EnumHacks;
using PathOfWarForWotR.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.CustomUI.InsertIntoExisting
{
    [HarmonyPatch(typeof(ActionBarVM))]
    static class HackActionBarVM
    {
        static List<ActionBarSlotVM> m_groupManuevers = new();

        public static List<ActionBarSlotVM> GroupManevers(this ActionBarVM vM)
        {
            Main.Context.Logger.Log($"Groupmanuevers has been called, length is {m_groupManuevers.Count}");
            return m_groupManuevers;
        }


        [HarmonyPatch(nameof(ActionBarVM.OnUnitChanged)), HarmonyPostfix]
        static void OnUnitChanged(ActionBarVM __instance, UnitEntityData unit)
        {
            using (ProfileScope.New("ActionBarVM CollectManeuvers", _: null))
            {
                Main.Context.Logger.Log("OnUnitChanged calling CollectManeuvers");
               __instance.CollectManeuvers(unit);
            }
        }

        [HarmonyPatch(nameof(ActionBarVM.CloseAllConverts)), HarmonyPostfix]
        static void CloseAllConverts(ActionBarVM __instance)
        {
            foreach (ActionBarSlotVM actionBarSlotVM4 in __instance.GroupManevers())
            {
                actionBarSlotVM4.CloseConvert();
            }
        }
        [HarmonyPatch(nameof(ActionBarVM.OnLateUpdateHandler)), HarmonyPostfix]
        static void OnLateUpdateHandler(ActionBarVM __instance)
        {
            if (!__instance.m_GroupState.ContainsKey(ActionBarGroupType_EXT.Manuevers))
            {
                __instance.m_GroupState.Add(ActionBarGroupType_EXT.Manuevers, false);
            }

            if (__instance.m_GroupState[ActionBarGroupType_EXT.Manuevers])
            {
                foreach (ActionBarSlotVM actionBarSlotVM in __instance.GroupManevers())
                {

                    actionBarSlotVM.UpdateResource();

                }
            }
        }

        [HarmonyPatch(nameof(ActionBarVM.UpdateGroupState)), HarmonyPrefix]
        static void UpdateGroupState(ActionBarVM __instance, ActionBarGroupType type, bool visible)
        {
            if (!__instance.m_GroupState.ContainsKey(ActionBarGroupType_EXT.Manuevers))
            {

                __instance.m_GroupState.Add(ActionBarGroupType_EXT.Manuevers, type == ActionBarGroupType_EXT.Manuevers ? visible : false);
            }

        }

        [HarmonyPatch(nameof(ActionBarVM.GroupClear)), HarmonyPostfix]
        static void GroupClear(ActionBarVM __instance, bool updateMechanicSlots)
        {
            __instance.GroupManevers().ForEach(delegate (ActionBarSlotVM g)
            {
                g.Dispose();
            });
        }

        public static void CollectManeuvers(this ActionBarVM vm, UnitEntityData unit)
        {
			List<AbilityData> list = new List<AbilityData>();
            m_groupManuevers = new();
            if (unit?.Get<UnitPartMartialDisciple>() == null)
            
                return;
            Main.Context.Logger.Log($"Collecting manuevers on {unit.CharacterName}");
			foreach (ManeuverBook spellbook in unit.ManeuverBooks())
			{
              
                var maneuvers = spellbook.AllReadiedManeuvers(Backend.NewUnitParts.SlotLayer.Readied);
                Main.Context.Logger.Log($"Collecting {maneuvers.Count} manuevers on {unit.CharacterName} for {spellbook.Name}");
                foreach (var v in maneuvers)
                {
                    Main.Context.Logger.Log($"Collecting {v.Name} on {unit.CharacterName} for {spellbook.Name}");
                    var ManeuverInfo = v.Blueprint.GetComponent<ManeuverInformation>();
					if (ManeuverInfo == null)
						continue;
					if (ManeuverInfo.ManeuverType == ManeuverType.Stance)
                    {
						//OOOPS
                    }
					else if (ManeuverInfo.ManeuverType == ManeuverType.Counter)
                    {
						//NOT READY YET
                    }
                    else
                    {
                        var slot = new ActionBarSlotVM(new MechanicActionBarReadiedManeuverNormal(v) {Unit = unit });//THIS IS NOT WORKING!
                        m_groupManuevers.Add(slot);
                        Main.Context.Logger.Log($"Collecting Manuever {v.Name} on {unit.CharacterName} for {spellbook.Name} as {slot.MechanicActionBarSlot.GetType().ToString()}");
                    }
					

				}
				
			}
		}


    }
}
