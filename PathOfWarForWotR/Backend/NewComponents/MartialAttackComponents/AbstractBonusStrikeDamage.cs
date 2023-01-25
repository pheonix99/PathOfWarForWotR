using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Utility;
using System.Linq;
using TheInfiniteCrusade.Backend.NewUnitParts;

namespace TheInfiniteCrusade.Backend.NewComponents.MartialAttackComponents
{
    public abstract class AbstractBonusStrikeDamage : AbstractMartialAttackWeaponModifier
    {
        public ConditionsChecker targetCondition;
        public ConditionsChecker userCondition;
        public int m_FlatDamage = 0;
        public int m_DiceCount = 0;
        public DiceType m_DiceType = DiceType.D6;
        public bool IsPrecision;
        public abstract DamageDescription GetDamage(RuleCalculateWeaponStats weapon);
        public override void OnEventDidTrigger_RuleCalculateWeaponStats(RuleCalculateWeaponStats evt)
        {
            if (userCondition != null)
            {
                using (evt.Reason.Context.GetDataScope())
                {
                    if (!userCondition.Check())
                        return;
                }
            }
            if (targetCondition != null)
            {
                using (evt.Reason.Context.GetDataScope(evt.AttackWithWeapon.Target))
                {
                    if (!targetCondition.Check())
                        return;
                }
            }
            var dmgDes = GetDamage(evt);
            if (dmgDes != null)
            {
                evt.DamageDescription.Add(dmgDes);
            }
        }
    }

    public class FixedTypeBonusDamge : AbstractBonusStrikeDamage
    {
        public DamageTypeDescription DamageTypeDescription;
        public override DamageDescription GetDamage(RuleCalculateWeaponStats weapon)
        {
            return new DamageDescription()
            {
                TypeDescription = DamageTypeDescription,
                Dice = new DiceFormula(m_DiceCount, m_DiceType),
                Bonus = m_FlatDamage

            };
        }
    }

    public class WeaponBonusDamage : AbstractBonusStrikeDamage
    {
        public int ExtraDamageOnTwoHands = 0;

        public bool IgnoresDr { get; internal set; }

        public override DamageDescription GetDamage(RuleCalculateWeaponStats weapon)
        {
            if (IsPrecision && (weapon.AttackWithWeapon?.AttackRoll?.ImmuneToCriticalHit == true || weapon.AttackWithWeapon?.AttackRoll?.ImmuneToSneakAttack == true))
            {
                return null;
            }
            if (weapon.DamageDescription.FirstOrDefault() != null)
            {


                DamageDescription damageDescription = new();
                var weaponDMG = weapon.DamageDescription.FirstItem();
                damageDescription.TypeDescription = weaponDMG.TypeDescription;
                damageDescription.IgnoreImmunities = weaponDMG.IgnoreImmunities;
                damageDescription.IgnoreReduction = weaponDMG.IgnoreReduction;
                damageDescription.SourceFact = weapon.Reason.Ability.Fact;
                damageDescription.CausedByCheckFail = weaponDMG.CausedByCheckFail;
                damageDescription.Dice = new DiceFormula(diceType: m_DiceType, rollsCount: m_DiceCount);
                if (weapon.Weapon.HoldInTwoHands)
                {
                    damageDescription.Bonus = m_FlatDamage + ExtraDamageOnTwoHands;
                }
                else
                {
                    damageDescription.Bonus = m_FlatDamage;
                }
                if (IgnoresDr)
                {
                    damageDescription.IgnoreReduction = true;
                    damageDescription.IgnoreImmunities = true;
                }


                return damageDescription;
            }
            else
            {

                var v = weapon.AttackWithWeapon.ResolveRules.Select(x => x.Damage).FirstOrDefault();
                if (v != null)
                {
                    DamageDescription damageDescription = new();
                    damageDescription.TypeDescription = weapon.DamageDescription.FirstItem().TypeDescription;
                    damageDescription.Dice = new DiceFormula(diceType: m_DiceType, rollsCount: m_DiceCount);
                    damageDescription.Bonus = m_FlatDamage;

                    return damageDescription;
                }
                else
                {

                    return null;
                }

            }
        }
    }

    public class VariableTypeBonusDamage : AbstractBonusStrikeDamage
    {

        public override DamageDescription GetDamage(RuleCalculateWeaponStats weapon)
        {
            return new DamageDescription()
            {
                TypeDescription = weapon.Initiator.Ensure<UnitPartManeuverVariableTypeDamage>().GetVariableDamageForManeuver(weapon.Reason.Ability.Blueprint),
                Dice = new DiceFormula(m_DiceCount, m_DiceType),
                Bonus = m_FlatDamage

            };
        }
    }
}
