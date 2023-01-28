using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using PathOfWarForWotR.Backend.NewComponents.ManeuverBookSystem;
using PathOfWarForWotR.Backend.NewUnitParts;

namespace PathOfWarForWotR.Backend.NewComponents.Prerequisites
{
    class PrerequisiteManeuverSelectionInitiatorLevelAccess : Prerequisite
    {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state)
        {
            
            if (selectionState.Selection is BlueprintFeatureSelection selection)//This is the one!
            {
                var selectionData = selection.GetComponent<ManeuverSelectorMenuComponent>();
                var abilityData = OwnerBlueprint.GetComponent<ManeuverSelectorPickComponent>()?.Maneuver;
                var classData = state.SelectedClass;
                var part = unit.Get<UnitPartMartialDisciple>();
                if (selectionData == null)
                {
#if DEBUG
                    Main.Context.Logger.Log($"Selection Data is null on PrerequisiteManeuverInitiatorLevelAccess");
#endif
                    return false;
                }
                if (part == null)
                {
#if DEBUG
                    Main.Context.Logger.Log($"part is null on PrerequisiteManeuverInitiatorLevelAccess");
#endif
                    return false;
                }
                if (abilityData == null)
                {
#if DEBUG
                    Main.Context.Logger.Log($"abilityData is null on PrerequisiteManeuverInitiatorLevelAccess");
#endif
                    return false;
                }
                
                var abilityData2 = abilityData.Get().GetComponent<ManeuverInformation>();
                if (abilityData2 == null || abilityData2.DisciplineKeys.Length != 1)
                {
#if DEBUG
                    Main.Context.Logger.Log($"abilityData2 is null on PrerequisiteManeuverInitiatorLevelAccess");
#endif
                    return false;
                }
                int moveLevel = abilityData2.ManeuverLevel;
                lastBook = selectionData.targetBook.Get().DisplayName;
                var level = selectionData.targetBook.Get().m_InitiatorLevelReference.Get().GetInt(unit);
                LastLevel = level;
                return level >= (1 + (moveLevel - 1) * 2);
               
            }
            return false;
        }

        private string lastBook = "N/a";
        private int LastLevel = 0;

        public override string GetUITextInternal(UnitDescriptor unit)
        {
           
            var abilityData = OwnerBlueprint.GetComponent<ManeuverSelectorPickComponent>()?.Maneuver?.Get().GetComponent<ManeuverInformation>(); ;
           
            

            if (abilityData == null)
            {
                return "BAD INPUT";
            }
            else
            {

                return $"Requires {lastBook} Initator Level {(1 + (abilityData.ManeuverLevel - 1) * 2)} ({LastLevel})";
            }
        }
    }
}
