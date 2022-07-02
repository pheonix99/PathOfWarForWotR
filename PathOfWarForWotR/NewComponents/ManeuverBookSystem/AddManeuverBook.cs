using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.NewComponents.UnitParts;

namespace TheInfiniteCrusade.NewComponents.ManeuverBookSystem
{
	public class AddManeuverBook : AddSpellbook, IUnitReapplyFeaturesOnLevelUpHandler, IUnitSubscriber, ISubscriber
	{

		public ManeuverBookComponent ManeuverBookComponent
		{
			get
			{
				return Spellbook.Components.OfType<ManeuverBookComponent>().FirstOrDefault();
			}
		}

		public override void OnActivate()
		{
			if (base.IsReapplying)
			{
				return;
			}
			Main.Context.Logger.Log($"Maneuver Book Component {OwnerBlueprint.NameSafe()} Added To {Owner.CharacterName}");


			var part = base.Owner.Ensure<UnitPartMartialDisciple>();
			part.RegisterBook(base.Fact, Spellbook, ManeuverBookComponent);


		}

	}
}
