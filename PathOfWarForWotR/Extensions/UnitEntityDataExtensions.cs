using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using System.Collections.Generic;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewUnitDataClasses;
using System.Linq;

namespace PathOfWarForWotR.Extensions
{
    public static class UnitEntityDataExtensions
    {
        private static Dictionary<UnitEntityData, Dictionary<BlueprintManeuverBook, ManeuverBook>> AllManeuverBooks = new();

        public static bool HasMartialStuff(this UnitEntityData unit)
        {
            if (AllManeuverBooks.TryGetValue(unit, out var books))
            {
                if (books != null)
                    return books.Any();
            }
            return false;
        }

        public static int CurrentHP(this UnitEntityData unit)
        {
            return unit.Stats.HitPoints.ModifiedValue - unit.Damage;
        }
        public static ManeuverBook DemandManeuverBook(this UnitEntityData data, BlueprintManeuverBookReference book)
        {
            return data.DemandManeuverBook(book.Get());
        }

        public static ManeuverBook DemandManeuverBook(this UnitEntityData data, BlueprintManeuverBook blueprintManeuverBook)
        {
            if (AllManeuverBooks.TryGetValue(data, out var keyValuePairs))
            {
                if (keyValuePairs.TryGetValue(blueprintManeuverBook, out var book))
                {

                }
                else
                {
                    Main.Context.Logger.Log($"Added {blueprintManeuverBook.name} to {data.CharacterName} via Demand");
                    book = new ManeuverBook(data, blueprintManeuverBook);
                    keyValuePairs.Add(blueprintManeuverBook, book);
                }
                return book;
            }
            else
            {
                AllManeuverBooks.Add(data, new());
                Main.Context.Logger.Log($"Added {blueprintManeuverBook.name} to {data.CharacterName} via Demand");
                var book = new ManeuverBook(data, blueprintManeuverBook);
                AllManeuverBooks[data].Add(blueprintManeuverBook, book);

                return book;
            }
        }

        public static ManeuverBook GetManeuverBook(this UnitEntityData data, BlueprintManeuverBookReference book)
        {
            return data.GetManeuverBook(book.Get());
        }

        public static ManeuverBook GetManeuverBook(this UnitEntityData data, BlueprintManeuverBook blueprintManeuverBook)
        {
            if (AllManeuverBooks.TryGetValue(data, out var keyValuePairs))
            {
                if (keyValuePairs.TryGetValue(blueprintManeuverBook, out var book))
                {
                    return book;
                }

            }

            return null;
        }

        public static ManeuverBook GetManeuverBook(this UnitDescriptor data, [NotNull] BlueprintManeuverBook blueprintManeuverBook)
        {
            return data.Unit.GetManeuverBook(blueprintManeuverBook);
        }

        

        public static ManeuverBook DemandManeuverBook(this UnitDescriptor data, BlueprintManeuverBook blueprintManeuverBook)
        {
            return data.Unit.DemandManeuverBook(blueprintManeuverBook);

        }

        public static IEnumerable<ManeuverBook> ManeuverBooks(this UnitEntityData data)
        {
        
            //Main.Context.Logger.Log($"Calling ManueverBooks on {data.CharacterName}");
            if (AllManeuverBooks.TryGetValue(data, out var dict))
            {
                return dict.Values;
            }
            else
            {
               // Main.Context.Logger.Log($"Entry For ManueverBooks not found on {data.CharacterName}, adding");
                AllManeuverBooks.Add(data, new());
                return AllManeuverBooks[data].Values;
            }
          
        }

        public static IEnumerable<ManeuverBook> ManeuverBooks (this UnitDescriptor data)
        {
            return data.Unit.ManeuverBooks();
        }

        [HarmonyPatch(typeof(UnitDescriptor), nameof(UnitDescriptor.Dispose))]
      
        static class UnitDescriptor_DisposeManeuverBooks
        {
            static void Postfix(UnitDescriptor __instance)
            {

                if (AllManeuverBooks.TryGetValue(__instance, out var books))
                {
                    foreach(var book in books.Values)
                    {
                        book.Dispose();
                    }
                    AllManeuverBooks.Remove(__instance);
                }

            }
        }
    }
}
