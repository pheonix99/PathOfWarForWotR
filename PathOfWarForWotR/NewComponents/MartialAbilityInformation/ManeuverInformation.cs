using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Owlcat.QA.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.MartialAbilityInformation
{
    [AllowedOn(typeof(BlueprintBuff))]
    [AllowedOn(typeof(BlueprintAbility))]
    class ManeuverInformation : BlueprintComponent
    {
        public int ManeuverLevel;
        public ManeuverType ManeuverType;
        /// <summary>
        /// If this is ever not length one on a maneuver that's *not* a PRC ability, bad things will happen!
        /// </summary>
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
