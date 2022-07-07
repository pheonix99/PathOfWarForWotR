using BlueprintCore.Actions.Builder;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.NewComponents.AbilityRestrictions;
using TheInfiniteCrusade.NewComponents.ManeuverProperties;

namespace TheInfiniteCrusade.Backend.NewActions
{
    class ContextActionMartialAttack : ContextAction
    {
        public override string GetCaption()
        {
            return "Martial Strike";
        }

        public override void RunAction()
        {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            AbilityData ability = Context.SourceAbilityContext.Ability;
            if (ability == null)
            {
                PFLog.Default.Error("Ability is missing", Array.Empty<object>());
                return;
            }
            var source = Context.SourceAbilityContext.Ability;
            
            bool isRanged = ability.Blueprint.GetComponent<ManeuverRangeRestriction>()?.Range == true;
            bool isMelee = ability.Blueprint.GetComponent<ManeuverRangeRestriction>()?.Range == false;
            if (forceShield && (!maybeCaster.Body.SecondaryHand.HasShield || maybeCaster.Body.SecondaryHand.Shield.WeaponComponent == null))
            {
                PFLog.Default.Error("Forced shield bash triggered, but no shield", Array.Empty<object>());
                return;
            }


            ItemEntityWeapon oldMaiWeapon = null;

            ItemEntityWeapon mainWeapon = null;
            if (forceUnarmed)
            {
                
                if (maybeCaster.Body.PrimaryHand.Weapon.Blueprint.Category == WeaponCategory.UnarmedStrike)
                {
                    mainWeapon = maybeCaster.Body.PrimaryHand.Weapon;
                }
                else
                {
                    oldMaiWeapon = maybeCaster.Body.PrimaryHand.Weapon;

                    var emptyHands = maybeCaster.Body.EmptyHandWeaponsStack.Where(x => x.Blueprint.Category == WeaponCategory.UnarmedStrike);
                    foreach (var v in emptyHands)
                    {
                        if (mainWeapon == null)
                        {
                            mainWeapon = v;
                        }
                        else
                        {
                            if (v.DamageDice.MaxValue(0) > mainWeapon.DamageDice.MaxValue(0))
                            {
                                mainWeapon = v;
                            }
                        }
                    }
                }
                
            }
            else if (forceShield)
            {
                if (maybeCaster.Body.SecondaryHand.MaybeShield == null)
                {
                    return;
                }
                else
                { 
                    mainWeapon = maybeCaster.Body.SecondaryHand.Shield.WeaponComponent;
                }
            }
            else
            {
                mainWeapon = maybeCaster.Body.PrimaryHand.Weapon;
            }

            
            float meters = mainWeapon.AttackRange.Meters;
            TargetWrapper target = base.Target;
            if (target.Unit == null)
            {
                PFLog.Default.Error("No Target", Array.Empty<object>());
                return;
            }
            EventHandlers eventHandlers = new EventHandlers();
            foreach (AbstractMartialAttackWeaponModifier v in ability.Blueprint.Components.OfType<AbstractMartialAttackWeaponModifier>())
            {
                eventHandlers.Add(new MartialStrikeModifier(maybeCaster, v));
            }
            foreach (AbstractMartialStrikeProc v in ability.Blueprint.Components.OfType<AbstractMartialStrikeProc>())
            {
                eventHandlers.Add(new MartialStrikeProc(maybeCaster, v));
            }


            UnitEntityData targetData = SelectTarget(maybeCaster, meters, CanRetarget, target.Unit);
            if (forceUnarmed && oldMaiWeapon!= null)
            {
                
                maybeCaster.Body.PrimaryHand.RemoveItem(true, false);
            }

            if (targetData != null)
            {
                if (Mode == MartialAttackMode.FullAttack)
                {
                    RuleCalculateAttacksCount attacksCount = Rulebook.Trigger<RuleCalculateAttacksCount>(new RuleCalculateAttacksCount(maybeCaster));
                    attacksCount.AddExtraAttacks(ExtraHits, false);
                    int num = 0;

                    

                    List<UnitAttack.AttackInfo> list = UnitAttack.EnumerateAttacks(attacksCount).ToTempList<UnitAttack.AttackInfo>();
                    List<WeaponSlot> list2 = (from h in maybeCaster.Body.AdditionalLimbs
                                              where h.HasWeapon && h.HasItem
                                              select h).ToTempList<WeaponSlot>();

                    int attacksCount2 = list.Count + list2.Count;
                    foreach (UnitAttack.AttackInfo attackInfo in list)
                    {
                        if (isRanged && attackInfo.Hand.Weapon.Blueprint.IsMelee)
                        {
                            continue;
                        }
                        if (isMelee && attackInfo.Hand.Weapon.Blueprint.IsRanged)
                            continue;

                        this.RunAttackRule(maybeCaster, targetData, attackInfo.Hand.Weapon, eventHandlers, attackInfo.AttackBonusPenalty +ToHitShift, num, attacksCount2);
                        num++;
                        if (targetData.State.IsDead && CanRetarget)
                        {
                            targetData = SelectTarget(maybeCaster, meters, CanRetarget, targetData);

                        }

                    }
                    using (List<WeaponSlot>.Enumerator enumerator2 = list2.GetEnumerator())
                    {
                        while (enumerator2.MoveNext())
                        {
                            WeaponSlot hand = enumerator2.Current;
                            if (isRanged && hand.Weapon.Blueprint.IsMelee)
                            {
                                continue;
                            }
                            if (isMelee && hand.Weapon.Blueprint.IsRanged)
                                continue;
                            this.RunAttackRule(maybeCaster, targetData, hand.Weapon, eventHandlers, ToHitShift, num, attacksCount2);

                            num++;
                            if (targetData.State.IsDead && CanRetarget)
                            {


                                targetData = SelectTarget(maybeCaster, meters, CanRetarget, targetData);
                            }
                        }
                        return;
                    }
                }
                else if (Mode == MartialAttackMode.EveryWeapon)
                {
                    int hitNum = 0;
                    int attackCount = 1;
                    if (maybeCaster.Body.SecondaryHand.HasWeapon)
                    {
                        attackCount++;
                    }
                    List<WeaponSlot> list2 = (from h in maybeCaster.Body.AdditionalLimbs
                                              where h.HasWeapon && h.HasItem
                                              select h).ToTempList<WeaponSlot>();
                    attackCount += list2.Count;
                    if (ExtraHits > 0)
                    {
                        attackCount *= ExtraHits;
                    }

                    for (int i = 0; i < ExtraHits + 1; i++)
                    {
                        this.RunAttackRule(maybeCaster, targetData, mainWeapon, eventHandlers, ToHitShift, hitNum, attackCount);
                        this.RunAttackRule(maybeCaster, targetData, maybeCaster.Body.SecondaryHand.Weapon, eventHandlers, ToHitShift, hitNum, attackCount);
                        foreach(var weapon in list2)
                        {
                            this.RunAttackRule(maybeCaster, targetData, weapon.Weapon, eventHandlers, ToHitShift, hitNum, attackCount);
                        }
                    }

                }
                else
                {
                    int hitNum = 0;
                    int attackCount = 1;
                    if (ExtraHits > 0)
                    {
                        attackCount *= ExtraHits;
                    }
                    for (int i = 0; i < ExtraHits + 1; i++)
                    {
                        this.RunAttackRule(maybeCaster, targetData, mainWeapon, eventHandlers, ToHitShift, hitNum, attackCount);

                    }

                }
            }
            if (forceUnarmed && oldMaiWeapon != null)
            {
                maybeCaster.Body.PrimaryHand.InsertItem(oldMaiWeapon);
            }



        }

        private void RunAttackRule(UnitEntityData caster, UnitEntityData target, ItemEntityWeapon weapon, EventHandlers eventHandlers, int attackBonusPenalty = 0, int attackNumber = 0, int attacksCount = 1)
        {
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(caster, target, weapon, attackBonusPenalty)
            {
                Reason = base.Context,
                AutoHit = this.AutoHit,
                AutoCriticalThreat = this.AutoCritThreat,
                AutoCriticalConfirmation = this.AutoCritConfirmation,
                ExtraAttack = false,
                IsFullAttack = Mode == MartialAttackMode.FullAttack,
                AttackNumber = attackNumber,
                AttacksCount = attacksCount
            };
            if (ForceFlatfoot)
                ruleAttackWithWeapon.ForceFlatFooted = true;
            if (IgnoreDR)
            {
                ruleAttackWithWeapon.MeleeDamage.IgnoreDamageReduction = true;
            }
            using (eventHandlers.Activate())
            {
                base.Context.TriggerRule<RuleAttackWithWeapon>(ruleAttackWithWeapon);
            }
            
        }




        public static UnitEntityData SelectTarget(UnitEntityData caster, float range, bool canRetarget, UnitEntityData target)
        {
            if (!canRetarget)
                return target;
            else
            {


                if (target.State.IsDead)
                {
                    var localRange = range + caster.View.Corpulence;
                    UnitEntityData unitEntityData = null;
                    foreach (UnitGroupMemory.UnitInfo unitInfo in caster.Memory.Enemies)
                    {
                        UnitEntityData unit = unitInfo.Unit;
                        if (!(unit == null) && !(unit.View == null) && !(unit == target) && caster.DistanceTo(unit) <= range + unit.View.Corpulence && unit.Descriptor.State.IsConscious && (unitEntityData == null || unit.DistanceTo(target.Position) < unitEntityData.DistanceTo(target.Position)))
                        {
                            unitEntityData = unit;
                        }
                    }
                    return unitEntityData;
                }
                else
                    return target;
            }

        }

        public bool CanRetarget = false;

        public MartialAttackMode Mode = MartialAttackMode.Normal;
        public int ToHitShift = 0;
        public int ExtraHits = 0;
        public bool ForceFlatfoot = false;
        public bool IgnoreDR = false;
        public bool AutoHit;
        public bool AutoCritThreat;
        public bool AutoCritConfirmation;
        public bool forceUnarmed;
        public bool forceShield;
       


        private class EventHandlers : IDisposable
        {
            // Token: 0x06010778 RID: 67448 RVA: 0x003A3CEF File Offset: 0x003A1EEF
            public void Add(object handler)
            {
                this.m_Handlers.Add(handler);
            }

            // Token: 0x06010779 RID: 67449 RVA: 0x003A3D00 File Offset: 0x003A1F00
            public ContextActionMartialAttack.EventHandlers Activate()
            {
                foreach (object subscriber in this.m_Handlers)
                {
                    EventBus.Subscribe(subscriber);
                }
                return this;
            }

            // Token: 0x0601077A RID: 67450 RVA: 0x003A3D54 File Offset: 0x003A1F54
            public void Dispose()
            {
                foreach (object subscriber in this.m_Handlers)
                {
                    EventBus.Unsubscribe(subscriber);
                }
            }

            // Token: 0x0400B5E9 RID: 46569
            private readonly List<object> m_Handlers = new List<object>();
        }

        public class MartialStrikeProc : IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
        {
            private readonly UnitEntityData m_Unit;
            private readonly AbstractMartialStrikeProc m_Modifer;
            public MartialStrikeProc(UnitEntityData user, AbstractMartialStrikeProc modifer)
            {
                m_Unit = user;
                m_Modifer = modifer;
            }
            public UnitEntityData GetSubscribingUnit()
            {
                return this.m_Unit;
            }

            public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
            {
                
            }

            public void OnEventDidTrigger(RuleAttackWithWeapon evt)
            {
                m_Modifer.DoProc(evt);
            }
        }

        public class MartialStrikeModifier : IInitiatorRulebookHandler<RuleCalculateWeaponStats>,
            IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
        {
            private readonly UnitEntityData m_Unit;
            private readonly AbstractMartialAttackWeaponModifier m_Modifer;
            public MartialStrikeModifier(UnitEntityData user, AbstractMartialAttackWeaponModifier modifer)
            {
                m_Unit = user;
                m_Modifer = modifer;
            }

            public UnitEntityData GetSubscribingUnit()
            {
                return this.m_Unit;
            }

            public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
            {
                
            }

            public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
            {
                m_Modifer.ModifyWeaponStats(evt);
            }
        }

    }



    public enum MartialAttackMode
    {
        Normal,
        FullAttack,
        EveryWeapon
    }
    public enum RangeRestriction
    {
        None,
        Melee,
        Ranged
    }
}
