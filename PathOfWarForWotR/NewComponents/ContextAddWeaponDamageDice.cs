using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.NewComponents.UnitParts;

namespace TheInfiniteCrusade.NewComponents
{
    [AllowedOn(typeof(BlueprintBuff))]
    class ContextAddWeaponDamageDice : EntityFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            int dice = contextValue.Calculate(evt.Reason.Context);
            if (DealWeaponDamage)
            {
                if (evt.DamageDescription.Any())
                {
                    evt.DamageDescription.Add(new DamageDescription()
                    {
                        Dice = new DiceFormula(dice, diceType),
                        TypeDescription = evt.DamageDescription[0].TypeDescription,
                        IgnoreImmunities = evt.DamageDescription[0].IgnoreImmunities,
                        IgnoreReduction = evt.DamageDescription[0].IgnoreReduction,

                    });
                }
                else
                {
                    evt.DamageDescription.Add(new DamageDescription()
                    {
                        Dice = new DiceFormula(dice, diceType),
                        TypeDescription = evt.Weapon.Blueprint.DamageType

                    });
                }

            }
            else if (!DealVariableTypeDamage)
            {
                evt.DamageDescription.Add(new DamageDescription()
                {
                    Dice = new DiceFormula(dice, diceType),
                    TypeDescription = DamageType

                });
            }
            else
            {
                var part = Owner.Ensure<UnitPartManeuverVariableTypeDamage>();
                evt.DamageDescription.Add(new DamageDescription()
                {
                    Dice = new DiceFormula(dice, diceType),
                    TypeDescription = part.GetVariableDamageForManeuver(evt.Reason.Ability.Blueprint)
                });
            }
           

        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            
        }

        public ContextValue contextValue;
        public DiceType diceType = DiceType.D6;

        public bool DealWeaponDamage;
        public bool DealVariableTypeDamage;
        public DamageTypeDescription DamageType;
    }
}
