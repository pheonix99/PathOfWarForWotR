using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System.Collections.Generic;
using System.Linq;
using TheInfiniteCrusade.Backend.NewComponents.ManeuverBookSystem;

namespace TheInfiniteCrusade.Backend.NewUnitParts
{
    public class UnitPartManeuverVariableTypeDamage : OldStyleUnitPart
    {
        private List<DiscipleVariableEnabled> DiscipleVariableEnableds = new();

        
        
        public DamageTypeDescription GetVariableDamageForManeuver(BlueprintAbility maneuver)
        {
            var comp = maneuver.GetComponent<ManeuverInformation>();
            if (comp != null)
            {
                if (comp.DisciplineKeys.Contains("ElementalFlux"))
                {
                    return GetVariableDamageType();
                }
                else if (DiscipleVariableEnableds.Any(x=> comp.DisciplineKeys.Contains(x.Key)))
                {


                    return GetVariableDamageType();
                }
                else
                {

                    return new DamageTypeDescription()
                    {
                        Type = DamageType.Energy,
                        Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire
                    };
                }


            }


            PFLog.Default.Error("Called Variable Damage Type On non-Variable Discipline!");
            return new DamageTypeDescription()
            {
                Type = DamageType.Energy,
                Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire
            };



        }

        public DamageTypeDescription GetVariableDamageType()
        {


            return new DamageTypeDescription()
            {
                Type = DamageType.Energy,
                Energy = Kingmaker.Enums.Damage.DamageEnergyType.Fire
            };
        }

        private class DiscipleVariableEnabled
        {
            public UnitFact source;
            public string Key;
        }
    }

    
}
