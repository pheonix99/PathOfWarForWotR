using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Defines;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent.MartialClasses
{
    class Mystic
    {
        public static void Make()
        {
            
            BlueprintFeatureReference MakeMysticAnimusFeature()
            {
               var config = MoreFeatTools.MakeFeature(Main.Context, "MysticAnimusFeature", "Mystic Animus", "A mystic’s martial prowess is in part fueled by a reservoir of roiling, turbulent energy within her soul, and the passion and danger of combat causes this arcane energy to overflow outwards. This power, called animus, waxes and wanes with a mystic’s use of her maneuvers in battle. Outside combat, a mystic has no animus to spend, but her inner power can still be used for more subtle arcane arts. Her levels in mystic count as arcane spellcaster levels for the purposes of qualifying for prerequisites (such as those of item creation feats or the Arcane Strike feat), and if a mystic ever develops arcane spellcasting from another class, she may add her mystic level to her levels in that class to determine her overall caster level for the purposes of item creation feats.\n When a mystic enters combat, she gains an animus pool equal to 1 + her mystic initiation modifier(minimum 1) at the start of her first turn, and adds one point of animus to her animus pool at the start of each of her turns thereafter.Her animus pool persists for one minute after the last enemy combatant is defeated or the encounter otherwise ends.At the end of any round in which the mystic initiates a maneuver, she adds an additional point of animus to her pool.Certain abilities, such as some class features, maneuvers, and feats, require the mystic to expend points of animus to use.\n The primal power of animus can be used in several ways—the foremost of which is the augmentation of maneuvers.A mystic can spend points of animus to augment her maneuvers in the following ways, depending on her class level. If the mystic has the ability to augment her maneuvers in other ways, such as from another class feature or the maneuver itself, this cannot be combined with the augments granted by her animus class feature; she must choose which augmentation type to use when initiating the maneuver. ", true);
                //TODO add animus resource
                //TODO add base animus level
                //TODO add initator modifier effect
                //TODO mystic for reqs
                


                return config.Configure().ToReference<BlueprintFeatureReference>();
            }

            BlueprintFeatureReference MakeMysticAnimusAugments1Feature()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticAnimusAugments1Feature", "Mystic Animus Augments", "Starting at 1st level, a mystic can spend a single point of animus to augment a maneuver as part of that maneuver’s initiation action to apply one of the following effects to it (if applicable):\nEnhance Maneuver: For each point of animus spent, the mystic adds a cumulative + 2 insight bonus to all d20 rolls made(including attack rolls, combat maneuver checks, and skill checks) when initiating that maneuver(maximum of three animus may be spent on this augmentation); if the maneuver allows the user to make multiple attacks, then this bonus only applies to the first attack.\nIncrease DC: For each point of animus spent, the save DC of that maneuver increases by 1.", true);
                //TODO add animus augments


                return config.Configure().ToReference<BlueprintFeatureReference>();
            }

            string sysname = "TIC_MysticClass";
            string descTemp = "Born with an untamed magical power buried deep within his soul, the mystic is a warrior who, much like a sorcerer, is filled with untapped energies. This power however, is too primal and unstable, and is difficult to be formed effectively into a spell. By following a martial medium to tame this energy, the mystic is able to shape his wild power into martial maneuvers and this allows him to discover the deeper mysteries of his own inborn power. ";
            var guid = Main.Context.Blueprints.GetGUID(sysname);
            var mysticCOnfig = CharacterClassConfigurator.New(sysname, guid.ToString()).AddPrerequisiteNoClassLevel("4cd1757a0eea7694ba5c933729a53920").AddPrerequisiteIsPet(not: true);
            mysticCOnfig.SetLocalizedName(LocalizationTool.CreateString(sysname + ".Name", "Mystic", false));
            mysticCOnfig.SetLocalizedDescription(LocalizationTool.CreateString(sysname + ".Desc", descTemp, false));
            mysticCOnfig.SetLocalizedDescriptionShort(LocalizationTool.CreateString(sysname + ".DescShort", descTemp, false));
            mysticCOnfig.SetSkillPoints(4);
            mysticCOnfig.SetHitDie(Kingmaker.RuleSystem.DiceType.D8);
            mysticCOnfig.SetClassSkills(StatType.SkillMobility, StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion, StatType.SkillStealth, StatType.SkillUseMagicDevice, StatType.SkillPerception);
            mysticCOnfig.SetBaseAttackBonus("BABMedium");
            mysticCOnfig.SetFortitudeSave("SavesLow");
            mysticCOnfig.SetReflexSave("SavesLow");
            mysticCOnfig.SetWillSave("SavesHigh");
            mysticCOnfig.SetDifficulty(3);

            InitiatorProgressionDefine mysticInitatiorDefine = new(Main.Context, "Mystic", grantedType: true);
            mysticInitatiorDefine.ManeuversKnownAtLevel1 = 7;
            mysticInitatiorDefine.ManeuversGrantedAtLevel1 = 2;
            mysticInitatiorDefine.ManeuverSlotsAtLevel1 = 5;
            mysticInitatiorDefine.ChosenManeuvers = 2;
            mysticInitatiorDefine.DefaultInitiatingStat = StatType.Wisdom;
            mysticInitatiorDefine.FixedUnlocks = new string[] { "ElementalFlux", "MithralCurrent", "RivenHourglass", "ShatteredMirror", "SolarWind", "VeiledMoon" };
            mysticInitatiorDefine.FullRoundRestoreName = "BladeMeditation";
            mysticInitatiorDefine.FullRoundRestoreDesc = "When a mystic finds that her martial power is beginning to wane or that few options remain available for use, she can pause in battle, drawing on her inner well of animus to reinvigorate her body and mind. As a full-round action, a mystic can spend one point of animus to grant herself all her remaining withheld maneuvers, then immediately expend them in a raging cadence of arcane power. As there are no remaining maneuvers to be granted, a new set of maneuvers is granted to the mystic at the end of her turn, as normal.\nIn addition, until the start of her next turn, creatures that target the mystic with melee attacks are engulfed in the explosion of energy, taking 1d6 points of damage of her active element’s associated energy type, plus an additional 1d6 points of damage for every two points of animus remaining in the mystic’s animus pool. ";
            mysticInitatiorDefine.HasFullRoundRestore = true;
            mysticInitatiorDefine.ProgressionSpecificSubstitutions = new string[] { "UnquietGrave" };
            mysticInitatiorDefine.NormalSlotsIncreaseAtLevels = new int[] { 3, 6, 9, 12, 15, 18, 20 };
            mysticInitatiorDefine.StancesLearnedAtLevels = new int[] { 1, 2, 5, 9, 11, 15, 18 };
            mysticInitatiorDefine.ManeuversLearnedAtLevels = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 11, 13, 15, 17, 19, 20 };
            mysticInitatiorDefine.HasStandardActionRestore = false;
            


            //TODO start tiems and monies

            //TODO equomentEntities

            var mysticBuild = mysticCOnfig.Configure();

            
            mysticInitatiorDefine.ClassesForClassTemplate.Add(mysticBuild.ToReference<BlueprintCharacterClassReference>());

            var mysticProgression = ProcessProgressionDefinition.BuildInitiatorProgress(mysticInitatiorDefine);

            mysticBuild.m_Progression = mysticProgression.ToReference<BlueprintProgressionReference>();

            var mysticAnimusFeature = MakeMysticAnimusFeature();

             MakeBladeMeditationDeets(mysticInitatiorDefine.m_FullRoundRestore.Get());
           


            void MakeBladeMeditationDeets(BlueprintAbility mediation)
            {
                
            }

        }
    }
}
