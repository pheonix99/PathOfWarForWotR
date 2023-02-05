using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using PathOfWarForWotR.Backend.NewUnitParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewComponents.TriggeredAbilitySystem
{
    public abstract class TriggeredAbilityData : UnitFactComponentDelegate 
    {
       

        public ActionList ActionList;

        

        public ContextCondition SelfRequirements;

        public ContextCondition OtherTriggererRequirements;

        public ContextCondition NearbyAllyRequirements;

        public Feet RadiusToCheck;

        public override void OnActivate()
        {
            OnTurnOn();
        }

        public override void OnDeactivate()
        {
            OnTurnOff();
        }

        public override void OnTurnOn()
        {

            base.OnTurnOn();
            var part = base.Owner.Ensure<UnitPartTriggeredAbilityHandler>();
            HookToHandler(part);


        }

        protected abstract void HookToHandler(UnitPartTriggeredAbilityHandler handler);
        

        public override void OnTurnOff()
        {
            var part = base.Owner.Get<UnitPartTriggeredAbilityHandler>();
            if (part != null)
                UnhookFromHander(part);

        }

        protected abstract void UnhookFromHander(UnitPartTriggeredAbilityHandler handler);
    }

    
}
