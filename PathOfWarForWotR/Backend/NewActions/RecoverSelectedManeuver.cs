using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.Extensions;

namespace TheInfiniteCrusade.Backend.NewActions
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
