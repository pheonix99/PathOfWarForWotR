using UnityEngine;
using UnityModManagerNet;
using UnityEngine.UI;
using HarmonyLib;
using PathOfWarForWotR.ModLogic;
using TabletopTweaks.Core.Utilities;

namespace PathOfWarForWotR
{
    static class Main
    {
        public static bool Enabled;
        public static PoWModContext Context;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            Context = new (modEntry);
            Context.ModEntry.OnSaveGUI = OnSaveGUI;
            Context.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize(Context);
            return true;
        }

        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Context.SaveAllSettings();
        }
    }
}