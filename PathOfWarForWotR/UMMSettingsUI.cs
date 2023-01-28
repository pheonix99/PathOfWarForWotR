using TabletopTweaks.Core.UMMTools;
using UnityModManagerNet;

namespace PathOfWarForWotR
{
    internal static class UMMSettingsUI
    {
        private static int selectedTab;
        public static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            UI.AutoWidth();
            UI.TabBar(ref selectedTab,
                    () => UI.Label("SETTINGS WILL NOT BE UPDATED UNTIL YOU RESTART YOUR GAME.".yellow().bold())
            );
        }
    }
}
