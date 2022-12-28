using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;
using TheInfiniteCrusade.Backend.NewUnitParts;

namespace TheInfiniteCrusade.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintBuff))]
    class ContextAddWeaponDamageDice : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            int dice = Value.Calculate(base.Fact.MaybeContext);
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
                        SourceFact = base.Fact
                        
                        
                    });
                    
                }
                else
                {
                    evt.DamageDescription.Add(new DamageDescription()
                    {
                        Dice = new DiceFormula(dice, diceType),
                        TypeDescription = evt.Weapon.Blueprint.DamageType,
                        SourceFact = base.Fact
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

        public ContextValue Value;
        public DiceType diceType = DiceType.D6;

        public bool DealWeaponDamage;
        public bool DealVariableTypeDamage;
        public DamageTypeDescription DamageType;

        //TODO ADD CONDITIONS
    }
}
