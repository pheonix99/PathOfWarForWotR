using Kingmaker.EntitySystem.Entities;


namespace PathOfWarForWotR.Backend.NewComponents.AbilityRestrictions
{
    public abstract class WeaponLimitingRestriction : AbstractMartialAbilityRestriction
    {


       
        

        

        public abstract bool WeaponStateIsAcceptable(UnitEntityData caster);
        
        
    }
}
