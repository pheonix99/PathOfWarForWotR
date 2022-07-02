using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.NewComponents.ManeuverBookSystem
{
    [AllowedOn(typeof(BlueprintSpellbook), true)]
    public class ManeuverBookComponent : BlueprintComponent
    {
        public ManeuverBookType BookType;
        public BlueprintCharacterClassReference[] ClassReference = new BlueprintCharacterClassReference[] { };
        public BlueprintArchetypeReference[] ArchetypeReference = new BlueprintArchetypeReference[] { };
        public bool IsGranted = false;
        public BlueprintUnitPropertyReference m_ManeuverSlotsReference;
        public BlueprintProgressionReference GrantingProgression;
        public BlueprintFeatureReference GrantingFeature;

        public enum ManeuverBookType
        {
            Level9Class,
            Level6Archetype,
            MartialTraining
        }


    }
}
