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
        private static Dictionary<UnitDescriptor, Dictionary<BlueprintManeuverBook, ManeuverBook>> AllManeuverBooks = new();

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
            return data.Descriptor.DemandManeuverBook(book);
        }

        public static ManeuverBook DemandManeuverBook(this UnitEntityData data, BlueprintManeuverBook book)
        {
            return data.Descriptor.DemandManeuverBook(book);
        }

        public static ManeuverBook GetManeuverBook(this UnitEntityData data, BlueprintManeuverBookReference book)
        {
            return data.Descriptor.GetManeuverBook(book);
        }

        public static ManeuverBook GetManeuverBook(this UnitEntityData data, BlueprintManeuverBook book)
        {
            return data.Descriptor.GetManeuverBook(book);
        }

        public static ManeuverBook GetManeuverBook(this UnitDescriptor data, [NotNull] BlueprintManeuverBook blueprintManeuverBook)
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

        

        public static ManeuverBook DemandManeuverBook(this UnitDescriptor data, BlueprintManeuverBook blueprintManeuverBook)
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

        public static IEnumerable<ManeuverBook> ManeuverBooks(this UnitEntityData data)
        {
            return data.Descriptor.ManeuverBooks();
        }

        public static IEnumerable<ManeuverBook> ManeuverBooks (this UnitDescriptor data)
        {
            if (AllManeuverBooks.TryGetValue(data, out var dict))
            {
                return dict.Values;
            }
            else
            {
                AllManeuverBooks.Add(data, new());
                return AllManeuverBooks[data].Values;
            }
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
