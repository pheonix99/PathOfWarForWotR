using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System.Linq;
using PathOfWarForWotR.Backend.NewBlueprints;
using PathOfWarForWotR.Backend.NewComponents;

namespace PathOfWarForWotR.Extensions
{
    public static class ClassDataExtensions
    {

        public static BlueprintManeuverBook ManeuverBook(this ClassData data)
        {
            if (data.CharacterClass.GetComponent<InitiatorClassComponent>() != null)
            {
                return data.CharacterClass.GetComponent<InitiatorClassComponent>().ManeuverBook;
            }
            else if (data.Archetypes.Any(x => x.GetComponent<InitiatorArchetypeComponent>() != null))
            {
                return data.Archetypes.FirstOrDefault(x => x.GetComponent<InitiatorArchetypeComponent>() != null).GetComponent<InitiatorArchetypeComponent>().ManeuverBook;
            }
            else
                return null;
        }
    }
}
