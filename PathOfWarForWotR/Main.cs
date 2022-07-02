using UnityEngine;
using UnityModManagerNet;
using UnityEngine.UI;
using HarmonyLib;
using TheInfiniteCrusade.ModLogic;
using TabletopTweaks.Core.Utilities;
using System;
using Kingmaker.Blueprints.JsonSystem;

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