using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Defines;
using TheInfiniteCrusade.NewComponents;
using TheInfiniteCrusade.NewComponents.AbilityRestrictions;
using TheInfiniteCrusade.NewComponents.Actions;
using TheInfiniteCrusade.NewComponents.ManeuverProperties;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
using UnityEngine;

namespace TheInfiniteCrusade.Utilities
{
    public static class ManeuverTools
    {
        public static void FinishManeuver(BlueprintAbility maneuver)
        {
            var comp = maneuver.GetComponent<ManeuverInformation>();
            if (comp == null)
            {
                return;
            }
          
            

            //Any universal stuff goes here.


            if (comp.isPrcAbility)
            {
                return;
            }
            else
            {
                if (comp.DisciplineKeys.Length == 1)
                {
                    if (comp.DisciplineKeys[0] == "BrokenBlade")
                    {
                        maneuver.AddComponent<BrokenBladeDisciplineWeaponsOnlyRestriction>();
                    }
                    if (comp.DisciplineKeys[0] == "IronTortoise" && !maneuver.Components.OfType<IronTortoiseShieldRequiredRestriction>().Any())
                    {
                        maneuver.AddComponent<IronTortoiseShieldRequiredRestriction>();
                    }
                    if (comp.DisciplineKeys[0] == "ScarletThrone" && !maneuver.Components.OfType<ScarletThroneNoShieldRule>().Any())
                    {
                        maneuver.AddComponent<IronTortoiseShieldRequiredRestriction>();
                    }
                }
                


                if (comp.ManeuverType == ManeuverType.Stance)
                {
                    BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.Context, "MasterStanceList").SpellsByLevel.FirstOrDefault(x => x.SpellLevel == comp.ManeuverLevel).m_Spells.Add(maneuver.ToReference<BlueprintAbilityReference>());
                }
                else
                {
                    BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.Context, "MasterManeuverList").SpellsByLevel.FirstOrDefault(x => x.SpellLevel == comp.ManeuverLevel).m_Spells.Add(maneuver.ToReference<BlueprintAbilityReference>());
                }
                Main.Context.Logger.LogPatch("Finished", maneuver);
            }

        }

        public static BlueprintAbility MakeStandardStrike(ModContextBase context, string sysName, string displayName, string desc, int level, DisciplineDefine discipline, MartialAttackMode mode = MartialAttackMode.Normal,  bool fullRound = false, int extraHits = 0, int extraDice = 0, DiceType diceSize = DiceType.D6, bool WeaponDamage = true, bool VariableDamage = false, DamageTypeDescription damageType = null, int toHitShift = 0, ActionsBuilder payload = null, bool forceFlatfoot = false, bool allDamageIgnoresDr = false, bool extraIsPrecision = false, bool strikeDamageIgnoresDr = false, bool forceUnarmed = false, int flatDamage = 0, bool shieldBash = false, bool canRetarget = false,  Sprite icon = null, bool autoHit = false)
        {
            var abilty = ManeuverTools.MakeStrikeStub(context, sysName, displayName, desc, level, discipline, fullRound, icon);
            abilty.Range = AbilityRange.Weapon;
            abilty.CanTargetEnemies = true;
            abilty.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
            abilty.EffectOnEnemy = AbilityEffectOnUnit.Harmful;

            abilty = AbilityConfigurator.For(abilty).AddAbilityEffectRunAction(ActionsBuilder.New().Add<ContextActionMartialAttack>(x =>
            {
                x.Mode = mode;
                x.ExtraHits = extraHits;
                x.ForceFlatfoot = forceFlatfoot;
                x.IgnoreDR = allDamageIgnoresDr;
                x.CanRetarget = canRetarget;
                x.AutoHit = autoHit;
                x.ToHitShift = toHitShift;
                x.forceUnarmed = forceUnarmed;
                x.forceShield = shieldBash;


            })).Configure();
            if (payload != null)
            {
                abilty.AddComponent<MartialStrikeProcActionsOnHit>(x =>
                {

                    x.ActionList = payload.Build();
                });
            }
            if (extraDice > 0 || flatDamage > 0)
            {
                if (WeaponDamage)
                {

                    abilty.AddComponent<WeaponBonusDamage>(x =>
                    {
                        x.m_DiceCount = extraDice;
                        if (strikeDamageIgnoresDr)
                            x.IgnoresDr = true;
                        if (extraIsPrecision)
                            x.IsPrecision = true;
                    });
                }
                else if (VariableDamage)
                {

                }
            }
            return abilty;
        }

       public static void AddBonusToCombatManeuversInAbility( this BlueprintAbility ability, int bonus, ModifierDescriptor descriptor, params CombatManeuver[] maneuvers )
        {

            ability.AddComponent<TICManeuverCMBBonus>(x =>
            {
                x.Bonus = bonus;
                x.Descriptor = descriptor;
                x.combatManeuvers = maneuvers.ToList();

            });
        }
       

        public static BlueprintAbility MakeStrikeStub(ModContextBase source, string sysName, string displayName, string description, int level, DisciplineDefine discipline, bool fullRound = false, Sprite icon = null)
        {
            var ability = MakeManeuverStub(source, sysName, displayName, description, ManeuverType.Strike, level, discipline, icon);
            if (fullRound)
            {
                ability.m_IsFullRoundAction = true;
            }
            else
            {
                ability.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
            }


            return ability;

        }

        public static ContextDurationValue InitiatorModifierRounds()
        {
            return new ContextDurationValue
            {
                BonusValue = new ContextValue { m_AbilityParameter = AbilityParameterType.CasterStatBonus, ValueType = ContextValueType.AbilityParameter },
                m_IsExtendable = false,
                Rate = DurationRate.Rounds,
                DiceCountValue = 0
            };
        }

        public static ActionsBuilder ApplyBuffIfNotSaved(Blueprint<BlueprintBuffReference> buff, ContextDurationValue durationValue, SavingThrowType savingThrowType, ConditionsBuilder conditions = null)
        {
            var baseEffect = ActionsBuilder.New().SavingThrow(savingThrowType).ConditionalSaved(failed: ApplyBuff(buff, durationValue));
            
            if (conditions != null)
            {
                return ActionsBuilder.New().Conditional(conditions, baseEffect);
            }

            return baseEffect;
        }

        public static ConditionsBuilder LivingTargetsOnly()
        {
            return ConditionsBuilder.New().HasFact("UndeadType", true).HasFact("ConstructType", true);
        }

        public static ConditionsBuilder CrittableTargetsOnly()
        {
            return ConditionsBuilder.New();
        }

        public static ActionsBuilder ApplyBuffForever(Blueprint<BlueprintBuffReference> buff)
        {
            return ActionsBuilder.New().ApplyBuffPermanent(buff: buff, isNotDispelable: true, isFromSpell: false);
        }
        public static ActionsBuilder ApplyBuff(Blueprint<BlueprintBuffReference> buff, ContextDurationValue durationValue)
        {
            return ActionsBuilder.New().ApplyBuff(buff: buff, durationValue: durationValue, isNotDispelable:true, isFromSpell:false);
        }

        public static ActionsBuilder ApplyBuff(Blueprint<BlueprintBuffReference> buff, ContextDurationValue durationValue, ConditionsBuilder conditions)
        {
            return ActionsBuilder.New().Conditional(conditions, ApplyBuff(buff, durationValue));
        }


        public static BlueprintAbility MakeSimpleDamageUpStance(ModContextBase source, string sysName, string displayName, string description, int level, DisciplineDefine discipline, int baseValue, int levelsToIncrease, out BlueprintBuff buff, DamageTypeDescription damage = null, bool weaponDamage = true, bool variableDamage = false, Sprite icon = null)
        {
            var ability = MakeStanceStub(source, sysName, displayName, description, level, discipline, out buff, icon);


            buff.AddComponent<ContextAddWeaponDamageDice>(x =>
            {
                if (weaponDamage)
                {
                    x.DealWeaponDamage = true;
                }
                else if (variableDamage)
                {
                    x.DealVariableTypeDamage = true;
                }
                else
                {
                    x.DamageType = damage;
                }
                x.contextValue = new ContextValue
                {
                    ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageBonus
                };

            });
            buff.AddComponent<ContextRankConfig>(x =>
            {
                x.MakeScalingConfig(AbilityRankType.DamageBonus, baseValue, levelsToIncrease);

            });

            return ability;
        }
        public static BlueprintAbility MakeSimpleStatUpStance(ModContextBase source, string sysName, string displayName, string description, int level, DisciplineDefine discipline, StatType stat, ModifierDescriptor descriptor, int baseValue, int levelsToIncrease, out BlueprintBuff buff, Sprite icon = null)
        {
            var ability = MakeStanceStub(source, sysName, displayName, description, level, discipline, out buff, icon);


            buff.AddComponent<AddContextStatBonus>(x =>
            {
                x.Stat = stat;
                x.Descriptor = descriptor;
                x.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue
                {
                    ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                    ValueRank = AbilityRankType.DamageBonus

                };
            });
            buff.AddComponent<ContextRankConfig>(x =>
            {
                x.MakeScalingConfig(AbilityRankType.DamageBonus, baseValue, levelsToIncrease);
                
            });

            return ability;
        }

        public static void AddScalingConfig(this BlueprintScriptableObject obj, AbilityRankType type, int baseValue, int levelsToIncrease)
        {
            obj.AddComponent<ContextRankConfig>(x =>
            {
                x.MakeScalingConfig(type, baseValue, levelsToIncrease);
            });
        }

        public static void MakeScalingConfig(this ContextRankConfig config, AbilityRankType type, int baseValue, int levelsToIncrease)
        {
            config.m_Type = AbilityRankType.DamageBonus;
            config.m_Progression = ContextRankProgression.StartPlusDivStep;
            if (baseValue > 1)
            {
                config.m_StartLevel = -1 * (baseValue - 1) * levelsToIncrease;
            }
            else
            {
                config.m_StartLevel = 1;
            }
            config.m_StepLevel = levelsToIncrease;
        }

        public static BlueprintAbility MakeBoostStub(ModContextBase source, string sysName, string displayName, string description, int level, DisciplineDefine discipline, out BlueprintBuff buff, Action<BlueprintBuff> Make, int duration = 1, Sprite icon = null)
        {
            var ability = MakeManeuverStub(source, sysName, displayName, description, ManeuverType.Boost, level, discipline, icon);

            buff = Helpers.CreateBlueprint<BlueprintBuff>(source, sysName + "Buff", x =>
              {
                  x.SetNameDescription(source, displayName, description);
                  x.FxOnStart = new();
                  x.FxOnRemove = new();
                  Make.Invoke(x);
                  x.m_Icon = icon ?? discipline.defaultSprite;
              });

            ability = AbilityConfigurator.For(ability).AddAbilityEffectRunAction(ApplyBuff(buff, ContextDuration.Fixed(duration))).Configure();


            return ability;
        }

        public static BlueprintAbility MakeStanceStub(ModContextBase source, string sysName, string displayName, string description, int level, DisciplineDefine discipline, out BlueprintBuff buff, Sprite icon = null)
        {
           var localbuff = Helpers.CreateBlueprint<BlueprintBuff>(source, sysName + "Buff", x =>
              {
                  x.SetNameDescription(source, displayName, description);
                  x.AddComponent<ManeuverInformation>(x =>
                  {
                      x.ManeuverLevel = level;
                      x.ManeuverType = ManeuverType.Stance;
                      x.isPrcAbility = false;
                      x.DisciplineKeys = new string[] { discipline.SysName };
                  });
                  x.m_Icon = icon ?? discipline.defaultSprite;
                  x.FxOnStart = new();
                  x.FxOnRemove = new();

              });

            var ability = MakeManeuverStub(source, sysName, displayName, description, ManeuverType.Stance, level, discipline, icon);
            ability.Range = AbilityRange.Personal;
            ability.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift;
            ability.AddComponent<PseudoActivatable>(x =>
            {
                x.m_Type = PseudoActivatable.PseudoActivatableType.BuffToggle;
                x.m_GroupName = "MartialStance";
                x.m_Buff = localbuff.ToReference<BlueprintBuffReference>();
            });
            ability.SetLocalizedDuration(Main.Context, "");
            ability.SetLocalizedSavingThrow(Main.Context, "");
            buff = localbuff;
            return ability;
        }

        public static BlueprintAbility MakeManeuverStub(ModContextBase source, string sysName, string displayName, string description, ManeuverType type, int level, DisciplineDefine discipline, Sprite icon = null)
        {
            return Helpers.CreateBlueprint<BlueprintAbility>(source, sysName, x =>
            {
                x.SetNameDescription(source, displayName, description);
                x.AddComponent<ManeuverInformation>(x =>
                {
                    x.ManeuverLevel = level;
                    x.ManeuverType = type;
                    x.isPrcAbility = false;
                    x.DisciplineKeys = new string[] { discipline.SysName };
                });
                x.m_Icon = icon ?? discipline.defaultSprite;
                if (discipline.alwaysSupernatural)
                    x.Type = AbilityType.Supernatural;
                else
                    x.Type = AbilityType.Extraordinary;
                if (discipline.descriptor != SpellDescriptor.None)
                {
                    x.AddComponent<SpellDescriptorComponent>(x => x.Descriptor = discipline.descriptor);
                }
            });



        }
    }
}
