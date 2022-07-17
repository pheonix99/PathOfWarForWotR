using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.NewComponents.ManeuverProperties;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent.Disciplines
{
    class SilverCrane
    {
        public static void Build()
        {
            var wingsicon = AssetLoader.LoadInternal(Main.Context, "", "Fly.png");

            DisciplineTools.AddDiscipline("SilverCrane", "Silver Crane", "Disciples of the Silver Crane are men and women for whom the power of the celestial and divine flow into the arts of their blade. The Silver Crane is a goodly discipline that is inspired by the teachings of celestials. It focuses on strong strikes designed to combat evil, celestial insights, and combat-predictions to defeat foes and enable the initiator and his allies to endure the hardships of battle against the forces of evil. Upon learning the art of Silver Crane, the disciple becomes in tune with the flows of the celestial realm, gaining heavenly insights into combat as if the angels themselves were granting insight to the warrior in battle. The Silver Crane discipline’s associated skill is Perception, and its associated weapon groups are bows, hammers, and spears.\n The discipline of Silver Crane is to be considered a supernatural discipline and all abilities within are considered supernatural abilities and follow the rules and restrictions of such. All abilities in this discipline carry the [good] descriptor. A character may always strike incorporeal foes as if they were corporea with strikes of this discipline.", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Bows, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Spears, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Hammers }, Kingmaker.EntitySystem.Stats.StatType.SkillPerception, wingsicon);
            DisciplineTools.Disciplines.TryGetValue("SilverCrane", out var silverCrane);

            SilverCraneWaltz();

            FlashingWings();
            void FlashingWings()
            {
                var strike = ManeuverTools.MakeStandardStrike(Main.Context, "FlashingWings", "Flashing Wings", "The flashing wings of the Silver Crane daze the foes of the martial disciple, burning their eyes with the light of Heaven. Make a melee attack and if successful, this attack inflicts an additional 1d4 points of damage and the target is dazzled for one round.", 1, silverCrane, extraDice: 1, diceSize: Kingmaker.RuleSystem.DiceType.D4, WeaponDamage: false, damageType: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                }, payload: ManeuverTools.ApplyBuff("df6d1025da07524429afbae248845ecc", ContextDuration.Fixed(1)));

                ManeuverTools.FinishManeuver(strike);
            }


            void SilverCraneWaltz()
            {
                var StanceOfTheDefendingShell = ManeuverTools.MakeSimpleStatUpStance(Main.Context, "SilverCraneWaltz", "Silver Crane Waltz", "The divine vantage point of the celestial realms infuses the mind of the disciple, granting him momentary flickers of foresight. A faint, ghostly image of wings is visible around the disciple but vanishes when he is looked upon directly. The initiator gains a +4 insight bonus to initiative checks if he is in this stance before combat begins, and a +2 insight bonus to Reflex saves and to AC. These bonuses increase by an additional +1 every eight levels after 1st level.", 1, silverCrane, Kingmaker.EntitySystem.Stats.StatType.AC, Kingmaker.Enums.ModifierDescriptor.Insight, 2, 8, out var IHSbuff);
                IHSbuff.AddComponent<AddContextStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveReflex;
                    x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Insight;
                    x.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus

                    };

                });
                IHSbuff.AddComponent<ContextRankConfig>(x =>
                {
                    x.m_Type = AbilityRankType.DamageDice;
                    x.m_Progression = ContextRankProgression.StartPlusDivStep;
                    
                        x.m_StartLevel = -32;
                    
                    x.m_StepLevel = 8;

                });
                IHSbuff.AddComponent<AddContextStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.Initiative;
                    x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Insight;
                    x.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice

                    };

                });
                ManeuverTools.FinishManeuver(StanceOfTheDefendingShell, Main.Context);
            }

            BlazingCranesWing();
            void BlazingCranesWing(){

                var boost = ManeuverTools.MakeBoostStub(Main.Context, "BlazingCranesWing", "Blazing Crane's Wing", "By filling the Silver Crane disciple’s weapon with the strength of his celestial patrons, the warrior funnels their righteous wrath against those that are abominations in the eyes of all that is good. The initiator’s attacks until his next turn inflict an additional 2d6 points of damage against undead or evil outsiders.", 2, silverCrane, out var buff, x =>
                {
                    x.AddComponent<AdditionalDiceOnAttack>(x =>
                    {
                        x.TargetConditions = ManeuverTools.SilverCraneSpecialTarget().Build();
                        x.DamageType = new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                        {
                            Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                            Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                        };
                        x.Value = new Kingmaker.UnitLogic.Mechanics.ContextDiceValue()
                        {
                            DiceType = Kingmaker.RuleSystem.DiceType.D6,
                            DiceCountValue = ContextValues.Constant(2)
                        };
                        x.m_RandomizeDamage = false;
                        x.m_DamageEntries = new List<AdditionalDiceOnAttack.DamageEntry>();
                    });
                });

                ManeuverTools.FinishManeuver(boost);
            };

            BlessedPinions();
            void BlessedPinions()
            {
                var strike = ManeuverTools.MakeStandardStrike(Main.Context, "BlessedPinions", "Blessed Pinions", "The pinion feathers of the Silver Crane’s wings gain an ethereal glimmer as if they were blessed steel. Heavenly agents guide their disciple’s attack so it may strike true as if it were blessed, even allowing it to strike the unseen. Make an attack against a foe that inflicts an additional 2d6 points of sacred damage and the attack is considered good aligned for the purposes of overcoming damage reduction. The disciple may also choose to strike incorporeal foes with this strike as if they were made manifest, including fiends who currently possess a creature. To strike a possessing fiend, the body they inhabit must be also struck, but all damage from the attack is inflicted upon the possessing fiend without harming the host.", 2, silverCrane, extraDice: 2, WeaponDamage: false, damageType: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });

                ManeuverTools.FinishManeuver(strike);
            }

            ExorcismStrike();
            void ExorcismStrike()
            {
                var strike = ManeuverTools.MakeStandardStrike(Main.Context, "ExorcismStrike", "Exorcism Strike", "The foes of the celestial realms tremble in fear at the wrath of the Heavens themselves, and a disciple of the Silver Crane wields that righteous anger in battle. This strike inflicts an additional 6d6 points of sacred damage to the foe if it is an undead creature or an outsider with the evil subtype, with a chance to daze the target for 1 round if it fails a Fortitude save (DC 13 + initiation modifier). Success negates the daze effect. If the target is a possessing entity within a host creature, the damage inflicted from this strike is solely inflicted upon the possessor, not the host. If the target is neither undead nor an evil outsider, this attack inflicts an additional 2d6 points of damage, and does not daze the target.", 3, silverCrane, extraDice: 4, WeaponDamage: false, damageType: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                }, payload: ManeuverTools.ApplyBuffIfNotSaved("9934fedff1b14994ea90205d189c8759", ContextDuration.Fixed(1), Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, ManeuverTools.SilverCraneSpecialTarget()));

                strike.GetComponent<AbstractBonusStrikeDamage>().targetCondition = ManeuverTools.SilverCraneSpecialTarget().Build();

                strike.AddComponent<FixedTypeBonusDamge>(x =>
                {
                    x.m_DiceType = Kingmaker.RuleSystem.DiceType.D6;
                    x.m_DiceCount = 2;
                    x.DamageTypeDescription = new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                    {
                        Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                        Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                    };
                });

                ManeuverTools.FinishManeuver(strike);
            }
            SacredPinions();
            void SacredPinions()
            {
                var strike = ManeuverTools.MakeStandardStrike(Main.Context, "SacredPinions", "Sacred Pinions", "The martial disciple enhances his melee strikes with the blessed wings of the Silver Crane, infusing them with celestial might to strike beyond flesh and bite spirit. Make an attack against a foe that inflicts an additional 5d6 points of damage. The initiator may also choose to strike incorporeal foes with this strike as if they were made manifest, including fiends who currently possess a creature. To strike a possessing fiend, the body they inhabit must be also struck, but all damage goes to the possessing fiend without harming the host.", 4, silverCrane, extraDice: 5, WeaponDamage: false, damageType: new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription()
                {
                    Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Energy,
                    Energy = Kingmaker.Enums.Damage.DamageEnergyType.Holy
                });

                ManeuverTools.FinishManeuver(strike);
            }
        }
    }
}
