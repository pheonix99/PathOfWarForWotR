using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Backend.NewComponents.AbilityRestrictions;
using TheInfiniteCrusade.Utilities;
using UnityEngine;

namespace TheInfiniteCrusade.NewContent.Disciplines
{
    class IronTortoise
    {
        public static void Build()
        {
            var bash = BlueprintTools.GetBlueprint<BlueprintActivatableAbility>("3bb6b76ed5b38ab4f957c7f923c23b68");

            DisciplineTools.AddDiscipline("IronTortoise", "Iron Tortoise", "The discipline known as Iron Tortoise rose up from the need to protect one’s self and allies from harm during wartime. Phalanx fighters knew that their shield protected them as much as their brother, and that a sturdy shield wall could repel almost any harm. Iron Tortoise disciples learn that their discipline requires their defensive stances to be perfect; they must not be budged from their spot unless they choose to move from them. Iron Tortoise requires its practitioners be proficient with a shield, and many of its maneuvers can only be used with a shield or shield-like device. The Iron Tortoise discipline’s associated skill is Bluff, and its associated weapon groups are axes, heavy blades, and close weapons.\n Maneuvers from Iron Tortoise require use of a shield in one hand. Animated shields are not allowed as they do not allow the full range of motions required to use these maneuvers. Tower shields may be used, but cannot be used to perform shield bash maneuvers unless the initiator has the ability to perform shield bashes with tower shields.", new Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup[] { Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Axes, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.BladesHeavy, Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Close }, Kingmaker.EntitySystem.Stats.StatType.SkillPersuasion, bash.Icon);
            DisciplineTools.Disciplines.TryGetValue("IronTortoise", out var ironTortoise);

            StanceOfTheDefendingShell();

            #region level 1

            AngeringSmash();
            void AngeringSmash()
            {

            }

            SnappingStrike();
            void SnappingStrike()
            {
                var snapping = MakeIronTortoiseStrike("SnappingStrike", "Snapping Strike", "Disciples of the Iron Tortoise school learn basics of the style, and that is to keep one’s shield high and one’s weapon low. By making a quick and vicious strike, the disciple may drive home a potent blow. The initiator makes a melee attack, and if successful, this attack inflicts an additional 1d6 points of damage.", 1, extraDice: 1);



                ManeuverTools.FinishManeuver(snapping, Main.Context);


            }

            SnappingTurtleStance();
            void SnappingTurtleStance()
            {
                var snappingStance = ManeuverTools.MakeSimpleDamageUpStance(Main.Context, "SnappingTurtleStance", "Snapping Turtle Stance", "The disciple of the Iron Tortoise in this stance holds his shield in a manner to deliver punishing shield bashes without sacrificing his ability to defend himself. While in this stance, the initiator may make shield bash attacks without losing his shield bonus to his Armor Class, and shield bash attacks inflict an additional 1d6 points of damage. This bonus damage increases by +1d6 every 8 initiator levels beyond 1st level.", 1, ironTortoise, 1, 8, out var buff);

                BuffConfigurator.For(buff).AddFacts(facts: new List<BlueprintCore.Utils.Blueprint<Kingmaker.Blueprints.BlueprintUnitFactReference>>() { "3bb6b76ed5b38ab4f957c7f923c23b68" }).Configure();

                ManeuverTools.FinishManeuver(snappingStance, Main.Context);
            }

            void StanceOfTheDefendingShell()
            {
                var StanceOfTheDefendingShell = ManeuverTools.MakeSimpleStatUpStance(Main.Context, "StanceoftheDefendingShell", "Stance of the Defending Shell", "An Iron Tortoise disciple learns a valuable stance that forms the core of their discipline, that being the nature of the unbreakable wall. By focusing one’s attention to defense with their shield, their martial skill improves their defense. The initiator gains an additional +1 bonus to his shield AC while in this stance, and this bonus increases by +1 for every 4 initiator levels he possesses after 1st level (+2 at 5th, +3 at 9th, +4 at 13th, and to a maximum of +5 at 17th level).", 1, ironTortoise, Kingmaker.EntitySystem.Stats.StatType.AC, Kingmaker.Enums.ModifierDescriptor.ShieldFocus, 1, 4, out var IHSbuff);

                ManeuverTools.FinishManeuver(StanceOfTheDefendingShell, Main.Context);
            
            }


            GreaterSnappingStrike();
            void GreaterSnappingStrike()
            {
                var snapping = MakeIronTortoiseStrike("GreaterSnappingStrike", "Greater Snapping Strike", "A decisive strike is often times what ends a battle or duel, and the disciple of the Iron Tortoise knows to keep his wits about him and wait for the moment to strike hard and strike true. Make a melee attack, and if successful, this strike inflicts an additional 3d6 points of damage and ignores the creature’s damage reduction (if any).", 3, extraDice: 3, ignoreDR : true);



                ManeuverTools.FinishManeuver(snapping, Main.Context);


            }


            SmashingShell();
            void SmashingShell()
            {
                var snapping = MakeIronTortoiseStrike("SmashingShell", "Smashing Shell", "The Iron Tortoise disciple is a master of using his shield both for offense and defense. By bringing his shield to bear and stepping in to meet a foe with a powerful and unexpected shield bash, the disciple may daze his foe into defeat. The initiator makes a shield bash attempt against his opponent, if successful this strike inflicts an additional 4d6 points of damage and has a chance to daze the target on a failed Fortitude save (DC 14 + initiation modifier).", 4, extraDice: 4, payload: ManeuverTools.ApplyBuffIfNotSaved("9934fedff1b14994ea90205d189c8759", durationValue: ManeuverTools.InitiatorModifierRounds(), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude));



                ManeuverTools.FinishManeuver(snapping, Main.Context);


            }

            ShellShock();
            void ShellShock()
            {

            }


            #endregion

            BlueprintAbility MakeIronTortoiseStrike(string sysName, string displayName, string desc, int level, bool sheildBash = false, int extraDice = 0, ActionsBuilder payload = null, Sprite icon = null, bool ignoreDR = false)
            {
                var ability = ManeuverTools.MakeStandardStrike(Main.Context, sysName, displayName, desc, level, ironTortoise, extraDice: extraDice, payload: payload, icon: icon, shieldBash: sheildBash, allDamageIgnoresDr: ignoreDR);
                ability.AddComponent<IronTortoiseShieldRequiredRestriction>(x =>
               {
                   x.IsBash = sheildBash;
               });
                ability.AddComponent<ManeuverRangeRestriction>();


                return ability;


            }
        }
    }
}
