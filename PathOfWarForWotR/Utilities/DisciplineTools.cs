using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using PathOfWarForWotR.Defines;
using PathOfWarForWotR.NewComponents.MartialAbilityInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace PathOfWarForWotR.Utilities
{
    public static class DisciplineTools
    {
        public static Dictionary<string, DisciplineDefine> Disciplines = new();
        public static Dictionary<string, BlueprintGuid> MasterGuids = new();
     

       

        public static bool AddDiscipline(string sysName, string displayName, string description, WeaponFighterGroup[] weaponGroups, StatType skill, Sprite defaultSprite, BlueprintItemWeaponReference[] specificWeapons = null, WeaponCategory[] weaponCategories = null, bool alwaysSupernatural = false, SpellDescriptor descriptor = SpellDescriptor.None)
        {
            if (Disciplines.ContainsKey(sysName))
                return false;
            else
            {
                var discipline = new DisciplineDefine(sysName, displayName, description, weaponGroups, skill, defaultSprite, specificWeapons, weaponCategories, alwaysSupernatural, descriptor);
                Disciplines.Add(sysName, discipline);
                MasterGuids.Add(sysName, discipline.masterGuid);
                return true;
            }


        }

    }
}
