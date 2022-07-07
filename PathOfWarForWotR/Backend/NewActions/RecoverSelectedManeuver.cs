using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewUnitParts;

namespace TheInfiniteCrusade.Backend.NewActions
{
    class RecoverSelectedManeuver : AbilityApplyEffect, IAbilityRestriction, IAbilityRequiredParameters
    {
        public AbilityParameter RequiredParameters => AbilityParameter.SpellSlot;

        public override void Apply(AbilityExecutionContext context, TargetWrapper target)
        {
            if (context.Ability.ParamSpellSlot == null || context.Ability.ParamSpellSlot.Spell == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spell is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            if (context.Ability.ParamSpellSlot.Spell.Spellbook == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Spellbook is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            context.Caster.Get<UnitPartMartialDisciple>().DoRecoverManeuversForBook(context.Ability.ParamSpellSlot.Spell.Spellbook, context.Ability.ParamSpellSlot.Spell.Blueprint.ToReference<BlueprintAbilityReference>());
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
                return part.CanRecoverManeuverForBook(spellbookReference, ability.Blueprint.ToReference<BlueprintAbilityReference>());
            }
        }

        

        public BlueprintSpellbookReference spellbookReference;
    }
}
