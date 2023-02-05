using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using PathOfWarForWotR.Backend.NewComponents.TriggeredAbilitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewUnitParts
{
    public class UnitPartTriggeredAbilityHandler : OldStyleUnitPart, IInitiatorRulebookHandler<RuleAttackWithWeapon>, ITargetRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            
        }

        internal void RemoveFromAfterLandHit(AfterLandHitTriggeredAbilityData afterLandHitTriggeredAbilityData, BlueprintScriptableObject ownerBlueprint)
        {
            throw new NotImplementedException();
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.Initiator == Owner.Unit)
            {

            }
        }

        internal void AddToAfterLandHit(AfterLandHitTriggeredAbilityData afterLandHitTriggeredAbilityData, BlueprintScriptableObject ownerBlueprint)
        {
            throw new NotImplementedException();
        }
    }
}
