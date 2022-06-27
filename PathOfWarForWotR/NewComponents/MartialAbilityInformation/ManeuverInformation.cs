using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.NewComponents.MartialAbilityInformation
{
    [AllowedOn(typeof(BlueprintBuff))]
    [AllowedOn(typeof(BlueprintAbility))]
    class ManeuverInformation : BlueprintComponent
    {
        public int ManeuverLevel;
        public ManeuverType ManeuverType;
        public string[] DisciplineKeys = new string[0];
        public bool isPrcAbility = false;


    }

    public enum ManeuverType
    {
        Strike,
        Boost,
        Counter,
        Stance,
        Other
    }
}
