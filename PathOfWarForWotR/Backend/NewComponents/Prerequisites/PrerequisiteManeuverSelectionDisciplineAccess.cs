using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System.Linq;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Backend.NewUnitParts;
using PathOfWarForWotR.Utilities;

namespace PathOfWarForWotR.Backend.NewComponents.Prerequisites
{
    public class PrerequisiteManeuverSelectionDisciplineAccess : Prerequisite
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
                if (abilityData2 == null || abilityData2.DisciplineKeys.Length != 1)
                    return false;
                switch (selectionData.SelectionMode)
                {
                    case ManeuverSelectionMode.Standard:
                       
                        if (classData.PrestigeClass)
                            return part.CanLearnDisciplineAsPrestigeClass(abilityData2.DisciplineKeys[0], classData.ToReference<BlueprintCharacterClassReference>());
                        else
                            return part.CanLearnDisciplineAsBaseClass(abilityData2.DisciplineKeys[0], classData.ToReference<BlueprintCharacterClassReference>());
                    case ManeuverSelectionMode.MartialTraining:
                        return part.CanLearnDisciplineWithNonClassBook(abilityData2.DisciplineKeys[0], selectionData.targetBook);

                        
                    case ManeuverSelectionMode.AdvancedStudy:
                        
                        return selectionData.targetBook.Get().ClassReference.Any(x=> part.CanLearnDisciplineAsFreeStudy(abilityData2.DisciplineKeys[0], selectionData.targetBook));

                       
                    case ManeuverSelectionMode.AdvancedStudySpecial:
                        return true;
                        
                    default:
                        return false;
                        
                }
                
                


            }
            else
            {
                return false;
            }


            
        }

        public override string GetUITextInternal(UnitDescriptor unit)
        {
            var abilityData = OwnerBlueprint.GetComponent<ManeuverSelectorPickComponent>()?.Maneuver;
            if (abilityData == null)
                return "BAD INPUT!";
            var abilityData2 = abilityData.Get().GetComponent<ManeuverInformation>();
            if (abilityData2 == null || abilityData2.DisciplineKeys.Length != 1)
                return "BAD INPUT!";
            else
            {
                if (DisciplineTools.Disciplines.TryGetValue(abilityData2.DisciplineKeys[0], out var discipline))
                {
                    return $"Access to {discipline.DisplayName}";
                }
                else
                {
                    return "BAD INPUT!";
                }
            }
                

          
        }
    }
}
