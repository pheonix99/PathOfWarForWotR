using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.NewComponents.ActionTriggers
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    class ActionOnCrit : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public ConditionsChecker attackerConditions;
        public ConditionsChecker targetConditions;
        public bool RequiresWorthy;
        public bool RequiresFocusedWeapon;

        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {

        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.AttackRoll.IsCriticalConfirmed)
            {
                if (RequiresWorthy)
                {
                    if (evt.Target.State.IsHelpless || evt.Target.State.IsUnconscious)
                        return;
                    if ((evt.Target.Descriptor.Progression.CharacterLevel / 2) < evt.Initiator.Descriptor.Progression.CharacterLevel)
                        return;
                }
                if (RequiresFocusedWeapon)
                {
                    var type = evt.Weapon.Blueprint.Type;
                    if (!evt.AttackRoll.WeaponStats.m_TemporaryModifiers.Any(x => x.Source.Blueprint.Components.OfType<WeaponFocus>().Any()))
                        //if (!evt.AttackRoll.WeaponStats.m_TemporaryModifiers.Any(x => x.Source.Blueprint.Components.OfType<WeaponFocus>().Any()) && !evt.MeleeDamage.m_TemporaryModifiers.Any(x => x.Source.Blueprint.Components.OfType<DisciplineFocusBaseComponent>().Any()))
                        return;
                }
                if (attackerConditions != null)
                {
                    using (base.Context.GetDataScope(evt.Initiator))
                    {
                        if (!this.attackerConditions.Check())
                            return;
                    }
                }
                if (targetConditions != null)
                {
                    using (base.Context.GetDataScope(evt.Target))
                    {
                        if (!this.targetConditions.Check())
                            return;

                    }
                }
                MechanicsContext context = base.Context;
                EntityFact fact = base.Fact;
                UnitEntityData unit = ActionsOnInitiator ? evt.Initiator : evt.Target;
                using (ContextData<ContextAttackData>.Request().Setup(evt.AttackRoll, null))
                {
                    if (!fact.IsDisposed)
                    {
                        fact.RunActionInContext(Action, unit);
                    }
                    else
                    {
                        using (context.GetDataScope(unit))
                        {
                            Action.Run();
                        }
                    }
                }
            }
        }



        public ActionList Action;

        public bool ActionsOnInitiator;
    }
}
