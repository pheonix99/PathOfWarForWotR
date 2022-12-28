using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;

namespace TheInfiniteCrusade.Backend.NewComponents.AbilityRestrictions
{
    [AllowedOn(typeof(BlueprintActivatableAbility), false)]
    [AllowedOn(typeof(BlueprintAbility), false)]
    public class ScarletThroneNoShieldRule : BlueprintComponent, IAbilityCasterRestriction
    {
        public string GetAbilityCasterRestrictionUIText()
        {
            return "Must Be Einhanding";
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            if (!caster.Body.SecondaryHand.HasItem )
            {
                return true;
            }
            if (caster.Body.SecondaryHand.HasShield)
            {
                if (caster.Body.SecondaryHand.Shield.Blueprint.Type.ProficiencyGroup == Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup.Buckler)
                    return true;
                else return false;
            }
            if (caster.Body.SecondaryHand.MaybeWeapon != null)
            {
                if (caster.Body.SecondaryHand.MaybeWeapon.HoldInTwoHands)
                    return AllowTwoHanderAtAll;
            }

            return false;
        }
        public bool AllowTwoHanderAtAll = true;
    }
}
