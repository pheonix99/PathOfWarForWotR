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
using TheInfiniteCrusade.Extensions;
using TheInfiniteCrusade.NewComponents;
using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
using TheInfiniteCrusade.NewComponents.UnitParts;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.Backend.NewComponents.Prerequisites
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
                    case TheInfiniteCrusade.NewComponents.ManeuverBookSystem.ManeuverSelectionMode.Standard:
                       
                        return part.DisciplineIsValidForClass(abilityData2.DisciplineKeys[0], classData, false);
                        
                    case TheInfiniteCrusade.NewComponents.ManeuverBookSystem.ManeuverSelectionMode.MartialTraining:
                        var part2 = unit.Get<UnitPartMartialTraining>();
                        if (part2 == null)
                            return false;
                        return part2.IsThisDiscipline(abilityData2.DisciplineKeys[0]);

                        
                    case TheInfiniteCrusade.NewComponents.ManeuverBookSystem.ManeuverSelectionMode.AdvancedStudy:
                        
                        return selectionData.targetBook.Get().ClassReference.Any(x=> part.DisciplineIsValidForClass(abilityData2.DisciplineKeys[0], x, true));

                       
                    case TheInfiniteCrusade.NewComponents.ManeuverBookSystem.ManeuverSelectionMode.AdvancedStudySpecial:
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
