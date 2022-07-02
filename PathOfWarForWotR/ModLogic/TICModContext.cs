using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.ModLogic;
using UnityModManagerNet;

namespace TheInfiniteCrusade.ModLogic
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
            LoadBlueprints("TheInfiniteCrusade.Config", this);
            LoadLocalization("TheInfiniteCrusade.Localization");

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
