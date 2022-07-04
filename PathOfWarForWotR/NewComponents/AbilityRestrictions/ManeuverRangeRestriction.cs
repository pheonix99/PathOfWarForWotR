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
    public class ManeuverRangeRestriction : BlueprintComponent, IAbilityCasterRestriction
    {
        public string GetAbilityCasterRestrictionUIText()
        {
            return Range ? "Requires Ranged Weapon" : "Requires Melee Weapon";
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            //TODO add stealth interop with ToggleableThrowingWeapons

            if (Range)
            {
                return caster.Body.PrimaryHand.Weapon.Blueprint.IsRanged;
            }
            else
            {
                return caster.Body.PrimaryHand.Weapon.Blueprint.IsMelee;
            }
        }

        public bool Range;
    }
}
