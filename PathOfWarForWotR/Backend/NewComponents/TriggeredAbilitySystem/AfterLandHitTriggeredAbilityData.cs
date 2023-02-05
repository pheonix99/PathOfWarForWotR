using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using PathOfWarForWotR.Backend.NewUnitParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewComponents.TriggeredAbilitySystem
{
    class AfterLandHitTriggeredAbilityData : TriggeredAbilityData
    {
        protected override void HookToHandler(UnitPartTriggeredAbilityHandler handler)
        {
            handler.AddToAfterLandHit(this, OwnerBlueprint);
        }

        protected override void UnhookFromHander(UnitPartTriggeredAbilityHandler handler)
        {
            handler.RemoveFromAfterLandHit(this, OwnerBlueprint);
        }
    }
}
