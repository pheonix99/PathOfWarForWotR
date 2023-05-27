using HarmonyLib;
using Kingmaker.Controllers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using TabletopTweaks.Core.Utilities;
using PathOfWarForWotR.Backend.NewEvents;
using PathOfWarForWotR.Extensions;
using PathOfWarForWotR.NewContent;

namespace PathOfWarForWotR.Backend.NewControllers
{
    class MartialDiscipleController : IController, IUnitNewCombatRoundHandler, IUnitCombatHandler, IUnitRestHandler
    {
        public void Activate()
        {

        }

        public void Deactivate()
        {

        }

        public void HandleNewCombatRound(UnitEntityData unit)
        {
            if (unit.HasFact(CommonBuffs.combatMode))
            {
                unit.RemoveFact(CommonBuffs.combatMode);
            }
            else
            {
                

                EventBus.RaiseEvent<ICombatStartedWhileCooledDownHandler>(unit, h => h.OnCombatStartWhileCooledDown());
            }


        }

        public void HandleUnitJoinCombat(UnitEntityData unit)
        {
            
        }

        public void HandleUnitLeaveCombat(UnitEntityData unit)
        {

            unit.AddBuff(CommonBuffs.combatMode, unit, new Rounds(10).Seconds);
            
        }

        public void HandleUnitRest(UnitEntityData unit)
        {
            foreach(var book in unit.ManeuverBooks())
            {
                book.Rest();
            }

            
        }

        public void Tick()
        {

        }


        [HarmonyPatch(typeof(GameModesFactory), "Initialize")]
        static class InsertController
        {
            static bool Initialized;

            [HarmonyPriority(Priority.Last)]
            static void Postfix()
            {
                GameModesFactory.Register(new MartialDiscipleController(), new GameModeType[]
                {
                GameModesFactory.Default,
                GameModesFactory.Dialog,
                GameModesFactory.Pause,
                GameModesFactory.Cutscene,
                GameModesFactory.Rest
                });
            }
        }

    }
}
