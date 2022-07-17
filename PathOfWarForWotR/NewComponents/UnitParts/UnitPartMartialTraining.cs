using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.UnitParts
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
