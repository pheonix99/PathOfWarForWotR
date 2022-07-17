using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.NewComponents;
using TheInfiniteCrusade.NewComponents.AbilityRestrictions;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent.Disciplines
{
    class ScarletThrone
    {
        public static void BuildScarletThrone()
        {
            var duelIcon = BlueprintTools.GetBlueprint<BlueprintWeaponType>("a6f7e3dc443ff114ba68b4648fd33e9f").Icon;

            DisciplineTools.AddDiscipline("ScarletThrone", "Scarlet Throne", "The discipline of Scarlet Throne arose in the battling aristocracies of the world, where its nobles initially only practiced dueling styles that were of little use. When war came, these nobles found their abilities were sorely under-prepared for the rigors of true combat. Combining their roots in the dueling arts dueling and subsequent training by masters of practical combat and leadership, the Scarlet Throne style was born. Regal and unflinching, a practitioner of Scarlet Throne owns any field of battle he walks upon, for it is his court and there he rules, painting his chambers red with the blood of his enemies. The associated skill for this discipline is Sense Motive, and its associated weapon groups are heavy blades, light blades, and spears.", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.BladesHeavy, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.BladesLight, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Spears }, Kingmaker.EntitySystem.Stats.StatType.SkillPerception, duelIcon);
            DisciplineTools.Disciplines.TryGetValue("ScarletThrone", out var scarletThrone);

            BladeOfBreaking();
            void BladeOfBreaking()
            {
                var baseBlade = ManeuverTools.MakeStrikeStub(Main.Context, "BladeOfBreaking", "Blade Of Breaking", "A Scarlet Throne disciple knows that sometimes to defeat the beast, you must remove its claws. By clashing blades with a foe to set their weapon vibrating, the disciple then delivers a final smashing blow in an attempt to shatter it or knock it from their grasp. The disciple may make a disarm or sunder attempt with a +4 competence bonus against his foe’s weapon without provoking attacks of opportunity.", 1, scarletThrone);

                var disarmBlade = ManeuverTools.MakeStrikeStub(Main.Context, "BladeOfBreakingDisarm", "Blade Of Breaking (Disarm)", "A Scarlet Throne disciple knows that sometimes to defeat the beast, you must remove its claws. By clashing blades with a foe to set their weapon vibrating, the disciple then delivers a final smashing blow in an attempt to shatter it or knock it from their grasp. The disciple may make a disarm attempt with a +4 competence bonus against his foe’s weapon without provoking attacks of opportunity.", 1, scarletThrone);
                ApplyCommonParts(disarmBlade);

                disarmBlade = AbilityConfigurator.For(disarmBlade).AddAbilityEffectRunAction(ActionsBuilder.New().CombatManeuver(type: Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, onSuccess:ActionsBuilder.New())).Configure();
                disarmBlade.AddBonusToCombatManeuversInAbility(4, Kingmaker.Enums.ModifierDescriptor.Competence, CombatManeuver.Disarm);
                var sunderBlade = ManeuverTools.MakeStrikeStub(Main.Context, "BladeOfBreakingSunder", "Blade Of Breaking (Sunder)", "A Scarlet Throne disciple knows that sometimes to defeat the beast, you must remove its claws. By clashing blades with a foe to set their weapon vibrating, the disciple then delivers a final smashing blow in an attempt to shatter it or knock it from their grasp. The disciple may make a sunder attempt with a +4 competence bonus against his foe’s weapon without provoking attacks of opportunity.", 1, scarletThrone);

                ApplyCommonParts(sunderBlade);

                sunderBlade = AbilityConfigurator.For(sunderBlade).AddAbilityEffectRunAction(ActionsBuilder.New().CombatManeuver(type: Kingmaker.RuleSystem.Rules.CombatManeuver.SunderArmor, onSuccess: ActionsBuilder.New())).Configure();
                sunderBlade.AddBonusToCombatManeuversInAbility(4, Kingmaker.Enums.ModifierDescriptor.Competence, CombatManeuver.SunderArmor);
                baseBlade = AbilityConfigurator.For(baseBlade).AddAbilityVariants(new List<BlueprintCore.Utils.Blueprint<Kingmaker.Blueprints.BlueprintAbilityReference>>() { disarmBlade, sunderBlade }).Configure();




                void ApplyCommonParts(BlueprintAbility blueprintAbility)
                {
                    blueprintAbility.Range = AbilityRange.Weapon;
                    blueprintAbility.CanTargetEnemies = true;
                    blueprintAbility.Animation = Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Special;
                    blueprintAbility.EffectOnEnemy = AbilityEffectOnUnit.Harmful;
                }

                ManeuverTools.FinishManeuver(baseBlade, Main.Context);

            }

            PrincesAttitude();
            void PrincesAttitude()
            {
                var ability = ManeuverTools.MakeBoostStub(Main.Context, "PrincesAttitude", "Prince's Attitude", "The Scarlet Throne disciple knows that he is nobility in the savagery of war, and conducts himself as such when moving through melee. The initiator gains a +4 dodge bonus to his armor class when he moves through threatened areas and provokes attacks of opportunity and a +2 competence bonus to Reflex and Will saves until his next turn.", 1, scarletThrone, out var buff, x =>
                {
                    x.AddComponent<ACBonusAgainstAttacks>(x =>
                    {
                        x.OnlyAttacksOfOpportunity = true;
                        x.Value = 4;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Dodge;

                    });
                    x.AddComponent<AddStatBonus>(x =>
                    {
                        x.Value = 2;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Competence;
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveReflex;

                    }); x.AddComponent<AddStatBonus>(x =>
                    {
                        x.Value = 2;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Competence;
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveWill;

                    });
                });

                ManeuverTools.FinishManeuver(ability, Main.Context);
            }

            ScarletEinhander();
            void ScarletEinhander()
            {
                var stance = ManeuverTools.MakeStanceStub(Main.Context, "ScarletEinhander", "Scarlet Einhander", "By adopting this stance, the disciple presents a slim profile to his foes, holding his weapon low and one- handed, and leads with his blade in an elegant and graceful manner, his strikes strong and his profile aiding him in defense. When wielding a melee weapon in one hand, the initiator presents a slim profile with strong offensive and defensive ability. The initiator gains a +2 shield bonus to his Armor Class and inflicts an additional 1d6 points of damage while in this stance. The shield bonus granted by this stance increases by +1 when the character’s initiator level reaches 6th, and increases again by +1 at 12th and 18th initiator level. Upon reaching 10th initiator level, the bonus damage increases to 2d6.", 1, scarletThrone, out var buff);
                buff.AddScalingConfig(Kingmaker.Enums.AbilityRankType.DamageDice, 1, 10);
                buff.AddComponent<ContextAddWeaponDamageDice>(x =>
                {
                    x.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice

                    };
                    x.DealWeaponDamage = true;
                    
                });
                buff.AddScalingConfig(Kingmaker.Enums.AbilityRankType.DamageBonus, 2, 6);
                buff.AddComponent<AddContextStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                    x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Shield;
                    x.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus

                    };
                });
                stance.AddComponent<ScarletThroneNoShieldRule>(x => { x.AllowTwoHanderAtAll = false; });

                ManeuverTools.FinishManeuver(stance, Main.Context);
            }

            GarnetLance();
            void GarnetLance()
            {
                var gl = ManeuverTools.MakeStandardStrike(Main.Context, "GarnetLance", "Garnet Lance", "With a sudden and powerful thrust, the disciple penetrates the defenses of his foe with devastating alacrity. The initiator makes a melee attack against a target creature, and if successful the attack inflicts an additional 2d6 points of damage and the attack automatically bypasses the target’s damage reduction.", 2, scarletThrone, extraDice: 2, allDamageIgnoresDr: true);


                ManeuverTools.FinishManeuver(gl, Main.Context);

            }
            DazingAttack();
            void DazingAttack()
            {
                var gl = ManeuverTools.MakeStandardStrike(Main.Context, "DazingAttack", "Dazing Attack", "The swiftness of the Scarlet Throne disciple’s blade is a thing of terrific power, and by initiating this strike he demonstrates this potency. The disciple makes a melee attack against a target which if successful inflicts an additional 3d6 points of damage and upon a failed Fortitude save (DC 13 + initiation modifier) dazes the opponent for one round.", 3, scarletThrone, extraDice: 3, payload: ManeuverTools.ApplyBuffIfNotSaved("9934fedff1b14994ea90205d189c8759", durationValue: ContextDuration.Fixed(1), Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude));


                ManeuverTools.FinishManeuver(gl, Main.Context);

            }
            UnfetteredMovement();
            void UnfetteredMovement()
            {
                var stance = ManeuverTools.MakeStanceStub(Main.Context, "UnfetteredMovement", "Unfettered Movement", "The Scarlet Throne disciple is swift in battle and in his ability to go from one skirmish to the next to choose his next foe. By assuming this stance, the disciple gains a +10-ft. bonus to his base speed and gains a +4 dodge bonus to his armor class against attacks of opportunity.", 3, scarletThrone, out var buff);

                buff.AddComponent<ACBonusAgainstAttacks>(x =>
                {
                    x.OnlyAttacksOfOpportunity = true;
                    x.Value = 4;
                    x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Dodge;

                });
                buff.AddComponent<AddStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.Speed;
                    x.Value = 10;
                    x.Descriptor = ModifierDescriptor.UntypedStackable;

                });

                ManeuverTools.FinishManeuver(stance, Main.Context);

            }
            RiddleofIron();
            void RiddleofIron()
            {
                var gl = ManeuverTools.MakeStandardStrike(Main.Context, "RiddleOfIron", "Riddle of Iron", "With a flourishing of his blade, the Scarlet Throne disciple confuses the senses of his foe as he lands a strike from an unexpected angle.The initiator must make a melee attack against a foe, and if successful the strike inflicts an additional 5d6 points of damage and dazes the target for one round on a failed Will save(DC 15 + initiation modifier).", 5, scarletThrone, extraDice: 5, payload: ManeuverTools.ApplyBuffIfNotSaved("9934fedff1b14994ea90205d189c8759", durationValue: ContextDuration.Fixed(1), Kingmaker.EntitySystem.Stats.SavingThrowType.Will));


                ManeuverTools.FinishManeuver(gl, Main.Context);

            }

            BladeOfPerfection();
            void BladeOfPerfection()
            {
                var bop = ManeuverTools.MakeStandardStrike(Main.Context, "BladeOfPerfection", "Blade of Perfection", "In an instant of perfect clarity, the disciple finds his moment to strike true. The disciple simply strikes the opponent with deadly swiftness, penetrating defenses as if they were not even there. This attack automatically hits its target and ignores damage reduction. For the purposes of counters that have systems that oppose an attack roll, treat this attack as if the disciple had rolled a natural 20.", 6, scarletThrone, allDamageIgnoresDr: true, autoHit: true);
            }

            ScarletDuelistAttitude();
            void ScarletDuelistAttitude()
            {
                var stance = ManeuverTools.MakeStanceStub(Main.Context, "ScarletDuelistAttitude", "Scarlet Duelist Attitude", "The force of will that a disciple of Scarlet Throne possesses is enough to protect him and guide his steps through battle, and his battle focus allows him to anticipate danger before it happens. By adopting this stance, the disciple may add +5 insight bonus to his Armor Class, CMD, and to his Initiative score.", 6, scarletThrone, out var buff);

                
                buff.AddComponent<AddStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                    x.Value = 5;
                    x.Descriptor = ModifierDescriptor.Insight;

                });
                buff.AddComponent<AddStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalCMD;
                    x.Value = 5;
                    x.Descriptor = ModifierDescriptor.Insight;

                });
                buff.AddComponent<AddStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.Initiative;
                    x.Value = 5;
                    x.Descriptor = ModifierDescriptor.Insight;

                });
                ManeuverTools.FinishManeuver(stance, Main.Context);

            }

            SanguineProclamation();
            void SanguineProclamation()
            {
                var gl = ManeuverTools.MakeStandardStrike(Main.Context, "SanguineProclamation", "Sanguine Proclamation", "With a mighty overhand swing the Scarlet Throne disciple puts his foe in their place; on their knees before the majesty of a prince of warriors. The initiator must make a melee attack against a target creature which if successful inflicts an additional 10d6 points of damage and the target must make a successful Will save (DC 17 + initiation modifier) to resist being driven to its knees, knocked prone.", 7, scarletThrone, extraDice: 10, payload: ManeuverTools.ApplyBuffIfNotSaved("24cf3deb078d3df4d92ba24b176bda97", durationValue: ContextDuration.Fixed(1), Kingmaker.EntitySystem.Stats.SavingThrowType.Will));
                gl = AbilityConfigurator.For(gl).AddSpellDescriptorComponent(new Kingmaker.Blueprints.Classes.Spells.SpellDescriptorWrapper(Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.MindAffecting)).Configure();

                ManeuverTools.FinishManeuver(gl, Main.Context);

            }

            RiddleofSteel();
            void RiddleofSteel()
            {
                var gl = ManeuverTools.MakeStandardStrike(Main.Context, "RiddleofSteel", "Riddle of Steel", "With a swift and deadly strike, the disciple is capable of disabling a foe’s senses for a few moments, effectively eliminating the threat. The disciple makes a melee attack against his opponent which if successful deals an additional 10d6 points of damage and stuns his opponent for 1d4 rounds on a failed Will save (DC 18 + initiation modifier).", 8, scarletThrone, extraDice: 10, payload: ManeuverTools.ApplyBuffIfNotSaved("09d39b38bb7c6014394b6daced9bacd3", durationValue: new Kingmaker.UnitLogic.Mechanics.ContextDurationValue() { DiceType = Kingmaker.RuleSystem.DiceType.D4, BonusValue = 0, DiceCountValue = 1, m_IsExtendable = false, Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds }, Kingmaker.EntitySystem.Stats.SavingThrowType.Will));


                ManeuverTools.FinishManeuver(gl, Main.Context);

            }

            HeavenlyBladeoftheScarletThrone();
            void HeavenlyBladeoftheScarletThrone()
            {
                var gl = ManeuverTools.MakeStandardStrike(Main.Context, "HeavenlyBladeoftheScarletThrone", "Heavenly Blade of the Scarlet Throne", "The supreme technique of the Scarlet Throne, the disciple who knows of it is the lord of all battlefields and marks his name amongst the legends of war. The disciple makes a melee attack which if successful inflicts an additional 100 points of damage. The target must then make a Will save (DC 19 + initiation modifier) to resist becoming paralyzed for 1d4 rounds.", 8, scarletThrone, flatDamage: 100, payload: ManeuverTools.ApplyBuffIfNotSaved("af1e2d232ebbb334aaf25e2a46a92591", durationValue: new Kingmaker.UnitLogic.Mechanics.ContextDurationValue() { DiceType = Kingmaker.RuleSystem.DiceType.D4, BonusValue = 0, DiceCountValue = 1, m_IsExtendable = false, Rate = Kingmaker.UnitLogic.Mechanics.DurationRate.Rounds }, Kingmaker.EntitySystem.Stats.SavingThrowType.Will));


                ManeuverTools.FinishManeuver(gl, Main.Context);

            }
        }
            
    }

}
