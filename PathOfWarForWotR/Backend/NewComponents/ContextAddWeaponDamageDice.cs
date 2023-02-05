using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using System.Linq;
using PathOfWarForWotR.Backend.NewUnitParts;
using Kingmaker.Blueprints.Facts;

namespace PathOfWarForWotR.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintBuff))]
    class ContextAddWeaponDamageDice : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        

        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (RangedOnly && !evt.Weapon.Blueprint.IsRanged)
                return;

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
                    TypeDescription = DamageType,
                    SourceFact = base.Fact

                });
            }
            else
            {
                var part = Owner.Ensure<UnitPartManeuverVariableTypeDamage>();
                evt.DamageDescription.Add(new DamageDescription()
                {
                    Dice = new DiceFormula(dice, diceType),
                    TypeDescription = part.GetVariableDamageForManeuver(evt.Reason.Ability.Blueprint),
                    SourceFact = base.Fact
                });
            }
           

        }

        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            if (RangedOnly && !evt.Weapon.Blueprint.IsRanged)
                return;
            if (Once)
                Owner.RemoveFact((BlueprintUnitFact)OwnerBlueprint);
        }

        public ContextValue Value;
        public DiceType diceType = DiceType.D6;

        public bool DealWeaponDamage;
        public bool DealVariableTypeDamage;
        public DamageTypeDescription DamageType;

        public bool Once = false;

        public bool RangedOnly = false;

        //TODO ADD CONDITIONS
    }
}
