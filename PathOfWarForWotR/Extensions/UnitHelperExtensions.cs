using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using TheInfiniteCrusade.Backend.NewUnitDataClasses;

namespace TheInfiniteCrusade.Extensions
{
    public static class UnitHelperExtensions
    {
		public static void CopyManeuverBook(UnitEntityData original, UnitEntityData target)
		{
			foreach (ManeuverBook spellbook in original.Descriptor.ManeuverBooks())
			{
				ManeuverBook spellbook2 = target.Descriptor.DemandManeuverBook(spellbook.Blueprint);
				
				foreach (var maneuver in spellbook.GetKnownMartialTechniques())
				{
					spellbook2.LearnManeuver(maneuver.ToReference<BlueprintAbilityReference>());
				}
			}
		}

		[HarmonyPatch(typeof(UnitHelper), nameof(UnitHelper.CopySpellbook))]
		static class UnitHelper_CopySpellbook
		{
			static void Postfix(UnitEntityData original, UnitEntityData target)
			{
				CopyManeuverBook(original, target);
			}
		}

	}
}
