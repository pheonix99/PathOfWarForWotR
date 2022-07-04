using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.AbilityRestrictions
{
    class BrokenBladeDisciplineWeaponsOnlyRestriction : WeaponLimitingRestriction
    {
        public override string GetAbilityCasterRestrictionUIText()
        {
            return "Requires Discipline Weapon";
        }

        public override bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return WeaponStateIsAcceptable(caster);
        }

        

       

        public override bool WeaponStateIsAcceptable(UnitEntityData caster)
        {
            if (!caster.Body.PrimaryHand.HasItem && !caster.Body.SecondaryHand.HasItem)
                return true;
            else
            {
                if (caster.Body.PrimaryHand.MaybeWeapon != null && CheckWeapon(caster.Body.PrimaryHand.MaybeWeapon))
                {
                    return false;

                }
                if (caster.Body.SecondaryHand.MaybeWeapon == null)
                    return true;
                else
                {
                    return CheckWeapon(caster.Body.SecondaryHand.MaybeWeapon);
                }


            }
        }

        private bool CheckWeapon(ItemEntityWeapon weapon)
        {
            if (weapon.Blueprint.Category == Kingmaker.Enums.WeaponCategory.UnarmedStrike)
                return true;
            else if (weapon.Blueprint.FighterGroup.HasFlag(Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroupFlags.Close))
                return true;
            else if (weapon.Blueprint.FighterGroup.HasFlag(Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroupFlags.Monk))
                return true;
            else if (weapon.Blueprint.FighterGroup.HasFlag(Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroupFlags.Natural))
                return true;

            return false;
        }
    }
}
