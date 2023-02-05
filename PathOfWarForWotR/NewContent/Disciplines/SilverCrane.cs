using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Utility;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using PathOfWarForWotR.Backend.NewActions;
using PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents;
using PathOfWarForWotR.Utilities;
using BlueprintCore.Utils;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Backend.NewComponents.AbilityRestrictions;
using PathOfWarForWotR.Backend.NewComponents.AbilitySpecific;
using Kingmaker.Blueprints.Classes.Spells;

namespace PathOfWarForWotR.NewContent.Disciplines
{
    class SilverCrane
    {
        public static void Build()
        {
            var wingsicon = AssetLoader.LoadInternal(Main.Context, "", "Fly.png");
            LocalizationTool.LoadLocalizationPack("Mods\\PathOfWarForWotR\\Localization\\SilverCrane.json");
            DisciplineTools.AddDiscipline("SilverCrane", "Silver Crane", "Disciples of the Silver Crane are men and women for whom the power of the celestial and divine flow into the arts of their blade. The Silver Crane is a goodly discipline that is inspired by the teachings of celestials. It focuses on strong strikes designed to combat evil, celestial insights, and combat-predictions to defeat foes and enable the initiator and his allies to endure the hardships of battle against the forces of evil. Upon learning the art of Silver Crane, the disciple becomes in tune with the flows of the celestial realm, gaining heavenly insights into combat as if the angels themselves were granting insight to the warrior in battle. The Silver Crane discipline’s associated skill is Perception, and its associated weapon groups are bows, hammers, and spears.\n The discipline of Silver Crane is to be considered a supernatural discipline and all abilities within are considered supernatural abilities and follow the rules and restrictions of such. All abilities in this discipline carry the [good] descriptor. A character may always strike incorporeal foes as if they were corporea with strikes of this discipline.", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Bows, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Spears, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Hammers }, Kingmaker.EntitySystem.Stats.StatType.SkillPerception, wingsicon);
            DisciplineTools.Disciplines.TryGetValue("SilverCrane", out var silverCrane);

            #region level 1
            //TODO CRANE STEP

            CraneStep();
            void CraneStep()
            {
                //TODO!
                //REQUIRES movement-as-ability!
            }

            FlashingWings();
            void FlashingWings()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "FlashingWings", silverCrane, 1, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(1, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                }, diceType: Kingmaker.RuleSystem.DiceType.D4);
                strikeConfig.AddPayload(ManeuverConfigurator.ApplyBuff("df6d1025da07524429afbae248845ecc", ContextDuration.Fixed(1)));
                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.SetLocalizedDuration(BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities.Duration.OneRound);
                strikeConfig.ConfigureManeuver(Main.Context);

            }



            SilverCraneWaltz();
            void SilverCraneWaltz()
            {




                var stanceConfig = ManeuverConfigurator.NewStance(Main.Context, "SilverCraneWaltz", silverCrane, 1, x =>
                {
                    x.AddContextStatBonus(stat: Kingmaker.EntitySystem.Stats.StatType.SaveReflex, value: new Kingmaker.UnitLogic.Mechanics.ContextValue()
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default

                    }, descriptor: ModifierDescriptor.Insight);
                    x.AddContextStatBonus(stat: Kingmaker.EntitySystem.Stats.StatType.AC, value: new Kingmaker.UnitLogic.Mechanics.ContextValue()
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default

                    }, descriptor: ModifierDescriptor.Insight);
                    x.AddComponent<ContextRankConfig>(x => { x.MakeScalingConfig(AbilityRankType.Default, 2, 8); });
                    x.AddContextStatBonus(stat: Kingmaker.EntitySystem.Stats.StatType.Initiative, value: new Kingmaker.UnitLogic.Mechanics.ContextValue()
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus

                    }, descriptor: ModifierDescriptor.Insight);
                    x.AddComponent<ContextRankConfig>(x => { x.MakeScalingConfig(AbilityRankType.Default, 4, 8); });
                });

                stanceConfig.ConfigureManeuver(Main.Context);
            }


            EnduringCraneStrike();
            void EnduringCraneStrike()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "EnduringCraneStrike", silverCrane, 1, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();

                strikeConfig.AddPayload(ActionsBuilder.New().Add<SilverCraneSingleTargetHeal>(x =>
                {

                }));
                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.ConfigureManeuver(Main.Context);
            }



            EyesOfTheCrane();
            void EyesOfTheCrane()
            {
                var stanceConfig = ManeuverConfigurator.NewStance(Main.Context, "EyesOfTheCrane", silverCrane, 1, x =>
                {
                    x.AddRerollConcealment();
                    x.AddModifyD20(rule: Kingmaker.Designers.Mechanics.Facts.RuleType.SkillCheck, skill: new Kingmaker.EntitySystem.Stats.StatType[] { Kingmaker.EntitySystem.Stats.StatType.SkillPerception }, rollsAmount: 1, takeBest: true, specificSkill: true);
                });

                stanceConfig.ConfigureManeuver(Main.Context);
                //TODO
                //Using the perception of the disciple’s heavenly training to assist him, the Silver Crane practitioner may see many things which would remain hidden from the eyes of the impure. While in this stance, the initiator rolls twice on Perception checks or when attempting to pierce concealment, using the higher of the two rolls. Additionally, he may use detect evil as a spell-like ability at will with a caster level equal to his initiator level. 
            }

            SilverStrike();
            void SilverStrike()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "SilverStrike", silverCrane, 1, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddComponent<AttackWithAdvantage>();
                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.ConfigureManeuver(Main.Context);
            }




            #endregion
            #region level 2 - all non-counters clear
            BlazingCranesWing();
            void BlazingCranesWing()
            {
                var config = ManeuverConfigurator.NewNormalBoost(Main.Context, "BlazingCranesWing", silverCrane, 2, x =>
                {
                    x.AdditionalDiceOnAttack(targetConditions: ManeuverConfigurator.SilverCraneSpecialTarget(), damageType: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                    {
                        Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                        Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                    }, value: new Kingmaker.UnitLogic.Mechanics.ContextDiceValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.D6,
                        DiceCountValue = ContextValues.Constant(2),
                        BonusValue = 0
                    }, randomizeDamage: false);

                });
                config.ConfigureManeuver(Main.Context);
            }

            EmeraldDisplacementStrike();
            void EmeraldDisplacementStrike()
            {
                var buffGuid = Main.Context.Blueprints.GetGUID("EmeraldDisplacementStrikeDebuff");
                var buff = BuffConfigurator.New("EmeraldDisplacementStrikeDebuff", buffGuid.ToString());
                buff.SetDisplayName("EmeraldDisplacementStrike");
                buff.SetDescription("EmeraldDisplacementStrikeDebuff");
                buff.AddSetFactOwnerMissChance(type: SetFactOwnerMissChance.Type.All, value: ContextValues.Constant(20));
                buff.AddStatBonus(ModifierDescriptor.None, stat: Kingmaker.EntitySystem.Stats.StatType.SkillPerception, value: -4);
                var buffref = buff.Configure().ToReference<BlueprintBuffReference>();

                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "EmeraldDisplacementStrike", silverCrane, 2, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddStrikeComponent();
                strikeConfig.SetLocalizedSavingThrow(BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities.SavingThrow.FortPartial);
                strikeConfig.AddPayload(ManeuverConfigurator.ApplyBuffIfNotSaved(buffref, durationValue: ManeuverConfigurator.InitiatorModifierRounds(), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude));
                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            BlessedPinions();
            void BlessedPinions()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "BlessedPinions", silverCrane, 2, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(2, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                }, diceType: Kingmaker.RuleSystem.DiceType.D6);

                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            DefensiveStep();
            void DefensiveStep()
            {
                //Requires Move-As-Ability;
                //No idea what happens if you step out of an ongoing full attack
            }






            #endregion
            #region level 3
            ExorcismStrike();
            void ExorcismStrike()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "ExorcismStrike", silverCrane, 3, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(4, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                }, target: ManeuverConfigurator.SilverCraneSpecialTarget());
                strikeConfig.AddFixedEnergyDamageToStrike(2, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });
                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.AddPayload(ManeuverConfigurator.ApplyBuffIfNotSaved("9934fedff1b14994ea90205d189c8759", ContextDuration.Fixed(1), Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, ManeuverConfigurator.SilverCraneSpecialTarget()));
                strikeConfig.SetLocalizedSavingThrow(BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities.SavingThrow.FortPartial);
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            SilverCranesBlessing();
            void SilverCranesBlessing()
            {
                //TODO requires reaction-boosts!
            }

            SilverKnightsBlade();
            void SilverKnightsBlade()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "SilverKnightsBlade", silverCrane, 3, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(4, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });
                strikeConfig.AddPayload(ActionsBuilder.New().Add<SilverCraneSingleTargetHeal>(x =>
                {
                    x.dice = 4;
                    x.useBonus = false;
                }));
                strikeConfig.ConfigureManeuver(Main.Context);
            }
            StanceOfTheSilverCrane();
            void StanceOfTheSilverCrane()
            {



                var stanceConfig = ManeuverConfigurator.NewStance(Main.Context, "StanceOfTheSilverCrane", silverCrane, 3, x =>
                {

                    x.AddBuffEnchantWornItem(allWeapons: true, enchantmentBlueprint: "28a9964d81fedae44bae3ca45710c140");
                    x.AddSavingThrowBonusAgainstAlignment(AlignmentComponent.Evil, ContextValues.Constant(2), ModifierDescriptor.Deflection);
                    x.AddArmorClassBonusAgainstAlignment(AlignmentComponent.Evil, ContextValues.Constant(2), ModifierDescriptor.Deflection);
                    x.AddFormationACBonus(2, unitProperty: false);

                });

                stanceConfig.ConfigureManeuver(Main.Context);
                //TODO
                //Using the perception of the disciple’s heavenly training to assist him, the Silver Crane practitioner may see many things which would remain hidden from the eyes of the impure. While in this stance, the initiator rolls twice on Perception checks or when attempting to pierce concealment, using the higher of the two rolls. Additionally, he may use detect evil as a spell-like ability at will with a caster level equal to his initiator level. 

                var protfromevil = BlueprintTool.Get<BlueprintBuff>("4a6911969911ce9499bf27dde9bfcedc");
                var buff = BuffConfigurator.For("StanceOfTheSilverCraneBuff");
                buff.AddSpecificBuffImmunity(AlignmentComponent.Evil, protfromevil.GetComponent<SpecificBuffImmunity>().m_Buff);
                buff.AddSpellImmunity(AlignmentComponent.Evil, type: Kingmaker.UnitLogic.Parts.SpellImmunityType.Specific, exceptions: protfromevil.GetComponent<AddSpellImmunity>().m_Exceptions.Select(x => (Blueprint<BlueprintAbilityReference>)x).ToList());
                buff.Configure(delayed: true);
            }

            #endregion
            #region level 4

            SacredPinions();
            void SacredPinions()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "SacredPinions", silverCrane, 4, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(5, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });
                //TODO balance pass? Stun incorporeal
                strikeConfig.ConfigureManeuver(Main.Context);
            }


            SapphireDisplacementStrike();
            void SapphireDisplacementStrike()
            {

                var buffGuid = Main.Context.Blueprints.GetGUID("SapphireDisplacementStrikeDebuff");
                var buff = BuffConfigurator.New("SapphireDisplacementStrikeDebuff", buffGuid.ToString());
                buff.SetDisplayName("SapphireDisplacementStrike");
                buff.SetDescription("SapphireDisplacementStrikeDebuff");
                buff.AddSetFactOwnerMissChance(type: SetFactOwnerMissChance.Type.All, value: ContextValues.Constant(50));
                buff.AddModifyD20(rule: RuleType.SkillCheck, specificSkill: true, skill: new StatType[] { Kingmaker.EntitySystem.Stats.StatType.SkillPerception }, replace: true, rollResult: ContextValues.Constant(1));
                var buffref = buff.Configure().ToReference<BlueprintBuffReference>();

                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "SapphireDisplacementStrike", silverCrane, 4, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddStrikeComponent();
                strikeConfig.AddPayload(ManeuverConfigurator.ApplyBuffIfNotSaved(buffref, durationValue: ManeuverConfigurator.InitiatorModifierRounds(), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude));
                strikeConfig.SetLocalizedSavingThrow(BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities.SavingThrow.FortNegates);
                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            SilverCraneResurgence();
            void SilverCraneResurgence()
            {
                //Requires reaction interface or smart-logic to determine threat level of failed save
            }

            SilverCranesLeap();
            void SilverCranesLeap()
            {
                //Requires move-as-ability
            }

            #endregion

            #region level 5

            SilverCranesSpiral();
            void SilverCranesSpiral()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "SilverCranesSpiral", silverCrane, 5, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddWhirlwindComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddComponent<TypedAccuracyModifier>(x =>
                {
                    x.bonus = 2;
                    x.modifierDescriptor = ModifierDescriptor.Insight;
                });
                
                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.ConfigureManeuver(Main.Context);
            }
            StanceOfTheCraneKnight();
            void StanceOfTheCraneKnight()
            {
                var stance = ManeuverConfigurator.NewStance(Main.Context, "StanceOfTheCraneKnight", silverCrane, 5, x =>
                {
                    x.AddDamageResistancePhysical(alignment: Kingmaker.Enums.Damage.DamageAlignment.Evil, bypassedByAlignment: true, value: ContextValues.Constant(10));
                    x.AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.Ground);
                    x.AddACBonusAgainstAttacks(againstMeleeOnly: true, armorClassBonus: 3, descriptor: ModifierDescriptor.Dodge);
                    x.AddBuffMovementSpeed(value: 30, descriptor: ModifierDescriptor.Trait);

                });
                stance.ConfigureManeuver(Main.Context);
            }

            ArgentKnightsBanner();
            void ArgentKnightsBanner()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "ArgentKnightsBanner", silverCrane, 5, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(8, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });
                strikeConfig.AddPayload(ActionsBuilder.New().Add<SilverCraneHealPulse>(x => {
                    x.dice = 4;
                    x.useBonus = false;
                    x.Feet = new Feet(30f);
                }));
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            EmeraldTippedFeathers();
            void EmeraldTippedFeathers()
            {
                //TODO requires boost-as-interrupt
            }

            #endregion

            #region level 6

            ArgentKingsScepter();
           void ArgentKingsScepter()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "ArgentKingsScepter", silverCrane, 6, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(12, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });
                strikeConfig.AddPayload(ActionsBuilder.New().Add<ManeuverHealSelf>(x=> {
                    x.Value = ContextDice.Value(Kingmaker.RuleSystem.DiceType.Zero, diceCount: ContextValues.Constant(0), ContextValues.Constant(60));
                }));
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            //HolyPinions();
            void HolyPinions()//TODO figure out how to implement making incorporeal corporeal or replace
            {
                var buffGuid = Main.Context.Blueprints.GetGUID("HolyPinionsDebuff");
                var buff = BuffConfigurator.New("HolyPinionsDebuff", buffGuid.ToString());
                buff.SetDisplayName("HolyPinions");
                buff.SetDescription("HolyPinionsDebuff");
                
                var buffref = buff.Configure().ToReference<BlueprintBuffReference>();

                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "HolyPinions", silverCrane, 6, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(10, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });
                strikeConfig.AddPayload(ManeuverConfigurator.ApplyBuffsIfNotSaved(new List<Blueprint<BlueprintBuffReference>>() {buffref, "09d39b38bb7c6014394b6daced9bacd3" }, ManeuverConfigurator.InitiatorModifierRounds(), SavingThrowType.Will, ConditionsBuilder.New().HasFact("c4a7f98d743bc784c9d4cf2105852c39")));
                strikeConfig.SetLocalizedSavingThrow(BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities.SavingThrow.WillPartial);
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            SilverCraneEndurance();
            void SilverCraneEndurance()
            {



                var stanceConfig = ManeuverConfigurator.NewStance(Main.Context, "SilverCraneEndurance", silverCrane, 6, x =>
                {

                    x.AddEffectFastHealing(5);
                });

                stanceConfig.ConfigureManeuver(Main.Context);
                
            }
            SilverCranesMercy();
            void SilverCranesMercy()
            {
                //TODO requires reaction-boosts!
            }


            #endregion

            #region level 7
            DiamondDisplacementStrike();
            void DiamondDisplacementStrike()
            {

                ActionsBuilder onHit = ActionsBuilder.New().SavingThrow(SavingThrowType.Will, onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().ApplyBuffPermanent("187f88d96a0ef464280706b63635f2af", isFromSpell: false), succeed: ActionsBuilder.New().ApplyBuff("df3950af5a783bd4d91ab73eb8fa0fd3", ContextDuration.Fixed(1), isFromSpell: false)));

                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "DiamondDisplacementStrike", silverCrane, 7, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddStrikeComponent();
                strikeConfig.AddPayload(onHit);
                strikeConfig.SetLocalizedSavingThrow(BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities.SavingThrow.WillPartial);
                
                strikeConfig.AddComponent<SilverCraneAntiFiendEffect>();
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            HolyRush();
            void HolyRush()
            {
                var config = ManeuverConfigurator.New(Main.Context, "HolyRush", silverCrane, 7, Backend.NewComponents.ManeuverBookSystem.ManeuverType.Boost, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift, false, Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional);
                config.AddAbilityCustomDimensionDoor();
                config.AddComponent<AbilityRestrictionAdjacentAlly>();
                config.SetRange(AbilityRange.Long);

                config.ConfigureManeuver(Main.Context);
            }
            DiamondTippedFeathers();
            void DiamondTippedFeathers()
            {
                //Condition removal mode requires UI?
                //Condition negator mode requires smart-logic?
                //TODO requires boost-as-interrupt
            }

            #endregion

            #region level 8
            CelestialPinions();
            void CelestialPinions()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "CelestialPinions", silverCrane, 8, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(15, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });
                strikeConfig.AddPayload(ActionsBuilder.New().Conditional(ManeuverConfigurator.SilverCraneSpecialTarget().HasFact("c4a7f98d743bc784c9d4cf2105852c39"), ifTrue: ActionsBuilder.New().SavingThrow(SavingThrowType.Will, onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().Kill(Kingmaker.UnitLogic.UnitState.DismemberType.None)))));
                strikeConfig.ConfigureManeuver(Main.Context);
            }
            DiamondWingsOfTheImperialCrane();
            void DiamondWingsOfTheImperialCrane()
            {



                var stanceConfig = ManeuverConfigurator.NewStance(Main.Context, "DiamondWingsOfTheImperialCrane", silverCrane, 8, x =>
                {

                    x.AddSpellResistance(value: ContextValues.Rank(AbilityRankType.Default));
                    x.AddBuffAllSavesBonus(ModifierDescriptor.Sacred, 4);
                    x.AddComponent<DWotICHealOnSpellResisted>();
                });
                stanceConfig.AddComponent<ContextRankConfig>(x =>
                {
                    ManeuverConfigurator.MakeScalingConfig(x, AbilityRankType.Default, 15, 1);
                });

                stanceConfig.ConfigureManeuver(Main.Context);
                //TODO
                //Using the perception of the disciple’s heavenly training to assist him, the Silver Crane practitioner may see many things which would remain hidden from the eyes of the impure. While in this stance, the initiator rolls twice on Perception checks or when attempting to pierce concealment, using the higher of the two rolls. Additionally, he may use detect evil as a spell-like ability at will with a caster level equal to his initiator level. 

                
            }
            BenedictionOfTheSilverCrane();
            void BenedictionOfTheSilverCrane()
            {
                //TODO requires boost-as-interrupt
            }

            #endregion

            #region level 9
            StrikeOfSilverExorcism();
            void StrikeOfSilverExorcism()
            {
                var strikeConfig = ManeuverConfigurator.NewStrike(Main.Context, "StrikeOfSilverExorcism", silverCrane, 9, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
                strikeConfig.AddStrikeComponent();
                strikeConfig.ApplyWeaponStrikeBits();
                strikeConfig.AddFixedEnergyDamageToStrike(0, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                }, flatdamage: 80);
                strikeConfig.AddFixedEnergyDamageToStrike(0, damageTypeDescription: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                }, flatdamage: 40, target: ManeuverConfigurator.SilverCraneSpecialTarget());
                strikeConfig.AddPayload(ActionsBuilder.New().ApplyBuff("df6d1025da07524429afbae248845ecc", durationValue: ContextDuration.Fixed(1), isFromSpell:false).Conditional(ManeuverConfigurator.SilverCraneSpecialTarget(), ifTrue: ActionsBuilder.New().SavingThrow(SavingThrowType.Will, onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().Kill(Kingmaker.UnitLogic.UnitState.DismemberType.None), succeed: ManeuverConfigurator.ApplyBuff("0bc608c3f2b548b44b7146b7530613ac", ManeuverConfigurator.InitiatorModifierRounds())))));
                strikeConfig.ConfigureManeuver(Main.Context);
            }

            #endregion



        }
    }
}
