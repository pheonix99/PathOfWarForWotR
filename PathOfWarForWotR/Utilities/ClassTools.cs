using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Assets;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Utilities
{
   static class MoreClassTools
    {
        public enum BAB
        {
            High,
            Medium,
            Low


        }

        public static CharacterClassConfigurator MakeClass(string name, string shortDesc, string desc, int skillPoints, DiceType hd, StatType[] classSkills, BAB bab, bool highFort, bool HighRef, bool HighWill, int difficulty, IEnumerable<Blueprint<BlueprintReference<BlueprintCharacterClassGroup>>> groups, StatType[] recommendedAttributes = null, StatType[] notRecommendedAttributes = null )
        {
            string sysName = "TIC_" + name.Replace(" ", "") + "Class";
            var guid = Main.Context.Blueprints.GetGUID(sysName);
            var config = CharacterClassConfigurator.New(sysName, guid.ToString());
            config.AddPrerequisiteNoClassLevel("4cd1757a0eea7694ba5c933729a53920");//block animal
              config.AddPrerequisiteIsPet(not: true);//block pet
            config.SetLocalizedName(LocalizationTool.CreateString(sysName + ".Name", name, false));
            config.SetLocalizedDescription(LocalizationTool.CreateString(sysName + ".Desc", desc, false));
            config.SetLocalizedDescriptionShort(LocalizationTool.CreateString(sysName + ".DescShort", shortDesc, false));
            config.SetHitDie(hd);
            config.SetDifficulty(difficulty);
            
            config.SetClassSkills(classSkills);
            if (bab == BAB.High)
            {
                config.SetBaseAttackBonus("BABFull");
            }
            else if (bab == BAB.Medium)
            {
                config.SetBaseAttackBonus("BABMedium");
            }
            else
            {
                config.SetBaseAttackBonus("BABLow");
            }
            config.SetFortitudeSave(highFort ? "SavesHigh" : "SavesLow");
            config.SetReflexSave(HighRef ? "SavesHigh" : "SavesLow");
            config.SetWillSave(HighWill ? "SavesHigh" : "SavesLow");
            if (recommendedAttributes != null)
                config.SetRecommendedAttributes(recommendedAttributes);
            if (notRecommendedAttributes != null)
                config.SetRecommendedAttributes(notRecommendedAttributes);

            foreach(var v in groups)
            {
                CharacterClassGroupConfigurator.For(v).AddToCharacterClasses(sysName).Configure();
            }

            return config;
        }

        public static CharacterClassConfigurator CloneClassAesthestics(this CharacterClassConfigurator workingOn, Blueprint<BlueprintCharacterClassReference> source)
        {
            var sourceObj = source.Reference.Get();

            workingOn.SetMaleEquipmentEntities((sourceObj.MaleEquipmentEntities.Select(x => (AssetLink<EquipmentEntityLink>)x).ToArray()));
            workingOn.SetFemaleEquipmentEntities((sourceObj.FemaleEquipmentEntities.Select(x => (AssetLink<EquipmentEntityLink>)x).ToArray()));
            workingOn.SetPrimaryColor(sourceObj.PrimaryColor);
            workingOn.SetSecondaryColor(sourceObj.SecondaryColor);

            return workingOn;
        }
    }
}
