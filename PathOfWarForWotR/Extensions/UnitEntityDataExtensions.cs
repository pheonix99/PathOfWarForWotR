using HarmonyLib;
using JetBrains.Annotations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewUnitDataClasses;

namespace TheInfiniteCrusade.Extensions
{
    public static class UnitEntityDataExtensions
    {
        private static Dictionary<UnitDescriptor, Dictionary<BlueprintManeuverBook, ManeuverBook>> AllManeuverBooks = new();

        public static ManeuverBook DemandManeuverBook(this UnitEntityData data, BlueprintManeuverBook book)
        {
            return data.Descriptor.DemandManeuverBook(book);
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
                    book = new ManeuverBook(data, blueprintManeuverBook);
                    keyValuePairs.Add(blueprintManeuverBook, book);
                }
                return book;
            }
            else
            {
                AllManeuverBooks.Add(data, new());
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
