using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.Backend.NewComponents.Prerequisites
{
    class PrerequisiteManeuverSelectionDisciplineManeuversKnown : Prerequisite
    {
        private int needed = 0;
        private int has = 0;
        string displayName = "";
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state)
        {
            displayName = "BAD INPUT";
            if (selectionState.Selection is BlueprintFeatureSelection selection)
            {
                var selectionData = selection.GetComponent<ManeuverSelectorMenuComponent>();
                var abilityData = OwnerBlueprint.GetComponent<ManeuverSelectorPickComponent>()?.Maneuver;
                var classData = state.SelectedClass;
                var part = unit.Get<UnitPartMartialDisciple>();
                if (selectionData == null || abilityData == null || part == null)
                    return false;
                var abilityData2 = abilityData.Get().GetComponent<ManeuverInformation>();
                if (abilityData2 == null || abilityData2.DisciplineKeys.Length != 1)
                    return false;
                if (DisciplineTools.Disciplines.TryGetValue(abilityData2.DisciplineKeys[0], out var discipline))
                {
                    displayName = discipline.DisplayName;
                }
                
                needed = UnitPartMartialDisciple.ManeuverKnowledgeRequirementForLevel(abilityData2.ManeuverLevel);
                has = part.ManeuverKnowledgeForDiscipline(abilityData2.DisciplineKeys[0]);
                return needed <= has;
                
            }
            else
            {
                return false;
            }
        }

        public override string GetUITextInternal(UnitDescriptor unit)
        {
            return $"Requires {needed} maneuvers known from {displayName}: ({has})";
        }
    }
}
