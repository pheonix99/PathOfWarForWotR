using Kingmaker;
using Kingmaker.UI;
using Kingmaker.UI.MVVM._PCView.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.Utility;
using Owlcat.Runtime.UI.Controls.Other;
using PathOfWarForWotR.Backend.NewUnitParts;
using PathOfWarForWotR.CustomUI.InsertIntoExisting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PathOfWarForWotR.CustomUI.ManeuverBookUI
{
    class ActionBarManeuverGroupPCView : ActionBarGroupPCView, IEventSystemHandler
	{
		
		public override void BindViewImplementation()
		{
			Main.Context.Logger.Log("ActionBarManeuverGroupPCView BindViewImplementation firing");
			base.BindViewImplementation();
			
		}

		public override int GetMinSlotsCount()
		{
			return 5;
		}

		public override void DrawSlots(List<ActionBarSlotVM> slotVms)
		{
			Main.Context.Logger.Log($"Calling DrawSlots on ActionBarManeuverGroupPCView, inbound list length is {slotVms.Count}");
			//int value = base.ViewModel.CurrentSpellLevel.Value;
			List<ActionBarSlotVM> list = new List<ActionBarSlotVM>();
			if (slotVms.Empty<ActionBarSlotVM>())
            {
				if(Game.Instance.SelectionCharacter.CurrentSelectedCharacter?.Get<UnitPartMartialDisciple>() is not null)
                {
					base.ViewModel.CollectManeuvers(Game.Instance.SelectionCharacter.CurrentSelectedCharacter);
					slotVms = ViewModel.GroupManevers();
					Main.Context.Logger.Log($"Patched inbout list to be {slotVms.Count}");
				}
            }
			foreach (ActionBarSlotVM actionBarSlotVM in slotVms)
			{
				if (actionBarSlotVM.MechanicActionBarSlot is MechanicActionBarMartialManeuver)
                {
					list.Add(actionBarSlotVM);
					Main.Context.Logger.Log("Adding MechanicActionBarMartial to ActionBarManeuverGroupPCView");
                }
				else
                {
					Main.Context.Logger.Log($"Not adding  {actionBarSlotVM.MechanicActionBarSlot.GetType().ToString()} to ActionBarManeuverGroupPCView");
				}

				
			}
			base.DrawSlots(list);
			
			MainThreadDispatcher.StartUpdateMicroCoroutine(this.SetPositionCoroutine());
			Main.LogDebug($"After ActionBarManeuverGroupPCView.DrawSlots, active slot count is {this.m_SlotsList.Count(x => x.gameObject.activeSelf)}, real slots are {this.m_SlotsList.Count(x => x.ViewModel?.MechanicActionBarSlot != null && x.ViewModel.MechanicActionBarSlot is not MechanicActionBarSlotEmpty)}, null slots are {this.m_SlotsList.Count(x=>x.ViewModel is not not null && x.ViewModel.MechanicActionBarSlot is null)}");
		}
		private IEnumerator SetPositionCoroutine()
		{
			yield return null;
			base.SetStatePosition(false);
			yield break;
		}

		public override void SetGroup()
		{
			Main.Context.Logger.Log($"SetGroup called on ActionBarManeuverGroupPCView");
			
			
			base.SetGroup();
		}


		public override void AddEmptySlots(List<ActionBarSlotVM> slotVms)
		{
			Main.Context.Logger.Log($"Calling AddEmptySlots on ActionBarManeuverGroupPCView, inbound list length is {slotVms.Count}");
			while (slotVms.Count < this.GetMinSlotsCount())
			{
				
				slotVms.Add(new ActionBarSlotVM(new MechanicActionBarSlotEmpty(), -1, -1));
			}
			base.AddEmptySlots(slotVms);
			Main.LogDebug($"After ActionBarManeuverGroupPCView.AddEmptySlots, active slot count is {this.m_SlotsList.Count(x => x.gameObject.activeSelf)}, real slots are {this.m_SlotsList.Count(x => x.ViewModel?.MechanicActionBarSlot != null && x.ViewModel.MechanicActionBarSlot is not MechanicActionBarSlotEmpty)}");
		}

	}
}
