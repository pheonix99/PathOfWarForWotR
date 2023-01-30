using System;
using System.Collections.Generic;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Properties;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Core.Utilities;
using PathOfWarForWotR.Backend.NewActions;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewComponents;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Defines;
using PathOfWarForWotR.Utilities;
using Kingmaker.Localization;

namespace PathOfWarForWotR.NewContent.Feats.MartialFeats
{
    class MartialTraining
    {
        static BlueprintFeatureSelectionReference disciplineSelector;
        static BlueprintFeatureSelectionReference maneuverLearnRef;
        static BlueprintFeatureSelectionReference stanceLearnRef;
        static BlueprintManeuverBookReference bookRef;
        public static void Build()
        {

            LocalizationTool.LoadLocalizationPack("Mods\\PathOfWarForWotR\\Localization\\MartialTraining.json");
            var slots = BuildMTSlotsFeature();
            BlueprintFeatureReference BuildMTSlotsFeature()
            {
                string sysName = "MartialTrainingAddManeuverSlotFeature";
                string displayName = "Add Martial Training Readied Maneuver";
                string discription = $"";
                var feature = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, sysName, x =>
                {
                    x.SetNameDescription(Main.Context, displayName, discription);
                    x.IsClassFeature = true;
                    x.Ranks = 50;
                    x.HideInUI = true;
                    x.HideInCharacterSheetAndLevelUp = true;

                });

                return feature.ToReference<BlueprintFeatureReference>();
            }

            var initatorLevelProp = Helpers.CreateBlueprint<BlueprintUnitProperty>(Main.Context, "MartialTrainingInitiatorLevelProperty", x => {
                x.AddComponent<CompositeCustomPropertyGetter>(x =>
                {
                    x.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    x.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[]
                    {
                        new CompositeCustomPropertyGetter.ComplexCustomProperty()
                        {
                            Property = new SimplePropertyGetter()
                            {
                                Property = UnitProperty.Level
                            },
                            Numerator = 1,
                            Denominator = 2

                        }
                    };

                });
            });
            var slotsProp = Helpers.CreateBlueprint<BlueprintUnitProperty>(Main.Context, "MartialTrainingSlotsProperty", x => {
                x.AddComponent<FactRankGetter>(x =>
                {
                    x.m_Fact = slots.Get().ToReference<BlueprintUnitFactReference>();
                });
            });

            var book = Helpers.CreateBlueprint<BlueprintManeuverBook>(Main.Context, "MartialTrainingManeuverBook", x =>
            {
                x.Name = LocalizationTool.CreateString("MartialTrainingManeuverBook.Name", "Martial Training", false);
                x.m_ManeuverSlotsReference = slotsProp.ToReference<BlueprintUnitPropertyReference>();
                x.BookType = BlueprintManeuverBook.ManeuverBookType.MartialTraining;
            });

            bookRef = book.ToReference<BlueprintManeuverBookReference>();

            var disciplineSelectConfig =  MoreFeatTools.MakeFeatureSelector(Main.Context, "MartialTrainingDisciplineSelector", "Select Discipline", "Select a martial discipline to learn with Martial Training");

          
            disciplineSelector = disciplineSelectConfig.Configure().ToReference<BlueprintFeatureSelectionReference>();

            var recoverConfig = AbilityTools.MakeAbility(Main.Context, "MartialTrainingRecovery", "Recover Maneuver", "You may recover one martial training maneuver by expending a full round action to recover it.", Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard, Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.Extraordinary, Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self, false);
            recoverConfig.SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal);
            recoverConfig.AddComponent<RecoverSelectedManeuver>(x=> {
                x.m_maneuverBook = book.ToReference<BlueprintManeuverBookReference>();
            });
            recoverConfig.SetIcon(ConstructionAssets.itemBondSprite);
            var recovery = recoverConfig.Configure();

            BlueprintFeatureReference StatInt()
            {
                var feature = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MartialTrainingIntFeature", x =>
                {
                    x.SetNameDescription(Main.Context, "Martial Training (Int)", "Select Intelligence as your Martial Training initiator stat;");
                    x.IsClassFeature = true;
                    x.AddComponent<SetStatOverrideComponent>(x =>
                    {
                        x.m_book = bookRef;
                        x.stat = Kingmaker.EntitySystem.Stats.StatType.Intelligence;
                    });
                    x.AddComponent<PrerequisiteStatValue>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.Intelligence;
                        x.Value = 11;
                    });
                    x.AddComponent<AddMartialTrainingPartComponent>(x => { x.m_ManeuverBook = bookRef; });


                });
                return feature.ToReference<BlueprintFeatureReference>();
            }
            BlueprintFeatureReference StatWis()
            {
                var feature = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MartialTrainingWisFeature", x =>
                {
                    x.SetNameDescription(Main.Context, "Martial Training (Wis)", "Select Wisdom as your Martial Training initiator stat;");
                    x.IsClassFeature = true;
                    x.AddComponent<SetStatOverrideComponent>(x =>
                    {
                        x.m_book = bookRef;
                        x.stat = Kingmaker.EntitySystem.Stats.StatType.Wisdom;
                    });
                    x.AddComponent<PrerequisiteStatValue>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.Wisdom;
                        x.Value = 11;
                    });
                    x.AddComponent<AddMartialTrainingPartComponent>(x => { x.m_ManeuverBook = bookRef; });


                });
                return feature.ToReference<BlueprintFeatureReference>();
            }


            BlueprintFeatureReference StatCha()
            {
                var feature = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MartialTrainingChaFeature", x =>
                {
                    x.SetNameDescription(Main.Context, "Martial Training (Cha)", "Select Charisma as your Martial Training initiator stat;");
                    x.IsClassFeature = true;
                    x.AddComponent<SetStatOverrideComponent>(x =>
                    {
                        x.m_book = bookRef;
                        x.stat = Kingmaker.EntitySystem.Stats.StatType.Charisma;
                    });
                    x.AddComponent<PrerequisiteStatValue>(x =>
                    {
                        x.Stat = Kingmaker.EntitySystem.Stats.StatType.Charisma;
                        x.Value = 11;
                    });
                    x.AddComponent<AddMartialTrainingPartComponent>(x => { x.m_ManeuverBook = bookRef; });


                });
                return feature.ToReference<BlueprintFeatureReference>();
            }
                
            
            var statSelectConfig = MoreFeatTools.MakeFeatureSelector(Main.Context, "MartialTrainingStatSelector", "Select Stat", "Select a mental stat to use with Martial Training");
            statSelectConfig.AddToAllFeatures(StatInt(), StatWis(), StatCha());


            var statSelector = statSelectConfig.Configure().ToReference<BlueprintFeatureSelectionReference>();
            var stanceLearn = BuildAdditionalManeuverLearner(true);
            var maneuverLearn = BuildAdditionalManeuverLearner(false);
            maneuverLearnRef = maneuverLearn.ToReference<BlueprintFeatureSelectionReference>();
            stanceLearnRef = stanceLearn.ToReference<BlueprintFeatureSelectionReference>();


            BlueprintFeatureSelection BuildAdditionalManeuverLearner(bool stance)
            {
                string sysName = "MartialTraining" + (stance ? "Stance" : "Manuever") + "LearnSelector";

                var feature = Helpers.CreateBlueprint<BlueprintFeatureSelection>(Main.Context, sysName, x =>
                {
                    x.SetName(Main.Context, "Learn " + (stance ? "Stance" : "Manuever"));
                    x.SetDescription(Main.Context, "Learn " + (stance ? "Stance" : "Manuever"));
                    x.IsClassFeature = true;
                    x.AddComponent<ManeuverSelectorMenuComponent>(x =>
                    {
                        x.targetBook = book.ToReference<BlueprintManeuverBookReference>();
                        x.SelectionMode = ManeuverSelectionMode.MartialTraining;
                    });
                    x.Ranks = 50;
                    x.Obligatory = false;

                });

                Main.LogPatch(feature);
                return feature;


            }
            MartialTrainingProgression();
            BlueprintFeature MartialTrainingProgression()
            {
                var guid = Main.Context.Blueprints.GetGUID("MartialTrainingProgression");
                var config = ProgressionConfigurator.New("MartialTrainingProgression", guid.ToString());
                config.SetDisplayName("MartialTraining1.Name");
                config.SetDescription("MartialTraining1.Desc");
                config.AddToFeaturesRankIncrease("MartialTrainingProgression");
                config.AddToLevelEntries(1, disciplineSelector.Guid.ToString(), statSelector.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString());
                config.AddToLevelEntries(2, slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString(), stanceLearnRef.Guid.ToString());
                config.AddToLevelEntries(3, slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString(), stanceLearnRef.Guid.ToString());
                config.AddToLevelEntries(4, slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString());
                config.AddToLevelEntries(5, slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString(), stanceLearnRef.Guid.ToString());
                config.AddToLevelEntries(6, slots.Guid.ToString(), maneuverLearnRef.Guid.ToString());
                config.SetHideInUI(false);
                config.SetHideInCharacterSheetAndLevelUp(false);
                config.SetClasses(new Blueprint<BlueprintCharacterClassReference>[] { });
                config.SetGroups(FeatureGroup.CombatFeat, FeatureGroup.Feat);
                var made = config.Configure();

                var feature = FeatureConfigurator.For(made);

                
                feature.AddPrerequisiteCharacterLevel(1);
                
                feature.AddComponent<AddMartialTrainingRankComponent>();
                feature.AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Intelligence, 3);
                feature.AddPrerequisiteIsPet(not: true);
                return feature.Configure();
            }

            //var level1 = MartialTraining1();

            BlueprintFeature MartialTraining1()
            {
                var featureConfig = MoreFeatTools.MakeFeature(Main.Context, "MartialTraining1Feature", "Martial Training I", "Select a martial discipline. The associated skill for this discipline is now a class skill. Your initiation modifier is chosen from Intelligence, Wisdom, or Charisma. Your martial initiator level maneuvers granted by this feat (and subsequent Martial Training feats) is equal to half your character level + your initiation modifier. You may select any two maneuvers from the 1st level maneuvers from this discipline, and you may ready one of your maneuvers for use. You may recover one maneuver by expending a full round action to recover it./nSpecial: If you ever gain levels in a martial adept class or possess them previously, these maneuvers continue to use their own initiator level and recovery method, independent of your martial adept level. Those wishing to add new maneuvers from a discipline that is already available to their class should instead select the Advanced Study feat instead.", true, featureGroups: new FeatureGroup[] { FeatureGroup.CombatFeat, FeatureGroup.Feat });
                featureConfig.AddPrerequisiteCharacterLevel(1);
                //featureConfig.AddComponent<AddMartialTrainingPartComponent>(x => { x.m_ManeuverBook = bookRef; });
                featureConfig.AddComponent<AddMartialTrainingRankComponent>();

                featureConfig.AddFeatureOnApply(disciplineSelector.Guid.ToString());
                featureConfig.AddFeatureOnApply(statSelector.Guid.ToString());
                featureConfig.AddFeatureOnApply(maneuverLearnRef.Guid.ToString());
                featureConfig.AddFeatureOnApply(maneuverLearnRef.Guid.ToString());
                //featureConfig.AddFacts(facts: new() { disciplineSelector.Guid.ToString(), statSelector.Guid.ToString(), slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString(), recovery });
              
                featureConfig.AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Intelligence, 3);
                featureConfig.AddPrerequisiteIsPet(not: true);



                return featureConfig.Configure(); 
            }

            var level2 = MartialTraining2();
            BlueprintFeature MartialTraining2()
            {
                var featureConfig = MoreFeatTools.MakeFeature(Main.Context, "MartialTraining2Feature", "Martial Training II", "You may select two new maneuvers and one stance from your chosen discipline of up to 2nd level, and you may ready an additional maneuver. You must meet the minimum initiator level to select any maneuver.", true, featureGroups: new FeatureGroup[] { FeatureGroup.CombatFeat, FeatureGroup.Feat });
                featureConfig.AddPrerequisiteCharacterLevel(5);
                featureConfig.AddComponent<AddMartialTrainingRankComponent>();
                


              
              
                featureConfig.AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Intelligence, 3);
                featureConfig.AddPrerequisiteIsPet(not: true);



               // featureConfig.AddFacts(facts: new() { slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString(), stanceLearnRef.Guid.ToString() });

                //featureConfig.AddPrerequisiteFeature("MartialTraining1Feature");
                featureConfig.AddPrerequisiteFeature("MartialTrainingProgression");
              

                var feature = featureConfig.Configure();

                FeatureConfigurator.For("MartialTrainingProgression").SetIsPrerequisiteFor("MartialTraining2Feature").Configure();
                ProgressionConfigurator.For("MartialTrainingProgression").AddToFeaturesRankIncrease(feature).Configure();

                return feature;
            }

            var level3 = MartialTraining3();
            BlueprintFeature MartialTraining3()
            {

                var featureConfig = MoreFeatTools.MakeFeature(Main.Context, "MartialTraining3Feature", "Martial Training III", "You may select an additional new maneuver from your chosen discipline of up to 3rd level, plus one new stance, and you may ready an additional maneuver. You must meet the minimum initiator level to select any maneuver.", true, featureGroups: new FeatureGroup[] { FeatureGroup.CombatFeat, FeatureGroup.Feat });
                featureConfig.AddPrerequisiteCharacterLevel(7);
                featureConfig.AddComponent<AddMartialTrainingRankComponent>();
                featureConfig.AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Intelligence, 3);
                featureConfig.AddPrerequisiteIsPet(not: true);
                //featureConfig.AddFacts(facts: new() { slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString(), stanceLearnRef.Guid.ToString() });
                featureConfig.AddPrerequisiteFeature("MartialTraining2Feature");
            
                featureConfig.AddPrerequisiteIsPet(not: true);
                var feature = featureConfig.Configure();

                FeatureConfigurator.For("MartialTraining2Feature").SetIsPrerequisiteFor("MartialTraining3Feature").Configure();
                ProgressionConfigurator.For("MartialTrainingProgression").AddToFeaturesRankIncrease(feature).Configure();
                return feature;

                
            }

            var level4 = MartialTraining4();
            BlueprintFeature MartialTraining4()
            {
                var featureConfig = MoreFeatTools.MakeFeature(Main.Context, "MartialTraining4Feature", "Martial Training IV", "You may select two new maneuvers from your chosen discipline of up to 4th level, and you may ready an additional maneuver. You must meet the minimum initiator level to select any maneuver.", true, featureGroups: new FeatureGroup[] { FeatureGroup.CombatFeat, FeatureGroup.Feat });
                featureConfig.AddPrerequisiteCharacterLevel(9);
                featureConfig.AddComponent<AddMartialTrainingRankComponent>();
                featureConfig.AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Intelligence, 3);
                featureConfig.AddPrerequisiteIsPet(not: true);
               // featureConfig.AddFacts(facts: new() { slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString() });
                featureConfig.AddPrerequisiteFeature("MartialTraining3Feature");
            
                var feature = featureConfig.Configure();

                FeatureConfigurator.For("MartialTraining3Feature").SetIsPrerequisiteFor("MartialTraining4Feature").Configure();
                ProgressionConfigurator.For("MartialTrainingProgression").AddToFeaturesRankIncrease(feature).Configure();
                return feature;

                
            }

            var level5 = MartialTraining5();
            BlueprintFeature MartialTraining5()
            {
                

                var featureConfig = MoreFeatTools.MakeFeature(Main.Context, "MartialTraining5Feature", "Martial Training V", "You may select two new maneuvers or one new maneuver and one new stance from your chosen discipline of up to 5th level, and you may ready an additional maneuver. You must meet the minimum initiator level to select any maneuver.", true, featureGroups: new FeatureGroup[] { FeatureGroup.CombatFeat, FeatureGroup.Feat });
                featureConfig.AddPrerequisiteCharacterLevel(11);
                featureConfig.AddComponent<AddMartialTrainingRankComponent>();
                featureConfig.AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Intelligence, 3);
                featureConfig.AddPrerequisiteIsPet(not: true);
                //featureConfig.AddFacts(facts: new() { slots.Guid.ToString(), maneuverLearnRef.Guid.ToString(), maneuverLearnRef.Guid.ToString(), stanceLearnRef.Guid.ToString() });
                featureConfig.AddPrerequisiteFeature("MartialTraining4Feature");

                var feature = featureConfig.Configure();

                FeatureConfigurator.For("MartialTraining4Feature").SetIsPrerequisiteFor("MartialTraining5Feature").Configure();
                ProgressionConfigurator.For("MartialTrainingProgression").AddToFeaturesRankIncrease(feature).Configure();
                return feature;

                
            }

            
            var level6 = MartialTraining6();
            BlueprintFeature MartialTraining6()
            {
               


                

                var featureConfig = MoreFeatTools.MakeFeatureSelector(Main.Context, "MartialTraining6Feature", "Martial Training VI", "You may select two new maneuvers or one new maneuver and one new stance from your chosen discipline of up to 6th level, and you may ready an additional maneuver.  You must meet the minimum initiator level to select any maneuver.", true, featureGroups: new FeatureGroup[] { FeatureGroup.CombatFeat, FeatureGroup.Feat });
                featureConfig.AddToAllFeatures(maneuverLearnRef.Guid.ToString(), stanceLearnRef.Guid.ToString());
                
                featureConfig.AddPrerequisiteCharacterLevel(13);
                featureConfig.AddComponent<AddMartialTrainingRankComponent>();
                featureConfig.AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Intelligence, 3);
                featureConfig.AddPrerequisiteIsPet(not: true);
                //featureConfig.AddFacts(facts: new() { slots.Guid.ToString() });
                featureConfig.AddPrerequisiteFeature("MartialTraining5Feature");

                var feature = featureConfig.Configure();

                FeatureConfigurator.For("MartialTraining5Feature").SetIsPrerequisiteFor("MartialTraining6Feature").Configure();
                ProgressionConfigurator.For("MartialTrainingProgression").AddToFeaturesRankIncrease(feature).AddToLevelEntries(6, feature).Configure();
                return feature;

               
            }

            //FeatTools.AddAsFeat(level1, level2, level3, level4, level5, level6);

        }


        public static void Finish()
        {
            var disciplines = DisciplineTools.GetAll();

            List<BlueprintFeature> unlocks = new();
            foreach(var discipline in disciplines)
            {
                unlocks.Add(MakeUnlock(discipline));
            }

            disciplineSelector.Get().AddFeatures(unlocks.ToArray());

            BlueprintFeature MakeUnlock(DisciplineDefine unlock)
            {
                string systemName =  "MartialTrainingUnlock" + unlock.SysName + "Feature";


                var feature = Helpers.CreateDerivedBlueprint<BlueprintFeature>(Main.Context, systemName, unlock.masterGuid, new List<SimpleBlueprint>() { bookRef.Get() }, x =>
                {
                    x.SetNameDescription(Main.Context, unlock.DisplayName, $"Select {unlock.DisplayName} for MartialTtraining discipline.\n{unlock.Description}");
                    x.AddComponent<DisciplineUnlockForManeuverBookComponent>(x =>
                    {
                        x.bookRef = bookRef;
                        x.disciplineType = unlock.SysName;

                    });
                    x.m_Icon = unlock.defaultSprite;
                    

                });
                Main.LogPatch(feature);


                return feature;
            }

            HandleLearn();
            void HandleLearn()
            {
                var selector = stanceLearnRef.Get();
                selector.AddFeatures(ManeuverConfigurator.StanceLearnFeatures.ToArray());

                Main.Context.Logger.Log($"{selector.name} options length:{selector.m_Features.Length}");

                var selector2 = maneuverLearnRef.Get();
                selector2.AddFeatures(ManeuverConfigurator.ManeuverLearnFeatures.ToArray());
                Main.Context.Logger.Log($"{selector2.name} options length:{selector2.m_Features.Length}");
            }

        }

    }
}
