using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.AbilityRestrictions
{
    class IronTortoiseShieldRequiredRestriction : BlueprintComponent, IAbilityCasterRestriction
    {
        public string GetAbilityCasterRestrictionUIText()
        {
            return "Shield Bash Enabled Shield Needed";
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return caster.Proficiencies.Contains(Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup.LightShield) && caster.Body.SecondaryHand.HasShield && (caster.Body.SecondaryHand.Shield.WeaponComponent != null || !IsBash);
        }

        public bool IsBash = false;
    }
}
