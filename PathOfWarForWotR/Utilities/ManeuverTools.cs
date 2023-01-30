using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.ModLogic;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using PathOfWarForWotR.Backend.NewActions;
using PathOfWarForWotR.Backend.NewComponents;
using PathOfWarForWotR.Backend.NewComponents.Prerequisites;
using PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents;
using PathOfWarForWotR.Defines;

using UnityEngine;
using PathOfWarForWotR.Backend.NewComponents.AbilityRestrictions;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;

namespace PathOfWarForWotR.Utilities
{
    public static class ManeuverTools
    {
        public static List<BlueprintFeatureReference> ManeuverLearnFeatures = new();
        public static List<BlueprintFeatureReference> StanceLearnFeatures = new();
        #region no-bp-core
        public static void FinishManeuver(BlueprintAbility maneuver, ModContextBase context)
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
                        maneuver.AddComponent<ScarletThroneNoShieldRule>(x => x.AllowTwoHanderAtAll = true);
                    }
                }
                if (comp.DisciplineKeys.Contains("SilverCrane"))
                {
                    maneuver.AddComponent<SilverCraneAntiFiendEffect>();
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


        public static BlueprintAbility MakeWhirlwindStrike(ModContextBase context, string sysName, string displayName, string desc, int level, DisciplineDefine discipline, MartialAttackMode mode = MartialAttackMode.Normal, bool fullRound = false, int extraHits = 0, int extraDice = 0, DiceType diceSize = DiceType.D6, bool WeaponDamage = true, bool VariableDamage = false, DamageTypeDescription damageType = null, int toHitShift = 0, ActionsBuilder payload = null, bool forceFlatfoot = false, bool allDamageIgnoresDr = false, bool extraIsPrecision = false, bool strikeDamageIgnoresDr = false, bool forceUnarmed = false, int flatDamage = 0, bool shieldBash = false, bool canRetarget = false, Sprite icon = null, bool autoHit = false)
        {
            var abilty = MakeWeaponBasedStrike(context, sysName, displayName, desc, level, discipline, mode, fullRound, extraHits, extraDice, diceSize, WeaponDamage, VariableDamage, damageType, toHitShift, payload, forceFlatfoot, allDamageIgnoresDr, extraIsPrecision, strikeDamageIgnoresDr, forceUnarmed, flatDamage, shieldBash, canRetarget, icon, autoHit);
            abilty.Range = AbilityRange.Personal;
            abilty.AddComponent<AbilityTargetsAround>(x => { x.m_Radius = new Kingmaker.Utility.Feet(5f); x.m_IncludeDead = false; x.m_TargetType = TargetType.Enemy; });
            abilty.AddComponent<AbilityTargetsInReach>();
            return abilty;
        }

        public static BlueprintAbility MakeStandardStrike(ModContextBase context, string sysName, string displayName, string desc, int level, DisciplineDefine discipline, MartialAttackMode mode = MartialAttackMode.Normal, bool fullRound = false, int extraHits = 0, int extraDice = 0, DiceType diceSize = DiceType.D6, bool WeaponDamage = true, bool VariableDamage = false, DamageTypeDescription damageType = null, int toHitShift = 0, ActionsBuilder payload = null, bool forceFlatfoot = false, bool allDamageIgnoresDr = false, bool extraIsPrecision = false, bool strikeDamageIgnoresDr = false, bool forceUnarmed = false, int flatDamage = 0, bool shieldBash = false, bool canRetarget = false, Sprite icon = null, bool autoHit = false)
        {
            var abilty = MakeWeaponBasedStrike(context, sysName, displayName, desc, level, discipline, mode, fullRound, extraHits, extraDice, diceSize, WeaponDamage, VariableDamage, damageType, toHitShift, payload, forceFlatfoot, allDamageIgnoresDr, extraIsPrecision, strikeDamageIgnoresDr, forceUnarmed, flatDamage, shieldBash, canRetarget, icon, autoHit);
            abilty.Range = AbilityRange.Weapon;
            return abilty;
        }


        private static BlueprintAbility MakeWeaponBasedStrike(ModContextBase context, string sysName, string displayName, string desc, int level, DisciplineDefine discipline, MartialAttackMode mode = MartialAttackMode.Normal, bool fullRound = false, int extraHits = 0, int extraDice = 0, DiceType diceSize = DiceType.D6, bool WeaponDamage = true, bool VariableDamage = false, DamageTypeDescription damageType = null, int toHitShift = 0, ActionsBuilder payload = null, bool forceFlatfoot = false, bool allDamageIgnoresDr = false, bool extraIsPrecision = false, bool strikeDamageIgnoresDr = false, bool forceUnarmed = false, int flatDamage = 0, bool shieldBash = false, bool canRetarget = false, Sprite icon = null, bool autoHit = false)
        {
            var abilty = ManeuverTools.MakeStrikeStub(context, sysName, displayName, desc, level, discipline, fullRound, icon);

            abilty.CanTargetEnemies = true;
            abilty.CanTargetSelf = false;
            abilty.CanTargetFriends = false;
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
                        x.m_DiceType = diceSize;
                        x.m_FlatDamage = flatDamage;
                        if (strikeDamageIgnoresDr)
                            x.IgnoresDr = true;
                        if (extraIsPrecision)
                            x.IsPrecision = true;
                    });
                }
                else if (VariableDamage)
                {
                    abilty.AddComponent<VariableTypeBonusDamage>(x =>
                    {
                        x.m_DiceCount = extraDice;
                        x.m_DiceType = diceSize;
                        x.m_FlatDamage = flatDamage;
                        if (extraIsPrecision)
                            x.IsPrecision = true;
                    });
                }
                else if (damageType != null)
                {
                    abilty.AddComponent<FixedTypeBonusDamge>(x =>
                    {
                        x.m_DiceCount = extraDice;
                        x.m_DiceType = diceSize;
                        x.m_FlatDamage = flatDamage;
                        x.DamageTypeDescription = damageType;
                    });
                }
            }
            return abilty;
        }



        public static void AddBonusToCombatManeuversInAbility(this BlueprintAbility ability, int bonus, ModifierDescriptor descriptor, params CombatManeuver[] maneuvers)
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
                x.Value = new ContextValue
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

            //ability = AbilityConfigurator.For(ability).AddAbilityEffectRunAction(ApplyBuff(buff, ContextDuration.Fixed(duration))).Configure();


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
            ability.AddComponent<AbilityEffectToggleBuff>(x => { x.m_Buff = localbuff.ToReference<BlueprintBuffReference>(); });
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

        #endregion

        #region bpcore
        
        public static ConditionsBuilder LivingTargetsOnly()
        {
            return ConditionsBuilder.New().HasFact("UndeadType", true).HasFact("ConstructType", true);
        }

        public static AbilityConfigurator MakeConventionalBoost(ModContextBase source, string sysname, string displayName, string desc, DisciplineDefine disciplineDefine, int level, Action<BuffConfigurator> makeBuff, Sprite icon = null, int duration = 1, AbilityType? abilityType = null)
        {
            var buffConfig = BuffTools.MakeBuff(source, sysname + "Buff", displayName, desc, icon ?? disciplineDefine.defaultSprite);
            buffConfig.AddComponent<ManeuverInformation>(x =>
            {
                x.ManeuverLevel = level;
                x.ManeuverType = ManeuverType.Boost;
                x.isPrcAbility = false;
                x.DisciplineKeys = new string[] { disciplineDefine.SysName };
            });
            makeBuff.Invoke(buffConfig);

            var buff = buffConfig.Configure();

            var config = MakeManeuverConfigurator(source, sysname, displayName, desc, UnitCommand.CommandType.Swift, UnitAnimationActionCastSpell.CastAnimationStyle.Self, disciplineDefine, level, ManeuverType.Boost, icon, abilityType);
            config.SetRange(AbilityRange.Personal);
            //config.AddAbilityEffectRunAction(ApplyBuff(buff, ContextDuration.Fixed(duration))).Configure();

            return config;
        }

        public static void FinishManeuver(AbilityConfigurator configIn, ModContextBase context)
        {
            var maneuver = configIn.Configure();
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
                if (comp.DisciplineKeys.Contains("SilverCrane"))
                {
                    maneuver.AddComponent<SilverCraneAntiFiendEffect>();
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

        

        public static AbilityConfigurator AddWeaponDamageToStrike(this AbilityConfigurator strike, int dice, int flatDamage = 0, int twohandbonusdamage = 0, DiceType diceType = DiceType.D6, bool isPrecision = false)
        {
            return strike.AddComponent<WeaponBonusDamage>(x =>
            {
                x.m_DiceCount = dice;
                x.m_DiceType = diceType;
                x.m_FlatDamage = flatDamage;
                x.ExtraDamageOnTwoHands = twohandbonusdamage;
                x.IsPrecision = isPrecision;
            });

        }

        public static AbilityConfigurator MakeSingleTargetWeaponStrike(ModContextBase source, string sysname, string displayName, string desc, DisciplineDefine disciplineDefine, int level, Sprite icon = null, AbilityType? abilityType = null, bool fullRound = false, Action<ActionsBuilder> beforeStrike = null, Action<ActionsBuilder> afterstrike = null, Action<ContextActionMartialAttack> attackConfig = null)
        {
            var config = MakeStrikeBase(source, sysname, displayName, desc, disciplineDefine, level, icon, abilityType, fullRound: fullRound);
            config.SetCanTargetFriends(false);
            config.SetCanTargetPoint(false);
            config.SetCanTargetSelf(false);
            config.SetRange(AbilityRange.Weapon);

            var act = ActionsBuilder.New();
            if (beforeStrike != null)
            {
                beforeStrike.Invoke(act);
            }
            act.Add<ContextActionMartialAttack>( attackConfig ?? ( x=>{ }));
            if (afterstrike != null)
            {
                afterstrike.Invoke(act);
            }
            config.AddAbilityEffectRunAction(act);


            return config;
        }

        public static AbilityConfigurator MakeStrikeBase(ModContextBase source, string sysname, string displayName, string desc, DisciplineDefine disciplineDefine, int level, Sprite icon = null, AbilityType? abilityType = null, UnitCommand.CommandType commandType = UnitCommand.CommandType.Standard, bool fullRound = false, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle = UnitAnimationActionCastSpell.CastAnimationStyle.Special)
        {
            var config = MakeManeuverConfigurator(source, sysname, displayName, desc, commandType, animationStyle, disciplineDefine, level, ManeuverType.Strike, icon, abilityType, fullRound);
          
            
            config.SetEffectOnEnemy(AbilityEffectOnUnit.Harmful);
            

            return config;
        }

        public static AbilityConfigurator MakeStance(ModContextBase source, string sysname, string displayName, string desc, DisciplineDefine disciplineDefine, int level, Action<BuffConfigurator> makeBuff, Sprite icon = null, AbilityType? abilityType = null)
        {
            var buffConfig = BuffTools.MakeBuff(source, sysname + "Buff", displayName, desc, icon ?? disciplineDefine.defaultSprite);
            buffConfig.AddComponent<ManeuverInformation>(x =>
            {
                x.ManeuverLevel = level;
                x.ManeuverType = ManeuverType.Stance;
                x.isPrcAbility = false;
                x.DisciplineKeys = new string[] { disciplineDefine.SysName };
            });
            makeBuff.Invoke(buffConfig);

            var buff = buffConfig.Configure();

            var config = MakeManeuverConfigurator(source, sysname, displayName, desc, UnitCommand.CommandType.Swift, UnitAnimationActionCastSpell.CastAnimationStyle.Self, disciplineDefine, level, ManeuverType.Stance, icon, abilityType);
            config.SetRange(AbilityRange.Personal);

            config.AddComponent<PseudoActivatable>(x =>
            {
                x.m_Type = PseudoActivatable.PseudoActivatableType.BuffToggle;
                x.m_GroupName = "MartialStance";

                x.m_Buff = buff.ToReference<BlueprintBuffReference>();
            });
            config.AddComponent<AbilityEffectToggleBuff>(x => { x.m_Buff = buff.ToReference<BlueprintBuffReference>(); });
            return config;
        }

        public static AbilityConfigurator MakeManeuverConfigurator(ModContextBase source, string sysname, string displayName, string desc, UnitCommand.CommandType commandType, UnitAnimationActionCastSpell.CastAnimationStyle animationStyle, DisciplineDefine disciplineDefine, int level, ManeuverType maneuverType, Sprite icon = null, AbilityType? abilityType = null, bool fullRound = false, Duration? duration = null)
        {

            var config = AbilityTools.MakeAbility(source, sysname, displayName, desc, commandType, abilityType ?? (disciplineDefine.alwaysSupernatural ? AbilityType.Supernatural : AbilityType.Extraordinary), animationStyle, fullRound, duration);
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


        #endregion


    }
}
