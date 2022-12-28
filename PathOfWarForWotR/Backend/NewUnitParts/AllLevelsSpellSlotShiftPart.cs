using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace TheInfiniteCrusade.Backend.NewUnitParts
{
    class AllLevelsSpellSlotShiftPart : OldStyleUnitPart
    {
        [JsonProperty]
        public Dictionary<BlueprintSpellbookReference, Shift> Changes = new();

        public class Shift
        {
            [JsonProperty]
            public int ShiftVal;
        }
    }
}
