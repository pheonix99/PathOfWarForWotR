using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Utilities;
using UnityEngine;

namespace TheInfiniteCrusade.NewContent.Disciplines
{
    class GoldenLion
    {
        public static void BuildGoldenLion()
        {
            var kishout = BlueprintTools.GetBlueprint<BlueprintAbility>("6e423d7de48eef74b999ebc8a62df8b8");

            DisciplineTools.AddDiscipline("GoldenLion", "Golden Lion", "The discipline of Golden Lion is a practice passed between war leaders, chieftains, generals, and militia leaders over the generations, meant to bring a group of warriors together into one cohesive unit. Golden Lion is a discipline that only greatly benefits a warrior who believes strongly in teamwork. The larger the group, the more who can benefit from the skilled leadership of a dedicated commander. Golden Lion benefits its practitioners indirectly, by aiding their allies instead. Because of this association with team work and working in groups with many differing people, the associated skill for this discipline is Diplomacy, and its associated weapon groups are heavy blades, hammers, and pole arms. ", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.BladesHeavy, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Hammers, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Polearms }, Kingmaker.EntitySystem.Stats.StatType.SkillPersuasion, kishout.Icon);
            DisciplineTools.Disciplines.TryGetValue("GoldenLion", out var goldenLion);
            #region level1
            EncouragingRoar();
            void EncouragingRoar()
            {
                var roar = MakeGoldenLionRoar("EncouragingRoar", "EncouragingRoar", "The disciple lets out shouts of encouragement to bolster his allies in battle. All allies within 30-ft. of the Golden Lion disciple gain a +2 morale bonus to attack and damage rolls for one round.", 1, 30, x =>
                {
                    x.AddComponent<AddStatBonus>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalAttackBonus;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                        x.Value = 2;
                    });
                    x.AddComponent<AddStatBonus>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.AdditionalDamage;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                        x.Value = 2;
                    });
                }, out var buff);
                ManeuverTools.FinishManeuver(roar);

            }

            DemoralizingRoar();
            void DemoralizingRoar()
            {

            }

            HuntingParty();
            void HuntingParty()
            {

            }

            PrideLeadersStance();
            void PrideLeadersStance()
            { 
            
            }

            #endregion

            #region level 2
            DefendingThePride();
            void DefendingThePride()
            {
                var roar = MakeGoldenLionRoar("DefendingThePride", "Defending the Pride", "With a cry to defend themselves from incoming attacks, the inspirational words of the disciple aid his allies in their defense. As a swift action, the initiator grants all allies within 60-ft. a +4 morale bonus to their AC for one round.", 2, 60, x =>
                {
                    x.AddComponent<AddStatBonus>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.AC;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                        x.Value = 4;
                    });
                    
                }, out var buff);
                ManeuverTools.FinishManeuver(roar);

            }

            DistractingStrike();
            void DistractingStrike()
            {

            }

            PyriteStrike();
            void PyriteStrike()
            {

            }

            #endregion

            #region level 3

            KillTheWounded();
            void KillTheWounded()
            {

            }

            PackPounce();
            void PackPounce()
            {

            }

            #endregion

            #region level4

            ChargeOfTheBattleCat();
            void ChargeOfTheBattleCat()
            {

            }

            GoldenLionCharger();
           void GoldenLionCharger()
            {

            }

            GoldenSwipe();
            void GoldenSwipe()
            {

            }
            #endregion

            #region level 5


            DisciplineOfThePride();
            void DisciplineOfThePride()
            {

            }
            GuardThePride();
            void GuardThePride()
            {

            }

            RoarOfBattle();
            void RoarOfBattle()
            {

            }

            StrategicBlow();
            void StrategicBlow()
            {

            }


            #endregion

            #region level 6

            GoldenGeneralsAttitude();
            void GoldenGeneralsAttitude()
                {

            }

            LionsFeast();
            void LionsFeast()
            {

            }


            #endregion

            #region level 7

            WarLionsCharge();
            void WarLionsCharge()
            {

            }

            #endregion

            #region level 8
            AlphasRoar();
            void AlphasRoar()
            {
                var roar = MakeGoldenLionRoar("AlphasRoar", "Alpha's Roar", "With an awesome cry for victory and bravery from his allies, the Golden Lion disciple bolsters allies’ defenses and attacks for a moment as his allies cannot help themselves but to win. Allies within 30-ft. of the disciple gain a +4 morale bonus to their saving throws and increase to the DC’s of their abilities (maneuvers, spells, powers, etc) for one round.", 4, 60, x =>
                {
                    x.AddComponent<AddStatBonus>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveFortitude;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                        x.Value = 4;
                    });
                    x.AddComponent<AddStatBonus>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveReflex;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                        x.Value = 4;
                    });
                    x.AddComponent<AddStatBonus>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveWill;
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                        x.Value = 4;
                    });
                    x.AddComponent<IncreaseAllSpellsDC>(x =>
                    {
                        x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                        x.Value = 4;
                        x.SpellsOnly = false;

                    });

                }, out var buff);
                ManeuverTools.FinishManeuver(roar);

            }

            LionLordsAgony();
            void LionLordsAgony()
            {

            }

            TriumphantLionsLeadership();
            void TriumphantLionsLeadership()
            {

            }

            #endregion

            #region level9

            LordOfThePridelands();
            void LordOfThePridelands()
            {

            }

            #endregion

            BlueprintAbility MakeGoldenLionRoar(string sysname, string displayName, string desc, int level, int radius, Action<BlueprintBuff> action, out BlueprintBuff buff, Sprite icon = null)
            {
                var ability = ManeuverTools.MakeManeuverStub(Main.Context, sysname, displayName, desc, NewComponents.MartialAbilityInformation.ManeuverType.Boost, level, goldenLion, icon);
                ability.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift;
                ability.AddComponent<AbilityTargetsAround>(x => {
                    x.m_Radius = new Kingmaker.Utility.Feet(radius);
                    x.m_TargetType = TargetType.Ally;
                });
                ability.Range = AbilityRange.Personal;
                buff = Helpers.CreateBlueprint<BlueprintBuff>(Main.Context, sysname + "Buff", x =>
                {
                    x.SetNameDescription(Main.Context, displayName, desc);
                    x.FxOnStart = new();
                    x.FxOnRemove = new();

                    action.Invoke(x);


                });

                return AbilityConfigurator.For(ability).AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff: buff, durationValue: ContextDuration.Fixed(1))).Configure();




            }
        }


    }
}
