﻿using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Text;
using PathOfWarForWotR.Utilities;

namespace PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem
{
    [AllowedOn(typeof(BlueprintBuff))]
    [AllowedOn(typeof(BlueprintAbility))]
    class ManeuverInformation : UnitFactComponentDelegate
    {
        public int ManeuverLevel;
        public ManeuverType ManeuverType;
        /// <summary>
        /// If this is ever not length one on a maneuver that's *not* a PRC ability, bad things will happen!
        /// </summary>
        public string[] DisciplineKeys = new string[0];
        public bool isPrcAbility = false;

        public string GetManeuverSchoolString()
        {
            StringBuilder s = new();
            if (DisciplineKeys.Length == 0)
            {
                return "No Discipline";
            }
            else
            {
                for(int i = 0;i<DisciplineKeys.Length;i++)
                {
                   if (DisciplineTools.Disciplines.TryGetValue(DisciplineKeys[i], out var discipline))
                    {
                        s.Append(discipline.DisplayName);
                        if (i+1 < DisciplineKeys.Length)
                        {
                            s.Append(", ");
                        }
                    }

                }

                return s.ToString();
            }
            
        }
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
