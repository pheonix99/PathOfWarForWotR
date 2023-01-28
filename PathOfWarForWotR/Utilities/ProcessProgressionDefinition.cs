using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Core.Utilities;
using PathOfWarForWotR.Backend.NewActions;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewComponents;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Backend.NewComponents.Properties;
using PathOfWarForWotR.Defines;

namespace PathOfWarForWotR.Utilities
{
    public static class ProcessProgressionDefinition
    {
        private static List<InitiatorProgressionDefine> Defines = new();

        public static BlueprintProgression BuildInitiatorProgress(InitiatorProgressionDefine define)
        {
            string stringProgressionBlueprintSysName;
            var source = define.Source;
            if (define.maneuverBookType == BlueprintManeuverBook.ManeuverBookType.Level6Archetype)
            {
                stringProgressionBlueprintSysName = define.InitiatorSysNameBase + "InitiatorProgression";

            }
            else if (define.maneuverBookType == BlueprintManeuverBook.ManeuverBookType.Level9Class)
            {
                stringProgressionBlueprintSysName = define.InitiatorSysNameBase + "Progression";
            }
            else
                return null;

            Defines.Add(define);



            var progression = Helpers.CreateBlueprint<BlueprintProgression>(source, stringProgressionBlueprintSysName, x =>
            {
                x.SetName(source, define.DisplayName);
                x.SetDescription(source, "");
                x.HideInCharacterSheetAndLevelUp = true;
                x.IsClassFeature = true;
                if (define.maneuverBookType == BlueprintManeuverBook.ManeuverBookType.Level6Archetype)
                {
                    foreach (var v in define.ArchetypesForArchetypeTemplate)
                    {

                        x.AddArchetype(v);
                    }
                }
                foreach (var c in define.ClassesForClassTemplate)
                {
                    x.AddClass(c);
                }

            });

            var spellbookSysName = define.InitiatorSysNameBase + "ManeuverBook";

            var MLProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(source, define.InitiatorSysNameBase + "InitiatorLevelProperty", x =>
            {
                if (define.maneuverBookType == BlueprintManeuverBook.ManeuverBookType.Level6Archetype)
                {
                    x.AddComponent<ArchetypeInitiatorLevelPropertyGetter>(x =>
                    {
                        x.m_Archetypes = define.ArchetypesForArchetypeTemplate.ToArray();
                        x.m_Classes = define.ClassesForClassTemplate.ToArray();

                    });

                }
                else
                {
                    x.AddComponent<ClassInitiatorLevelPropertyGetter>(x =>
                    {

                        x.m_Class = define.ClassesForClassTemplate[0];

                    });
                }


            });

            Main.LogPatch(MLProperty);

            var ManeuverBook = Helpers.CreateBlueprint<BlueprintManeuverBook>(source, spellbookSysName, x =>
            {
                x.Name = LocalizationTool.CreateString(spellbookSysName + ".Name", define.DisplayName, false);
                x.DefaultMainStat = define.DefaultInitiatingStat;
                x.BookType = define.maneuverBookType;
                x.ArchetypeReference = define.ArchetypesForArchetypeTemplate.ToArray();
                x.IsGranted = define.GrantedType;
                x.ClassReference = define.ClassesForClassTemplate.ToArray();
                x.GrantingProgression = progression.ToReference<BlueprintProgressionReference>();
                x.m_InitiatorLevelReference = MLProperty.ToReference<BlueprintUnitPropertyReference>();
  
            });

            
            define.m_ManueverBook = ManeuverBook.ToReference<BlueprintManeuverBookReference>();

            //TODO before publishing any class stuff - refine everything below iro a m
            //PRIMAL DISCIPLE ASPLODES NORTH OF HERE
            
            if (define.maneuverBookType == BlueprintManeuverBook.ManeuverBookType.Level9Class)
            {
                foreach(var v in define.ClassesForClassTemplate)
                {
                    v.Get().AddComponent<InitiatorClassComponent>(x => x.m_ManeuverBook = ManeuverBook.ToReference<BlueprintManeuverBookReference>());
                }
            }
            else if (define.maneuverBookType == BlueprintManeuverBook.ManeuverBookType.Level6Archetype)
            {
                foreach(var v in define.ArchetypesForArchetypeTemplate)
                {
                    v.Get().AddComponent<InitiatorArchetypeComponent>(x => x.m_ManeuverBook = ManeuverBook.ToReference<BlueprintManeuverBookReference>());
                }
            }


            if (define.StandardRecovery != null)
            {
                var standardRestore = MakeRestore(define, define.StandardRecovery);
                if (define.IsTemplateArchetype)
                {
                    progression.AddToProgressionLevels(1, standardRestore.ToReference<BlueprintFeatureBaseReference>());
                }
                else if (define.ArchetypesForArchetypeTemplate.Any())
                {
                    foreach(var a in define.ArchetypesForArchetypeTemplate)
                    {
                        a.Get().AddToAddFeatures(1, standardRestore.ToReference<BlueprintFeatureBaseReference>());
                    }
                }
                else
                {
                    progression.AddToProgressionLevels(1, standardRestore.ToReference<BlueprintFeatureBaseReference>());
                }

            }
            if (define.FullRoundRecovery != null)
            {
                var fullRoundRestore = MakeRestore(define, define.FullRoundRecovery);
                if (define.IsTemplateArchetype)
                {
                    progression.AddToProgressionLevels(1, fullRoundRestore.ToReference<BlueprintFeatureBaseReference>());
                }
                else if (define.ArchetypesForArchetypeTemplate.Any())
                {
                    foreach (var a in define.ArchetypesForArchetypeTemplate)
                    {
                        a.Get().AddToAddFeatures(1, fullRoundRestore.ToReference<BlueprintFeatureBaseReference>());
                    }
                }
                else
                {
                    progression.AddToProgressionLevels(1, fullRoundRestore.ToReference<BlueprintFeatureBaseReference>());
                }
            }
            if (define.SwiftRecovery != null)
            {
                var swiftRestore = MakeRestore(define, define.SwiftRecovery);
                if (define.IsTemplateArchetype)
                {
                    progression.AddToProgressionLevels(1, swiftRestore.ToReference<BlueprintFeatureBaseReference>());
                }
                else if (define.ArchetypesForArchetypeTemplate.Any())
                {
                    foreach (var a in define.ArchetypesForArchetypeTemplate)
                    {
                        a.Get().AddToAddFeatures(1, swiftRestore.ToReference<BlueprintFeatureBaseReference>());
                    }
                }
                else
                {
                    progression.AddToProgressionLevels(1, swiftRestore.ToReference<BlueprintFeatureBaseReference>());
                }
            }


            var exchangerBuild = BuildExchanger(define, out var finalPick);


           

            

            define.maneuverSelector = BuildAdditionalManeuverLearner(define, false).ToReference<BlueprintFeatureSelectionReference>();
            define.stanceSelector = BuildAdditionalManeuverLearner(define, true).ToReference<BlueprintFeatureSelectionReference>();

            var factsToAdd = new List<BlueprintUnitFactReference>();
            for (int i = 0; i < define.ManeuversKnownAtLevel1; i++)
            {
                factsToAdd.Add(define.maneuverSelector.Get().ToReference<BlueprintUnitFactReference>());
            }
            factsToAdd.Add(define.stanceSelector.Get().ToReference<BlueprintUnitFactReference>());

            /*finalPick.AddComponent<AddFacts>(x =>
            {

                x.m_Facts = factsToAdd.ToArray();
            });*/
            var featureUp = BuildAdditionalSlotFeatures(define);
            var slotsAt1Feature = BuildBaseSlotFeature(define);
            if (!define.ManuallyBuildSelectors)
            {

                var selectorFeature = Helpers.CreateBlueprint<BlueprintFeatureSelection>(define.Source, define.InitiatorSysNameBase + "DisciplineSelector", x =>
                {
                    x.SetName(define.Source, "Pick Disciplines");
                    x.SetDescription(define.Source, "Select Disciplines");
                    x.IsClassFeature = true;

                });
                for (int i = 0; i < define.SelectionCount; i++)
                {
                    progression.AddToProgressionLevels(1, selectorFeature.ToReference<BlueprintFeatureBaseReference>());
                }
                define.m_disciplineSelector = selectorFeature.ToReference<BlueprintFeatureSelectionReference>();



            }
           
               progression.AddToProgressionLevels(1, slotsAt1Feature.ToReference<BlueprintFeatureBaseReference>());
            

            foreach (int i in define.ManeuversLearnedAtLevels)
            {
                progression.AddToProgressionLevels(i, define.maneuverSelector.Get().ToReference<BlueprintFeatureBaseReference>());
            }
            foreach (int i in define.StancesLearnedAtLevels)
            {
                progression.AddToProgressionLevels(i, define.stanceSelector.Get().ToReference<BlueprintFeatureBaseReference>());
            }
            foreach (int i in define.NormalSlotsIncreaseAtLevels)
            {
                progression.AddToProgressionLevels(i, featureUp.ToReference<BlueprintFeatureBaseReference>());
            }


            progression.AddToProgressionLevels(1, exchangerBuild.ToReference<BlueprintFeatureBaseReference>());
            for (int i = 0; i < define.ManeuversKnownAtLevel1; i++)
            {
                progression.AddToProgressionLevels(1, define.maneuverSelector.Get().ToReference<BlueprintFeatureBaseReference>());
               
            }
            progression.AddToProgressionLevels(1, define.stanceSelector.Get().ToReference<BlueprintFeatureBaseReference>());
            define.m_Progression = progression.ToReference<BlueprintProgressionReference>();

            return progression;

        }

        private static BlueprintFeature BuildBaseSlotFeature(InitiatorProgressionDefine definition)
        {
            string sysName = definition.InitiatorSysNameBase + "BaseManeuverSlotsFeature";
            string displayName = definition.DisplayName + " Readied Maneuvers";
            string discription = $"The {definition.DisplayName} can ready {definition.ManeuverSlotsAtLevel1} maneuvers at level 1";
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(definition.Source, sysName, x =>
            {
                x.SetNameDescription(definition.Source, displayName, discription);
                x.IsClassFeature = true;
                x.Ranks = 1;
                List<BlueprintUnitFactReference> facts = new();
                for (int i = 0; i < definition.ManeuverSlotsAtLevel1; i++)
                    facts.Add(definition.AddSlotComponent.Get().ToReference<BlueprintUnitFactReference>());
                x.AddComponent<AddFacts>(x =>
                {
                    x.m_Facts = facts.ToArray();
                });

            });
            definition.ManeuverSlotsAtLevel1Feature = feature.ToReference<BlueprintFeatureReference>();
            return feature;
        }

        private static BlueprintFeature BuildAdditionalSlotFeatures(InitiatorProgressionDefine definition)
        {
            string sysName = definition.InitiatorSysNameBase + "AddManeuverSlotFeature";
            string displayName = "Add " + definition.DisplayName + " Readied Maneuver";
            string discription = $"The {definition.DisplayName} can ready another maneuver";
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(definition.Source, sysName, x =>
            {
                x.SetNameDescription(definition.Source, displayName, discription);
                x.IsClassFeature = true;
                x.Ranks = 50;


            });
            if (definition.ArchetypesForArchetypeTemplate.Any())
            {
                foreach (var a in definition.ArchetypesForArchetypeTemplate)
                {
                    feature.AddComponent<PrerequisiteArchetypeLevel>(x =>
                    {
                        x.m_Archetype = a;
                        x.m_CharacterClass = a.Get().m_ParentClass.ToReference<BlueprintCharacterClassReference>();
                        x.Level = 1;
                        x.Group = Prerequisite.GroupType.Any;
                    });
                }

            }
            else
            {
                feature.AddComponent<PrerequisiteClassLevel>(x =>
                {

                    x.m_CharacterClass = definition.ClassesForClassTemplate[0];
                    x.Level = 1;
                });
            }
            var slotprop = Helpers.CreateBlueprint<BlueprintUnitProperty>(definition.Source, definition.InitiatorSysNameBase + "ManeuverSlotsProperty", x =>
            {
                x.AddComponent<CompositeCustomPropertyGetter>(x =>
                {
                    x.CalculationMode = CompositeCustomPropertyGetter.Mode.Sum;
                    x.Properties = new CompositeCustomPropertyGetter.ComplexCustomProperty[]
                    {
                        new CompositeCustomPropertyGetter.ComplexCustomProperty()
                        {
                            Property = new FactRankGetter()
                            {
                                m_Fact = feature.ToReference<BlueprintUnitFactReference>()
                            }
                        }
                    };

                });

            });
            definition.SlotsProperty = slotprop.ToReference<BlueprintUnitPropertyReference>();
            definition.m_ManueverBook.Get().m_ManeuverSlotsReference = slotprop.ToReference<BlueprintUnitPropertyReference>();
            definition.AddSlotComponent = feature.ToReference<BlueprintFeatureReference>();
            Main.LogPatch(feature);
            Main.LogPatch(slotprop);
            return feature;
        }



         private static BlueprintFeature MakeRestore(InitiatorProgressionDefine define, InitiatorProgressionDefine.ClassRecoveryDefine recoveryDefine)
        {
            BlueprintBuff associatedBuff = null;
            if (recoveryDefine.NeedsBuff)
            {
                var buffConfig = BuffTools.MakeBuff(define.Source, define.InitiatorSysNameBase + recoveryDefine.Name.Replace(" ", "") + "Ability", recoveryDefine.Name, recoveryDefine.Desc);

                
                associatedBuff = buffConfig.Configure();
                recoveryDefine.m_RestoreBuff = associatedBuff.ToReference<BlueprintBuffReference>();
            }

           

            var ability = Helpers.CreateBlueprint<BlueprintAbility>(define.Source, define.InitiatorSysNameBase + recoveryDefine.Name.Replace(" ", "") + "Ability", x =>
            {
                x.SetNameDescription(define.Source, recoveryDefine.Name, recoveryDefine.Desc);
                switch (recoveryDefine.recoveryAction)
                {
                    case InitiatorProgressionDefine.RecoveryAction.Swift:
                        x.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift;
                        break;
                    case InitiatorProgressionDefine.RecoveryAction.Standard:
                        x.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                        break;
                    case InitiatorProgressionDefine.RecoveryAction.FullRound:
                        x.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                        x.m_IsFullRoundAction = true;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(); 
                        
                }
                if (associatedBuff != null && recoveryDefine.recoveryAction == InitiatorProgressionDefine.RecoveryAction.Swift)
                {
                    x.AddComponent<TabletopTweaks.Core.NewComponents.AbilityRequirementHasBuff>(x =>
                    {
                        x.RequiredBuff = associatedBuff.ToReference<BlueprintBuffReference>();
                        x.Not = true;


                    });
                }
                if (associatedBuff != null)
                {
                    x = AbilityConfigurator.For(x).AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(associatedBuff, ContextDuration.Fixed(1))).Configure();
                }
                
                x.Type = AbilityType.Extraordinary;
                x.Range = AbilityRange.Personal;
                switch (recoveryDefine.recoveryAction)
                {
                    case InitiatorProgressionDefine.RecoveryAction.Swift:
                    case InitiatorProgressionDefine.RecoveryAction.Standard:
                        x.AddComponent<RecoverSelectedManeuver>(x =>
                        {
                            x.m_maneuverBook = define.m_ManueverBook;
                        });
                        break;
                    case InitiatorProgressionDefine.RecoveryAction.FullRound:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                        break;
                }

                if (recoveryDefine.Sprite != null)
                {
                    x.m_Icon = recoveryDefine.Sprite;
                }
                else
                {
                    x.m_Icon = ConstructionAssets.itemBondSprite;
                }


            });

            recoveryDefine.m_RestoreAction = ability.ToReference<BlueprintAbilityReference>();
            Main.LogPatch(ability);
            var feature = Helpers.CreateBlueprint<BlueprintFeature>(define.Source, define.InitiatorSysNameBase + recoveryDefine.Name.Replace(" ", "") + "Feature", x =>
            {
                x.SetNameDescription(define.Source, recoveryDefine.Name, recoveryDefine.Desc);
                x.IsClassFeature = true;

            });

           


            feature.AddComponent<AddFacts>(x =>
            {
                x.m_Facts = new BlueprintUnitFactReference[]
                {
                    ability.ToReference<BlueprintUnitFactReference>()
                };
            });
            recoveryDefine.m_RestoreFeature = feature.ToReference<BlueprintFeatureReference>();
            Main.LogPatch(feature);

            return feature;
        }

     


        private static BlueprintFeatureSelection BuildExchanger(InitiatorProgressionDefine define, out BlueprintFeature finalize)
        {
            if (define.m_exchanger != null)
            {
                Main.Context.Logger.Log($"Warning, attempt to rebuild exchanger for {define}");
            }

            var exchanger = Helpers.CreateBlueprint<BlueprintFeatureSelection>(define.Source, define.InitiatorSysNameBase + "DisciplineExchangeFeatureSelection", x =>
            {
                x.SetNameDescription(define.Source, define.DisplayName + " Discipline Exchange", "Exchange disciplines and finalize.");
                x.IsClassFeature = true;
                x.HideInUI = true;
                x.HideInCharacterSheetAndLevelUp = true;
            });

            finalize = Helpers.CreateBlueprint<BlueprintFeature>(define.Source, define.InitiatorSysNameBase + "DiscplineExchangeDone", x =>
            {
                x.SetNameDescription(define.Source, "Finished Exchanges", "Select to finish doing swaps and pick initial maneuvers");
                x.IsClassFeature = true;
                x.HideInUI = true;
                x.HideInCharacterSheetAndLevelUp = true;

            });
            exchanger.AddFeatures(finalize);
            Main.LogPatch(exchanger);
            return exchanger;
        }


        private static BlueprintFeatureSelection BuildAdditionalManeuverLearner(InitiatorProgressionDefine define, bool stance)
        {
            string sysName = define.InitiatorSysNameBase + (stance ? "Stance" : "Manuever") + "LearnSelector";

            var feature = Helpers.CreateBlueprint<BlueprintFeatureSelection>(define.Source, sysName, x =>
            {
                x.SetName(define.Source, "Learn " + (stance ? "Stance" : "Manuever"));
                x.SetDescription(define.Source, "Learn " + (stance ? "Stance" : "Manuever") + " as " + define.DisplayName);
                x.IsClassFeature = true;
                x.AddComponent<ManeuverSelectorMenuComponent>(x =>
                {
                    x.targetBook = define.m_ManueverBook;
                    x.SelectionMode = ManeuverSelectionMode.Standard;
                });
                x.Ranks = 50;
                x.Obligatory = false;

            });

            Main.LogPatch(feature);
            return feature;


        }


        public static void FinalRun()
        {
            foreach (var define in Defines)
            {
                FinalRun(define);

            }

        }

        private static void FinalRun(InitiatorProgressionDefine define)
        {

            HandleSelection();
            void HandleSelection()
            {
                var disciplines = DisciplineTools.GetAllDisciplinesForProgressionDefinition(define);
                Dictionary<DisciplineDefine, BlueprintFeatureReference> normalUnlockList = new();
                Main.Context.Logger.Log($"There are {disciplines.Count} disciplines to build selectors for");

                foreach (var fixedUnlock in define.FixedUnlocks)
                {
                    var discipline = disciplines.FirstOrDefault(x => x.SysName.Equals(fixedUnlock));
                    if (discipline != null && !normalUnlockList.ContainsKey(discipline))
                    {
                        var built = MakeUnlock(define, discipline);
                        normalUnlockList.Add(discipline, built.ToReference<BlueprintFeatureReference>());
                        define.m_Progression.Get().AddToProgressionLevels(1, built.ToReference<BlueprintFeatureBaseReference>());
                    }




                }

                if (!define.ManuallyBuildSelectors)
                {
                    foreach (var selectUnlock in define.SelectionUnlocks)
                    {
                        var discipline = disciplines.FirstOrDefault(x => x.SysName.Equals(selectUnlock));
                        if (discipline != null)
                        {
                            if (!normalUnlockList.TryGetValue(discipline, out var unlock))
                            {
                                var built = MakeUnlock(define, discipline);
                                normalUnlockList.Add(discipline, built.ToReference<BlueprintFeatureReference>());
                                define.m_disciplineSelector.Get().AddFeatures(built);
                            }
                            else
                            {
                                define.m_disciplineSelector.Get().AddFeatures(unlock);
                            }
                        }




                    }
                }
            }


            HandleExtraReadiedManeuver();
            void HandleExtraReadiedManeuver()
            {
                var selector = BlueprintTools.GetModBlueprint<BlueprintFeatureSelection>(Main.Context, "ExtraReadiedManeuverSelector");

                selector.AddFeatures(define.AddSlotComponent);
            }

            HandleLearn();
            void HandleLearn()
            {
                var selector = define.stanceSelector.Get();
                selector.AddFeatures(ManeuverTools.StanceLearnFeatures.ToArray());

                Main.Context.Logger.Log($"{selector.name} options length:{selector.m_Features.Length}");

                var selector2 = define.maneuverSelector.Get();
                selector2.AddFeatures(ManeuverTools.ManeuverLearnFeatures.ToArray());
                Main.Context.Logger.Log($"{selector2.name} options length:{selector2.m_Features.Length}");
            }

        }

       

        private static BlueprintFeature MakeUnlock(InitiatorProgressionDefine define, DisciplineDefine unlock)
        {
            string systemName = define.InitiatorSysNameBase + "Unlock" + unlock.SysName + "Feature";


            var feature = Helpers.CreateDerivedBlueprint<BlueprintFeature>(define.Source, systemName, unlock.masterGuid, new List<SimpleBlueprint>() { define.m_ManueverBook.Get() }, x =>
            {
                x.SetNameDescription(define.Source, unlock.DisplayName, $"Gain access to {unlock.DisplayName} as {define.DisplayName} manuevers\n{unlock.Description}");
                x.AddComponent<DisciplineUnlockForManeuverBookComponent>(x =>
                {
                    x.bookRef = define.m_ManueverBook;
                    x.disciplineType = unlock.SysName;

                });
                x.m_Icon = unlock.defaultSprite;
                x.AddComponent<AddClassSkill>(x =>
                {

                    x.Skill = unlock.skill;
                });

            });
            Main.LogPatch(feature);


            return feature;
        }

    }
}
