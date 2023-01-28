using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PathOfWarForWotR.Utilities;

namespace PathOfWarForWotR.NewContent.MartialClasses.Archetypes
{
    class Mystic_KnightChandler
    {
        public static void Make()
        {


            var config = ArchetypeTools.MakeArchetype(Main.Context, "KnightChandlerArchetype", "Knight-Chandler", "Like all mystics, those who become knight-chandlers are warriors who utilize discipline and enlightenment to shape powerful and unstable magical energies within them. What sets these warriors apart from other mystics, however, is the fierce hope that burns within their souls. Knight-chandlers are marked by indomitable hope, intense loyalty, deep compassion, and a concern for others that shapes their worldview and actions. They transform the arcane energies within themselves into something that illuminates the world with their unrelenting passion and hope. The light they wield shelters those they call friends and family—and burns those that would threaten the ones they love.", BlueprintTool.Get<BlueprintCharacterClass>("MysticClass"));
            config.AddPrerequisiteAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.Good, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any);
            config.AddPrerequisiteAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.LawfulNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any);
            config.AddPrerequisiteAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.TrueNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any);
            config.AddPrerequisiteAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.ChaoticNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any);


        }

        /*
         * 
Knight-chandler
View source

Like all mystics, those who become knight-chandlers are warriors who utilize discipline and enlightenment to shape powerful and unstable magical energies within them. What sets these warriors apart from other mystics, however, is the fierce hope that burns within their souls. Knight-chandlers are marked by indomitable hope, intense loyalty, deep compassion, and a concern for others that shapes their worldview and actions. They transform the arcane energies within themselves into something that illuminates the world with their unrelenting passion and hope. The light they wield shelters those they call friends and family—and burns those that would threaten the ones they love.
Contents

    1 Alignment
    2 Soul Candle (Su)
    3 Illumination (Su)
    4 Candle Magic (Su)
        4.1 Votive Effects:
        4.2 Lantern Effects:
        4.3 Bonfire Effects:
    5 Share the Light (Su)
    6 Eternal Candle (Su)
    7 Ex-Knight Chandlers

Alignment

Any non-evil. Knight-chandlers are defined by their concern for others, passionate friendship, and unquenchable hope.
Soul Candle (Su)

At 1st level, a knight-chandler creates a flickering ball of energy formed from a combination of arcane power and her own soulstuff, known as her candle. The knight-chandler’s candle is a fist-sized mote of light that normally hovers in the knight-chandler’s space, and has the following properties:

    The candle is not a creature or an object and is not subject to attacks of any kind.
    The candle sheds bright light out to a distance of 5 feet per point of illumination the knight-chandler has in her illumination pool (see below), and normal light for an equal distance beyond that.
    Once per round as a free action, the knight-chandler can will her candle to move up to her speed. The candle’s movement is not inhibited in any way by difficult terrain or environmental conditions, although it cannot move through solid objects. The candle remains within one mile of its creator at all times; the knight-chandler may not move the candle beyond that radius, and if she attempts to move further than one mile from her candle, it floats along gently at the limit of its range. If the knight-chandler moves outside this range faster than the candle can follow (for example, by teleportation), the candle appears one round later in a random space precisely one mile from the knight-chandler.
    If the candle is in the knight-chandler’s space, she can instruct it to move with her; it moves when she does, remaining in her space until directed elsewhere. In addition, the knight-chandler may include her candle in teleportation effects that affect her. If she does so, it arrives in the same space she does.
    As a swift action, the knight-chandler may summon her candle to her space from wherever it is.
    Allies within 15 feet of the candle gain resistance to the knight-chandler’s active element’s associated energy type equal to the number of points in the knight-chandler’s illumination pool, up to a maximum of the knight-chandler’s class level plus her knight-chandler initiation modifier.

Unlike other supernatural abilities, a knight-chandler’s candle is not completely suppressed when it or the knight-chandler is within an antimagic field or similar effect. If the knight-chandler or her candle enters the area of such an effect, the candle immediately dims, shedding normal light with a radius of 5 feet, rather than its bright light. It loses all other abilities (including those gained through other class features) except for the knight-chandler’s ability to direct it to move once per round as a free action. The candle regains its full power once both it and the knight-chandler remove themselves from the area of effect suppressing the ability.
Illumination (Su)

Unlike a normal mystic, a knight-chandler tempers her inner animus into an energy called illumination, fueling her candle and its effects. Outside combat, a knight-chandler has one point of illumination in her illumination pool; she may not gain additional points of illumination outside of combat. When a knight-chandler enters combat, she adds one point of illumination to her illumination pool at the start of her first turn, and one more point of illumination to her illumination pool at the start of each turn thereafter. In addition, a knight-chandler gains one point of illumination whenever she initiates a boost, and she can focus her power as a swift action to add two points of illumination to her pool. A knight-chandler’s illumination pool persists for one minute after the last enemy combatant is defeated or the encounter otherwise ends, after which it drops back down to one point.

In addition, the knight-chandler gains Tap Animus as a bonus feat, even if she does not meet the prerequisites. Her levels in knight-chandler count as arcane spellcaster levels for the purposes of qualifying for prerequisites (such as those of item creation feats or the Arcane Strike feat), and if a knight-chandler ever develops arcane spellcasting from another class, she may add her knight-chandler level to her levels in that class to determine her overall caster level for the purposes of item creation feats.

A knight-chandler’s inner illumination manifests in both her candle and a powerful enhancement of her body and soul:

    Starting at 1st level, a knight-chandler can empower her attacks with a brilliant luminescence. Whenever the knight-chandler hits a creature with an attack, she can choose to have her target become outlined as if by a faerie fire spell, with a caster level equal to the knight-chandler’s initiator level. Successive uses of this ability against the same creature do not stack; instead, they extend the duration. 
    Starting at 4th level, a knight-chandler deals additional damage equal to her initiator level on any attack she makes as part of a strike against an opponent that threatens one of her allies other than herself.
    Starting at 9th level, when a knight-chandler hits a creature outlined by her illumination with an attack, she can choose to snuff out the light affecting that creature to heal herself for a number of hit points equal to 1/2 the damage dealt by her attack. Activating this ability is a free action that can be taken even if it isn’t the knight-chandler’s turn, although it can only be used once per round. The knight-chandler cannot heal more damage than her target’s maximum hit points with this ability.
    Starting at 17th level, the knight-chandler can perceive the surroundings of her candle as if she herself were there. She can treat the candle’s space as her own for the purposes of determining line of sight, making Perception checks, or the range of any special senses she possesses (such as blindsense). In addition, once per encounter, the knight-chandler can teleport to her candle as a standard action. The knight-chandler does not require line of effect to use this ability, although if her candle’s space is occupied by another creature, she is shunted to the nearest unoccupied space. This is a teleportation effect.

This ability replaces animus.
Candle Magic (Su)

The bright energy that the knight-chandler’s candle releases is more than mere light; it is her raw power, infused with her loyalty and devotion. As her illumination builds, the knight-chandler can shape this energy into various effects.

At 3rd level, the knight-chandler can project a votive effect from her candle. At 8th level, she can also project a lantern effect from her candle. Finally, at 15th level, the knight-chandler can also project a bonfire effect from her candle.

The knight-chandler may change which effects her candle projects as a swift action, though she can never project more than one effect of the same type at a time. All allies within 15 feet of the knight-chandler’s candle gain the benefits of the projected effect.
Votive Effects:

    Affected allies gain a morale bonus to saving throws against mind-affecting abilities equal to the knight-chandler’s illumination, up to a maximum bonus equal to her knight-chandler initiation modifier.
    Affected allies add energy damage of the knight-chandler’s active element’s associated energy type equal to the knight-chandler’s illumination to their melee and ranged attacks, up to a maximum amount equal to her class level.
    Affected allies gain the ability to move 5 feet as a swift action without provoking attacks of opportunity.
    Affected allies gain the ability to tap into the candle’s energy as a swift action, gaining temporary hit points equal to the knight-chandler’s illumination, up to a maximum number of temporary hit points equal to her class level. These temporary hit points do not stack with themselves, and last for up to 1 minute.

Lantern Effects:

    Affected allies gain the ability to teleport to an unoccupied space adjacent to the knight-chandler as a move action. If there is no such space available, the ally does not spend their move action and does not teleport, instead getting a sense of being impeded. The knight-chandler cannot benefit from this effect when it is projected from her own candle.
    Affected allies gain immunity to death effects, and gain a bonus on saving throws against psi-like abilities, psionic powers, spell-like abilities, spells, and supernatural abilities of undead creatures equal to the knight-chandler’s illumination, to a maximum bonus equal to the knight-chandler’s initiation modifier.
    Affected allies gain a bonus on caster level checks, manifester level checks, and skill checks equal to the knight-chandler’s illumination, up to a maximum bonus equal to her knight-chandler initiation modifier.
    Affected allies gain a deflection bonus to their AC equal to the knight-chandler’s illumination, to a maximum bonus equal to her knight-chandler initiation modifier.

Bonfire Effects:

    Affected allies gain fast healing equal to the knight-chandler’s illumination, to a maximum amount equal to her class level.
    Whenever an affected ally recovers one or more maneuvers, that ally heals up to 2 points of ability damage to a single ability score of their choice. If an ally does not have maneuvers or does not wish to recover maneuvers, they can activate this ability as a standard action.
    Affected allies gain the ability to move up to twice their speed as a swift action without provoking attacks of opportunity.
    If an affected ally would be reduced to 0 or fewer hit points, that ally can trigger the candle’s power as an immediate action. If they do, that ally is immediately restored to 1/2 their maximum hit point total, and all of the following adverse conditions affecting that ally (if any) immediately end: ability damage, blinded, confused, dazed, dazzled, deafened, diseased, exhausted, fatigued, feebleminded, insanity, nauseated, poisoned, sickened, and stunned. The damage that would have reduced the ally to 0 or fewer hit points is negated. Triggering the candle’s power in this fashion is draining to the knight-chandler; doing so reduces the knight-chandler’s illumination pool to one point (even if her minimum is normally higher) and renders her incapable of projecting bonfire effects for the remainder of the encounter.

This ability replaces elemental glyphs.
Share the Light (Su)

Starting at 9th level, the knight-chandler can share the fierce light that wells up from deep within her soul, bestowing it upon her allies. As a swift action, she can select an ally within 15 feet. That ally chooses a single boost or counter that the knight-chandler has readied and unexpended. Until the beginning of the knight-chandler’s next turn, that ally can initiate that maneuver as if they had readied it normally, using the knight-chandler’s initiator level or their own, whichever is higher. If the ally chooses to initiate the maneuver, the knight-chandler expends that boost or counter (even if it isn’t currently granted to her; if it wasn’t granted to her, treat it as though it had been granted, then expended).

This ability replaces quell magic.
Eternal Candle (Su)

At 20th level, the knight-chandler transcends to something more than mortal, her fierce soul transforms her body with its overflowing light. Her type changes to Outsider, and she gains the Native and Good subtypes. In addition, she ceases aging, and her minimum illumination becomes equal to her knight-chandler initiation modifier; increasing as normal during combat and resetting to her new minimum after spending one minute outside combat. Finally, the knight-chandler gains the ability to initiate two boosts with the same swift action, expending each as normal.

This ability replaces glyph mastery.
Ex-Knight Chandlers

A knight-chandler who becomes of evil alignment finds themselves deprived of the hopeful, valorous worldview that shapes their unique power. Such characters may not advance further as knight-chandlers until such a time as they cease being of evil alignment, though they do not lose access to their class features. Even an evil knight-chandler remembers her former loyalty and compassion, and by focusing on these memories, she can still summon a candle and use her candle magic. 
         */

    }
}
