using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.NewComponents;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Defines;
using UnityEngine;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using PathOfWarForWotR.Backend.NewActions;
using BlueprintCore.Actions.Builder;
using Kingmaker.RuleSystem;
using PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents;
using BlueprintCore.Conditions.Builder;
using Kingmaker.RuleSystem.Rules.Damage;
using BlueprintCore.Utils;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Classes;
using PathOfWarForWotR.Backend.NewComponents;
using PathOfWarForWotR.Backend.NewComponents.Prerequisites;
using PathOfWarForWotR.Backend.NewComponents.AbilityRestrictions;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Enums;
using BlueprintCore.Utils.Types;
using BlueprintCore.Conditions.Builder.ContextEx;

namespace PathOfWarForWotR.Utilities
{
    public static class ManeuverConfigurator
    {
        #region lists
        public static List<BlueprintFeatureReference> ManeuverLearnFeatures = new();
        public static List<BlueprintFeatureReference> StanceLearnFeatures = new();

        private static void MakeManuverPicker(BlueprintAbility v, ModContextBase context, bool stance)
        {
            var picker = Helpers.CreateDerivedBlueprint<BlueprintFeature>(context, v.name + "PickDummyFeature", Main.Context.Blueprints.GetDerivedMaster("ManeuverPickDummyFeatureMaster"), new SimpleBlueprint[] { v }, x =>
            {
                x.SetNameDescription(context, v.Name, v.Description);
                x.m_Icon = v.Icon;
                x.AddComponent<ManeuverSelectorPickComponent>(x => { x.Maneuver = v.ToReference<BlueprintAbilityReference>(); });
                x.AddComponent<PrerequisiteManeuverSelectionDisciplineAccess>();
                x.AddComponent<PrerequisiteManeuverSelectionInitiatorLevelAccess>();
                //x.AddComponent<PrerequisiteManeuverSelectionNotKnown>();
                x.AddComponent<PrerequisiteManeuverSelectionLevelAllowed>();
                x.AddComponent<PrerequisiteManeuverSelectionDisciplineManeuversKnown>();
                x.IsClassFeature = true;

            });
            if (stance)
                StanceLearnFeatures.Add(picker.ToReference<BlueprintFeatureReference>());
            else
                ManeuverLearnFeatures.Add(picker.ToReference<BlueprintFeatureReference>());

        }

        #endregion


        public static AbilityConfigurator New(ModContextBase contextBase, string sysName, DisciplineDefine disciplineDefine, int level, ManeuverType maneuverType, UnitCommand.CommandType actionType, bool fullRound, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle, Sprite icon = null, AbilityType? type = null)
        {
            var guid = contextBase.Blueprints.GetGUID(sysName);
            var config = AbilityConfigurator.New(sysName, guid.ToString()).SetDisplayName(sysName + ".Name").SetDescription(sysName + ".Desc");
            config.SetActionType(actionType);
            if (actionType == CommandType.Standard && fullRound)
                config.SetIsFullRoundAction(true);

            config.SetType((disciplineDefine.alwaysSupernatural ? AbilityType.Supernatural : AbilityType.Extraordinary));
            config.SetAnimation(animationStyle);

            config.AddComponent<ManeuverInformation>(x =>
            {
                x.ManeuverLevel = level;
                x.ManeuverType = maneuverType;
                x.isPrcAbility = false;
                x.DisciplineKeys = new string[] { disciplineDefine.SysName };
            });

            config.SetIcon(icon ?? disciplineDefine.defaultSprite);
            if (disciplineDefine.descriptor != SpellDescriptor.None)
            {
                config.SetSpellDescriptor(disciplineDefine.descriptor);

            }
                

            return config;
        }
       
        public static ActionsBuilder ApplyBuffIfNotSaved(Blueprint<BlueprintBuffReference> buff, ContextDurationValue durationValue, SavingThrowType savingThrowType, ConditionsBuilder conditions = null)
        {
            var baseEffect = ActionsBuilder.New().SavingThrow(savingThrowType, onResult: ActionsBuilder.New().ConditionalSaved(failed: ApplyBuff(buff, durationValue)));

            if (conditions != null)
            {
                return ActionsBuilder.New().Conditional(conditions, baseEffect);
            }

            return baseEffect;
        }
        public static ActionsBuilder ApplyBuffsIfNotSaved(List<Blueprint<BlueprintBuffReference>> buff, ContextDurationValue durationValue, SavingThrowType savingThrowType, ConditionsBuilder conditions = null)
        {
            var effect = ApplyBuff(buff[0], durationValue);
            for (int i = 1;  i< buff.Count; i++)
            {
                effect.ApplyBuff(buff: buff[i], durationValue: durationValue, isNotDispelable: true, isFromSpell: false);
            }

            var baseEffect = ActionsBuilder.New().SavingThrow(savingThrowType, onResult: ActionsBuilder.New().ConditionalSaved(effect));

            if (conditions != null)
            {
                return ActionsBuilder.New().Conditional(conditions, baseEffect);
            }

            return baseEffect;
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
            return ActionsBuilder.New().ApplyBuff(buff: buff, durationValue: durationValue, isNotDispelable: true, isFromSpell: false);
        }

        public static ActionsBuilder ApplyBuff(Blueprint<BlueprintBuffReference> buff, ContextDurationValue durationValue, ConditionsBuilder conditions)
        {
            return ActionsBuilder.New().Conditional(conditions, ApplyBuff(buff, durationValue));
        }

        
        public static AbilityConfigurator AddPayload(this AbilityConfigurator strike, ActionsBuilder payload)
        {

            strike.AddComponent<MartialStrikeProcActionsOnHit>(x =>
            {
                x.ActionList = payload.Build();
            });

            return strike;
        }
        public static ConditionsBuilder EvilOutsider()
        {
           return ConditionsBuilder.New().HasFact("5279fc8380dd9ba419b4471018ffadd1").HasFact("9054d3988d491d944ac144e27b6bc318");
        }

        public static ConditionsBuilder EvilOutsider(this ConditionsBuilder conditionsBuilder)
        {
            return conditionsBuilder.HasFact("5279fc8380dd9ba419b4471018ffadd1").HasFact("9054d3988d491d944ac144e27b6bc318");
        }
        public static ConditionsBuilder SilverCraneSpecialTarget()
        {
            //
            return ConditionsBuilder.New().AddOrAndLogic(EvilOutsider()).HasFact("734a29b693e9ec346ba2951b27987e33").UseOr();
        }

        public static ConditionsBuilder SilverCraneSpecialTarget(this ConditionsBuilder builder)
        {
            //
            return builder.AddOrAndLogic(EvilOutsider()).HasFact("734a29b693e9ec346ba2951b27987e33").UseOr();
        }


        public static AbilityConfigurator AddFixedEnergyDamageToStrike(this AbilityConfigurator strike, int dice, DamageTypeDescription damageTypeDescription, int flatdamage = 0, DiceType diceType = DiceType.D6, bool precision = false, ConditionsBuilder target = null, ConditionsBuilder user = null)
        {
            return strike.AddComponent<FixedTypeBonusDamge>(x =>
            {
                x.m_DiceCount = dice;
                x.m_DiceType = diceType;
                x.DamageTypeDescription = damageTypeDescription;
                x.IsPrecision = precision;
                x.m_FlatDamage = flatdamage;
                if (target != null)
                    x.targetCondition = target.Build();
                if (user != null)
                    x.userCondition = user.Build();
            });

        }

        public static void ConfigureManeuver(this AbilityConfigurator abilityConfigurator, ModContextBase context)
        {
            var maneuver = abilityConfigurator.Configure();
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
                        maneuver.AddComponent<ScarletThroneNoShieldRule>(x => x.AllowTwoHanderAtAll = true);
                    }
                }
                


                if (comp.ManeuverType == ManeuverType.Stance)
                {
                    BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.Context, "MasterStanceList").SpellsByLevel.FirstOrDefault(x => x.SpellLevel == comp.ManeuverLevel).m_Spells.Add(maneuver.ToReference<BlueprintAbilityReference>());
                    MakeManuverPicker(maneuver, context, true);
                }
                else
                {
                    BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.Context, "MasterManeuverList").SpellsByLevel.FirstOrDefault(x => x.SpellLevel == comp.ManeuverLevel).m_Spells.Add(maneuver.ToReference<BlueprintAbilityReference>());
                    MakeManuverPicker(maneuver, context, false);
                }
                Main.Context.Logger.LogPatch("Finished", maneuver);
            }
        }

        public static AbilityConfigurator AddStrikeComponent(this AbilityConfigurator configurator, int extraHit = 0, bool canRetarget = true, bool autoHit = false, bool autoThreat = false, bool forceShield = false, bool ignoreDR = false, bool forceUnarmed = false, bool autocritconfirm = false, bool forceFlatfoot = false, MartialAttackMode mode = MartialAttackMode.Normal, int tohitShift = 0)
        {
            var act = ActionsBuilder.New().Add<ContextActionMartialAttack>(x =>
            {
                x.AutoCritConfirmation = autocritconfirm;
                x.AutoCritThreat = autoThreat;
                x.AutoHit = autoHit;
                x.CanRetarget = canRetarget;
                x.ExtraHits = extraHit;
                x.ForceFlatfoot = forceFlatfoot;
                x.forceShield = forceShield;
                x.forceUnarmed = forceUnarmed;
                x.IgnoreDR = ignoreDR;
                x.Mode = mode;
                x.ToHitShift = tohitShift;

            });

            configurator.AddAbilityEffectRunAction(act);//TODO consider what happens if need more actions

            return configurator;
        }

        public static AbilityConfigurator AddWhirlwindComponent(this AbilityConfigurator configurator, int extraHit = 0, bool canRetarget = true, bool autoHit = false, bool autoThreat = false, bool forceShield = false, bool ignoreDR = false, bool forceUnarmed = false, bool autocritconfirm = false, bool forceFlatfoot = false, MartialAttackMode mode = MartialAttackMode.Normal, int tohitShift = 0)
        {
            var act = ActionsBuilder.New().Add<ContextActionWhirlwind>(y =>
            {
                y.WhirlwindThis = ActionsBuilder.New().Add<ContextActionMartialAttack>(x =>
              {

                  x.AutoCritConfirmation = autocritconfirm;
                  x.AutoCritThreat = autoThreat;
                  x.AutoHit = autoHit;
                  x.CanRetarget = canRetarget;
                  x.ExtraHits = extraHit;
                  x.ForceFlatfoot = forceFlatfoot;
                  x.forceShield = forceShield;
                  x.forceUnarmed = forceUnarmed;
                  x.IgnoreDR = ignoreDR;
                  x.Mode = mode;
                  x.ToHitShift = tohitShift;
              }).Build();

               

            }); 

            configurator.AddAbilityEffectRunAction(act);//TODO consider what happens if need more actions
            configurator.SetRange(AbilityRange.Personal);
            configurator.SetCanTargetEnemies(true);
            configurator.SetCanTargetFriends(false);
            configurator.SetCanTargetSelf(true);
            configurator.SetEffectOnAlly(AbilityEffectOnUnit.Harmful);
            configurator.SetEffectOnEnemy(AbilityEffectOnUnit.Harmful);
            return configurator;
        }

        public static AbilityConfigurator ApplyWeaponStrikeBits(this AbilityConfigurator strike)
        {
            strike.SetRange(AbilityRange.Weapon);
            strike.SetCanTargetEnemies(true);
            strike.SetCanTargetFriends(true);
            strike.SetCanTargetSelf(false);
            strike.SetEffectOnAlly(AbilityEffectOnUnit.Harmful);
            return strike.SetEffectOnEnemy(AbilityEffectOnUnit.Harmful);
        }

        


        public static AbilityConfigurator NewStrike(ModContextBase contextBase, string sysName, DisciplineDefine disciplineDefine, int level, UnitCommand.CommandType actionType, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle = UnitAnimationActionCastSpell.CastAnimationStyle.Special, bool fullRound = false, Sprite icon = null, AbilityType? type = null)
        {
            return New(contextBase, sysName, disciplineDefine, level, ManeuverType.Strike, actionType, fullRound, animationStyle, icon, type);


        }

        public static AbilityConfigurator NewStance(ModContextBase contextBase, string sysName, DisciplineDefine disciplineDefine, int level, Action<BuffConfigurator> buffmaker, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle = UnitAnimationActionCastSpell.CastAnimationStyle.Self, Sprite icon = null, AbilityType? type = null)
        {
            var guid = contextBase.Blueprints.GetGUID(sysName + "Buff");
            var buffConfig = BuffConfigurator.New(sysName + "Buff", guid.ToString());
            buffConfig.SetDisplayName(sysName + ".Name").SetDescription(sysName + ".Desc");
            buffConfig.SetIcon(icon ?? disciplineDefine.defaultSprite);
            buffConfig.AddComponent<ManeuverInformation>(x =>
            {
                x.ManeuverLevel = level;
                x.ManeuverType = ManeuverType.Stance;
                x.isPrcAbility = false;
                x.DisciplineKeys = new string[] { disciplineDefine.SysName };
            });
            buffmaker.Invoke(buffConfig);


            var buff = buffConfig.Configure().ToReference<BlueprintBuffReference>();

            return NewStance(contextBase, sysName, disciplineDefine, level, buff, animationStyle, icon, type);

        }

        public static BuffConfigurator AddScalingConfig(this BuffConfigurator buff, AbilityRankType type, int baseValue, int levelsToIncrease)
        {
            int start;
            if (baseValue > 1)
            {
                start = 1 - ((baseValue - 1) * levelsToIncrease);
            }
            else
            {
                start = 1;
            }

            return buff.AddContextRankConfig(new ContextRankConfig()
            {
                m_Type = type,
                m_Progression = ContextRankProgression.StartPlusDivStep,
                m_StartLevel = start,
                m_StepLevel = levelsToIncrease,

            });
        }

        public static AbilityConfigurator AddScalingConfig(this AbilityConfigurator buff, AbilityRankType type, int baseValue, int levelsToIncrease)
        {
            int start;
            if (baseValue > 1)
            {
                start = 1 - ((baseValue - 1) * levelsToIncrease);
            }
            else
            {
                start = 1;
            }

            return buff.AddContextRankConfig(new ContextRankConfig()
            {
                m_Type = type,
                m_Progression = ContextRankProgression.StartPlusDivStep,
                m_StartLevel = start,
                m_StepLevel = levelsToIncrease,

            });
        }

        public static void MakeScalingConfig(this ContextRankConfig config, AbilityRankType type, int baseValue, int levelsToIncrease)
        {

            config.m_Type = type;
            config.m_Progression = ContextRankProgression.StartPlusDivStep;
            if (baseValue > 1)
            {
                config.m_StartLevel = 1 - ((baseValue - 1) * levelsToIncrease);
            }
            else
            {
                config.m_StartLevel = 1;
            }
            config.m_StepLevel = levelsToIncrease;
        }

        public static AbilityConfigurator NewTriggeredBoost(ModContextBase contextBase, string sysName, DisciplineDefine disciplineDefine, int level,  UnitAnimationActionCastSpell.CastAnimationStyle animationStyle = UnitAnimationActionCastSpell.CastAnimationStyle.Self, Sprite icon = null, AbilityType? type = null)
        {
            

            var config = New(contextBase, sysName, disciplineDefine, level, ManeuverType.Boost, CommandType.Swift, false, animationStyle, icon, type);


            return config;
        }

        public static AbilityConfigurator NewNormalBoost(ModContextBase contextBase, string sysName, DisciplineDefine disciplineDefine, int level, Action<BuffConfigurator> buffmaker, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle = UnitAnimationActionCastSpell.CastAnimationStyle.Self, Sprite icon = null, AbilityType? type = null, int duration = 1)
        {
            var guid = contextBase.Blueprints.GetGUID(sysName + "Buff");
            var buffConfig = BuffConfigurator.New(sysName + "Buff", guid.ToString());
            buffConfig.SetDisplayName(sysName + ".Name").SetDescription(sysName + ".Desc");
            buffConfig.SetIcon(icon ?? disciplineDefine.defaultSprite);
            buffConfig.AddComponent<ManeuverInformation>(x =>
            {
                x.ManeuverLevel = level;
                x.ManeuverType = ManeuverType.Boost;
                x.isPrcAbility = false;
                x.DisciplineKeys = new string[] { disciplineDefine.SysName };
            });
            buffmaker.Invoke(buffConfig);


            var buff = buffConfig.Configure().ToReference<BlueprintBuffReference>();

            return NewNormalBoost(contextBase, sysName, disciplineDefine, level, buff, animationStyle, icon, type, duration);
        }

        public static AbilityConfigurator NewNormalBoost(ModContextBase contextBase, string sysName, DisciplineDefine disciplineDefine, int level, BlueprintBuffReference blueprintBuffReference, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle = UnitAnimationActionCastSpell.CastAnimationStyle.Self, Sprite icon = null, AbilityType? type = null, int duration = 1)
        {
            var config = New(contextBase, sysName, disciplineDefine, level, ManeuverType.Boost, CommandType.Swift, false, animationStyle, icon, type);
            config.AddAbilityEffectRunAction(ApplyBuff(blueprintBuffReference, ContextDuration.Fixed(duration)));
            if (duration == 1)
                config.SetLocalizedDuration(Duration.OneRound);

            return config;
        }

        public static AbilityConfigurator NewStance(ModContextBase contextBase, string sysName, DisciplineDefine disciplineDefine, int level, BlueprintBuffReference blueprintBuffReference, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle = UnitAnimationActionCastSpell.CastAnimationStyle.Self, Sprite icon = null, AbilityType? type = null)
        {
           
            var config = New(contextBase, sysName, disciplineDefine, level, ManeuverType.Stance, CommandType.Swift, false, animationStyle, icon, type);
            

            config.SetRange(AbilityRange.Personal);
            config.AddComponent<PseudoActivatable>(x =>
            {
                x.m_Type = PseudoActivatable.PseudoActivatableType.BuffToggle;
                x.m_GroupName = "MartialStance";

                x.m_Buff = blueprintBuffReference;
            });
            config.AddComponent<AbilityEffectToggleBuff>(x => { x.m_Buff = blueprintBuffReference; });

            return config;


        }
    }
}
