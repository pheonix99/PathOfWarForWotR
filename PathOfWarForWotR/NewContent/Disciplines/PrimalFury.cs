using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.NewComponents.AbilityRestrictions;
using TheInfiniteCrusade.NewComponents.ManeuverProperties;
using TheInfiniteCrusade.Utilities;
using UnityEngine;
using Kingmaker.UnitLogic.Mechanics;
using TheInfiniteCrusade.Backend.NewActions;

namespace TheInfiniteCrusade.NewContent.Disciplines
{
    class PrimalFury
    {
        public static void BuildPrimalFury()
        {
            var bash = BlueprintTools.GetBlueprint<BlueprintBuff>("a1ffec0ce7c167a40aaea13dc49b757b");

            DisciplineTools.AddDiscipline("PrimalFury", "Primal Fury", "The way of the Primal Fury is a simple method of undeniable ferocity coupled with unstoppable aggression in the face of the enemy. By focusing the cold rage within a warrior’s heart and combining that power with calculated skill and intellect, the Primal Fury practitioner is a force of remorseless warfare that is capable of truly devastating shows of force. Learned by those emulating the hunt and attack methods of great cats, such as pumas, lions, leopards, and tigers, the early practitioners of this discipline spread throughout the world, teaching it nearly everywhere. Many even regard this discipline as the oldest of all disciplines. The disciples of the Primal Fury have a few unifying principles, however, and that is firstly survival. All disciples of the Primal Fury are survivors of hardships in battle, trading blows stoically and fighting on with indomitable will to live to fight again. This drive to victory makes many of them very taciturn, but others simply shrug off the specter of death and focus more on the moment. The associated skill for the Primal Fury discipline is Survival, and its associated weapon groups are axes, heavy blades, and hammers.", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Axes, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.BladesHeavy, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Hammers }, Kingmaker.EntitySystem.Stats.StatType.SkillLoreNature, bash.Icon);
            DisciplineTools.Disciplines.TryGetValue("PrimalFury", out var primalFury);

            CrushingBlow();
           void CrushingBlow()
            {
                var crushing = MakePrimalFiryStrike("CrushingBlow", "Crushing Blow", "By bringing his weapon high and bringing it down with ferocious finality, the disciple’s forceful blow is enough to weaken the defenses of his foe momentarily.The initiator makes an attack and if successful, the strike inflicts an additional 1d6 points of damage and may potentially render his foe flat-footed until its next turn on a failed Fortitude save(DC 11 + initiation modifier).", 1, extraDice: 1, payload: ManeuverTools.ApplyBuffIfNotSaved(buff: CommonBuffs.enforcedFlatfooted, ContextDuration.Fixed(1), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude));

                ManeuverTools.FinishManeuver(crushing, Main.Context);

            }

            PrimalWrath();
            void PrimalWrath()
            {
                var crushing = MakePrimalFiryStrike("PrimalWrath", "Primal Wrath","A wild but powerful strike is better than a frail and accurate one, says the disciple of Primal Fury.By sacrificing accuracy for potency, the disciple achieves his desired result.The initiator inflicts an additional 4 points of damage upon a successful attack, or an additional 6 points of damage if the initiator is wielding the weapon in two hands.", 1, extraDice: 0, flatDamage: 4, twohandBonusDamage: 2);

                ManeuverTools.FinishManeuver(crushing, Main.Context);

            }

            StanceOfAgression();
            void StanceOfAgression()
            {
                var stance = ManeuverTools.MakeSimpleDamageUpStance(Main.Context, "StanceOfAggression", "Stance Of Aggression", "By focusing his fury on the ending of his foes, the disciple forgoes finesse for devastating power. While in this stance, the initiator suffers a -2 penalty on his AC but any successful attack inflicts an additional 1d6 points of damage. This damage bonus increases by 1d6 every eight initiator levels.", 1, primalFury, 1, 8, out var buff);

                buff.AddComponent<AddStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                    x.Descriptor = Kingmaker.Enums.ModifierDescriptor.UntypedStackable;
                    x.Value = -2;
                });

                ManeuverTools.FinishManeuver(stance, Main.Context);

            }
            CripplingStrike();
            void CripplingStrike()
            {
                var crushing = MakePrimalFiryStrike("CripplingStrike", "Crippling Strike", "By specifically targeting a victim’s most vulnerable areas, the disciple is capable of rending arterial injuries into his foe. The initiator initiating this maneuver inflicts an additional 2d6 points of damage upon a successful melee attack, and his victim gains the bleeding condition suffering 1d4 points of damage per round until the application of any spell or effect that cures hit point damage or someone staunches the blood with a DC 15 Heal check.", 2, extraDice: 2, payload: ManeuverTools.ApplyBuffForever(buff: "5eb68bfe186d71a438d4f85579ce40c1") );

                ManeuverTools.FinishManeuver(crushing, Main.Context);

            }

            DevastatingRush();
            void DevastatingRush()
            {
                var crushing = MakePrimalFiryStrike("DevastatingRush", "Devastating Rush", "The disciple performing this attack executes a powerful attack that rushes in past a foe’s defenses to shatter them completely. The initiator inflicts an additional 2d6 points of damage upon a successful attack and his strike ignores the damage reduction of a subject or the hardness of an object he is attacking.", 2, extraDice: 2, allDamageignoresDR: true);

                ManeuverTools.FinishManeuver(crushing, Main.Context);

            }

            FrenzyStrike();
            void FrenzyStrike()
            {
                var crushing = MakePrimalFiryStrike("FrenzyStrike", "Frenzy Strike", "By emulating the ferocious assault of a raging great cat, the disciple’s knowledge of the natural world and the way its predators operate assists him in combat with a furious set of attacks. The initiator makes one attack with each weapon he has wielded (or with both heads of a double weapon; this includes natural attacks and armor or shield spikes) at his full base attack bonus. Upon a each successful attack, the initiator inflicts an additional 2d6 points of damage.", 3, extraDice: 2, mode: MartialAttackMode.EveryWeapon );

                ManeuverTools.FinishManeuver(crushing, Main.Context);

            }

            FuriousPrimalWrath();
            void FuriousPrimalWrath()
            {
                var crushing = MakePrimalFiryStrike("FuriousPrimalWrath", "Furious Primal Wrath", "The disciple’s attacks grow strong with practiced fury and primal power, forgoing accuracy for pure deadly force. The initiator suffers a -4 penalty to his attack roll, and makes an attack against a target creature. If successful, this attack inflicts an additional 20 points of damage, or 35 if the weapon is wielded in two hands.", 4, toHitShift: -4, extraDice: 0, flatDamage: 20, twohandBonusDamage: 15);

                ManeuverTools.FinishManeuver(crushing, Main.Context);

            }

            ImpalingStrike();
            void ImpalingStrike()
            {
                var crushing = MakePrimalFiryStrike("ImpalingStrike", "Impaling Strike", "The disciple who has reached this level of mastery in the art of Primal Fury has learned that by striking to a foe’s heart and center with a singular but powerful blow, it causes the foe’s ability to withstand punishment to falter. The initiator makes an attack against his target and if successful, inflicts an additional 4d6 points of damage to this attack which ignores damage reduction. The target must make a Fortitude save (DC 14 + primary initiation modifier) or suffer an additional 1d4 points of Constitution damage from being hit in a vital area. Targets immune to critical hits are not subject to this Constitution damage.", 4, extraDice: 4, strikeDamageIgnoresDR: true, payload: ActionsBuilder.New().Conditional(conditions:ManeuverTools.CrittableTargetsOnly(), ifTrue: ActionsBuilder.New().DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Constitution, new ContextDiceValue()
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.D4,
                    DiceCountValue = 1,
                    BonusValue = 0

                })));

                ManeuverTools.FinishManeuver(crushing, Main.Context);

            }
            CorneredFrenzyStrike();
            void CorneredFrenzyStrike()
            {
                var crushing = ManeuverTools.MakeWhirlwindStrike(Main.Context, "CorneredFrenzyStrike", "Cornered Frenzy Strike", "As an animal cornered lashes out at all comers, so does the Primal Fury disciple when he finds himself surrounded. By using his intuition and striking where he feels a foe to be, he uses his instincts as a powerful weapon. As a full attack action, the initiator makes an attack roll at full base attack bonus against each target within his reach with each weapon he is currently wielding (including natural attacks; armor and shield spikes always count as wielded weapons), and each successful attack inflicts an additional 4d6 points of damage. After initiating this maneuver, the initiator’s focus on offense causes his AC to suffer a -2 penalty until his next turn.", 5, primalFury, extraDice: 4, fullRound: true, mode: MartialAttackMode.EveryWeapon);

                ManeuverTools.FinishManeuver(crushing, Main.Context);
            }
            BloodSprayStrike();
            void BloodSprayStrike()
            {
                var crushing = MakePrimalFiryStrike("BloodSprayStrike", "Blood-Spray Strike", "The Primal Fury disciple makes a chest-ruining arterial strike on his foe, causing the enemy to suffer grievous amounts of injury. The initiator makes an attack against a target foe and if successful, inflicts an additional 8d6 points of damage and inflicts 2d4 points of Constitution damage due to striking vital areas and organs. If the target is immune to critical hits, the target does not suffer this Constitution damage.", 7, extraDice: 8, strikeDamageIgnoresDR: false, payload: ActionsBuilder.New().Conditional(conditions: ManeuverTools.CrittableTargetsOnly(), ifTrue: ActionsBuilder.New().DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Constitution, new ContextDiceValue()
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.D4,
                    DiceCountValue = 2,
                    BonusValue = 0

                })));

                ManeuverTools.FinishManeuver(crushing, Main.Context);

            }


            BlueprintAbility MakePrimalFiryStrike(string sysName, string displayName, string desc, int level, int flatDamage = 0, int twohandBonusDamage = 0, int extraDice = 0, ActionsBuilder payload = null, int toHitShift = 0, Sprite icon = null, bool allDamageignoresDR = false, bool strikeDamageIgnoresDR = false, MartialAttackMode mode = MartialAttackMode.Normal, bool canRetarget =false )
            {
                var ability = ManeuverTools.MakeStandardStrike(Main.Context, sysName, displayName, desc, level, primalFury, extraDice: extraDice, payload: payload, icon: icon, allDamageIgnoresDr: allDamageignoresDR, mode: mode, toHitShift: toHitShift, strikeDamageIgnoresDr: strikeDamageIgnoresDR, canRetarget: canRetarget);
               
                ability.AddComponent<ManeuverRangeRestriction>();
                if (flatDamage > 0)
                {
                    ability.AddComponent<WeaponBonusDamage>(x =>
                    {
                        x.m_FlatDamage = flatDamage;
                        x.ExtraDamageOnTwoHands = twohandBonusDamage;
                    });
                }

                return ability;


            }
        }

        
    }
}
