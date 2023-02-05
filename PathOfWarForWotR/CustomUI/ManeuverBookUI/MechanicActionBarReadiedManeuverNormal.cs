using Kingmaker.UnitLogic.Abilities;
using Newtonsoft.Json;
using PathOfWarForWotR.Backend.NewUnitDataClasses;
using PathOfWarForWotR.Backend.NewUnitParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.CustomUI.ManeuverBookUI
{
    class MechanicActionBarReadiedManeuverNormal : MechanicActionBarMartial
    {
		
		public MechanicActionBarReadiedManeuverNormal(AbilityData spellSlot)
		{
			this._manuever = spellSlot;
		}

		private AbilityData _manuever { get; set; }

		public override AbilityData Maneuver
		{
			get
			{
				
				
				return _manuever;
			}
		}
	}
}
