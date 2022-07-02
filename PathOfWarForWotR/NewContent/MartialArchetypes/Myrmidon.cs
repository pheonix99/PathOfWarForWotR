using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using TheInfiniteCrusade.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.NewComponents.AbilitySpecific;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using TheInfiniteCrusade.NewComponents;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using TheInfiniteCrusade.Defines;

namespace TheInfiniteCrusade.NewContent.MartialArchetypes
{
    class Myrmidon
    {
        public static void BuildMyrmidon()
        {
            BlueprintCharacterClass fighter = BlueprintTool.Get<BlueprintCharacterClass>("48ac8db94d5de7645906c7d0ad3bcfbd");
            var GritResource = Helpers.CreateBlueprint<BlueprintAbilityResource>(Main.Context, "MyrmidonGritResource", x =>
            {
                x.m_MaxAmount = new BlueprintAbilityResource.Amount
                {
                    IncreasedByStat = true,
                    ResourceBonusStat = Kingmaker.EntitySystem.Stats.StatType.Wisdom
                };
                x.m_Min = 1;


            });
            Main.Context.Logger.LogPatch(GritResource);
            var SkillFeature = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonSkillsFeature",x =>
            {
                x.SetNameDescription(Main.Context, "Myrmidon Skills", "");
                x.IsClassFeature = true;
                x.AddComponent<AddClassSkill>(x =>
                {
                    x.Skill = Kingmaker.EntitySystem.Stats.StatType.SkillMobility;
                });
                x.AddComponent<AddClassSkill>(x =>
                {
                    x.Skill = Kingmaker.EntitySystem.Stats.StatType.SkillPersuasion;
                });
            });
            Main.LogPatch(SkillFeature);
            var GritFeature = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonGritFeature", x =>
            {
                x.SetName(Main.Context, "Grit");
                x.SetDescription(Main.Context, "At 1st level, the myrmidon makes his mark upon the world with nerves of steel and superior training. Through determination, verve, or otherwise dumb luck, the myrmidon is capable of forcing incredible feats of daring and skill through their own tenacity. In game terms, grit is a fluctuating measure of a myrmidon’s ability to perform incredible actions in combat. At the start of each day, a myrmidon gains a number of grit points equal to his myrmidon initiation modifier (minimum 1). His grit goes up or down throughout the day, but usually cannot go higher than his myrmidon initiation modifier (minimum 1), though some feats and magic items may affect this maximum. A myrmidon spends grit to accomplish deeds (see below), and regains grit in the following ways.\n Critical Hit: Each time the myrmidon confirms a critical hit while in the heat of combat with a weapon with which he has Weapon Focus or is in a weapon group associated with a discipline he has Discipline Focus for, he regains one grit point. Confirming a critical hit on a helpless or unaware creature or on a creature that has fewer Hit Dice than half the myrmidon’s character level does not restore grit.\n Killing Blow with a Maneuver: When the myrmidon reduces a creature to 0 or fewer hit points with a maneuver or with a weapon he has Weapon Focus with, he regains 1 grit point. Destroying an unattended object, reducing a helpless or unaware creature to 0 or fewer hit points, or reducing a creature that has fewer Hit Dice than half the myrmidon’s character level to 0 or fewer hit points does not restore any grit.\n This ability replaces the bonus feat gained at 2nd level.");
                x.IsClassFeature = true;
                x.AddComponent<AddAbilityResources>(x =>
                {
                    x.m_Resource = GritResource.ToReference<BlueprintAbilityResourceReference>();
                    x.RestoreAmount = true;
                    x.RestoreOnLevelUp = false;

                });
                

            });
            Main.LogPatch(GritFeature);
            var unbreakableDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonUnbreakableDeedFeature", x =>
            {
                x.SetName(Main.Context, "Unbreakable");
                x.SetDescription(Main.Context, "Starting at 1st level, a myrmidon is trained very well to protect himself against the many unnatural elements of this world where he must rely on only his wits and training to protect him from harm. He can spend 1 grit point as an immediate action to gain a +4 circumstance bonus on a single saving throw.");
                x.IsClassFeature = true;
            });
            Main.LogPatch(unbreakableDeed);
            var HeroicRecoveryAbility = Helpers.CreateBlueprint<BlueprintAbility>(Main.Context, "MyrmidonHeroicRecoveryDeedAbility", x => {
                x.SetName(Main.Context, "Heroic Recovery");
                x.SetDescription(Main.Context, " Starting at 1st level, a myrmidon can spend 1 grit point as a swift action to recover a single expended maneuver.");
                x.AddComponent<AbilityResourceLogic>(x =>
                {
                    x.Amount = 1;
                    x.m_IsSpendResource = true;
                    x.m_RequiredResource = GritResource.ToReference<BlueprintAbilityResourceReference>();

                });
                x.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift;
            });
            Main.LogPatch(HeroicRecoveryAbility);


            var heroicRecoveryDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonHeroicRecoveryDeedFeature", x =>
            {
                x.SetName(Main.Context, "Heroic Recovery");
                x.SetDescription(Main.Context, " Starting at 1st level, a myrmidon spend 1 grit point as a swift action to recover a single expended maneuver.");
                x.IsClassFeature = true;
                x.AddComponent<AddFacts>(x =>
                {
                    x.m_Facts = new BlueprintUnitFactReference[] { HeroicRecoveryAbility.ToReference<BlueprintUnitFactReference>() };
                    
                });
            });
            Main.LogPatch(heroicRecoveryDeed);
            var ManofActionDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonManofActionDeedFeature", x =>
            {
                x.SetName(Main.Context, "Man of Action");
                x.SetDescription(Main.Context, "Starting at 1st level, a myrmidon can spend 1 grit point as a swift action to gain a circumstance bonus on a single Acrobatics, Climb, or Swim check equal to his class level.");
                x.IsClassFeature = true;
            });
            Main.LogPatch(ManofActionDeed);
            


            var ReadyForTroubleDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonReadyForTroubleDeedFeature", x =>
            {
                x.SetName(Main.Context, "Ready For Trouble");
                x.SetDescription(Main.Context, "Starting at 3rd level, as long as the myrmidon has at least 1 grit point, he gains a +2 bonus on initiative checks and Will saves to resist compulsion and fear effects.");
                x.IsClassFeature = true;

                
            });
            Main.LogPatch(ReadyForTroubleDeed);
            var FieldBandageAbility = Helpers.CreateBlueprint<BlueprintAbility>(Main.Context, "MyrmidonFieldBandageAbility", x => {
                x.SetName(Main.Context, "Field Bandage");
                x.SetDescription(Main.Context, "By using a healer’s kit to quickly dress and bandage a wound, the myrmidon can grant 1d6 temporary hit points per three character levels to himself or an adjacent creature as a full-round action. These temporary hit points cannot increase a creature’s hit points beyond its normal maximum, and last for ten minutes. A creature can only only receive the benefits of this ability for one day or until they have received magical healing equal to or greater than the amount of temporary hit points granted by the myrmidon’s field bandage, whichever comes first. This ability also halts a bleeding wound, stopping a creature from taking further bleed damage.");
                x.AddComponent<AbilityResourceLogic>(x =>
                {
                    x.Amount = 1;
                    x.m_IsSpendResource = false;
                    x.m_RequiredResource = GritResource.ToReference<BlueprintAbilityResourceReference>();

                });
                x.ActionType = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
                x.m_IsFullRoundAction = true;
                
            });
            Main.LogPatch(FieldBandageAbility);
            var UtilityTrickeDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonUtilityTrickDeedFeature", x =>
            {
                x.SetName(Main.Context, "UtilityTricks");
                x.SetDescription(Main.Context, "Starting at 3rd level, as long as the myrmidon has at least 1 grit point, he can perform any of the following utility tricks. The myrmidon must declare the utility trick he is using before using the ability.\nField Bandage: By using a healer’s kit to quickly dress and bandage a wound, the myrmidon can grant 1d6 temporary hit points per three character levels to himself or an adjacent creature as a full-round action. These temporary hit points cannot increase a creature’s hit points beyond its normal maximum, and last for ten minutes. A creature can only only receive the benefits of this ability for one day or until they have received magical healing equal to or greater than the amount of temporary hit points granted by the myrmidon’s field bandage, whichever comes first. This ability also halts a bleeding wound, stopping a creature from taking further bleed damage.\nImprovise Weapon: The myrmidon can use objects not intended to be normal weapons or cobble together something that can be used as a weapon. Penalties from improvised or low quality weapons are reduced by two.");
                x.IsClassFeature = true;
                x.AddComponent<MyrmidonImproviseWeaponComponent>();
                x.AddComponent<AddFacts>(x =>
                {
                    x.m_Facts = new BlueprintUnitFactReference[] { FieldBandageAbility.ToReference<BlueprintUnitFactReference>() };
                });

            });
            Main.LogPatch(UtilityTrickeDeed);
            var WarriorsDetermination = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonWarriorsDetermination1Feature", x =>
            {
                x.SetName(Main.Context, "Warrior’s Determination");
                x.SetDescription(Main.Context, "The myrmidon gains an uncanny ability to force himself through many hardships and keep on going through his superior training and experience. Starting at 6th level, he can spend 1 grit point as an immediate action to negate a single condition currently affecting him until the end of the encounter, at which point it returns as if its duration had not been interrupted. The myrmidon can activate this ability even if he would not normally be able to act because of the condition in question. A myrmidon can use this ability multiple times in an encounter, spending 1 grit point and negating a single condition each time he does.\nAt 6th level, the myrmidon can temporarily negate the fatigued, shaken, or sickened conditions using this ability.");
                x.IsClassFeature = true;


            });
            Main.LogPatch(WarriorsDetermination);
            var WarriorsDetermination2 = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonWarriorsDetermination2Feature", x =>
            {
                x.SetName(Main.Context, "Warrior’s Determination - Improved");
                x.SetDescription(Main.Context, "Warrior's Determination now extends to dazed and staggered conditions.");
                x.IsClassFeature = true;


            });
            Main.LogPatch(WarriorsDetermination2);
            var WarriorsDetermination3 = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonWarriorsDetermination3Feature", x =>
            {
                x.SetName(Main.Context, "Warrior’s Determination -Greater ");
                x.SetDescription(Main.Context, "Warrior's Determination now extends to the exhausted, frightened, and nauseated conditions.");
                x.IsClassFeature = true;


            });
            Main.LogPatch(WarriorsDetermination3);
            InitiatorProgressionDefine myrmidonDefine = new InitiatorProgressionDefine(Main.Context, "Myrmidon", maneuverBookType: NewComponents.ManeuverBookSystem.ManeuverBookComponent.ManeuverBookType.Level6Archetype);
           


            var fighterBonusFeat = TabletopTweaks.Core.Utilities.FeatTools.Selections.FighterFeatSelection;
            var fighterBonusFeatRef = fighterBonusFeat.ToReference<BlueprintFeatureBaseReference>();
            var myrm = Helpers.CreateBlueprint<BlueprintArchetype>(Main.Context, "MyrmidonArchetype", x =>
            {
                x.SetName(Main.Context, "Myrmidon");
                x.SetDescription(Main.Context, "Some fighters attend grand colleges of war where they learn to master more esoteric martial forms, and some learn the techniques of many different schools of combat and forge their own path. Others are trained in small regiments to fight as a single cohesive, adaptable unit where all members are capable of playing the other’s parts. These fighters are known as myrmidons, the pinnacle of the fighter’s tradition of adaptability, ingenuity, and enduring strength.");
                x.AddSkillPoints = 1;
                x.m_ParentClass = fighter;
                x.AddToAddFeatures(1, SkillFeature.ToReference<BlueprintFeatureBaseReference>());
                x.AddToRemoveFeatures(2, fighterBonusFeatRef);
                x.AddToRemoveFeatures(6, fighterBonusFeatRef);
                x.AddToRemoveFeatures(10, fighterBonusFeatRef);
                x.AddToRemoveFeatures(14, fighterBonusFeatRef);
                x.AddToRemoveFeatures(18, fighterBonusFeatRef);
                x.AddToAddFeatures(1, GritFeature.ToReference<BlueprintFeatureBaseReference>());
                x.AddToAddFeatures(1, unbreakableDeed.ToReference<BlueprintFeatureBaseReference>());
                
                x.AddToAddFeatures(1, heroicRecoveryDeed.ToReference<BlueprintFeatureBaseReference>());
                x.AddToAddFeatures(1, ManofActionDeed.ToReference<BlueprintFeatureBaseReference>());
                x.AddToAddFeatures(3, ReadyForTroubleDeed.ToReference<BlueprintFeatureBaseReference>());
                x.AddToAddFeatures(3, UtilityTrickeDeed.ToReference<BlueprintFeatureBaseReference>());
                x.AddToAddFeatures(6, WarriorsDetermination.ToReference<BlueprintFeatureBaseReference>());
                x.AddToAddFeatures(10, WarriorsDetermination2.ToReference<BlueprintFeatureBaseReference>());
                x.AddToAddFeatures(14, WarriorsDetermination3.ToReference<BlueprintFeatureBaseReference>());
            });

            myrmidonDefine.ClassesForClassTemplate.Add(fighter.ToReference<BlueprintCharacterClassReference>());
            myrmidonDefine.ArchetypesForArchetypeTemplate.Add(myrm.ToReference<BlueprintArchetypeReference>());
            myrmidonDefine.DefaultInitiatingStat = Kingmaker.EntitySystem.Stats.StatType.Wisdom;
            myrmidonDefine.LoadDefaultArchetypeProgression();
            myrmidonDefine.SelectionCount = 4;
            myrmidonDefine.SelectionUnlocks = new string[] { "BrokenBlade", "GoldenLion", "IronTortoise", "MithrilCurrent", "PiercingThunder", "PrimalFury", "ScarletThrone", "TempestGale", "ThrashingDragon" };
            myrmidonDefine.FullRoundRestoreName = "Assume Defensive Form";
            myrmidonDefine.FullRoundRestoreDesc = "In order for the myrmidon to recover maneuvers, he must take on a defensive form as a full-round action, resetting his rhythm to continue the battle. When he does so, he recovers a number of maneuvers equal to his myrmidon initiation modifier (minimum 2) and until the start of his next turn, attacks made against the myrmidon provoke an attack of opportunity from him. In addition, he gains the benefit of the Combat Reflexes feat, and can use his myrmidon initiation modifier instead of his Dexterity modifier for determining how many additional attacks of opportunity he can make.";
            myrmidonDefine.StandardActionRestoreName = "Recover Myrmidon Maneuver";
            myrmidonDefine.StandardActionRestoreDesc = "The myrmidon may take a moment to focus, recovering a single maneuver as a standard action.";

            var prog = ProcessProgressionDefinition.BuildInitiatorProgress(myrmidonDefine);
            myrm.AddToAddFeatures(1, prog.ToReference<BlueprintFeatureBaseReference>());

            Main.LogPatch(myrm);

            var list = myrm.AddFeatures.Select(x => x).ToList();
            list.Sort(LevelEntrySorter.SortLEs);
            myrm.AddFeatures = list.ToArray();
            fighter.m_Archetypes = fighter.m_Archetypes.AppendToArray(myrm.ToReference<BlueprintArchetypeReference>());
        }
    }
}
