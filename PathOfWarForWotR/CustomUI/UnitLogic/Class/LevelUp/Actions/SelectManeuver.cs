using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Extensions;

namespace TheInfiniteCrusade.CustomUI.UnitLogic.Class.LevelUp.Actions
{
    public class SelectManeuver : ILevelUpAction
    {
        [NotNull]
        [JsonProperty]
        public readonly BlueprintManeuverBook ManeuverBook;

        [NotNull]
        [JsonProperty]
        public readonly BlueprintSpellList SpellList;

       
        [JsonProperty]
        public readonly int SlotIndex;

        [NotNull]
        [JsonProperty]
        public readonly BlueprintAbility Spell;
        public LevelUpActionPriority Priority
        {
            get
            {
                return LevelUpActionPriority.Spells;
            }
        }

        public bool NeedUpdateUnitView
        {
            get
            {
                return false;
            }
        }

        [JsonConstructor]
        public SelectManeuver()
        {
        }
        public SelectManeuver([NotNull] BlueprintManeuverBook maneuverBook, [NotNull] BlueprintSpellList spellList, [NotNull] BlueprintAbility spell, int slotIndex)
        {
            this.ManeuverBook = maneuverBook;
            this.SpellList = spellList;
            this.Spell = spell;
            this.SlotIndex = slotIndex;
        }

        public void Apply([NotNull] LevelUpState state, [NotNull] UnitDescriptor unit)
        {
            state.DemandManeuverSelection(this.ManeuverBook, this.SpellList).SpendSlot( this.Spell, this.SlotIndex);
            unit.DemandManeuverBook(this.ManeuverBook).LearnManeuver(this.Spell.ToReference<BlueprintAbilityReference>());
        }

        public bool Check([NotNull] LevelUpState state, [NotNull] UnitDescriptor unit)
        {
            ManeuverSelectionData spellSelection = state.GetManeuverSelection(this.ManeuverBook, this.SpellList);
            return spellSelection != null && spellSelection.CanSpendSlot(this.SlotIndex);
        }

        public void PostLoad()
        {
            
        }
    }
}
