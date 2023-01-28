using TabletopTweaks.Core.ModLogic;
using UnityModManagerNet;

namespace PathOfWarForWotR.ModLogic
{
    class TICModContext : ModContextBase
    {
        public TICModContext(UnityModManager.ModEntry modEntry) : base(modEntry)
        {
#if DEBUG
            Debug = true;
#endif
            LoadAllSettings();
        }

        public override void LoadAllSettings()
        {
            LoadBlueprints("PathOfWarForWotR.Config", this);
            LoadLocalization("PathOfWarForWotR.Localization");

        }

        public override void AfterBlueprintCachePatches()
        {
            base.AfterBlueprintCachePatches();
            if (Debug)
            {
                Blueprints.RemoveUnused();
                SaveSettings(BlueprintsFile, Blueprints);
                ModLocalizationPack.RemoveUnused();
                SaveLocalization(ModLocalizationPack);
            }
        }
       
    }
}
