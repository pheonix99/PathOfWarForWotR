using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace PathOfWarForWotR.NewContent.MartialSystem
{
    class SystemLists
    {
        public static void BuildMasterSpellLists()
        {
            BuildManeuverList();
            BuildStanceList();

            void BuildManeuverList()
            {
                var bp = Helpers.CreateBlueprint<BlueprintSpellList>(Main.Context, "MasterManeuverList", x =>
                {


                });

                var config = SpellListConfigurator.For(bp);
                for (int i = 0; i < 11; i++)
                {
                    config.AddToSpellsByLevel(new SpellLevelList(i));
                }
                config.Configure();
            }
            void BuildStanceList()
            {
                var bp2 = Helpers.CreateBlueprint<BlueprintSpellList>(Main.Context, "MasterStanceList", x =>
                {


                });

                var config2 = SpellListConfigurator.For(bp2);
                for (int i = 0; i < 11; i++)
                {
                    config2.AddToSpellsByLevel(new SpellLevelList(i));
                }
                config2.Configure();
            }

        }

    }
}
