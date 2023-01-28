using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Localization;
using Kingmaker.Pathfinding;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using Kingmaker.View;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TurnBased.Controllers;
using UnityEngine;

namespace PathOfWarForWotR.Backend.NewActions
{
    class AbilityMartialCharge : AbilityCustomLogic, IAbilityTargetRestriction, IAbilityMinRangeProvider
    {
		public BlueprintAbilityReference m_ManeuverAtEnd;

		public BlueprintAbility ManeuverAtEnd => m_ManeuverAtEnd.Get();

		public override bool IsEngageUnit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600CCD5 RID: 52437 RVA: 0x003517F7 File Offset: 0x0034F9F7
		public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper targetWrapper)
		{
			UnitEntityData target = targetWrapper.Unit;
			if (target == null)
			{
				PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
				yield break;
			}
			UnitEntityData caster = context.Caster;
			if (caster.GetThreatHandMelee() == null)
			{
				PFLog.Default.Error("Invalid caster's weapon", Array.Empty<object>());
				yield break;
			}
			Vector3 position = caster.Position;
			Vector3 endPoint = target.Position;
			caster.View.StopMoving();
			caster.View.AgentASP.IsCharging = true;
			caster.View.AgentASP.ForcePath(new ForcedPath(new List<Vector3>
			{
				position,
				endPoint
			}), true);
			caster.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.ChargeBuff, context, new TimeSpan?(1.Rounds().Seconds));
			caster.Descriptor.State.IsCharging = true;
			UnitAttack attack = new UnitAttack(target, null);
			attack.Init(caster);
			IEnumerator turnBasedRoutine = null;
			IEnumerator runtimeRoutine = null;
			for (; ; )
			{
				IEnumerator enumerator;
				if (CombatController.IsInTurnBasedCombat())
				{
					enumerator = (turnBasedRoutine = (turnBasedRoutine ?? TurnBasesRoutine(caster, target, attack, ManeuverAtEnd)));
				}
				else
				{
					enumerator = (runtimeRoutine = (runtimeRoutine ?? RuntimeRoutine(caster, target, attack, endPoint, ManeuverAtEnd)));
				}
				if (!enumerator.MoveNext())
				{
					break;
				}
				yield return null;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600CCD6 RID: 52438 RVA: 0x0035180D File Offset: 0x0034FA0D
		private static IEnumerator TurnBasesRoutine(UnitEntityData caster, UnitEntityData target, UnitAttack attack, BlueprintAbility charge)
		{
			UnitEntityData mount = caster.GetSaddledUnit();
			if (mount == null)
			{
				UnitMovementAgent agentASP = caster.View.AgentASP;
				float timeSinceStart = 0f;
				while (attack.ShouldUnitApproach)
				{
					if (Game.Instance.TurnBasedCombatController.WaitingForUI)
					{
						yield return null;
					}
					else
					{
						timeSinceStart += Game.Instance.TimeController.GameDeltaTime;
						if (timeSinceStart > 6f)
						{
							PFLog.TBM.Log("Charge: timeSinceStart > 6f", Array.Empty<object>());
							break;
						}
						if (caster.GetThreatHand() == null)
						{
							PFLog.TBM.Log("Charge: caster.GetThreatHand() == null", Array.Empty<object>());
							break;
						}
						if (!caster.Descriptor.State.CanMove)
						{
							PFLog.TBM.Log("Charge: !caster.Descriptor.State.CanMove", Array.Empty<object>());
							break;
						}
						if (!agentASP)
						{
							PFLog.TBM.Log("Charge: !(bool)caster.View.AgentASP", Array.Empty<object>());
							break;
						}
						if (!agentASP.IsReallyMoving)
						{
							agentASP.ForcePath(new ForcedPath(new List<Vector3>
							{
								caster.Position,
								target.Position
							}), true);
							if (!agentASP.IsReallyMoving)
							{
								PFLog.TBM.Log("Charge: !caster.View.AgentASP.IsReallyMoving", Array.Empty<object>());
								break;
							}
						}
						agentASP.MaxSpeedOverride = new float?(Math.Max(agentASP.MaxSpeedOverride.GetValueOrDefault(), caster.CombatSpeedMps * 2f));
						yield return null;
					}
				}
				agentASP = null;
			}
			else
			{
				while (IsMountCharging(caster))
				{
					yield return null;
				}
			}
			caster.View.StopMoving();
			

			
			if (!attack.ShouldUnitApproach)
			{
				

				caster.Commands.AddToQueueFirst(new UnitUseAbility(new AbilityData(charge, caster), target, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free));
				/*
				attack.IgnoreCooldown(null);
				attack.IsCharge = true;
				UnitEntityData rider = caster.GetRider();
				if (rider != null)
				{
					if (rider.Commands.Attack != null)
					{
						attack.AddRiderCommand(rider.Commands.Attack);
						rider.Commands.Attack.AddMountCommand(attack);
					}
				}
				else if (mount != null && mount.Commands.Attack != null)
				{
					attack.AddMountCommand(mount.Commands.Attack);
					mount.Commands.Attack.AddRiderCommand(attack);
				}
				caster.Commands.AddToQueueFirst(attack);
				*/
			}

			yield break;
		}

		// Token: 0x0600CCD7 RID: 52439 RVA: 0x0035182A File Offset: 0x0034FA2A
		private static IEnumerator RuntimeRoutine(UnitEntityData caster, UnitEntityData target, UnitAttack attack, Vector3 endPoint, BlueprintAbility charge)
		{
			float maxDistance = GetMaxRangeMeters(caster);
			UnitEntityData mount = caster.GetSaddledUnit();
			if (mount == null)
			{
				float passedDistance = 0f;
				while (caster.View.MovementAgent.IsReallyMoving)
				{
					float valueOrDefault = caster.View.MovementAgent.MaxSpeedOverride.GetValueOrDefault();
					caster.View.MovementAgent.MaxSpeedOverride = new float?(Math.Max(valueOrDefault, caster.CombatSpeedMps * 2f));
					passedDistance += (caster.Position - caster.PreviousPosition).magnitude;
					if (passedDistance > maxDistance || !attack.ShouldUnitApproach)
					{
						PFLog.Default.Log("Charge: passedDistance > maxDistance || !attack.ShouldUnitApproach", Array.Empty<object>());
						break;
					}
					if (caster.GetThreatHand() == null)
					{
						PFLog.Default.Log("Charge: caster.GetThreatHand() == null", Array.Empty<object>());
						break;
					}
					Vector3 position = target.Position;
					if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, position) != position)
					{
						PFLog.Default.Log("Charge: obstacle != newEndPoint", Array.Empty<object>());
						break;
					}
					if (position != endPoint)
					{
						endPoint = position;
						caster.View.AgentASP.ForcePath(new ForcedPath(new List<Vector3>
						{
							caster.Position,
							endPoint
						}), true);
					}
					yield return null;
				}
				if (!caster.View.MovementAgent.IsReallyMoving)
				{
					PFLog.Default.Log("Charge: !caster.View.MovementAgent.IsReallyMoving", Array.Empty<object>());
				}
			}
			else
			{
				while (IsMountCharging(caster))
				{
					yield return null;
				}
			}
			if (!attack.ShouldUnitApproach)
			{
				attack.IgnoreCooldown(null);
				attack.IsCharge = true;
			}
			caster.Commands.AddToQueueFirst(new UnitUseAbility(new AbilityData(charge, caster), target, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free));
			/*
			UnitEntityData rider = caster.GetRider();
			if (rider != null)
			{
				if (rider.Commands.Attack != null)
				{
					attack.AddRiderCommand(rider.Commands.Attack);
					rider.Commands.Attack.AddMountCommand(attack);
				}
			}
			else if (mount != null && mount.Commands.Attack != null)
			{
				attack.AddMountCommand(mount.Commands.Attack);
				mount.Commands.Attack.AddRiderCommand(attack);
			}
			caster.Commands.AddToQueueFirst(attack);
			*/
			yield break;
		}

		// Token: 0x0600CCD8 RID: 52440 RVA: 0x00351850 File Offset: 0x0034FA50
		private static bool IsMountCharging(UnitEntityData rider)
		{
			UnitEntityData saddledUnit = rider.GetSaddledUnit();
			if (saddledUnit == null)
			{
				return false;
			}
			UnitUseAbility unitUseAbility = saddledUnit.Commands.Standard as UnitUseAbility;
			return unitUseAbility != null && unitUseAbility.Ability.Blueprint.GetComponent<AbilityMartialCharge>();
		}

		// Token: 0x0600CCD9 RID: 52441 RVA: 0x0035189C File Offset: 0x0034FA9C
		public override void Cleanup(AbilityExecutionContext context)
		{
			context.Caster.View.AgentASP.IsCharging = false;
			context.Caster.View.AgentASP.MaxSpeedOverride = null;
			context.Caster.Descriptor.State.IsCharging = false;
		}

		// Token: 0x0600CCDA RID: 52442 RVA: 0x003518F4 File Offset: 0x0034FAF4
		public float GetMinRangeMeters(UnitEntityData caster, [CanBeNull] UnitEntityData target)
		{
			float num = (target != null) ? target.View.Corpulence : 0.5f;
			if (Game.Instance.Player.IsTurnBasedModeOn())
			{
				return TurnController.MetersOfFiveFootStep + GameConsts.MinWeaponRange.Meters + caster.View.Corpulence + num;
			}
			return 10.Feet().Meters + caster.View.Corpulence + num;
		}

		// Token: 0x0600CCDB RID: 52443 RVA: 0x00351967 File Offset: 0x0034FB67
		public float GetMinRangeMeters(UnitEntityData caster)
		{
			return this.GetMinRangeMeters(caster, null);
		}

		// Token: 0x0600CCDC RID: 52444 RVA: 0x00351971 File Offset: 0x0034FB71
		private static float GetMaxRangeMeters(UnitEntityData caster)
		{
			return caster.CombatSpeedMps * 6f;
		}

		// Token: 0x0600CCDD RID: 52445 RVA: 0x00351980 File Offset: 0x0034FB80
		public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper targetWrapper)
		{
			LocalizedString localizedString;
			return this.CheckTargetRestriction(caster, targetWrapper, out localizedString);
		}

		// Token: 0x0600CCDE RID: 52446 RVA: 0x00351998 File Offset: 0x0034FB98
		public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
		{
			LocalizedString localizedString;
			this.CheckTargetRestriction(caster, target, out localizedString);
			return localizedString;
		}

		// Token: 0x0600CCDF RID: 52447 RVA: 0x003519B8 File Offset: 0x0034FBB8
		private bool CheckTargetRestriction(UnitEntityData caster, TargetWrapper targetWrapper, [CanBeNull] out LocalizedString failReason)
		{
			UnitEntityData unitEntityData = (targetWrapper != null) ? targetWrapper.Unit : null;
			if (unitEntityData == null)
			{
				failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsInvalid;
				return false;
			}
			float magnitude = (unitEntityData.Position - caster.Position).magnitude;
			if (magnitude > GetMaxRangeMeters(caster))
			{
				failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsTooFar;
				return false;
			}
			if (magnitude < this.GetMinRangeMeters(caster, unitEntityData))
			{
				failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsTooClose;
				return false;
			}
			if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, unitEntityData.Position) != unitEntityData.Position)
			{
				failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.ObstacleBetweenCasterAndTarget;
				return false;
			}
			UnitEntityData saddledUnit = caster.GetSaddledUnit();
			if (!(saddledUnit ?? caster).View.MovementAgent.AvoidanceDisabled)
			{
				float num = caster.View.Corpulence + unitEntityData.View.Corpulence;
				ItemEntityWeapon firstWeapon = caster.GetFirstWeapon();
				float valueOrDefault = (num + ((firstWeapon != null) ? new float?(firstWeapon.AttackRange.Meters) : null)).GetValueOrDefault();
				Vector2 normalized = (unitEntityData.Position - caster.Position).To2D().normalized;
				Vector2 a = unitEntityData.Position.To2D() - normalized * valueOrDefault;
				foreach (UnitEntityData unitEntityData2 in Game.Instance.State.AwakeUnits)
				{
					if (!(unitEntityData2 == caster) && !(unitEntityData2 == unitEntityData) && unitEntityData2.View && !unitEntityData2.View.MovementAgent.AvoidanceDisabled)
					{
						magnitude = (a - unitEntityData2.Position.To2D()).magnitude;
						if (magnitude < (caster.View.Corpulence + unitEntityData2.View.Corpulence) * 0.8f)
						{
							failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.ObstacleBetweenCasterAndTarget;
							return false;
						}
					}
				}
			}
			UnitEntityData unitEntityData3 = caster.GetSaddledUnit() ?? caster;
			bool flag = caster.State.IsCharging || (saddledUnit != null && saddledUnit.State.IsCharging) || (unitEntityData3 != null && unitEntityData3.State.IsCharging);
			if (CombatController.IsInTurnBasedCombat() && caster.IsCurrentUnit() && !flag && unitEntityData3.CombatState.TBM.TimeMoved > 0f)
			{
				failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.AlreadyMovedThisTurn;
				return false;
			}
			failReason = null;
			return true;
		}
	}
}
