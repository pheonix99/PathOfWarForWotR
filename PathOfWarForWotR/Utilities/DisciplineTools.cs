using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using TheInfiniteCrusade.Defines;
using System.Collections.Generic;
using UnityEngine;

namespace TheInfiniteCrusade.Utilities
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

        internal static List<DisciplineDefine> GetAllDisciplinesForProgressionDefinition(InitiatorProgressionDefine define)
        {
            List<DisciplineDefine> disciplineDefines = new();
            foreach(string s in define.FixedUnlocks)
            {
                if (Disciplines.TryGetValue(s, out var disc))
                {
                    if (!disciplineDefines.Contains(disc))
                        disciplineDefines.Add(disc);
                }
            }
            foreach(string s in define.SelectionUnlocks)
            {
                if (Disciplines.TryGetValue(s, out var disc))
                {
                    if (!disciplineDefines.Contains(disc))
                        disciplineDefines.Add(disc);
                }
            }
            foreach(string s in define.ProgressionSpecificSubstitutions)
            {
                if (Disciplines.TryGetValue(s, out var disc))
                {
                    if (!disciplineDefines.Contains(disc))
                        disciplineDefines.Add(disc);
                }
            }    

            return disciplineDefines;
        }
    }
}
