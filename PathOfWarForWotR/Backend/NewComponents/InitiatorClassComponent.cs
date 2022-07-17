using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Backend.NewBlueprints;

namespace TheInfiniteCrusade.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintCharacterClass))]
    class InitiatorClassComponent : BlueprintComponent
    {
        public BlueprintManeuverBook ManeuverBook => m_ManeuverBook.Get();
        public BlueprintManeuverBookReference m_ManeuverBook;
    }

    [AllowedOn(typeof(BlueprintArchetype))]
    class InitiatorArchetypeComponent : BlueprintComponent
    {
        public BlueprintManeuverBook ManeuverBook => m_ManeuverBook.Get();
        public BlueprintManeuverBookReference m_ManeuverBook;
    }
}
