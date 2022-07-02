using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheInfiniteCrusade.Utilities
{
    public static class ConstructionAssets
    {
        public static void LoadGUIDS()
        {
            BlueprintTool.AddGuidsByName(("RangerClass", "cda0615668a6df14eb36ba19ee881af6"));
            BlueprintTool.AddGuidsByName(("Evasion", "576933720c440aa4d8d42b0c54b77e80"));
            BlueprintTool.AddGuidsByName(("PowerAttackToggle", "a7b339e4f6ff93a4697df5d7a87ff619"));
            BlueprintTool.AddGuidsByName(("DivineFavor", "9d5d2d3ffdd73c648af3eb3e585b1113"));
            BlueprintTool.AddGuidsByName(("BABFull", "b3057560ffff3514299e8b93e7648a9d"));
            BlueprintTool.AddGuidsByName(("ImprovedUnarmedStrike", "7812ad3672a4b9a4fb894ea402095167"));
            BlueprintTool.AddGuidsByName(("UndeadType", "734a29b693e9ec346ba2951b27987e33"));
            BlueprintTool.AddGuidsByName(("ConstructType", "fd389783027d63343b4a5634bd81645f"));
            BlueprintTool.AddGuidsByName(("UnarmedType", "fcca8e6b85d19b14786ba1ab553e23ad"));

            BlueprintTool.AddGuidsByName(("SavesHigh", "ff4662bde9e75f145853417313842751"));
            BlueprintTool.AddGuidsByName(("SavesLow", "dc0c7c1aba755c54f96c089cdf7d14a3"));
            BlueprintTool.AddGuidsByName(("BABMedium", "4c936de4249b61e419a3fb775b9f2581"));
            BlueprintTool.AddGuidsByName(("LightArmorProficiency", "6d3728d4e9c9898458fe5e9532951132"));
            BlueprintTool.AddGuidsByName(("MediumArmorProficiency", "46f4fb320f35704488ba3d513397789d"));
            BlueprintTool.AddGuidsByName(("HeavyArmorProficiency", "1b0f68188dcc435429fb87a022239681"));
            BlueprintTool.AddGuidsByName(("SimpleWeaponProficiency", "e70ecf1ed95ca2f40b754f1adb22bbdd"));
            BlueprintTool.AddGuidsByName(("MartialWeaponProficiency", "203992ef5b35c864390b4e4a1e200629"));
            BlueprintTool.AddGuidsByName(("ShieldsProficiency", "cb8686e7357a68c42bdd9d4e65334633"));
            BlueprintTool.AddGuidsByName(("CombatReflexes", "0f8939ae6f220984e8fb568abbdfba95"));

            BlueprintTool.AddGuidsByName(("StaggeredBuff", "df3950af5a783bd4d91ab73eb8fa0fd3"));
            BlueprintTool.AddGuidsByName(("DazedBuff", "9934fedff1b14994ea90205d189c8759"));
            BlueprintTool.AddGuidsByName(("NauseatedBuff", "956331dba5125ef48afe41875a00ca0e"));

        }
        public static BlueprintRoot Root()
        {
            return BlueprintTool.Get<BlueprintRoot>("2d77316c72b9ed44f888ceefc2a131f6");
        }

        public static Sprite FlatfootedSprite()
        {
            return Root().UIRoot.FlatFootedFakeFeature.Icon;
        }
    }
}
