using UnityModManagerNet;
using HarmonyLib;
using TheInfiniteCrusade.ModLogic;
using TabletopTweaks.Core.Utilities;
using System;
using Kingmaker.Blueprints.JsonSystem;
using TheInfiniteCrusade.CustomUI.ManeuverBook;

namespace TheInfiniteCrusade
{
    static class Main
    {
        public static bool Enabled;
        public static TICModContext Context;
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            Context = new (modEntry);
            Context.ModEntry.OnSaveGUI = OnSaveGUI;
            Context.ModEntry.OnGUI = UMMSettingsUI.OnGUI;
            harmony.PatchAll();
            PostPatchInitializer.Initialize(Context);
            Context.Logger.Log("About To Run GlobalUIHandler.Install");
            GlobalUIHandler.Install();
            Context.Logger.Log("Ran GlobalUIHandler.Install");
            return true;
        }
        public static void Safely(Action act)
        {
            try
            {
                act();
            }
            catch (Exception ex)
            {
                Context.Logger.LogError(ex, "trying to safely invoke action");
            }
        }
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            Context.SaveAllSettings();
        }

        public static void LogPatch(IScriptableObjectWithAssetId obj)
        {
            Context.Logger.LogPatch(obj);
        }
    }
}