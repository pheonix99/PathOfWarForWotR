using Kingmaker.Blueprints.Classes;
using PathOfWarForWotR.Utilities;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.ModLogic;
using UnityEngine;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.RuleSystem.Rules;
using PathOfWarForWotR.Backend.NewActions;
using PathOfWarForWotR.Backend.NewComponents.AbilityRestrictions;
using PathOfWarForWotR.Backend.NewComponents.MartialAttackComponents;
using PathOfWarForWotR.Backend.NewComponents.AbilitySpecific;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Utils;

namespace PathOfWarForWotR.NewContent.Disciplines
{
    public static class BrokenBlade
    {
        public static void BuildBrokenBlade()
        {
            var ius = BlueprintTools.GetBlueprint<BlueprintFeature>("7d1bac926c0945a892bf0eda76004379");
            LocalizationTool.LoadLocalizationPack("Mods\\PathOfWarForWotR\\Localization\\BrokenBlade.json");
            DisciplineTools.AddDiscipline("BrokenBlade", "Broken Blade", "Legend has it the first practitioner of the Broken Blade style was a powerful swordsman who in the middle of a life-or-death duel with an old enemy found his sword broken by his opponent and had to toss it aside. Disheartened by his lack of weapons, he quickly realized that his years of training, exercise, and conditioning had made his body a weapon all on its own. Using only his fists and his nerve, this long-forgotten swordsman became the first to develop this discipline’s techniques, and he passed his experience on to others. Disciples of the Broken Blade teach these methods in monasteries, to cloistered warrior-monks who learn to operate without the use of traditional weapons of combat. Others learn from parents or individual mentors, haphazard or otherwise, and scrap their way through as it suits them. The Broken Blade’s associated skill is Acrobatics, and its associated weapon groups are close, monk, and natural. ", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Monk, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Close, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Natural }, Kingmaker.EntitySystem.Stats.StatType.SkillMobility, ius.Icon);
            DisciplineTools.Disciplines.TryGetValue("BrokenBlade", out var brokenBlade);

            #region level1 - CLEAR!
            IronHandStance();
            void IronHandStance()
            {
                var stance = ManeuverConfigurator.NewStance(Main.Context, "IronHandStance", brokenBlade, 1, x =>
                {
                    x.AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.AC, ContextValues.Rank(Kingmaker.Enums.AbilityRankType.Default), Kingmaker.Enums.ModifierDescriptor.Shield);
                    x.AddComponent<ContextRankConfig>(x => ManeuverConfigurator.MakeScalingConfig(x, Kingmaker.Enums.AbilityRankType.Default, 2, 6));

                });

                stance.ConfigureManeuver(Main.Context);

                
            }
            /*
            PugilistStance();
            void PugilistStance()
            {
                var PugilistStance = ManeuverTools.MakeSimpleDamageUpStance(Main.Context, "PugilistStance", "Pugilist Stance", "By adopting a powerful kickboxing stance, the initiator positions himself for lightning fast, potent strikes with his hands and feet. While in this stance, unarmed or discipline weapon strikes inflict an additional 1d6 points of damage, increasing to 2d6 at initator level 8 and another die every eight levels", 1, brokenBlade, baseValue: 1, levelsToIncrease: 8, out var IHSbuff);
                PugilistStance.SetLocalizedDuration(Main.Context, "");
                PugilistStance.SetLocalizedSavingThrow(Main.Context, "");
                ManeuverTools.FinishManeuver(PugilistStance, Main.Context);
            }

            FlurryStrike();
            void FlurryStrike()
            {
                var flurry = BrokenBladeStandardStrikeStrike(Main.Context, "FlurryStrike", "Flurry Strike", "The disciple of the Broken Blade learns to maximize openings in his opponent’s defenses and makes lightning fast attacks whenever possible. As a standard action, the initiator may make two attacks at his full base attack bonus.", 1, unarmedOnly: false, extraHits: 1, icon: ius.Icon);

                ManeuverTools.FinishManeuver(flurry, Main.Context);
            }

            PommelBash();
            void PommelBash()
            {
                var bash = BrokenBladeStandardStrikeStrike(Main.Context, "PommelBash", "Pommel Bash", "When watching a Broken Blade fighter work his art, most watch for the fists and feet. The disciple knows this, and surprises his foe, catching him unaware. The disciple makes a surprise elbow strike to the foe that leaves them reeling. The initiator makes an unarmed attack against the target’s flat-footed armor class, and the blow inflicts 1d6 points of additional damage. Creatures immune to sneak attacks and critical hits are unaffected by this extra damage.", 1, unarmedOnly: true, extraDice: 1, extraIsPrecision: true, forceFlatfoot: false, icon: ius.Icon);

                ManeuverTools.FinishManeuver(bash, Main.Context);
            }

            ShardsofIronStrike();
            void ShardsofIronStrike()
            {
                var shards = BrokenBladeStandardStrikeStrike(Main.Context, "ShardsofIronStrike", "Shards of Iron Strike", "By mimicking the speed and piercing power of the legendary shattered blade of the founder of this discipline, the disciple makes a hard jabbing strike at his opponent’s vulnerable spots for maximum pain. The initiator makes an attack against his target foe and if successful, the target is staggered for one round in addition to normal damage.", 1, unarmedOnly: false, payload: ManeuverTools.ApplyBuff("df3950af5a783bd4d91ab73eb8fa0fd3", ContextDuration.Fixed(1)), icon: ius.Icon);

                ManeuverTools.FinishManeuver(shards, Main.Context);
            }

            BrawlersAttitude();
            void BrawlersAttitude()
            {
                //TODO rework into toggle?
                var brawlers = ManeuverTools.MakeBoostStub(Main.Context, "BrawlersAttitude", "Brawler's Attitude", "By using the skill of the empty-handed warriors that came before him, the disciple focuses his will to using his body in less conventional ways in combat. When initiating this boost, the initiator gains a +4 competence bonus to his next CMB check when using his unarmed strike or discipline weapon to perform the following combat maneuvers: dirty trick, disarm, grapple, or trip.", 1, brokenBlade, out var buff, x =>
                {
                    x.AddComponent<ManeuverBonus>(x =>
                    {
                        x.Bonus = 4;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Competence;
                        x.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.Trip;

                    });
                    x.AddComponent<ManeuverBonus>(x =>
                    {
                        x.Bonus = 4;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Competence;
                        x.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm;

                    });
                    x.AddComponent<ManeuverBonus>(x =>
                    {
                        x.Bonus = 4;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Competence;
                        x.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickBlind;

                    });
                    x.AddComponent<ManeuverBonus>(x =>
                    {
                        x.Bonus = 4;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Competence;
                        x.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickEntangle;

                    });
                    x.AddComponent<ManeuverBonus>(x =>
                    {
                        x.Bonus = 4;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Competence;
                        x.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickSickened;

                    });
                    x.AddComponent<ManeuverBonus>(y =>
                    {
                        y.Bonus = 4;
                        y.Descriptor = Kingmaker.Enums.ModifierDescriptor.Competence;
                        y.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple;

                    });
                    x.AddComponent<ManeuverTrigger>(y =>
                    {
                        y.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.Trip;
                        y.Action = ActionsBuilder.New().RemoveBuff(buff: x).Build();

                    });
                    x.AddComponent<ManeuverTrigger>(y =>
                    {
                        y.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm;
                        y.Action = ActionsBuilder.New().RemoveBuff(buff: x).Build();

                    });
                    x.AddComponent<ManeuverTrigger>(y =>
                    {
                        y.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickBlind;
                        y.Action = ActionsBuilder.New().RemoveBuff(buff: x).Build();

                    });
                    x.AddComponent<ManeuverTrigger>(y =>
                    {
                        y.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickEntangle;
                        y.Action = ActionsBuilder.New().RemoveBuff(buff: x).Build();

                    });
                    x.AddComponent<ManeuverTrigger>(y =>
                    {
                        y.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickSickened;
                        y.Action = ActionsBuilder.New().RemoveBuff(buff: x).Build();

                    });
                    x.AddComponent<ManeuverTrigger>(y =>
                    {
                        y.ManeuverType = Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple;
                        y.Action = ActionsBuilder.New().RemoveBuff(buff: x).Build();

                    });
                   
                }, icon: ius.Icon);
            }
            */

            #endregion
            /*
            #region level 2

            KnuckleToTheBlade();
            void KnuckleToTheBlade()
            {
                var knuckle = BrokenBladeStandardStrikeStrike(Main.Context, "KnuckleToTheBlade", "Knuckle To The Blade", "By striking at the weapon-wielding arm of his opponent, the disciple of the Broken Blade can cleverly disarm his opponent and potentially even bring his foe’s weapon to bear against him. The initiator makes an attack as normal upon his foe, if successful, the initiator may make a free disarm attempt against the opponent without provoking attacks of opportunity.", 2, unarmedOnly: false, payload: ActionsBuilder.New().CombatManeuver(onSuccess: ActionsBuilder.New(), type: Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm), icon: ius.Icon);

                ManeuverTools.FinishManeuver(knuckle, Main.Context);
            }

            BronzeKnuckle();
            void BronzeKnuckle()
            {
                var bk = ManeuverTools.MakeBoostStub(Main.Context, "BronzeKnucke", "Bronze Knuckle", "With a spectacular crack of his knuckles, the disciple delivers an extra-potent blow to his foe in the form of a bone-crushing strike. As part of a unarmed attack or a strike from this discipline, the initiator’s attacks for the duration of his turn inflict an additional 2d6 points of damage and these attacks ignore a target’s damage reduction. ", 2, brokenBlade, out var buff, x =>
                {
                    x.AddComponent<BronzeKnuckleComponent>();
                }, icon: ius.Icon);

                ManeuverTools.FinishManeuver(bk, Main.Context);
            }

            LegSweepingHilt();
            void LegSweepingHilt()
            {
                var lsh = ManeuverTools.MakeStrikeStub(Main.Context,  "LegSweepingHilt", "Leg Sweeping Hilt", "With quick feint high, the disciple hooks his foot behind the leg of his foe and stiffens it like the hilt of a sword, drawing it back to knock his opponent down. The initiator makes a trip attempt with a +2 competence bonus which does not provoke attacks of opportunity. If successful, the initiator trips his opponent and may make an immediate attack at his full base attack bonus.", 2, brokenBlade);


                lsh = AbilityConfigurator.For(lsh).AddAbilityEffectRunAction(ActionsBuilder.New().CombatManeuver(type: Kingmaker.RuleSystem.Rules.CombatManeuver.Trip, onSuccess: ActionsBuilder.New().Add<ContextActionMartialAttack>(x=> { 
                    
                
                }))).Configure();
                lsh.AddBonusToCombatManeuversInAbility(2, Kingmaker.Enums.ModifierDescriptor.Competence, CombatManeuver.Trip);
                lsh.AddComponent<ManeuverRangeRestriction>(x =>
                {
                    x.Range = false;
                });

                lsh.AddComponent<WeaponBonusDamage>(x =>
                {
                    x.m_FlatDamage = 2;
                    

                });
                lsh.Range = AbilityRange.Weapon;
                lsh.CanTargetEnemies = true;
                lsh.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
                lsh.EffectOnEnemy = AbilityEffectOnUnit.Harmful;

                ManeuverTools.FinishManeuver(lsh, Main.Context);
            }

            #endregion


            #region level3
            SteelFlurryStrike();
            void SteelFlurryStrike()
            {
                var flurry = BrokenBladeStandardStrikeStrike(Main.Context, "SteelFlurryStrike", "Steel Flurry Strike", "The disciple makes a furious set of attacks upon his foe, hammering through defenses and striking rapidly. The initiator may make three attacks against his foe at full base attack bonus with a -2 penalty to hit. Successful hits inflict an additional 3d6 points of damage per hit.", 3, unarmedOnly: false, extraHits: 2, toHitShift: -2, extraDice: 3, icon: ius.Icon);

                ManeuverTools.FinishManeuver(flurry, Main.Context);
            }

            IronDust();
            void IronDust()
            {

            }

            BrokenBladeStance();
            void BrokenBladeStance()
            {
                var stance = ManeuverTools.MakeStanceStub(Main.Context, "BrokenBladeStance", "Broken Blade Stance", "The disciple of the Broken Blade has learned the flows of combat to a degree and has the insight necessary to see the path to victory through the patterns of steel his opponents weave before his eyes. While in this stance, the initiator gains a competence bonus equal to his initiator level to Acrobatics checks to avoid attacks of opportunity, and may make an additional attack when making a full attack action. If the initiator is two weapon fighting, he gains this extra attack for both hands. The attack(s) uses the initiator’s full base attack bonus, plus any modifiers appropriate to the situation.", 3, brokenBlade, out var buff);

                stance.AddComponent<BuffExtraAttack>(x =>
                {
                    x.Number = 1;
                    x.Haste = false;
                });
                //TODO ADD OFFHAND HIT

                //TODO ADD Acrobatics effect (HOW ON EARTH)

                ManeuverTools.FinishManeuver(stance, Main.Context);
            }

            #endregion

            #region level 4

            BrokenBladeRiposte();
            void BrokenBladeRiposte()
            {

            }

            IronAxeKick();
            void IronAxeKick()
            {
                var flurry = BrokenBladeStandardStrikeStrike(Main.Context, "IronAxeKick", "Iron Axe Kick", "The disciple leaps up into the air and raises his leg up to hammer it down in a bone-shattering axe kick. The initiator delivers a potent kick (as an unarmed strike) that inflicts an additional 6d6 points of damage. The target must make a Fortitude save (DC 14 + primary initiation modifier) or be dazed for 1d4 rounds.", 4, unarmedOnly: true, extraDice: 6, icon: ius.Icon, payload: ManeuverTools.ApplyBuffIfNotSaved("9934fedff1b14994ea90205d189c8759", new Kingmaker.UnitLogic.Mechanics.ContextDurationValue() { DiceType = Kingmaker.RuleSystem.DiceType.D4, BonusValue = 0, DiceCountValue = 1, m_IsExtendable = false, Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds }, Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude));

                ManeuverTools.FinishManeuver(flurry, Main.Context);
            }



            #endregion

            #region level5

            IronMongersThrow();
            void IronMongersThrow()
            {

            }

            ShardsOfSteelStrike();
            void ShardsOfSteelStrike()
            {
                var bleed = BuffTools.MakeBuff(Main.Context, "Bleed2d4Buff", "Bleed", "This creature takes {g|Encyclopedia:Dice}2d4{/g} hit point {g|Encyclopedia:Damage}damage{/g} each turn. Bleeding can be stopped through the application of any {g|Encyclopedia:Spell}spell{/g} that cures hit point damage (even if the bleed is {g|Encyclopedia:AbilityDamage}ability damage{/g}).", BlueprintTools.GetBlueprint<BlueprintBuff>("16249b8075ab8684ca105a78a047a5ef").Icon);
                var bleedActOnRound = ActionsBuilder.New().DealDamage(new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription() { Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Direct }, new ContextDiceValue()
                {
                    DiceCountValue = new ContextValue() { Value = 2 },
                    DiceType = Kingmaker.RuleSystem.DiceType.D6
                });
                var bleedmade = bleed.AddFactContextActions(newRound: bleedActOnRound).AddHealTrigger(action: ActionsBuilder.New().RemoveSelf(), allowZeroHealDamage: true, onHealDamage: true).Configure();

                var shards2 = BrokenBladeStandardStrikeStrike(Main.Context, "ShardsOfSteelStrike", "Shards Of Steel Strike", "By targeting vital soft tissues with a pointed, viper head-like finger jab, the disciple punctures flesh and releases the vital blood supply of his foe all over the ground in a deluge. The initiator makes an attack at a target creature, and if successful this strike inflicts an additional 8d6 points of damage which ignores damage reduction and the target suffers the bleeding condition, bleeding 2d4 points of damage per round for the initiator’s initiation modifier in rounds. A successful DC 20 Heal check or the application of any effect that cures hit point damage will stop the bleeding.", 5, unarmedOnly: false, extraDice: 8, strikeDamageIgnoresDr: true, payload: ManeuverTools.ApplyBuff(bleedmade, ManeuverTools.InitiatorModifierRounds(), ManeuverTools.LivingTargetsOnly()), icon: ius.Icon);

                ManeuverTools.FinishManeuver(shards2, Main.Context);
            }

            IronKnuckle();
            void IronKnuckle()
            {

            }

            #endregion


            #region level6

            SteelAxeKick();
            void SteelAxeKick()
            {
                var flurry = BrokenBladeStandardStrikeStrike(Main.Context, "SteelAxeKick", "Steel Axe Kick", "With a sweeping axe kick that strikes as hard as any hammer strike could ever be hoped to, the disciple of the Broken Blade lands a crushing blow that causes even the strongest of opponents to pause in pain. The initiator delivers a devastating spinning kick (as an unarmed strike) that inflicts an additional 10d6 points of damage and potentially dazes the target for 2d3 rounds on a failed Fortitude save (DC 16 + initiation modifier).", 6, unarmedOnly: true, extraDice: 10, icon: ius.Icon, payload: ManeuverTools.ApplyBuffIfNotSaved("9934fedff1b14994ea90205d189c8759", new Kingmaker.UnitLogic.Mechanics.ContextDurationValue() { DiceType = Kingmaker.RuleSystem.DiceType.D3, BonusValue = 0, DiceCountValue = 2, m_IsExtendable = false, Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds }, Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude));

                ManeuverTools.FinishManeuver(flurry, Main.Context);
            }

            PitFightersStance();
            void PitFightersStance()
            {

            }

            #endregion

            #region level 7

            SpinningFlurryRush();
            void SpinningFlurryRush()
            {

            }

            AdamantiteKnuckle();
            void AdamantiteKnuckle()
            {

            }

            ShardsOfAdamantiteStrike();
            void ShardsOfAdamantiteStrike()
            {
                var shards3 = BrokenBladeStandardStrikeStrike(Main.Context, "ShardsOfAdamantineStrike", "Shards of Adamantine Strike", "It is said that masters of the discipline are capable of using their hand in the manner of striking serpent to punch through solid stone as if it were straw, and disciples with this maneuver are those who can. Taking this training to the theater of war, a punch so potent that it can leave the foe sickened with pain is what this art form can deliver. The initiator makes an attack against the target creature and if successful, this attack inflicts an additional 12d6 points of damage that ignores the target’s damage reduction or an object’s hardness. If striking a living creature, the target must make a Fortitude save (DC 17 + initiation modifier) or be nauseated with the pain of the strike for 1d4 rounds.", 7, unarmedOnly: false, extraDice: 12, strikeDamageIgnoresDr: true, payload: ManeuverTools.ApplyBuffIfNotSaved("956331dba5125ef48afe41875a00ca0e", new ContextDurationValue()
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.D3,
                    BonusValue = 0,
                    DiceCountValue = 1,
                    m_IsExtendable = false,
                    Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds
                }, Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, ManeuverTools.LivingTargetsOnly()), icon: ius.Icon);

                ManeuverTools.FinishManeuver(shards3, Main.Context);
            }

            #endregion

            #region level8
            SpinningAdamantiteAxe();
            void SpinningAdamantiteAxe()
            {

            }

            MeteoricThrow();
            void MeteoricThrow()
            {
                
            }

            #endregion

            #region level 9

            StormOfIronFistsStrike();
            void StormOfIronFistsStrike()
            {

            };

            #endregion
            */
            BlueprintAbility BrokenBladeStandardStrikeStrike(ModContextBase context, string sysName, string displayName, string desc, int level, bool unarmedOnly, bool fullRound = false, MartialAttackMode mode = MartialAttackMode.Normal, int extraHits = 0, int extraDice = 0, bool strikeDamageIgnoresDr = false, int toHitShift = 0, bool allDamageIgnoresDr = false, bool extraIsPrecision = false, bool forceFlatfoot = false, ActionsBuilder payload = null, Sprite icon = null)
            {
                var abilty = ManeuverTools.MakeStandardStrike(context, sysName, displayName, desc, level, brokenBlade, mode, fullRound, extraHits: extraHits, extraDice: extraDice, Kingmaker.RuleSystem.DiceType.D6, WeaponDamage: true, VariableDamage: false, forceFlatfoot: forceFlatfoot, damageType: null, toHitShift: 0, allDamageIgnoresDr: allDamageIgnoresDr, payload: payload, forceUnarmed: unarmedOnly, extraIsPrecision: extraIsPrecision, strikeDamageIgnoresDr:strikeDamageIgnoresDr, icon: icon);
                /*abilty.AddComponent<AbilitySimpleMartialStrike>(x =>
                {


                });*/
                

                
                
                abilty.AddComponent<ManeuverRangeRestriction>(x =>
                {
                    x.Range = false;
                });

                abilty.AddComponent<WeaponBonusDamage>(x =>
                {
                    x.m_FlatDamage = 2;
                    if (strikeDamageIgnoresDr)
                        x.IgnoresDr = true;

                });
                
                

                return abilty;
            }

        }

    }
}
