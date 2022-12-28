using Kingmaker.Blueprints;
using TabletopTweaks.Core.NewComponents;
using TheInfiniteCrusade.Backend.NewComponents;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent
{
    class CommonBuffs
    {
        public static BlueprintBuffReference enforcedFlatfooted;

        public static void Build()
        {

            EnforcedFlatfoot();
            void EnforcedFlatfoot()
            {
                var buff = BuffTools.MakeBuff(Main.Context, "TICEnforcedFlatfootBuff", "Forced Flatfooted", "This unit has been rended flatfooted by a martial strike", ConstructionAssets.FlatfootedSprite());
                buff.AddComponent<ForceFlatFooted>();
                var buffmade = buff.Configure();
                enforcedFlatfooted = buffmade.ToReference<BlueprintBuffReference>();
            }
            BuildExtendedCombatTime();
            void BuildExtendedCombatTime()
            {
                var buff = BuffTools.MakeBuff(Main.Context, "InCombatModeSystemBuff", "The Thrill Of Battle", "You Shouldn't See This");
                buff.SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi);

                
                buff.AddNotDispelable().SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Replace).AddComponent<OnCombatModeExpired>().Configure();

                

            }

        }
    }
}
