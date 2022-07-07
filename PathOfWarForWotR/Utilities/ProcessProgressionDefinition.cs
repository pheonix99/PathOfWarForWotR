using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents.Properties;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Backend.NewActions;
using TheInfiniteCrusade.Defines;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.NewComponents.Properties;

namespace TheInfiniteCrusade.Utilities
{
    public static class ProcessProgressionDefinition
    {
        private static List<InitiatorProgressionDefine> Defines = new();

        public static BlueprintProgression BuildInitiatorProgress(InitiatorProgressionDefine define)
        {
            string stringProgressionBlueprintSysName;
            var source = define.Source;
            if (define.maneuverBookType == ManeuverBookComponent.ManeuverBookType.Level6Archetype)
            {
                stringProgressionBlueprintSysName = define.InitiatorSysNameBase + "InitiatorProgression";

            }
            else if (define.maneuverBookType == ManeuverBookComponent.ManeuverBookType.Level9Class)
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
                if (define.maneuverBookType == ManeuverBookComponent.ManeuverBookType.Level6Archetype)
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


            var spellbook = Helpers.CreateBlueprint<BlueprintSpellbook>(source, spellbookSysName, x =>
            {
                x.Name = LocalizationTool.CreateString(spellbookSysName + ".Name", define.DisplayName, false);
                x.m_SpellsPerDay = BlueprintTools.GetModBlueprintReference<BlueprintSpellsTableReference>(Main.Context, "MartialManeuversTable");
                x.m_SpellSlots = BlueprintTools.GetModBlueprintReference<BlueprintSpellsTableReference>(Main.Context, "MartialManeuversTable");
                x.m_SpellList = BlueprintTools.GetModBlueprintReference<BlueprintSpellListReference>(Main.Context, "DummyManeuverList");
                x.CastingAttribute = define.DefaultInitiatingStat;
                x.CanCopyScrolls = false;
                x.CantripsType = CantripsType.Orisions;
                x.m_CharacterClass = define.ClassesForClassTemplate[0];
                x.AddComponent<ManeuverBookComponent>(bookdef =>
                {
                    bookdef.BookType = define.maneuverBookType;
                    bookdef.ArchetypeReference = define.ArchetypesForArchetypeTemplate.ToArray();
                    bookdef.IsGranted = define.GrantedType;
                    bookdef.ClassReference = define.ClassesForClassTemplate.ToArray();
                    bookdef.GrantingProgression = progression.ToReference<BlueprintProgressionReference>();


                });
            });
            Main.LogPatch(spellbook);
            define.m_spellbook = spellbook.ToReference<BlueprintSpellbookReference>();

            var MLProperty = Helpers.CreateBlueprint<BlueprintUnitProperty>(source, define.InitiatorSysNameBase + "InitiatorLevelProperty", x =>
            {
                if (define.maneuverBookType == ManeuverBookComponent.ManeuverBookType.Level6Archetype)
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
            //PRIMAL DISCIPLE ASPLODES NORTH OF HERE
            var addSpellbookSysName = define.InitiatorSysNameBase + "AddManeuverBook";

            var AddSpellbookFact = Helpers.CreateBlueprint<BlueprintFeature>(source, addSpellbookSysName, x =>
            {
                x.SetNameDescription(source, $"{define.DisplayName} Maneuvers", "Temp Placeholder");
                x.AddComponent<AddManeuverBook>(y =>
                {
                    y.m_CasterLevel = new Kingmaker.UnitLogic.Mechanics.ContextValue { m_CustomProperty = MLProperty.ToReference<BlueprintUnitPropertyReference>(), ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.CasterCustomProperty };
                    y.m_Spellbook = spellbook.ToReference<BlueprintSpellbookReference>();


                });
                x.ReapplyOnLevelUp = true;


            });
            spellbook.GetComponent<ManeuverBookComponent>().GrantingFeature = AddSpellbookFact.ToReference<BlueprintFeatureReference>();

            progression.AddToProgressionLevels(1, AddSpellbookFact.ToReference<BlueprintFeatureBaseReference>());

            var standardRestore = MakeStandardActionRestore(define);
            progression.AddToProgressionLevels(1, standardRestore.ToReference<BlueprintFeatureBaseReference>());
            if (define.HasFullRoundRestore)
            {
                var fullRoundRestore = MakeFullRoundRestore(define);
                progression.AddToProgressionLevels(1, fullRoundRestore.ToReference<BlueprintFeatureBaseReference>());
            }


            var exchangerBuild = BuildExchanger(define, out var finalPick);


            finalPick.AddComponent<ManeuverSelectionFeature>(x =>
            {
                x.Count = define.ManeuversKnownAtLevel1;
                x.targetSpellbook = spellbook.ToReference<BlueprintSpellbookReference>();
                x.stance = false;
                x.mode = ManeuverSelectionFeature.Mode.Standard;
                x.m_SpellList = BlueprintTools.GetModBlueprintReference<BlueprintSpellListReference>(Main.Context, $"MasterManeuverList");
            });

            finalPick.AddComponent<ManeuverSelectionFeature>(x =>
            {
                x.Count = 1;
                x.targetSpellbook = spellbook.ToReference<BlueprintSpellbookReference>();
                x.stance = true;
                x.mode = ManeuverSelectionFeature.Mode.Standard;
                x.m_SpellList = BlueprintTools.GetModBlueprintReference<BlueprintSpellListReference>(Main.Context, $"MasterStanceList");
            });

            var maneuverPick = BuildAdditionalManeuverLearner(define, false);
            var stancePick = BuildAdditionalManeuverLearner(define, true);
            var featureUp = BuildAdditionalSlotFeatures(define);
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
            for (int i = 0; i < define.ManeuverSlotsAtLevel1; i++)
            {
                progression.AddToProgressionLevels(1, featureUp.ToReference<BlueprintFeatureBaseReference>());
            }

            foreach (int i in define.ManeuversLearnedAtLevels)
            {
                progression.AddToProgressionLevels(i, maneuverPick.ToReference<BlueprintFeatureBaseReference>());
            }
            foreach (int i in define.StancesLearnedAtLevels)
            {
                progression.AddToProgressionLevels(i, stancePick.ToReference<BlueprintFeatureBaseReference>());
            }
            foreach (int i in define.NormalSlotsIncreaseAtLevels)
            {
                progression.AddToProgressionLevels(i, featureUp.ToReference<BlueprintFeatureBaseReference>());
            }


            progression.AddToProgressionLevels(1, exchangerBuild.ToReference<BlueprintFeatureBaseReference>());

            define.m_Progression = progression.ToReference<BlueprintProgressionReference>();

            return progression;

        }

        private static BlueprintFeature BuildAdditionalSlotFeatures(InitiatorProgressionDefine definition)
        {
            string sysName = definition.InitiatorSysNameBase + "AddManeuverSlot";
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
            definition.m_spellbook.Get().GetComponent<ManeuverBookComponent>().m_ManeuverSlotsReference = slotprop.ToReference<BlueprintUnitPropertyReference>();
            definition.AddSlotComponent = feature.ToReference<BlueprintFeatureReference>();
            Main.LogPatch(feature);
            Main.LogPatch(slotprop);
            return feature;
        }

        private static BlueprintFeature MakeStandardActionRestore(InitiatorProgressionDefine define)
        {

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(define.Source, define.InitiatorSysNameBase + define.StandardActionRestoreName.Replace(" ", "") + "Feature", x =>
             {
                 x.SetNameDescription(define.Source, define.StandardActionRestoreName, define.StandardActionRestoreDesc);
                 x.IsClassFeature = true;

             });

            var ability = Helpers.CreateBlueprint<BlueprintAbility>(define.Source, define.InitiatorSysNameBase + define.StandardActionRestoreName.Replace(" ", "") + "Ability", x =>
            {
                x.SetNameDescription(define.Source, define.StandardActionRestoreName, define.StandardActionRestoreDesc);
                x.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                x.Type = AbilityType.Extraordinary;
                x.Range = AbilityRange.Personal;
                x.AddComponent<RecoverSelectedManeuver>(x =>
                {
                    x.spellbookReference = define.m_spellbook;
                });
                if (define.CustomStandardActionRestoreSprite != null)
                {
                    x.m_Icon = define.CustomStandardActionRestoreSprite;
                }
                else
                {
                    x.m_Icon = ConstructionAssets.itemBondSprite;
                }

            });
            feature.AddComponent<AddFacts>(x =>
            {
                x.m_Facts = new BlueprintUnitFactReference[]
                {
                    ability.ToReference<BlueprintUnitFactReference>()
                };
            });
            define.m_StandardActionRestore = ability.ToReference<BlueprintAbilityReference>();
            define.m_StandardActionRestoreFeature = feature.ToReference<BlueprintFeatureReference>();
            Main.LogPatch(feature);
            Main.LogPatch(ability);
            return feature;
        }

        private static BlueprintFeature MakeFullRoundRestore(InitiatorProgressionDefine define)
        {

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(define.Source, define.InitiatorSysNameBase + define.FullRoundRestoreName.Replace(" ", "") + "Feature", x =>
            {
                x.SetNameDescription(define.Source, define.FullRoundRestoreName, define.FullRoundRestoreDesc);
                x.IsClassFeature = true;

            });

            var ability = Helpers.CreateBlueprint<BlueprintAbility>(define.Source, define.InitiatorSysNameBase + define.FullRoundRestoreName.Replace(" ", "") + "Ability", x =>
            {
                x.SetNameDescription(define.Source, define.FullRoundRestoreName, define.FullRoundRestoreDesc);
                x.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                x.m_IsFullRoundAction = true;
                x.Type = AbilityType.Extraordinary;
                x.Range = AbilityRange.Personal;


            });
            feature.AddComponent<AddFacts>(x =>
            {
                x.m_Facts = new BlueprintUnitFactReference[]
                {
                    ability.ToReference<BlueprintUnitFactReference>()
                };
            });
            define.m_FullRoundRestore = ability.ToReference<BlueprintAbilityReference>();
            define.m_FullRoundRestoreFeature = feature.ToReference<BlueprintFeatureReference>();
            Main.LogPatch(feature);
            Main.LogPatch(ability);
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


        private static BlueprintFeature BuildAdditionalManeuverLearner(InitiatorProgressionDefine define, bool stance)
        {
            string sysName = define.InitiatorSysNameBase + (stance ? "Stance" : "Manuever") + "LearnSelector";

            var feature = Helpers.CreateBlueprint<BlueprintFeature>(define.Source, sysName, x =>
            {
                x.SetName(define.Source, "Learn " + (stance ? "Stance" : "Manuever"));
                x.SetDescription(define.Source, "Learn " + (stance ? "Stance" : "Manuever") + " as " + define.DisplayName);
                x.IsClassFeature = true;
                x.AddComponent<ManeuverSelectionFeature>(y =>
                {
                    y.Count = 1;
                    y.targetSpellbook = define.m_spellbook;
                    y.stance = stance;
                    y.mode = ManeuverSelectionFeature.Mode.Standard;
                    y.m_SpellList = stance ? BlueprintTools.GetModBlueprintReference<BlueprintSpellListReference>(Main.Context, $"MasterStanceList") : BlueprintTools.GetModBlueprintReference<BlueprintSpellListReference>(Main.Context, $"MasterManeuverList");


                });
                x.Ranks = 50;

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

        }

       

        private static BlueprintFeature MakeUnlock(InitiatorProgressionDefine define, DisciplineDefine unlock)
        {
            string systemName = define.InitiatorSysNameBase + "Unlock" + unlock.SysName + "Feature";


            var feature = Helpers.CreateDerivedBlueprint<BlueprintFeature>(define.Source, systemName, unlock.masterGuid, new List<SimpleBlueprint>() { define.m_spellbook.Get() }, x =>
            {
                x.SetNameDescription(define.Source, unlock.DisplayName, $"Gain access to {unlock.DisplayName} as {define.DisplayName} manuevers\n{unlock.Description}");
                x.AddComponent<DisciplineUnlockForInitiatorProgression>(x =>
                {
                    x.m_Progression = define.m_Progression;
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
