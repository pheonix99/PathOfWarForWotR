using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheInfiniteCrusade.Backend.NewBlueprints
{
    public class BlueprintManeuverBook : BlueprintScriptableObject, IUIDataProvider
    {
        public string DisplayName
        {
            get
            {
                return this.Name;
            }
        }


        string IUIDataProvider.Name
        {
            get
            {
                return this.Name;
            }
        }
        string IUIDataProvider.Description
        {
            get
            {
                return this.Name;
            }
        }
        Sprite IUIDataProvider.Icon
        {
            get
            {
                return null;
            }
        }

        // Token: 0x17001799 RID: 6041
        // (get) Token: 0x06009AB8 RID: 39608 RVA: 0x002670ED File Offset: 0x002652ED
        string IUIDataProvider.NameForAcronym
        {
            get
            {
                return this.name;
            }
        }

        public LocalizedString Name;

        public int BaseManeuversReadied = 3;
        public int[] SlotsGainedAtLevels = new int[0];
        


        public int BaseManeuversKnown = 3;
        public int[] ManeuversLearnedAtLevels = new int[0];
        public int[] StancesLearnedAtLevels = new int[0];

        public bool IsMartialTraining => BookType == ManeuverBookType.MartialTraining;

        public ManeuverBookType BookType;
        public BlueprintCharacterClassReference[] ClassReference = new BlueprintCharacterClassReference[] { };
        public BlueprintArchetypeReference[] ArchetypeReference = new BlueprintArchetypeReference[] { };
        public bool IsGranted = false;
        public BlueprintUnitPropertyReference m_ManeuverSlotsReference;
        public BlueprintUnitPropertyReference m_InitiatorLevelReference;
        public BlueprintProgressionReference GrantingProgression;


        public StatType DefaultMainStat { get; set; }

        public enum ManeuverBookType
        {
            Level9Class,
            Level6Archetype,
            MartialTraining
        }

    }
}
