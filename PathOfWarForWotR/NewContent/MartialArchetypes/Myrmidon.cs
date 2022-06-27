using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using PathOfWarForWotR.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace PathOfWarForWotR.NewContent.MartialArchetypes
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

            var unbreakableDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonUnbreakableDeedFeature", x =>
            {
                x.SetName(Main.Context, "Unbreakable");
                x.SetDescription(Main.Context, "Starting at 1st level, a myrmidon is trained very well to protect himself against the many unnatural elements of this world where he must rely on only his wits and training to protect him from harm. He can spend 1 grit point as an immediate action to gain a +4 circumstance bonus on a single saving throw.");
                x.IsClassFeature = true;
            });
            var heroicRecoveryDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonHeroicRecoveryDeedFeature", x =>
            {
                x.SetName(Main.Context, "Heroic Recovery");
                x.SetDescription(Main.Context, " Starting at 1st level, a myrmidon spend 1 grit point as a swift action to recover a single expended maneuver.");
                x.IsClassFeature = true;
            });
            var ManofActionDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonManofActionDeedFeature", x =>
            {
                x.SetName(Main.Context, "Man of Action");
                x.SetDescription(Main.Context, "Starting at 1st level, a myrmidon can spend 1 grit point as a swift action to gain a circumstance bonus on a single Acrobatics, Climb, or Swim check equal to his class level.");
                x.IsClassFeature = true;
            });
            var ReadyForTroubleDeed = Helpers.CreateBlueprint<BlueprintFeature>(Main.Context, "MyrmidonReadyForTroubleDeedFeature", x =>
            {
                x.SetName(Main.Context, "Ready For Trouble");
                x.SetDescription(Main.Context, "Starting at 3rd level, as long as the myrmidon has at least 1 grit point, he gains a +2 bonus on initiative checks and Will saves to resist compulsion and fear effects.");
                x.IsClassFeature = true;
               
            });


            var fighterBonusFeat = TabletopTweaks.Core.Utilities.FeatTools.Selections.FighterFeatSelection;
            var fighterBonusFeatRef = fighterBonusFeat.ToReference<BlueprintFeatureBaseReference>();
            var myrm = Helpers.CreateBlueprint<BlueprintArchetype>(Main.Context, "MyrmidonArchetype", x =>
            {
                x.SetName(Main.Context, "Myrmidon");
                x.SetDescription(Main.Context, "Some fighters attend grand colleges of war where they learn to master more esoteric martial forms, and some learn the techniques of many different schools of combat and forge their own path. Others are trained in small regiments to fight as a single cohesive, adaptable unit where all members are capable of playing the other’s parts. These fighters are known as myrmidons, the pinnacle of the fighter’s tradition of adaptability, ingenuity, and enduring strength.");
                x.AddSkillPoints = 1;
                x.m_ParentClass = fighter;

                x.AddToRemoveFeatures(2, fighterBonusFeatRef);
                x.AddToRemoveFeatures(6, fighterBonusFeatRef);
                x.AddToRemoveFeatures(10, fighterBonusFeatRef);
                x.AddToRemoveFeatures(14, fighterBonusFeatRef);
                x.AddToRemoveFeatures(18, fighterBonusFeatRef);

            });
        }
    }
}
