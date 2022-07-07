using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.Extensions;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
using TheInfiniteCrusade.NewComponents.UnitParts;
using TheInfiniteCrusade.NewComponents.UnitParts.ManeuverBookSystem;

namespace TheInfiniteCrusade.ModifiedComponents
{

    class HijackSpellbookMemorization
    {
        /*
        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.IsMemorized), new Type[] { typeof(AbilityData) })]
        public static class IsMemorizedHijackPatch
        {

            static bool Prefix(Spellbook __instance, ref bool __result, [NotNull] AbilityData spell)
            {
                if (__instance.Blueprint.Components.OfType<ManeuverBookComponent>().Any())
                {
                    var comp = spell.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                    if (comp != null && comp.ManeuverType == ManeuverType.Stance)
                    {
                        __result = true;
                        return false;
                    }
                    else
                    {
                        var part = __instance.Owner.Get<UnitPartMartialDisciple>();
                        __result = part.ma book.ManeuverIsReadied(spell.Blueprint);
                        return false;

                    }

                    var book = __instance.Owner.Get<UnitPartManeuverBook>()?.GetBook(__instance);
                    if (book != null)
                    {
                     
                    }
                    else
                    {
                        __result = false;
                        return false;
                    }
                }
                return true;
            }

        }
        */

        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.GetMemorizedSpellSlots), new Type[] { typeof(int) })]
        static class Spellbook_GetMemorizedSpellSlots//This hijacking is for the spellbook display - just plain hijacking the level 1 display
        {
            static bool Prefix(Spellbook __instance, ref IEnumerable<SpellSlot> __result, int spellLevel)
            {
                var bookdef = __instance.Blueprint.Components.OfType<ManeuverBookComponent>().FirstOrDefault();

                if (bookdef != null)
                {
                    var part = __instance.Owner.Ensure<UnitPartMartialDisciple>();
                    //Main.Context.Logger.Log($"Entering GetMemorizedSpellSlots for level {spellLevel}");
                    if (spellLevel == 1)
                    {


                        __result = part.GetCurrentReadiedManeuversSlotDisplay(__instance, false, spellLevel);

                    }

                    else
                    {
                        if (__instance.m_KnownSpells.SelectMany(x => x).Any(x => x.SpellLevel == spellLevel))
                        {
                            __result = part.GetCurrentReadiedManeuversSlotDisplay(__instance, false, spellLevel);
                        }
                        else
                        {
                            __result = new List<SpellSlot>();
                        }
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
                    //Main.Context.Logger.Log($"Entering GetMemorizedSpells for level {spellLevel}");
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


        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.IsPossibleMemorize), new Type[] { typeof(AbilityData), typeof(SpellSlot) })]
        public static class IsPossibleMemorizeHijackPatch
        {

            static bool Prefix(Spellbook __instance, ref bool __result, [NotNull] AbilityData data, [CanBeNull] SpellSlot slot = null)
            {
                if (__instance.Blueprint.Components.OfType<ManeuverBookComponent>().Any())
                {

                    var disciple = __instance.Owner.Get<UnitPartMartialDisciple>();
                    if (disciple == null)
                    {
                        __result = false;
                        return false;
                    }
                    if (!disciple.CanMemorizeForBook(__instance))
                    {
                        __result = false;
                        return false;
                    }
                    var typecomp = data.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                    if (typecomp == null)
                    {
                        __result = false;
                        return false;
                    }
                    else if (typecomp.ManeuverType == ManeuverType.Stance)
                    {
                        __result = false;
                        return false;
                    }
                    Spellbook spellbook = data.Spellbook;
                    if (((spellbook != null) ? spellbook.Blueprint : null) != __instance.Blueprint)
                    {
                        PFLog.Default.Warning("Trying to memorize spell from different spellbook", Array.Empty<object>());
                        __result = false;
                        return false;
                    }





                    int memorizedCount = disciple.GetTimesMemorized(data.Blueprint, SlotLayer.Readied);
                    int memorizedCountLocal = disciple.GetTimesMemorized(data.Blueprint, SlotLayer.Readied, __instance);
                    int memorizedLimit = 1;
                    if (memorizedCount == memorizedLimit && memorizedCountLocal == memorizedLimit)
                    {
                        __result = slot.Spell != null;//Swapping!
                        return false;
                    }
                    else if (memorizedCount > memorizedLimit || memorizedCountLocal > memorizedLimit)
                    {
                        return false;
                    }
                    else
                    {
                        __result = true;
                        return false;
                    }











                    //Main.ModContextPathOfTheCrusade.Logger.Log($"Blocked IsPossibleMemorize with {data.Name}  in {__instance.Blueprint.Name} in {slot.Index}");
                    //Adapting the system to work with my setup at this layer looks like way too much work?

                }
                return true;
            }

        }



        //IsPossibleMemorize - false it to block interactions from outside my setup?
        [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.Memorize), new Type[] { typeof(AbilityData), typeof(SpellSlot) })]
        public static class MemorizePatch
        {
            static bool Prefix(Spellbook __instance, ref bool __result, [NotNull] AbilityData data, [CanBeNull] SpellSlot slot = null)
            {
                if (__instance.Blueprint.Components.OfType<ManeuverBookComponent>().Any())
                {
                    var disciple = __instance.Owner.Ensure<UnitPartMartialDisciple>();
                    if (!disciple.CanMemorizeForBook(__instance))
                    {
                        __result = false;
                        return false;
                    }

                    var typecomp = data.Blueprint.Components.OfType<ManeuverInformation>().FirstOrDefault();
                    if (typecomp == null)
                    {
                        __result = false;
                        return false;
                    }
                    else if (typecomp.ManeuverType == ManeuverType.Stance)
                    {
                        __result = false;
                        return false;
                    }
                    Spellbook spellbook = data.Spellbook;
                    if (((spellbook != null) ? spellbook.Blueprint : null) != __instance.Blueprint)
                    {
                        PFLog.Default.Warning("Trying to memorize spell from different spellbook", Array.Empty<object>());
                        __result = false;
                        return false;
                    }
                    int memorizedCount = disciple.GetTimesMemorized(data.Blueprint, SlotLayer.Readied);
                    int memorizedCountLocal = disciple.GetTimesMemorized(data.Blueprint, SlotLayer.Readied, __instance);
                    int memorizedLimit = 1;
                    if (memorizedCount == memorizedLimit && memorizedCountLocal == memorizedLimit)
                    {
                        var book = disciple.GetManeuverBook(__instance);
                        var swap1Slot =  book.ManeuverSlots.FirstOrDefault(x => x.Readied.Equals(data.Blueprint.ToReference<BlueprintAbilityReference>()));
                        var swap2Slot = book.ManeuverSlots[slot.Index];
                        swap1Slot.SetAsReadied(slot.Spell.Blueprint.ToReference<BlueprintAbilityReference>());
                        swap2Slot.SetAsReadied(data.Blueprint.ToReference<BlueprintAbilityReference>());
                        __result = true;
                        EventBus.RaiseEvent<ISpellBookUIHandler>(delegate (ISpellBookUIHandler h)
                        {
                            h.HandleMemorizedSpell(data, __instance.Owner);
                        }, true);
                        return false;
                    }
                    else if (memorizedCount > memorizedLimit || memorizedCountLocal > memorizedLimit)
                    {
                        __result = false;
                        return false;
                    }
                    else
                    {
                        var book = disciple.GetManeuverBook(__instance);
                        book.ManeuverSlots[slot.Index].SetAsReadied(data.Blueprint.ToReference<BlueprintAbilityReference>());
                        __result = true;
                        EventBus.RaiseEvent<ISpellBookUIHandler>(delegate (ISpellBookUIHandler h)
                        {
                            h.HandleMemorizedSpell(data, __instance.Owner);
                        }, true);
                        return false;
                    }
                    

                    
                   

                    
                }
                return true;
            }

        }




    }
}
