using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.CustomUI.UnitLogic.Class.LevelUp;

namespace TheInfiniteCrusade.Extensions
{
    
   public static class LevelUpStateExtensions
    { 
        private static List<ManeuverSelectionData> ManeuverSelectionDatas;

        public static ManeuverSelectionData DemandManeuverSelection(this LevelUpState state, BlueprintManeuverBook book, BlueprintSpellList list)
        {
            ManeuverSelectionData data = state.ManeuverSelections().FirstOrDefault((ManeuverSelectionData s) => s.ManeuverBook == book && s.SpellList == list);
            if (data == null)
            {
                data = new ManeuverSelectionData(book, list);
                state.ManeuverSelections().Add(data);
            }
            return data;
        }

        public static ManeuverSelectionData GetManeuverSelection(this LevelUpState state, [NotNull] BlueprintManeuverBook spellbook, [NotNull] BlueprintSpellList spellList)
        {
            var list = state.ManeuverSelections();
            return list.FirstOrDefault((ManeuverSelectionData s) => s.ManeuverBook == spellbook && s.SpellList == spellList);
        }

        public static List<ManeuverSelectionData> ManeuverSelections(this LevelUpState state)
        {
            return ManeuverSelectionDatas;
           
        }

        [HarmonyPatch(typeof(LevelUpState),  MethodType.Constructor)]
        [HarmonyPatch(new[] { typeof(UnitEntityData), typeof(LevelUpState.CharBuildMode), typeof(bool) })]
        static class Spellbook_GetMemorizedSpellSlots//This hijacking is for the spellbook display - just plain hijacking the level 1 display
        {
            static void Postfix(LevelUpState __instance, [NotNull] UnitEntityData unit, LevelUpState.CharBuildMode mode, bool isPregen)
            {

                ManeuverSelectionDatas = new();

            }
        }

    }
}
