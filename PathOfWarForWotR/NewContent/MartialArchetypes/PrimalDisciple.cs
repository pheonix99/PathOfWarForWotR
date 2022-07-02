using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Defines;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent.MartialArchetypes
{
    class PrimalDisciple
    {
        public static void Make()
        {
            var ancestral = Helpers.CreateBlueprint<BlueprintFeatureSelection>(Main.Context, "AncestralStyleFeatureSelector", x =>
            {
                x.SetNameDescription(Main.Context, "Ancestral Style", "Primal disciples draw their skill and strength from longstanding traditions of combat and skill in their tribes. At 1st level and again at 6th level, a primal disciple gains one of the following feats as a bonus feat: Combat Reflexes, Enduring Protector, Martial Charge, Martial Power, Prodigious Two-Weapon Fighting, or Victorious Recovery. She must meet the prerequisites for these feats as normal..\nThis ability replaces fast movement and trap sense.");
                x.IsClassFeature = true;
                x.AddFeatures(BlueprintTool.Get<BlueprintFeature>("CombatReflexes"));

            });


            BlueprintCharacterClass barbarian = BlueprintTool.Get<BlueprintCharacterClass>("f7d7eb166b3dd594fb330d085df41853");

            var primal = Helpers.CreateBlueprint<BlueprintArchetype>(Main.Context, "PrimalDiscipleArchetype", x =>
            {
                x.SetName(Main.Context, "Primal Disciple");
                x.SetDescription(Main.Context, "Primal Disciples draw on the strength and wisdom of their ancestors to grant them the insight to use powerful martial techniques during their furious rages.");
                x.m_ParentClass = barbarian;


            });
            primal.AddToAddFeatures(1, ancestral.ToReference<BlueprintFeatureBaseReference>());
            primal.AddToAddFeatures(6, ancestral.ToReference<BlueprintFeatureBaseReference>());
            primal.AddToRemoveFeatures(1, BlueprintTool.GetRef<BlueprintFeatureBaseReference>("d294a5dddd0120046aae7d4eb6cbc4fc"));
            primal.RemoveFeatureFromAllLevels(BlueprintTool.GetRef<BlueprintFeatureBaseReference>("fdd591c1fbf1c0b41a359d59756f2888"));
            for (int i = 4; i <= 20; i += 2)
            {
                primal.AddToRemoveFeatures(i, BlueprintTool.GetRef<BlueprintFeatureBaseReference>("28710502f46848d48b3f0d6132817c4e"));
            }
            var primalDefine = new InitiatorProgressionDefine(Main.Context, "PrimalDisciple", "Primal Disciple", false, NewComponents.ManeuverBookSystem.ManeuverBookComponent.ManeuverBookType.Level6Archetype);
            primalDefine.LoadDefaultArchetypeProgression();
            primalDefine.FixedUnlocks = new string[] { "GoldenLion", "PiercingThunder", "PrimalFury", "ThrashingDragon" };
            primalDefine.ClassesForClassTemplate = new List<BlueprintCharacterClassReference>() { barbarian.ToReference<BlueprintCharacterClassReference>() };
            primalDefine.ArchetypesForArchetypeTemplate = new() { primal.ToReference<BlueprintArchetypeReference>() };
            primalDefine.DefaultInitiatingStat = Kingmaker.EntitySystem.Stats.StatType.Wisdom;
            primalDefine.FullRoundRestoreName = "Invoke Ancestral Strength";
            primalDefine.FullRoundRestoreDesc = "In order for the primal disciple to recover maneuvers, she must draw on the strength of her ancestors as a full-round action. When she does so, she recovers a number of expended maneuvers equal to her primal disciple initiation modifier (minimum 2), regains one round of rage, and if she is fatigued as a result of her rage class feature, she can make a Fortitude save (DC 10 + the number of rounds the fatigue would last) to not become fatigued.";
            primalDefine.StandardActionRestoreName = "Focus Inward";
            primalDefine.StandardActionRestoreDesc = "A primal disciple may focus inward and recover a single maneuver as a standard action.";

            var primalProg = ProcessProgressionDefinition.BuildInitiatorProgress(primalDefine);

            primal.AddToAddFeatures(1, primalProg.ToReference<BlueprintFeatureBaseReference>());



            barbarian.m_Archetypes = barbarian.m_Archetypes.AppendToArray(primal.ToReference<BlueprintArchetypeReference>());

        }
    }
}
