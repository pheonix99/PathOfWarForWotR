using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PathOfWarForWotR.Backend.NewComponents;
using PathOfWarForWotR.Defines;
using PathOfWarForWotR.Utilities;

namespace PathOfWarForWotR.NewContent.Disciplines
{
    class SolarWind
    {
        public static void Make()
        {
            var sprite = BlueprintTool.Get<BlueprintAbility>("1fca0ba2fdfe2994a8c8bc1f0f2fc5b1").Icon;
            LocalizationTool.LoadLocalizationPack("Mods\\PathOfWarForWotR\\Localization\\SolarWind.json");
            DisciplineTools.AddDiscipline("SolarWind", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Bows, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Crossbows, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Thrown }, Kingmaker.EntitySystem.Stats.StatType.SkillPerception, sprite, out DisciplineDefine solarWind);
            #region level 1
            HorizonWindLancet();
            void HorizonWindLancet()
            {
                var boost = ManeuverConfigurator.NewNormalBoost(Main.Context, "HorizonWindLancet", solarWind, 1, x =>
                {
                    x.AddComponent<ContextAddWeaponDamageDice>(y =>
                    {
                        y.DealVariableTypeDamage = true;
                        y.Value = ContextValues.Constant(1);
                        y.diceType = Kingmaker.RuleSystem.DiceType.D6;
                        y.RangedOnly = true;
                        y.Once = true;
                    });

                });
                boost.ConfigureManeuver(Main.Context);
            }

            StanceOfPiercingRays();
            void StanceOfPiercingRays()
            {
                var stance = ManeuverConfigurator.NewStance(Main.Context, "StanceOfPiercingRays", solarWind, 1, x =>
                {
                    x.AddComponent<ContextAddWeaponDamageDice>(y =>
                    {
                        y.DealVariableTypeDamage = true;
                        y.Value = ContextValues.Rank(Kingmaker.Enums.AbilityRankType.DamageDice);
                        y.diceType = Kingmaker.RuleSystem.DiceType.D6;
                        y.RangedOnly = true;
                       
                    });
                    x.AddScalingConfig(Kingmaker.Enums.AbilityRankType.DamageDice, 1, 8);
                }, type: AbilityType.Supernatural);

                stance.ConfigureManeuver(Main.Context);
            }

            #endregion

            #region level 2
            SolarLance();
            void SolarLance()
            {
               //TODO requires decent bonus logic.
               //Can afford to skip for now
            }


            #endregion

            #region level 3
            

            #endregion
        }
    }
}
