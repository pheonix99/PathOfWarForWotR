using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewUnitParts;

namespace TheInfiniteCrusade.Backend.NewComponents
{
    [AllowedOn(typeof(BlueprintCharacterClass))]
    class InitiatorClassComponent : UnitFactComponentDelegate, IGlobalSubscriber, ISubscriber
    {
        public BlueprintManeuverBook ManeuverBook => m_ManeuverBook.Get();
        public BlueprintManeuverBookReference m_ManeuverBook;
        public override void OnActivate()
        {
            OnTurnOn();
        }

        public override void OnDeactivate()
        {
            OnTurnOff();
        }

        public override void OnTurnOn()
        {

            base.OnTurnOn();
            var part = base.Owner.Ensure<UnitPartMartialDisciple>();
            part.RegisterClassManueverBook(OwnerBlueprint as BlueprintCharacterClass, m_ManeuverBook);
          

        }

        public override void OnTurnOff()
        {
            var part = base.Owner.Get<UnitPartMartialDisciple>();
            if (part != null)
                part.UnregisterClassManueverBook(OwnerBlueprint as BlueprintCharacterClass, m_ManeuverBook);

        }
    }

    [AllowedOn(typeof(BlueprintArchetype))]
    class InitiatorArchetypeComponent : UnitFactComponentDelegate, IGlobalSubscriber, ISubscriber
    {
        public BlueprintManeuverBook ManeuverBook => m_ManeuverBook.Get();
        public BlueprintManeuverBookReference m_ManeuverBook;
        public override void OnActivate()
        {
            OnTurnOn();
        }

        public override void OnDeactivate()
        {
            OnTurnOff();
        }

        public override void OnTurnOn()
        {

            base.OnTurnOn();
            var part = base.Owner.Ensure<UnitPartMartialDisciple>();
            var ownerArche = OwnerBlueprint as BlueprintArchetype;
            part.RegisterClassManueverBook(ownerArche.m_ParentClass, m_ManeuverBook);

        }

        public override void OnTurnOff()
        {
            var part = base.Owner.Get<UnitPartMartialDisciple>();
            var ownerArche = OwnerBlueprint as BlueprintArchetype;
            if (part != null)
                part.UnregisterClassManueverBook(ownerArche.m_ParentClass as BlueprintCharacterClass, m_ManeuverBook);

        }
    }
}
