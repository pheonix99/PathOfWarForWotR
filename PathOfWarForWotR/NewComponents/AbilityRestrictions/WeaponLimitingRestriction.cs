using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Extensions;


namespace TheInfiniteCrusade.NewComponents.AbilityRestrictions
{
    public abstract class WeaponLimitingRestriction : AbstractMartialAbilityRestriction
    {


       
        

        

        public abstract bool WeaponStateIsAcceptable(UnitEntityData caster);
        
        
    }
}
