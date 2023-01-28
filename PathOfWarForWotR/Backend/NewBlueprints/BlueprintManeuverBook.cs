using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Localization;
using Kingmaker.UI;
using UnityEngine;

namespace PathOfWarForWotR.Backend.NewBlueprints
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

        

        public bool IsMartialTraining => BookType == ManeuverBookType.MartialTraining;

        public ManeuverBookType BookType;
        public BlueprintCharacterClassReference[] ClassReference = new BlueprintCharacterClassReference[] { };
        public BlueprintArchetypeReference[] ArchetypeReference = new BlueprintArchetypeReference[] { };
        public bool IsGranted = false;
        public BlueprintUnitPropertyReference m_ManeuverSlotsReference;
        public BlueprintUnitPropertyReference m_InitiatorLevelReference;

        /// <summary>
        /// Remember that anytime you check this you have to make sure it's not martial training, because martial training doesn't have one
        /// </summary>
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
