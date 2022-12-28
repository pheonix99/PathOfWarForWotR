using HarmonyLib;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Persistence;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewUnitParts;


namespace TheInfiniteCrusade.Serialization
{
    class ManeuverBookStorage
    {
        [JsonProperty]
        public Dictionary<string, MartialDiscipleRecord> PerCharacter = new();
        public MartialDiscipleRecord ForCharacter(UnitEntityData unit)
        {
            var key = unit.UniqueId;
            if (!PerCharacter.TryGetValue(key, out var record))
            {
                record = new();
                PerCharacter[key] = record;
            }
            return record;
        }

        public static ManeuverBookStorage Instance = new();
    }

    public class MartialDiscipleRecord
    {
        /// <summary>
        /// Dictionary of maneuver guid -> manuever data pairs, key is spellbook guid
        /// </summary>
        [JsonProperty]
        public Dictionary<string, ManeuverBookRecord> ManeuverBooks = new();

        //public List<ManeuverBookRecord> ManeuverBooks = new();






        public ManeuverBookRecord ForSpellbook(BlueprintManeuverBook book)
        {
            var key = book.AssetGuidThreadSafe;

            if (!ManeuverBooks.TryGetValue(key, out var record))
            {
                record = new();
                ManeuverBooks[key] = record;
            }
            return record;
        }


        [HarmonyPatch]
        static class SaveHooker
        {

            [HarmonyPatch(typeof(ZipSaver))]
            [HarmonyPatch("SaveJson"), HarmonyPostfix]
            static void Zip_Saver(string name, ZipSaver __instance)
            {
                DoSave(name, __instance);
            }

            [HarmonyPatch(typeof(FolderSaver))]
            [HarmonyPatch("SaveJson"), HarmonyPostfix]
            static void Folder_Saver(string name, FolderSaver __instance)
            {
                DoSave(name, __instance);
            }

            static void DoSave(string name, ISaver saver)
            {
                foreach (var character in Game.Instance.Player.GetCharactersList(Player.CharactersList.Everyone))
                {
                    var part = character.Get<UnitPartMartialDisciple>();
                    if (part != null)
                    {
                        part.OnPreSave();
                    }
                }

                if (name != "header")
                    return;
                Main.Context.Logger.Log($"Saving Maneuver Books! - from ManeuverBookStorage");
                Main.Safely(() =>
                {
                    var serializer = new JsonSerializer();
                    serializer.Formatting = Formatting.Indented;
                    var writer = new StringWriter();
                    serializer.Serialize(writer, ManeuverBookStorage.Instance);
                    writer.Flush();
                    saver.SaveJson(LoadHooker.FileName, writer.ToString());
                });
            }
        }

        [HarmonyPatch(typeof(Game))]
        static class LoadHooker
        {
            public const string FileName = "header.json.PotCDiscipleRecords";

            [HarmonyPatch("LoadGame"), HarmonyPostfix]
            static void LoadGame(SaveInfo saveInfo)
            {
                Main.Context.Logger.Log($"Loading DisciplineSaveInfo - from ManeuverBookStorage");
                using (saveInfo)
                {
                    using (saveInfo.GetReadScope())
                    {
                        ThreadedGameLoader.RunSafelyInEditor((Action)(() =>
                        {
                            string raw;
                            using (ISaver saver = saveInfo.Saver.Clone())
                            {
                                raw = saver.ReadJson(FileName);
                            }
                            if (raw != null)
                            {
                                var serializer = new JsonSerializer();
                                var rawReader = new StringReader(raw);
                                var jsonReader = new JsonTextReader(rawReader);
                                ManeuverBookStorage.Instance = serializer.Deserialize<ManeuverBookStorage>(jsonReader);
                            }
                            else
                            {
                                ManeuverBookStorage.Instance = new ManeuverBookStorage();
                            }
                        })).Wait();
                    }
                }
            }
        }


    }

    public class ManeuverBookRecord
    {
        [JsonProperty]
        public List<SlotRecord> SlotRecords = new();

        [JsonProperty]
        public List<string> ManeuverGuids = new();


       
    }

    public class SlotRecord
    {
        [JsonProperty]
        public int State;

        [JsonProperty]
        public int SlotType;

        [JsonProperty]
        public int Index;

        [JsonProperty]
        public string CombatGuid;
        [JsonProperty]
        public string ReadiedGuid;
        [JsonProperty]
        public string PlannedGuid;


        [JsonConstructor]
        public SlotRecord()
        {

        }

        public SlotRecord(ManeuverSlot slot)
        {
            CombatGuid = slot.Layers[0]?.deserializedGuid.ToString() ?? string.Empty;
            ReadiedGuid = slot.Layers[1]?.deserializedGuid.ToString() ?? string.Empty;
            PlannedGuid = slot.Layers[2]?.deserializedGuid.ToString() ?? string.Empty;
            State = (int)slot.State;
            Index = slot.Index;
            SlotType = (int)slot.SlotType;

        }





    }
}
