using Kingmaker.Blueprints.Classes;
using PathOfWarForWotR.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace PathOfWarForWotR.NewContent.Disciplines
{
    public static class BrokenBlade
    {
        public static void BuildBrokenBlade()
        {
            var ius = BlueprintTools.GetBlueprint<BlueprintFeature>("7812ad3672a4b9a4fb894ea402095167");

            DisciplineTools.AddDiscipline("BrokenBlade", "Broken Blade", "Legend has it the first practitioner of the Broken Blade style was a powerful swordsman who in the middle of a life-or-death duel with an old enemy found his sword broken by his opponent and had to toss it aside. Disheartened by his lack of weapons, he quickly realized that his years of training, exercise, and conditioning had made his body a weapon all on its own. Using only his fists and his nerve, this long-forgotten swordsman became the first to develop this discipline’s techniques, and he passed his experience on to others. Disciples of the Broken Blade teach these methods in monasteries, to cloistered warrior-monks who learn to operate without the use of traditional weapons of combat. Others learn from parents or individual mentors, haphazard or otherwise, and scrap their way through as it suits them. The Broken Blade’s associated skill is Acrobatics, and its associated weapon groups are close, monk, and natural. ", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Monk, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Close, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Natural }, Kingmaker.EntitySystem.Stats.StatType.SkillMobility, ius.Icon);


        }

    }
}
