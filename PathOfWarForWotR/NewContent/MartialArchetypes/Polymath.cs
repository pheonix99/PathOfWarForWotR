using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System.Linq;
using TheInfiniteCrusade.Defines;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent.MartialArchetypes
{
    class Polymath
    {

        //static DisciplineType[] disciplines = new DisciplineType[] { DisciplineType.PrimalFury, DisciplineType.SolarWind, DisciplineType.SteelSerpent,};
        public static void MakePolymath()
        {
            var prog = new InitiatorProgressionDefine(Main.Context, "Polymath", maneuverBookType: Backend.NewBlueprints.BlueprintManeuverBook.ManeuverBookType.Level6Archetype);
            prog.LoadDefaultArchetypeProgression();

            var alchemist = BlueprintTool.Get<BlueprintCharacterClass>("0937bec61c0dabc468428f496580c721");
            
            prog.DefaultInitiatingStat = Kingmaker.EntitySystem.Stats.StatType.Intelligence;
            prog.FixedUnlocks = new string[] { "PrimalFury", "SolarWind", "SteelSerpent" };

            //TODO finish filling this out - needs 

            var alchemistPolymath = MakeAlchemist();

            BlueprintArchetype investigatorPolymath = null;

            prog.ClassesForClassTemplate.Add(alchemist.ToReference<BlueprintCharacterClassReference>());
            prog.ArchetypesForArchetypeTemplate.Add(alchemistPolymath.ToReference<BlueprintArchetypeReference>());

            var investigator = BlueprintTool.Get<BlueprintCharacterClass>("adb9e138bee9ecc4db246b64d563f900");
            if (investigator != null)
            {
                investigatorPolymath = MakeInvestigator();
                prog.ClassesForClassTemplate.Add(investigator.ToReference<BlueprintCharacterClassReference>());
                prog.ArchetypesForArchetypeTemplate.Add(investigatorPolymath.ToReference<BlueprintArchetypeReference>());
            }

            var polymathProgressoin = ProcessProgressionDefinition.BuildInitiatorProgress(prog);

            alchemist.m_Archetypes = alchemist.m_Archetypes.Append(alchemistPolymath.ToReference<BlueprintArchetypeReference>()).ToArray();
            alchemistPolymath.AddToAddFeatures(1, polymathProgressoin.ToReference<BlueprintFeatureBaseReference>());
            if (investigator != null)
            {
                investigator.m_Archetypes = investigator.m_Archetypes.Append(alchemistPolymath.ToReference<BlueprintArchetypeReference>()).ToArray();
                investigatorPolymath.AddToAddFeatures(1, polymathProgressoin.ToReference<BlueprintFeatureBaseReference>());
            }

            




            BlueprintArchetype MakeAlchemist()
            {


                string AlchemistPolymathSysName = "AlchemistPolymath";
                var AlchemistPolymathConfig = ArchetypeTools.MakeArchetype(Main.Context, AlchemistPolymathSysName, "Polymath", "Knowledge is power. For those who truly seek knowledge in all its forms, knowledge of martial maneuvers is no different than knowledge of magic or science, and all are equally worthy of study. Sometimes geniuses, sometimes madmen, these seekers of knowledge hunger for new skills and abilities with an addiction and obsessiveness that can often be off putting, but is exceptionally useful. Polymaths are alchemists or investigators who learn to combine their intellect and extracts with the training and discipline of the Path of War. These cunning warriors learn to use any and all tools available to them to master any challenge set before them.", alchemist);
                var AlchemistPolyConfigTemp = AlchemistPolymathConfig.Configure();
                AlchemistPolyConfigTemp.RemoveFeatureFromAllLevels(BlueprintTool.GetRef<BlueprintFeatureBaseReference>("c9022272c87bd66429176ce5c597989c"));
                AlchemistPolyConfigTemp.RemoveFeatureFromAllLevels(BlueprintTool.GetRef<BlueprintFeatureBaseReference>("202af59b918143a4ab7c33d72c8eb6d5"));
                var reducedCasting = ArchetypeTools.MakeSpellsPerDayChangeFeature(Main.Context, AlchemistPolyConfigTemp.ToReference<BlueprintArchetypeReference>(), "Polymath", "Alchemist", "A polymath alchemist prepares one fewer extract per day of each level of extract he knows. If this would reduce his extracts prepared to 0, he may only prepare bonus extracts that he received of that level due to a high Intelligence score.");
                AlchemistPolyConfigTemp.AddToAddFeatures(1, reducedCasting.ToReference<BlueprintFeatureBaseReference>());
                return AlchemistPolyConfigTemp;
            }




            BlueprintArchetype MakeInvestigator()
            {
                string InvestigatorPolymathSysName = "InvestigatorPolymath";

                if (investigator != null)
                {

                    var InvestigatorPolymathConfig = ArchetypeTools.MakeArchetype(Main.Context, InvestigatorPolymathSysName, "Polymath", "Knowledge is power. For those who truly seek knowledge in all its forms, knowledge of martial maneuvers is no different than knowledge of magic or science, and all are equally worthy of study. Sometimes geniuses, sometimes madmen, these seekers of knowledge hunger for new skills and abilities with an addiction and obsessiveness that can often be off putting, but is exceptionally useful. Polymaths are alchemists or investigators who learn to combine their intellect and extracts with the training and discipline of the Path of War. These cunning warriors learn to use any and all tools available to them to master any challenge set before them.", investigator);
                    InvestigatorPolymathConfig.SetSignatureAbilities();

                    var InvestigatorPolyConfigTemp = InvestigatorPolymathConfig.Configure();
                    InvestigatorPolyConfigTemp.RemoveFeatureFromAllLevels(BlueprintTool.GetRef<BlueprintFeatureBaseReference>("152ea6d8f279a73489c9dc0815e9e9b1"));
                    InvestigatorPolyConfigTemp.RemoveFeatureFromAllLevels(BlueprintTool.GetRef<BlueprintFeatureBaseReference>("03d1877a5ffcbd34d9501b005434cb4f"));

                    var investreducedCasting = ArchetypeTools.MakeSpellsPerDayChangeFeature(Main.Context, InvestigatorPolyConfigTemp.ToReference<BlueprintArchetypeReference>(), "Polymath", "Investigator", "A polymath investigator prepares one fewer extract per day of each level of extract he knows. If this would reduce his extracts prepared to 0, he may only prepare bonus extracts that he received of that level due to a high Intelligence score.");


                    InvestigatorPolyConfigTemp.AddToAddFeatures(1, investreducedCasting.ToReference<BlueprintFeatureBaseReference>());


                    return InvestigatorPolyConfigTemp;
                }
                else
                    return null;
            }












        }







    }
}

