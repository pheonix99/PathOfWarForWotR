using Kingmaker.PubSubSystem;
using System;

namespace TheInfiniteCrusade.CustomUI.ManeuverBook
{
    internal class BookWatcher : IAreaHandler
    {
        public static void Safely(Action a)
        {
            try
            {
                a.Invoke();
            }
            catch (Exception ex)
            {
                Main.Context.Logger.LogError(ex, "");
            }
        }

        public void OnAreaBeginUnloading()
        {

        }

        public void OnAreaDidLoad()
        {
            Main.Context.Logger.Log($"Area Loaded In SpellbookWatcher");
            GlobalUIHandler.Instance.TryInstallUI();
        }
    }
}
