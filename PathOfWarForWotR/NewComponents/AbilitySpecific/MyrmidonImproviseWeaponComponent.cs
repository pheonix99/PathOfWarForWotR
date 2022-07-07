using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace TheInfiniteCrusade.NewComponents.AbilitySpecific
{
    class MyrmidonImproviseWeaponComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber
    {
        

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if(Owner.Resources.GetResourceAmount(BlueprintTools.GetModBlueprint<BlueprintAbilityResource>(Main.Context, "MyrmidonGritResource")) >= 1)
            {
                if (evt.AllBonuses.Any(x=>x.Fact.Blueprint.AssetGuidThreadSafe== "9f69df901e2a4106a8d871b968c9acd8"))
                {
                    evt.AddModifier(2, Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
                }
                if (evt.AllBonuses.Any(x => x.Fact.Blueprint.AssetGuidThreadSafe == "53632fb9663445b19105025adf450b0a"))
                {
                    evt.AddModifier(2, Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
                }
                if (evt.AllBonuses.Any(x => x.Fact.Blueprint.AssetGuidThreadSafe == "07de6c83aa2c423f897b6904ba270216"))
                {
                    evt.AddModifier(2, Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
                }
                if (evt.AllBonuses.Any(x => x.Fact.Blueprint.AssetGuidThreadSafe == "e09c0fd1fd8248059db544e2a14bc2f0"))
                {
                    evt.AddModifier(2, Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
                }
            }
        }

        

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            
        }
    }
}
