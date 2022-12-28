using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;

namespace TheInfiniteCrusade.Backend.NewUnitParts
{
    class UnitPartMartialTraining : OldStyleUnitPart
    {
        internal bool CanLearnManeuver(BlueprintAbilityReference manuever)
        {
            return false;
        }

        internal bool IsThisDiscipline(string v)
        {
            return false;
        }

        internal int GetMaxLevel()
        {
            throw new NotImplementedException();
        }
    }
}
