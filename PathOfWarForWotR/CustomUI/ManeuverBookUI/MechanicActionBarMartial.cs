using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.Common;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using Owlcat.Runtime.UI.Tooltips;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace PathOfWarForWotR.CustomUI.ManeuverBookUI
{
    public abstract class MechanicActionBarMartial : MechanicActionBarSlot
    {
        public abstract AbilityData Maneuver { get; }

		public override string KeyName
		{
			get
			{
				AbilityData spell = this.Maneuver;
				if (spell == null)
				{
					return null;
				}
				BlueprintAbility blueprint = spell.Blueprint;
				if (blueprint == null)
				{
					return null;
				}
				return blueprint.name;
			}
		}

		public override bool IsNotAvailable
		{
			get
			{
				return this.Maneuver == null || this.Maneuver.IsVariable || !this.Maneuver.IsAvailable;
			}
		}

		public override bool IsPossibleActive(int? resource = null)
		{
			using (ProfileScope.New("GetResource", _: null))
			{
				if (resource == null)
				{
					resource = new int?(this.GetResource());
				}
			}
			return (!CombatController.IsInTurnBasedCombat() || base.CanUseIfTurnBased()) && !base.IsDisabled(resource.Value) && !this.IsNotAvailable;
		}

		public override bool CanUseIfTurnBasedInternal()
		{
			if (this.Maneuver == null)
			{
				return false;
			}
			bool requireFullRoundAction = this.Maneuver.RequireFullRoundAction;
			UnitCommand.CommandType runtimeActionType = this.Maneuver.RuntimeActionType;
			return base.CanUseByActionType(requireFullRoundAction, runtimeActionType);
		}

		public override bool IsDisabled(int resourceCount)
		{
			return base.IsDisabled(resourceCount) || !this.Maneuver.IsAvailable;
		}

		public override void OnClick()
		{
			base.OnClick();
			if (!this.IsPossibleActive(null) || this.Maneuver == null)
			{
				return;
			}
			if (this.Maneuver.TargetAnchor != AbilityTargetAnchor.Owner)
			{
				Game.Instance.SelectedAbilityHandler.SetAbility(this.Maneuver);
				return;
			}
			base.Unit.Commands.Run(UnitUseAbility.CreateCastCommand(this.Maneuver, base.Unit));
		}

		public override void OnRightClick()
		{
			base.OnRightClick();
			if (base.Unit.Brain.AutoUseAbility == this.Maneuver)
			{
				base.Unit.Brain.AutoUseAbility = null;
				return;
			}
			if (this.Maneuver != null && this.Maneuver.IsSuitableForAutoUse)
			{
				base.Unit.Brain.AutoUseAbility = this.Maneuver;
			}
		}

		public override void OnHover(bool state)
		{
			EventBus.RaiseEvent<IAbilityTargetHoverUIHandler>(delegate (IAbilityTargetHoverUIHandler h)
			{
				h.HandleAbilityTargetHover(this.Maneuver, state);
			}, true);
			if (this.Maneuver != null)
			{
				if (state)
				{
					if (this.Maneuver.TargetAnchor == AbilityTargetAnchor.Owner)
					{
						EventBus.RaiseEvent<IShowAoEAffectedUIHandler>(delegate (IShowAoEAffectedUIHandler h)
						{
							h.HandleAoEMove(this.Unit.Position, this.Maneuver);
						}, true);
						return;
					}
				}
				else
				{
					EventBus.RaiseEvent<IShowAoEAffectedUIHandler>(delegate (IShowAoEAffectedUIHandler h)
					{
						h.HandleAoECancel();
					}, true);
				}
			}
		}

		public override bool IsBad()
		{
			return this.Maneuver == null || this.Maneuver.Blueprint == null || this.Maneuver.Caster == null || this.Maneuver.Blueprint.Hidden || base.Unit == null || this.Maneuver.Blueprint.GetComponent<ManeuverInformation>() == null;
		}

		public override int GetResource()
		{
			AbilityData spell = this.Maneuver;
			if (spell == null)
			{
				return 0;
			}
			return spell.GetAvailableForCastCount();
		}

		public override Sprite GetIcon()
		{
			
			return Maneuver?.Icon;
		}

		public override Sprite GetDecorationSprite()
		{
			AbilityData spell = this.Maneuver;
			return UIUtility.GetDecorationBorderByIndex((spell != null) ? spell.DecorationBorderNumber : -1);
			
		}

		public override Color GetDecorationColor()
		{
			AbilityData spell = this.Maneuver;
			return UIUtility.GetDecorationColorByIndex((spell != null) ? spell.DecorationColorNumber : -1);
		}

		public override string GetTitle()
		{
			AbilityData spell = this.Maneuver;
			return ((spell != null) ? spell.Name : null) ?? "";
		}

	
		public override string GetDescription()
		{
			AbilityData spell = this.Maneuver;
			return ((spell != null) ? spell.ShortenedDescription : null) ?? "";
		}

		public override void UpdateSlotInternal(ActionBarSlot slot)
		{
			bool flag = this.Maneuver != null && this.Maneuver.TargetAnchor != AbilityTargetAnchor.Owner && Game.Instance.SelectedAbilityHandler != null && Game.Instance.SelectedAbilityHandler.SelectedAbility == this.Maneuver;
			if (!flag)
			{
				UnitEntityData unit = base.Unit;
				if (((unit != null) ? unit.Brain.AutoUseAbility : null) == this.Maneuver)
				{
					flag = true;
				}
			}
			if (flag && slot.ActiveMark != null)
			{
				slot.ActiveMark.color = slot.DefaultColor;
			}
			Image activeMark = slot.ActiveMark;
			if (activeMark == null)
			{
				return;
			}
			activeMark.gameObject.SetActive(flag && this.IsPossibleActive(null));
		}

		public override bool IsCasting()
		{
			if (this.IsBad())
			{
				return false;
			}
			UnitUseAbility unitUseAbility = base.Unit.Commands.Standard as UnitUseAbility;
			if (unitUseAbility != null)
			{
				BlueprintAbility blueprint = unitUseAbility.Ability.Blueprint;
				AbilityData spell = this.Maneuver;
				return blueprint == ((spell != null) ? spell.Blueprint : null);
			}
			return false;
		}

		public override string WarningMessage()
		{
			AbilityData spell = this.Maneuver;
			if (spell == null)
			{
				return null;
			}
			return spell.GetUnavailableReason();
		}

		public override object GetContentData()
		{
			return this.Maneuver;
		}

		// Token: 0x06003B3E RID: 15166 RVA: 0x000E3298 File Offset: 0x000E1498
		public override TooltipBaseTemplate GetTooltipTemplate()
		{
			return new TooltipTemplateAbility(this.Maneuver);
		}

		// Token: 0x17000A4C RID: 2636
		// (get) Token: 0x06003B3F RID: 15167 RVA: 0x000E32A5 File Offset: 0x000E14A5
		public override bool IsAutoUse
		{
			get
			{
				return this.Maneuver != null && (base.Unit.Brain.AutoUseAbility == this.Maneuver || this.Maneuver.IsSuitableForAutoUse);
			}
		}

		public override void OnAutoUseToggle()
		{
			base.OnAutoUseToggle();
			if (base.Unit.Brain.AutoUseAbility == this.Maneuver)
			{
				base.Unit.Brain.AutoUseAbility = null;
				return;
			}
			AbilityData spell = this.Maneuver;
			if (spell != null && spell.IsSuitableForAutoUse)
			{
				base.Unit.Brain.AutoUseAbility = this.Maneuver;
			}
		}

		public override SlotConversion GetConvertedAbilityData()
		{
			if (!(this.Maneuver != null))
			{
				return base.GetConvertedAbilityData();
			}
			return new SlotConversion(this.Maneuver.GetConversions().ToList<AbilityData>());
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x000E337C File Offset: 0x000E157C
		public override bool IsRuntimeOnly()
		{
			AbilityData spell = this.Maneuver;
			return spell != null && spell.IsRuntimeOnly;
		}
	}
}
