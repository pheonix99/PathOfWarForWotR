using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using static Kingmaker.UI.Context.MenuItem;

namespace PathOfWarForWotR.Backend.NewComponents.AbilitySpecific
{
    [AllowedOn(typeof(BlueprintBuff), false)]
    class BronzeKnuckleComponent : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, IInitiatorRulebookHandler<RulePrepareDamage>, IRulebookHandler<RulePrepareDamage>, ISubscriber, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            bool apply = false;
            if (evt.DamageBundle.Weapon != null)
            {
                if (evt.DamageBundle.Weapon.Blueprint.IsUnarmed)
                {
                    apply = true;
                }
            }
            if (evt.Reason.Ability != null)
            {
                var maneuver = evt.Reason.Ability.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                if (maneuver != null)
                {

                    if (maneuver.ManeuverType == ManeuverType.Strike && maneuver.DisciplineKeys.Contains("BrokenBlade"))
                    {
                        apply = true;
                    }

                }

            }
            if (apply == true)
            {
                foreach (BaseDamage baseDamage in evt.DamageBundle)
                {
                    if (baseDamage is PhysicalDamage)
                    {
                        baseDamage.IgnoreReduction = true;
                    }
                }

            }
        }

        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            bool apply = false;
            if (evt.DamageBundle.Weapon != null)
            {

                if (evt.DamageBundle.Weapon.Blueprint.IsUnarmed)
                {
                    apply = true;
                }
            }
            if (evt.Reason.Ability != null)
            {
                var maneuver = evt.Reason.Ability.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                if (maneuver != null)
                {
                    
                    if (maneuver.ManeuverType == ManeuverType.Strike && maneuver.DisciplineKeys.Contains("BrokenBlade"))
                    {
                        apply = true;
                    }
                    
                }

            }
            if (apply == true)
            {
                //TODO MAKE THIS NOT DIRECT
                BaseDamage dmg = new DamageDescription() { Dice = new Kingmaker.RuleSystem.DiceFormula(2, Kingmaker.RuleSystem.DiceType.D6), TypeDescription = new DamageTypeDescription() { Type = DamageType.Direct } }.CreateDamage();
                evt.Add(dmg);
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {

        }

        public void OnEventDidTrigger(RulePrepareDamage evt)
        {

        }
    }
}
