using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.AbilityRestrictions
{
   
    [AllowedOn(typeof(BlueprintActivatableAbility), false)]
    [AllowedOn(typeof(BlueprintAbility), false)]
    
    public abstract class AbstractMartialAbilityRestriction : BlueprintComponent, IAbilityCasterRestriction
    {
        public abstract string GetAbilityCasterRestrictionUIText();

        public abstract bool IsCasterRestrictionPassed(UnitEntityData caster);
        
    }
}
