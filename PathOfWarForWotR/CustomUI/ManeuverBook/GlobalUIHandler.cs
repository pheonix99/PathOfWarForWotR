using Kingmaker.PubSubSystem;
using System;

namespace PathOfWarForWotR.CustomUI.ManeuverBook
{
    class GlobalUIHandler
    {
        public static GlobalUIHandler Instance { get; private set; }
        public static BookWatcher BookWatcher;
        public ManeuverBookController ManeuverBookController;

        public static void Install()
        {
            Main.Context.Logger.Log("Installed GlobalPath");
            Instance = new();
            BookWatcher = new();
            EventBus.Subscribe(Instance);

            EventBus.Subscribe(BookWatcher);

        }

        internal void TryInstallUI()
        {
            try
            {
                var spellScreen = UIHelpers.SpellbookScreen.gameObject;
                if (spellScreen.GetComponent<ManeuverBookController>() == null)
                {
                    Main.Context.Logger.Log("Creating new controller");
                    ManeuverBookController = spellScreen.AddComponent<ManeuverBookController>();
                    ManeuverBookController.Create();




                    
                }
            }
            catch (Exception e)
            {
                Main.Context.Logger.LogError(e, "Error in tryInstallUI");
            }

        }
    }
}
