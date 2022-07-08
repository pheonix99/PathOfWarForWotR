using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheInfiniteCrusade.NewComponents.Abilities
{
    [AllowedOn(typeof(BlueprintAbility))]
    class AbilityTargetsInReach : UnitFactComponentDelegate
    {
        public Feet Reach()
        {

            return new Feet(Owner.Body.PrimaryHand.Weapon.AttackRange.Meters + Owner.Corpulence);

        }
    }

    static class HackAbilityTargetsAround
    {


        [HarmonyPatch(typeof(AbilityTargetsAround), nameof(AbilityTargetsAround.AoERadius), MethodType.Getter)]
        static class HackAbilityTargetsAround_AoERadius
        {
            static void Postfix(AbilityTargetsAround __instance, ref Feet __result)
            {
                var hijacker = __instance.OwnerBlueprint.GetComponent<AbilityTargetsInReach>();

                try
                {
                    if (hijacker != null && hijacker.Owner != null)
                    {
                        __result = hijacker.Reach();
                        return;
                    }



                }
                catch
                {

                }


            }

        }

    }

}

