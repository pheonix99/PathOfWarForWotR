using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PathOfWarForWotR.Backend.NewUnitParts
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
