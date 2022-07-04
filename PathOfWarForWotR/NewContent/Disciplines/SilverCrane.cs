using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Utilities;

namespace TheInfiniteCrusade.NewContent.Disciplines
{
    class SilverCrane
    {
        public static void Build()
        {
            var wingsicon = AssetLoader.LoadInternal(Main.Context, "", "Fly.png");

            DisciplineTools.AddDiscipline("SilverCrane", "Silver Crane", "Disciples of the Silver Crane are men and women for whom the power of the celestial and divine flow into the arts of their blade. The Silver Crane is a goodly discipline that is inspired by the teachings of celestials. It focuses on strong strikes designed to combat evil, celestial insights, and combat-predictions to defeat foes and enable the initiator and his allies to endure the hardships of battle against the forces of evil. Upon learning the art of Silver Crane, the disciple becomes in tune with the flows of the celestial realm, gaining heavenly insights into combat as if the angels themselves were granting insight to the warrior in battle. The Silver Crane discipline’s associated skill is Perception, and its associated weapon groups are bows, hammers, and spears.\n The discipline of Silver Crane is to be considered a supernatural discipline and all abilities within are considered supernatural abilities and follow the rules and restrictions of such. All abilities in this discipline carry the [good] descriptor. A character may always strike incorporeal foes as if they were corporea with strikes of this discipline.", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Bows, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Spears, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Hammers }, Kingmaker.EntitySystem.Stats.StatType.SkillPerception, wingsicon);
            DisciplineTools.Disciplines.TryGetValue("SilverCrane", out var silverCrane);

            SilverCraneWaltz();

            void SilverCraneWaltz()
            {
                var StanceOfTheDefendingShell = ManeuverTools.MakeSimpleStatUpStance(Main.Context, "SilverCraneWaltz", "Silver Crane Waltz", "The divine vantage point of the celestial realms infuses the mind of the disciple, granting him momentary flickers of foresight. A faint, ghostly image of wings is visible around the disciple but vanishes when he is looked upon directly. The initiator gains a +4 insight bonus to initiative checks if he is in this stance before combat begins, and a +2 insight bonus to Reflex saves and to AC. These bonuses increase by an additional +1 every eight levels after 1st level.", 1, silverCrane, Kingmaker.EntitySystem.Stats.StatType.AC, Kingmaker.Enums.ModifierDescriptor.Insight, 2, 8, out var IHSbuff);
                IHSbuff.AddComponent<AddContextStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.SaveReflex;
                    x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Insight;
                    x.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageBonus

                    };

                });
                IHSbuff.AddComponent<ContextRankConfig>(x =>
                {
                    x.m_Type = AbilityRankType.DamageDice;
                    x.m_Progression = ContextRankProgression.StartPlusDivStep;
                    
                        x.m_StartLevel = -32;
                    
                    x.m_StepLevel = 8;

                });
                IHSbuff.AddComponent<AddContextStatBonus>(x =>
                {
                    x.Stat = Kingmaker.EntitySystem.Stats.StatType.Initiative;
                    x.Descriptor = Kingmaker.Enums.ModifierDescriptor.Insight;
                    x.Value = new Kingmaker.UnitLogic.Mechanics.ContextValue
                    {
                        ValueType = Kingmaker.UnitLogic.Mechanics.ContextValueType.Rank,
                        ValueRank = AbilityRankType.DamageDice

                    };

                });
                ManeuverTools.FinishManeuver(StanceOfTheDefendingShell);
            }
        }
    }
}
