using Kingmaker.EntitySystem.Entities;


namespace TheInfiniteCrusade.Backend.NewComponents.AbilityRestrictions
{
    public abstract class WeaponLimitingRestriction : AbstractMartialAbilityRestriction
    {


       
        

        

        public abstract bool WeaponStateIsAcceptable(UnitEntityData caster);
        
        
    }
}
