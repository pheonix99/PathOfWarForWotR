using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System.Collections.Generic;
using TabletopTweaks.Core.ModLogic;
using TheInfiniteCrusade.Backend.NewBlueprints;
using UnityEngine;

namespace TheInfiniteCrusade.Defines
{
    public class InitiatorProgressionDefine
    {
        #region defines
        public readonly string InitiatorSysNameBase;
        private readonly string _displayName;
        public readonly ModContextBase Source;
        public readonly bool GrantedType;
        public readonly BlueprintManeuverBook.ManeuverBookType maneuverBookType = BlueprintManeuverBook.ManeuverBookType.Level9Class;

        public string DisplayName => string.IsNullOrEmpty(_displayName) ? InitiatorSysNameBase : _displayName;

      

        public int ManeuversKnownAtLevel1;
        public int ManeuverSlotsAtLevel1;
        public int ChosenManeuvers;
        public int ManeuversGrantedAtLevel1;
        public int[] StancesLearnedAtLevels = new int[0];
        public int[] ManeuversLearnedAtLevels = new int[0];
        public int[] NormalSlotsIncreaseAtLevels = new int[0];




        public StatType DefaultInitiatingStat = StatType.Wisdom;
        public string[] FixedUnlocks = new string[0];//All members of the class get this
        public string[] SelectionUnlocks = new string[0];//Selector picks
        public int SelectionCount = 0;//Selection count
        public string[] ProgressionSpecificSubstitutions = new string[0];//Anything for x substitutions limited to this class only
        
        
        public List<BlueprintCharacterClassReference> ClassesForClassTemplate = new();
        public List<BlueprintArchetypeReference> ArchetypesForArchetypeTemplate = new();
        public bool ManuallyBuildSelectors;
        public bool DisallowExchanges;
        public ClassRecoveryDefine SwiftRecovery { get; private set; }
        public ClassRecoveryDefine StandardRecovery { get; private set; }
        public ClassRecoveryDefine FullRoundRecovery { get; private set; }

        public bool IsTemplateArchetype;


        #endregion
        #region builtComponents
        public BlueprintManeuverBookReference m_ManueverBook;
        public BlueprintFeatureSelectionReference m_exchanger;
        public BlueprintFeatureSelectionReference m_disciplineSelector;
        public BlueprintProgressionReference m_Progression;

        
        internal BlueprintFeatureSelectionReference maneuverSelector;
        internal BlueprintFeatureSelectionReference stanceSelector;

        public BlueprintFeatureReference AddSlotComponent { get; internal set; }
        public BlueprintUnitPropertyReference SlotsProperty { get; internal set; }
        public BlueprintFeatureReference ManeuverSlotsAtLevel1Feature { get; internal set; }

        #endregion

        public InitiatorProgressionDefine(ModContextBase source, string initiatorSysNameBase, string displayName = null,  bool grantedType = false, BlueprintManeuverBook.ManeuverBookType maneuverBookType = BlueprintManeuverBook.ManeuverBookType.Level9Class)
        {
            InitiatorSysNameBase = initiatorSysNameBase;
            _displayName = displayName;
            Source = source;
            GrantedType = grantedType;
            this.maneuverBookType = maneuverBookType;
        }

        public void MakeStandardRecovery(string name, string desc, Sprite sprite = null)
        {
            StandardRecovery = new ClassRecoveryDefine(RecoveryAction.Standard, name, desc, sprite);

        }
        public void MakeSwiftRecovery(string name, string desc, Sprite sprite = null)
        {
            SwiftRecovery = new ClassRecoveryDefine(RecoveryAction.Swift, name, desc, sprite);

        }
        public void MakeFullRecovery(string name, string desc, Sprite sprite = null)
        {
            FullRoundRecovery = new ClassRecoveryDefine(RecoveryAction.FullRound, name, desc, sprite);

        }
        public void LoadDefaultArchetypeProgression()
        {
            if (GrantedType)
            {
                
                ManeuversGrantedAtLevel1 = 0;
            
                ChosenManeuvers = 1;
            }
           
            ManeuverSlotsAtLevel1 = 3;
            ManeuversKnownAtLevel1 = 3;

            StancesLearnedAtLevels = new int[] { 4, 7, 11, 13 };
            NormalSlotsIncreaseAtLevels = new int[] { 4, 10, 15, 20 };
            ManeuversLearnedAtLevels = new int[] { 2, 3, 4, 7, 9, 11, 13, 14, 16, 18, 20 };
           
        }

        public class ClassRecoveryDefine
        {
            public readonly RecoveryAction recoveryAction;
            public Sprite Sprite;
            public string Name;
            public string Desc;
            public readonly bool AddToProgression;
            public readonly bool NeedsBuff;
            public AbilityType AbilityType;

            public BlueprintFeatureReference m_RestoreFeature;
            public BlueprintAbilityReference m_RestoreAction;
            public BlueprintBuffReference m_RestoreBuff;
            public ClassRecoveryDefine(RecoveryAction r, string name, string desc, bool? manuallySetBuff = null, AbilityType type = AbilityType.Extraordinary, Sprite sprite = null)
            {
                recoveryAction = r;
                Name = name;
                Desc = desc;
                Sprite = sprite;
                AbilityType = type;
                if (recoveryAction == RecoveryAction.Swift)
                {
                    NeedsBuff = true;//COOLDOWN IS MANDATORY
                }
                if (manuallySetBuff.HasValue)
                    NeedsBuff = manuallySetBuff.Value;
                else if (recoveryAction == RecoveryAction.FullRound)
                {
                    NeedsBuff = true;
                }
                else
                {
                    NeedsBuff = false;
                }
            }

        }
        public enum RecoveryAction
        {
            Swift,
            Standard,
            FullRound
                
        }
        
      

    }

    
}
