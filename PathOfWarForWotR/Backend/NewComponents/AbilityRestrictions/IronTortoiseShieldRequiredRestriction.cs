using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PathOfWarForWotR.Backend.NewComponents.AbilityRestrictions
{
    class IronTortoiseShieldRequiredRestriction : BlueprintComponent, IAbilityCasterRestriction
    {
        public string GetAbilityCasterRestrictionUIText()
        {
            return "Iron Tortoise Maneuvers Require A Shield";
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return caster.Proficiencies.Contains(Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup.LightShield) && caster.Body.SecondaryHand.HasShield && (caster.Body.SecondaryHand.Shield.WeaponComponent != null || !IsBash);
        }

        public bool IsBash = false;
    }
}
