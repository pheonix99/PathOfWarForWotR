using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent
{
    class CommonBuffs
    {
        public static BlueprintBuffReference enforcedFlatfooted;

        public static void Build()
        {

            EnforcedFlatfoot();
            void EnforcedFlatfoot()
            {
                var buff = BuffTools.MakeBuff(Main.Context, "TICEnforcedFlatfootBuff", x =>
                {
                    x.SetNameDescription(Main.Context, "Forced Flatfooted", "This unit has been rended flatfooted by a martial strike");
                    x.m_Icon = ConstructionAssets.FlatfootedSprite();
                    x.AddComponent<ForceFlatFooted>();

                });
                enforcedFlatfooted = buff.ToReference<BlueprintBuffReference>();
            }


        }
    }
}
