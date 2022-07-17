using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.ModifiedComponents
{
    class PatchForLoading
    {
        

        [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.OnPostLoad))]
        static class UnitEntityData_QueryPostload
        {
            static void Postfix(UnitEntityData __instance)
            {
                Main.Context.Logger.Log($"UnitEntityData {__instance.CharacterName} Postload fired");
            }
        }

    }
}
