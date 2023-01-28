using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewUnitParts;
using PathOfWarForWotR.Extensions;

namespace PathOfWarForWotR.Backend.NewActions
{
    class RecoverSelectedManeuver : AbilityApplyEffect, IAbilityRestriction, IAbilityRequiredParameters
    {
        public AbilityParameter RequiredParameters => AbilityParameter.SpellSlot;

        public override void Apply(AbilityExecutionContext context, TargetWrapper target)
        {

            context.Caster.DemandManeuverBook(maneuverBook).RecoverManeuver(context.Ability.Blueprint.ToReference<BlueprintAbilityReference>());
        }

       


        public string GetAbilityRestrictionUIText()
        {
            return "";
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            var part = ability.Caster?.Get<UnitPartMartialDisciple>();
            if (part == null)
            {
                return false;
            }
            else
            {
                return ability.Caster.DemandManeuverBook(maneuverBook).CanRecover(ability.Blueprint.ToReference<BlueprintAbilityReference>());
                
            }
        }



        public BlueprintManeuverBook maneuverBook => m_maneuverBook.Get();
        public BlueprintManeuverBookReference m_maneuverBook;
    }
}
