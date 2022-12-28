using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using Kingmaker.Blueprints.Classes.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TabletopTweaks.Core.Utilities;

namespace TheInfiniteCrusade.NewContent.MartialSystem
{
    class SystemLists
    {
        public static void BuildSystemSpellTables()
        {
            var bp = Helpers.CreateBlueprint<BlueprintSpellsTable>(Main.Context, "MartialManeuversTable", x =>
            {
               

            });
           var config = SpellsTableConfigurator.For(bp);
            List<SpellsLevelEntry> spellsLevelEntries = new();

            for (int i = 1; i < 51; i++)
            {
                List<int> levels = new List<int>();
                int maxLevel = Math.Min(9, (i / 2) + 1);
                for (int j = 0; j <= maxLevel; j++)
                {
                    if (j == 1)
                    {
                        levels.Add(1);
                    }
                    else
                    {
                        levels.Add(0);
                    }
                }
                StringBuilder levelTable = new();
                levelTable.Append($"Full Progression Martial Table for level {i}:");
                for (int k = 0; k < levels.Count(); k++)
                {
                    levelTable.Append($" " + levels[k] + " ");

                }

                var level = new SpellsLevelEntry()
                {
                    Count = levels.ToArray()

                };
                spellsLevelEntries.Add(level);

            }
            config.SetLevels(spellsLevelEntries.ToArray());
            config.Configure();
        }
        

        public static void BuildSystemSpellLists()
        {
            BuildMasterManeuverList();
            BuildMasterStanceList();
            BuildDummySpellList();

            void BuildMasterManeuverList()
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
            void BuildMasterStanceList()
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
            void BuildDummySpellList()
            {
                var bp = Helpers.CreateBlueprint<BlueprintSpellList>(Main.Context, "DummyManeuverList", x =>
                {


                });

                var config = SpellListConfigurator.For(bp);
                for (int i = 0; i < 11; i++)
                {
                    config.AddToSpellsByLevel(new SpellLevelList(i));
                }
                config.Configure();
            }

        }

    }
}
