using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Serialization;

namespace TheInfiniteCrusade.NewComponents.UnitParts.ManeuverBookSystem
{
    public class ManeuverSlot
    {
        public readonly int Index;

        public readonly SlotType SlotType;

        public SlotManeuverState State = SlotManeuverState.Readied;
        private BlueprintAbilityReference[] _layers = new BlueprintAbilityReference[3];
        public bool IllegallyEmpty => Combat == null || Readied == null;

        public bool Available => State == SlotManeuverState.Readied;
        public BlueprintAbilityReference[] Layers => _layers.ToArray();

        public BlueprintAbilityReference Combat => _layers[0];
        public BlueprintAbilityReference Readied => _layers[1];
        public BlueprintAbilityReference Planned => _layers[2];

        public ManeuverSlot(int index, SlotType slotType)
        {
            Index = index;
            SlotType = slotType;

        }

        public ManeuverSlot()
        {

        }

        public ManeuverSlot(SlotRecord slotRecord)
        {

            _layers[0] = TryLoad(slotRecord.CombatGuid);
            _layers[1] = TryLoad(slotRecord.ReadiedGuid);
            _layers[2] = TryLoad(slotRecord.PlannedGuid);
            State = (SlotManeuverState)slotRecord.State;
            SlotType = (SlotType)slotRecord.SlotType;
            Index = slotRecord.Index;

            


            
        }
        public void SetAsReadied(BlueprintAbilityReference ability)
        {
            Main.Context.Logger.Log($"Memorized {ability.NameSafe()}");
            _layers[1] = ability;
            _layers[0] = ability;
        }

        public void CombatSwap(BlueprintAbilityReference ability)
        {
            _layers[0] = ability;
            if (_layers[1] == null)
            {
                _layers[1] = ability;
            }
        }
        public void SetPlanned(BlueprintAbilityReference blueprintAbilityReference)
        {
            _layers[2] = blueprintAbilityReference;

        }

        private BlueprintAbilityReference TryLoad(string guid)
        {
            try
            {
                return BlueprintTool.GetRef<BlueprintAbilityReference>(guid);
            }
            catch
            {
                return null;
            }
        }

        

        public bool Grant()
        {
            if (State == SlotManeuverState.Withheld)
            {
                State = SlotManeuverState.Readied;
                return true;
            }
            return false;
        }

        public void DeployPLan()
        {
            _layers[0] = _layers[2];
            _layers[1] = _layers[2];
        }

        internal bool Expend()
        {
            if (State == SlotManeuverState.Readied)
            {
                State = SlotManeuverState.Exhausted;
                return true;
            }
            return false;
        }

        internal bool Recover()
        {
            if (State == SlotManeuverState.Exhausted)
            {
                State = SlotManeuverState.Readied;
                return true;
            }
            return false;
        }
    }
    public enum SlotType
    {
        Common = 1,
        Chosen = 2
    }


    public enum SlotManeuverState
    {
        Withheld = 0,
        Readied = 1,
        Exhausted = 2,
    }

    public enum SlotLayer
    {
        Combat = 0,
        Readied = 1,
        Planned = 2,
    }
}
