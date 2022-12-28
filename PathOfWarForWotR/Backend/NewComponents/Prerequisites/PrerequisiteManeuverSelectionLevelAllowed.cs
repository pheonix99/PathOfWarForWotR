using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using TheInfiniteCrusade.Backend.NewComponents.ManeuverBookSystem;
using TheInfiniteCrusade.Backend.NewUnitParts;

namespace TheInfiniteCrusade.Backend.NewComponents.Prerequisites
{
    class PrerequisiteManeuverSelectionLevelAllowed : Prerequisite
    {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state)
        {
            if (selectionState.Selection is BlueprintFeatureSelection selection)
            {
                var selectionData = selection.GetComponent<ManeuverSelectorMenuComponent>();
                var abilityData = OwnerBlueprint.GetComponent<ManeuverSelectorPickComponent>()?.Maneuver;
                var classData = state.SelectedClass;
                var part = unit.Get<UnitPartMartialDisciple>();
                if (selectionData == null || abilityData == null || part == null)
                    return false;
                var abilityData2 = abilityData.Get().GetComponent<ManeuverInformation>();
                if (abilityData2 == null)
                    return false;
                LevelAccessNeeded = abilityData2.ManeuverLevel;
                if (selectionData.SelectionMode == (ManeuverSelectionMode.AdvancedStudy | ManeuverSelectionMode.AdvancedStudySpecial))
                    return true;

                else if (selectionData.SelectionMode == ManeuverSelectionMode.MartialTraining)
                {
                    var part2 = unit.Get<UnitPartMartialTraining>();
                    if (part2 != null)
                    {
                        return abilityData2.ManeuverLevel <= part2.GetMaxLevel();
                    }
                    else
                        return false;
                }
                else
                {
                    var book = selectionData.targetBook;
                    if (classData.PrestigeClass)
                    {
                        return true;
                    }
                    else if (book.Get().BookType == NewBlueprints.BlueprintManeuverBook.ManeuverBookType.Level9Class)
                        return true;
                    else
                    {
                        return abilityData2.ManeuverLevel <= part.ArchetypeAllowedLevel(book.Get().GrantingProgression);
                    }
                }
                    

            }
            else
                return false;
        }

        private int LevelAccessNeeded = 0;

        public override string GetUITextInternal(UnitDescriptor unit)
        {
            return $"Can select level {LevelAccessNeeded} maneuvers";
        }
    }
}
