using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace TheInfiniteCrusade.Defines
{
    public class DisciplineDefine
    {
        public string SysName;
        public string DisplayName;
        public string Description;
        public WeaponFighterGroup[] WeaponGroups;
        public StatType skill;
        public BlueprintItemWeaponReference[] m_specificWeapons = new BlueprintItemWeaponReference[0];
        public WeaponCategory[] weaponCategories = new WeaponCategory[0];
        public bool alwaysSupernatural = false;
        public SpellDescriptor descriptor = SpellDescriptor.None;
        public Sprite defaultSprite;
        public readonly BlueprintGuid masterGuid;

        public DisciplineDefine(string sysName, string displayName, string description, WeaponFighterGroup[] weaponGroups, StatType skill, Sprite sprite, BlueprintItemWeaponReference[] specificWeapons = null, WeaponCategory[] weaponCategories = null, bool alwaysSupernatural = false, SpellDescriptor descriptor = SpellDescriptor.None)
        {
            SysName = sysName;
            DisplayName = displayName;
            Description = description;
            WeaponGroups = weaponGroups;
            defaultSprite = sprite;
            this.skill = skill;
            m_specificWeapons = specificWeapons != null ? specificWeapons : new BlueprintItemWeaponReference[0];
            this.weaponCategories = weaponCategories != null ? weaponCategories : new WeaponCategory[0];
            this.alwaysSupernatural = alwaysSupernatural;
            this.descriptor = descriptor;
            masterGuid = Main.Context.Blueprints.GetDerivedMaster(sysName+"MasterGUID");
        }
    }
}
