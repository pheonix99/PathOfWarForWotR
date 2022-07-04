using HarmonyLib;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Extensions;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.NewComponents.UnitParts;

namespace TheInfiniteCrusade.ModifiedComponents
{
    
    class HijackSpellbookMemorization
    {
        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.GetMemorizedSpellSlots), new Type[] { typeof(int) })]
        static class Spellbook_GetMemorizedSpellSlots//This hijacking is for the spellbook display - just plain hijacking the level 1 display
        {
            static bool Prefix(Spellbook __instance, ref IEnumerable<SpellSlot> __result, int spellLevel)
            {
                var bookdef = __instance.Blueprint.Components.OfType<ManeuverBookComponent>().FirstOrDefault();

                if (bookdef != null)
                {
                    Main.Context.Logger.Log($"Entering GetMemorizedSpellSlots for level {spellLevel}");
                    if (spellLevel == 1)
                    {

                        var part = __instance.Owner.Ensure<UnitPartMartialDisciple>();
                        __result = part.GetCurrentReadiedManeuversSlotDisplay(__instance, false, spellLevel);

                    }
                    
                    else
                    {
                        __result = new List<SpellSlot>();

                    }
                    return false;




                }
                return true;

            }

        }


        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.GetMemorizedSpells), new Type[] { typeof(int) })]
        static class Spellbook_GetMemorizedSpells//This is for displaying availaible moves in the combat zone
        {
            static bool Prefix(Spellbook __instance, ref IEnumerable<SpellSlot> __result, int spellLevel)
            {
                var bookdef = __instance.Blueprint.Components.OfType<ManeuverBookComponent>().FirstOrDefault();

                if (bookdef != null)
                {
                    Main.Context.Logger.Log($"Entering GetMemorizedSpells for level {spellLevel}");
                    var part = __instance.Owner.Ensure<UnitPartMartialDisciple>();
                    
                    if (spellLevel == 1)
                    {

                        var stances = __instance.GetAllKnownStances().ToList();
                        List<SpellSlot> spellSlots = new();
                        for (int i = 0; i < stances.Count; i++)
                        {
                            var slot = stances[i];
                            var newSlot = new SpellSlot(0, SpellSlotType.Common, i);
                            newSlot.Spell = slot;
                            newSlot.Available = true;
                            spellSlots.Add(newSlot);
                        }
                        spellSlots.AddRange(part.GetCurrentReadiedManeuversSlotDisplay(__instance, false, spellLevel).Where(x => x.Available));
                        __result = spellSlots;
                        return false;

                    }

                    else
                    {
                        __result = new List<SpellSlot>();

                    }

                    return false;

                }
                return true;

            }

        }
    }
}
