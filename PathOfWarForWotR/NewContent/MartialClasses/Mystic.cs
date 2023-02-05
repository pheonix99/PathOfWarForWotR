using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.Utilities;
using PathOfWarForWotR.Defines;
using PathOfWarForWotR.Utilities;

namespace PathOfWarForWotR.NewContent.MartialClasses
{
    class Mystic
    {
        private static BlueprintFeatureSelectionReference BonusFeat;
        public static void Make()
        {

            void MakeElementalGlyphs()
            {
                AbilityConfigurator MakeGlyphBlank(string elementName, string desc)
                {
                    var glyph = AbilityTools.MakeAbility(Main.Context, "Mystic" + elementName + "GlyphAbility", "Elemental Glyph: " + elementName, desc, UnitCommand.CommandType.Move, AbilityType.Supernatural, Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point);

                    glyph.SetLocalizedDuration("1 + caster initation modifier");

                    return glyph;
                }

                var airGlyphAbility = MakeGlyphBlank("Air", "Air: The glyph of air fills the mystic’s allies with energy and speed.\nAt 3rd level, all movement speeds possessed by each ally under the effect of the glyph gain a + 10 - foot enhancement bonus to speed.In addition allies may choose to make one turn of up to 90 degrees while making charge attacks.\nAt 8th level, this enhancement bonus to movement speed increases to 30 feet, and they may make Acrobatics checks to jump as if they had a running start.\nAt 13th level, allies affected by the glyph of air gain the ability to move up to 30 feet as a swift action. This movement provokes attacks of opportunity as normal.\nAt 19th level, whenever one of the mystic’s allies uses the glyph of air’s swift action movement ability, they can make a single attack with a weapon they are wielding at their highest base attack bonusat any point during this movement.");
                var airMade = airGlyphAbility.Configure();
                Main.LogPatch(airMade);
                var DarknessGlyphAbility = MakeGlyphBlank("Darkness", "Darkness: Though darkness often connotates evil, for the mystic its shroud offers both protection and insight, allowing the mystic’s allies tactical avenues that they would otherwise be unable to access.\nAt 3rd level, allies affected by the glyph of darkness gain concealment(20 % miss chance).\nAt 8th level, affected allies gain darkvision and the effects of a see invisibility spell out to a range of 60 feet.\nAt 13th level, affected allies are shrouded in a pitch - black veil, gaining total concealment(50 % miss chance).\nAt 19th level, affected allies gain the blindsight trait with a range of 30 feet.");
                var darkMade = DarknessGlyphAbility.Configure();
                Main.LogPatch(darkMade);
                var earthGlyphAbility = MakeGlyphBlank("Earth", "Earth: The dour, steadfast nature of earth allows the mystic’s allies to better stand their ground and weather assaults.\nAt 3rd level, this glyph makes allies more difficult to move against their will.Affected allies gain a bonus to their CMD equal to the mystic’s initiation modifier.\nAt 8th level, affected allies gain DR / adamantine equal to the mystic’s initiation modifier.\nAt 13th level, affected allies gain resistance to all energy types equal to the mystic’s class level.\nAt 19th level, the first time during the encounter that an affected ally is reduced to 0 or fewer hit points, they are instead reduced to 0 hit points and automatically stabilize.Their hit points cannot be reduced below 0 for the rest of the round.Once this effect triggers on an ally, that ally cannot gain its effect again until the end of the encounter.In addition, affected are allies are immune to bleed damage.");
               var earthMade = earthGlyphAbility.Configure();
                Main.LogPatch(earthMade);
                var fireGlyphAbility = MakeGlyphBlank("Fire", "Fire: The unquenchable flames of passion drive the mystic’s allies to feats of glory.\nAt 3rd level, affected allies gain a circumstance bonus on attack rolls equal to 1 / 4 the mystic’s class level (minimum +1).\nAt 8th level, affected allies add 1/2 the mystic’s class level as fire damage to attacks they make.\nAt 13th level, affected allies’ attacks ignore a number of points of energy resistance equal to the mystic’s class level.\nAt 19th level, whenever an affected ally is targeted by a melee attack, the attacker takes fire damage equal to the mystic’s class level, regardless of whether or not the attack hits.");
               var fireMade = fireGlyphAbility.Configure();


                Main.LogPatch(fireMade);
                var IlluminationGlyphAbility = MakeGlyphBlank("Illumination", "Illumination: The light of the universe reveals truth wherever it hides.\nAt 3rd level, affected allies’ attacks ignore the miss chance from concealment granted to targets by anything less than total concealment.\nAt 8th level, affected allies gain a circumstance bonus on Will saving throws against illusion spells and effects equal to the mystic’s initiation modifier.\nAt 13th level, affected allies gain the effects of a true seeing spell out to a range of 30 feet.\nAt 19th level, affected allies are protected from any falsehood, gaining the effects of a mind blank spell.");
                var lightMade = IlluminationGlyphAbility.Configure();
                Main.LogPatch(lightMade);
                var MetalGlyphAbility = MakeGlyphBlank("Metal", "Metal: Sturdy and unyielding like the forged iron from which it takes its name, the glyph of metal imparts resolute strength to the mystic’s allies./nAt 3rd level, affected allies increase their natural armor bonus to AC by an amount equal to 1 / 4 the mystic’s class level (minimum +1)./nAt 8th level, affected allies gain a circumstance bonus on Fortitude saves equal to 1/4 the mystic’s class level (minimum +1)./nAt 13th level, affected allies’ attacks ignore a number of points of damage reduction and hardness equal to the mystic’s initiation modifier./nAt 19th level, affected allies gain damage reduction/– equal to the mystic’s initiation modifier and gain spell resistance equal to 15 + the mystic’s class level.");
                var metalMade = MetalGlyphAbility.Configure();
                Main.LogPatch(metalMade);
                var WaterGlyphAbility = MakeGlyphBlank("Water", "Water: Fluid and changeable, the glyph of water grants allies flexibility in both their movement and their thinking./nAt 3rd level, affected allies gain a circumstance bonus to their CMB and on Swim checks equal to the mystic’s initiation modifier./nAt 8th level, affected allies ignore difficult terrain when they move./nAt 13th level, affected allies gain the effects of a freedom of movement spell./nAt 19th level, affected allies gain fast healing 10.");
                var waterMade = WaterGlyphAbility.Configure();
                Main.LogPatch(waterMade);
            }

            BlueprintFeatureBaseReference MakeMysticProficiencies()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticProficienciesFeature", "Mystic Proficiencies", "A mystic is proficient with all simple and martial weapons, bastard swords and with light armor and shields (except tower shields). ", true);
                config.AddFacts(facts: new() { "6d3728d4e9c9898458fe5e9532951132", "e70ecf1ed95ca2f40b754f1adb22bbdd", "203992ef5b35c864390b4e4a1e200629", "cb8686e7357a68c42bdd9d4e65334633", "57299a78b2256604dadf1ab9a42e2873" });
                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeMysticAnimusFeature()
            {
               var config = MoreFeatTools.MakeFeature(Main.Context, "MysticAnimusFeature", "Mystic Animus", "A mystic’s martial prowess is in part fueled by a reservoir of roiling, turbulent energy within her soul, and the passion and danger of combat causes this arcane energy to overflow outwards. This power, called animus, waxes and wanes with a mystic’s use of her maneuvers in battle. Outside combat, a mystic has no animus to spend, but her inner power can still be used for more subtle arcane arts. Her levels in mystic count as arcane spellcaster levels for the purposes of qualifying for prerequisites (such as those of item creation feats or the Arcane Strike feat), and if a mystic ever develops arcane spellcasting from another class, she may add her mystic level to her levels in that class to determine her overall caster level for the purposes of item creation feats.\n When a mystic enters combat, she gains an animus pool equal to 1 + her mystic initiation modifier(minimum 1) at the start of her first turn, and adds one point of animus to her animus pool at the start of each of her turns thereafter.Her animus pool persists for one minute after the last enemy combatant is defeated or the encounter otherwise ends.At the end of any round in which the mystic initiates a maneuver, she adds an additional point of animus to her pool.Certain abilities, such as some class features, maneuvers, and feats, require the mystic to expend points of animus to use.\n The primal power of animus can be used in several ways—the foremost of which is the augmentation of maneuvers.A mystic can spend points of animus to augment her maneuvers in the following ways, depending on her class level. If the mystic has the ability to augment her maneuvers in other ways, such as from another class feature or the maneuver itself, this cannot be combined with the augments granted by her animus class feature; she must choose which augmentation type to use when initiating the maneuver. ", true);
                //TODO add animus resource
                //TODO add base animus level
                //TODO add initator modifier effect
                //TODO mystic for reqs



                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeMysticAnimusAugments1Feature()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticAnimusAugments1Feature", "Mystic Animus Augments", "Starting at 1st level, a mystic can spend a single point of animus to augment a maneuver as part of that maneuver’s initiation action to apply one of the following effects to it (if applicable):\nEnhance Maneuver: For each point of animus spent, the mystic adds a cumulative + 2 insight bonus to all d20 rolls made(including attack rolls, combat maneuver checks, and skill checks) when initiating that maneuver(maximum of three animus may be spent on this augmentation); if the maneuver allows the user to make multiple attacks, then this bonus only applies to the first attack.\nIncrease DC: For each point of animus spent, the save DC of that maneuver increases by 1.", true);
                //TODO add animus augments


                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureReference MakeMysticAnimusAugments2Feature()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticAnimusAugments2Feature", "Mystic Animus Augments", "When the mystic reaches 4th level, she can spend up to two points of animus on maneuver augmentation, rather than one, and she gains access to the following additional augmentations:\nAnima Burn: The mystic adds 1 / 2 her class level to damage rolls made during that maneuver.This augment costs two points of animus, and can only be applied once to a given maneuver.\nIncrease Potency: For each point of animus spent, the mystic may ignore 10 points of energy resistance or 5 points of damage reduction.", true);
                //TODO add animus augments


                return config.Configure().ToReference<BlueprintFeatureReference>();
            }
            BlueprintFeatureBaseReference MakeMysticAnimusAugments3Feature()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticAnimusAugments3Feature", "Mystic Animus Augments", "When the mystic reaches 9th level, she can spend up to three points of animus on maneuver augmentation, and gains access to the following additional augmentation:Animus Rush: The mystic make move up to her base movement speed as part of the initiation action for the maneuver before initiating the strike.This is a teleportation effect and the mystic must clearly see her destination.This augmentation costs three points of animus.Increase Range: The mystic may target a creature within 30 feet with a strike that normally uses a melee attack. Resolve the strike normally, as if the targeted creature was within the mystic’s melee reach.This augmentation costs two points of animus.", true);
                //TODO add animus augments


                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }
            BlueprintFeatureBaseReference MakeMysticAnimusAugments4Feature()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticAnimusAugments4Feature", "Mystic Animus Augments", "At 13th level, a mystic can spend up to four points of animus on maneuver augmentation,", true);
                //TODO add animus augments


                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }
            BlueprintFeatureBaseReference MakeMysticAnimusAugments5Feature()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticAnimusAugments5Feature", "Mystic Animus Augments", "At 19th level, a mystic can spend up to five points of animus on maneuver augmentation,", true);
                //TODO add animus augments


                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeElementalAttunement()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticElementalAttunementFeature", "Elemental Attunement ", "A mystic contains incredible elemental power within her body, surging energies that constantly flow through her blood and muscle. Bringing these energies to bear is as easy as breathing for a mystic, shifting the flow of power with the subtle movements of her martial stances. When a mystic readies her maneuvers, she may select one of the following elements (and associated energy type) to be her active element: air (electricity), earth (acid), fire (fire), and water (cold).\n After readying maneuvers, a mystic can change her active element by taking a standard action to focus inwards, or by expending one point of animus as a free action while assuming a new stance.Whenever she initiates a maneuver that deals damage, she may spend one point of animus as part of its initiation action to change all damage the maneuver deals to her active element’s associated energy type. For example, if a mystic whose active element is currently air initiated the cursed fate Veiled Moon strike, she could spend one point of animus to change her attack’s damage(including the strike’s bonus damage) to from her weapon’s normal damage type to electricity damage.\n If the mystic has access to the Elemental Flux discipline, then her active element from this class feature is the same as her active element for Elemental Flux maneuvers.If she is psionic, she can change her active energy type whenever she changes her active element, and vice versa.Her active energy type need not match her active element. .", true);
                //TODO ALL


                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }
            

            BlueprintFeatureBaseReference MakeBonusFeat()//DONE!
            {
                var config = MoreFeatTools.MakeFeatureSelector(Main.Context, "MysticBonusFeatureSelector", "Bonus Feat", "At 2nd level and every five levels thereafter, a mystic gain a bonus combat or item creation feat. She must meet the prerequisites for these feats as normal.", true);
                

                var made = config.Configure();
                FeatureSelectionConfigurator.For(made).SetGroup(FeatureGroup.CombatFeat).Configure(delayed: true);
                BonusFeat = made.ToReference<BlueprintFeatureSelectionReference>();
            
                Main.LogPatch(made);
             
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeArcaneDefense()
            {
                var config2 = MoreFeatTools.MakeFeature(Main.Context, "MysticArcaneDefenseUpgradeFeature", "Arcane Defense", "Starting at 2nd level, the sorcerous power within a mystic’s body makes her resilient to the supernatural. These energies defend her from magical and psionic powers, granting her a +1 insight bonus to her AC and saving throws against psionic powers, psi-like abilities, spells, and spell-like abilities. This bonus increases by +1 at 6th level, and again at 11th level, 16th level, and 20th level.", true);
                var upgradeBuilt = config2.Configure();
              
                Main.LogPatch(upgradeBuilt);
             
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticArcaneDefenseFeature", "Arcane Defense", "Starting at 2nd level, the sorcerous power within a mystic’s body makes her resilient to the supernatural. These energies defend her from magical and psionic powers, granting her a +1 insight bonus to her AC and saving throws against psionic powers, psi-like abilities, spells, and spell-like abilities. This bonus increases by +1 at 6th level, and again at 11th level, 16th level, and 20th level.", true);
                config = FeatureConfigurator.For(config.Configure());
                //TODO CONFIRM WORKS!
                config.AddSavingThrowBonusAgainstAbilityType(AbilityType.SpellLike, bonus: new ContextValue() { 
                    ValueType = ContextValueType.Rank,
                    Value = 0,
                    ValueRank = Kingmaker.Enums.AbilityRankType.Default
                }, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Insight);
                config.AddSavingThrowBonusAgainstAbilityType(AbilityType.Spell, bonus: new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    Value = 0,
                    ValueRank = Kingmaker.Enums.AbilityRankType.Default
                }, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Insight);
                config.AddContextRankConfig(new Kingmaker.UnitLogic.Mechanics.Components.ContextRankConfig()
                {
                    m_Type = Kingmaker.Enums.AbilityRankType.Default,
                    m_BaseValueType = Kingmaker.UnitLogic.Mechanics.Components.ContextRankBaseValueType.FeatureList,
                    m_FeatureList = new BlueprintFeatureReference[] { BlueprintTool.GetRef<BlueprintFeatureReference>("MysticArcaneDefenseFeature"), BlueprintTool.GetRef<BlueprintFeatureReference>("MysticArcaneDefenseUpgradeFeature") }
                });
                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }


            MakeElementalGlyphs();
            BlueprintFeatureBaseReference MakeElementalGlyphFeature()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticElementalGlyphFeature", "Elemental Glyph ", "Starting at 3rd level, a mystic learns to aid her friends with the arcane power of the elements. Although at first this surging energy was raw and unformed, she has begun to master this ability, and can use it to empower her allies, granting them benefits in combat. As a move action, the mystic can spend one point of animus to apply an elemental glyph to a number of allies equal to her mystic initiation modifier modifier within her sight. The effect of this glyph depends on the element it is associated with, but all glyphs last for a number of rounds equal to 1 + the mystic’s initiation modifier. A mystic is not limited to casting glyphs of her active element(as darkness, illumination, and metal have no elemental type). Allies may only be affected by one of her glyphs at a time, with new glyphs ending the current glyph in effect and replacing it on the affected ally. The benefits of these glyphs are cumulative (for example, an 8th level mystic grants both the 3rd level and 8th level benefits to her allies). Different glyphs from different mystics may apply to the same target. Glyphs are supernatural abilities and not subject to spells or effects like dispel magic but do not function within an antimagic field or similar effect. ", true);
                //TODO ALL


                config.AddFacts(facts: new System.Collections.Generic.List<Blueprint<BlueprintUnitFactReference>>() { "MysticAirGlyphAbility", "MysticDarknessGlyphAbility", "MysticEarthGlyphAbility", "MysticFireGlyphAbility", "MysticIlluminationGlyphAbility", "MysticMetalGlyphAbility", "MysticWaterGlyphAbility" });

                return config.Configure().ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeMysticArtifice()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticArtificeFeature", "Mystic Artifice", "Starting at 4th level, a mystic is able to channel her animus into her craft, substituting primal arcane energies in place of more ordinary spells. When crafting an item, the mystic uses her initiator level as her caster level to determine how potent a creation she can and for the purposes of meeting caster level prerequisites. In order to create a spell trigger or spell completion item, the mystic’s initiator must be at least the minimum required to cast the spell or spells in question. When attempting to create a magical item for which she does not possess a prerequisite spell, the mystic can attempt to replicate the spell through her innate power with a Spellcraft check (DC 15 + the level of the spell being replicated). If successful, she can create the item as if she had cast the prerequisite spell. The mystic must possess any material components necessary for casting the spell that she is replicating. If the item being created requires multiple spells, she must make this check for each spell she intends to replicate. If she fails the skill check, she may not try again for that spell while creating that item—the item creation DC increases as normal for the creation of an item without a prerequisite spell in such a case.", true);
                //TODO ALL


                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeWithstandSpell()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticWithstandSpellFeature", "Withstand Spell", "Starting at 5th level, the mystic’s natural instincts with magic work to protect her, guiding her reactions against offensive spells cast by her enemies and allowing her to siphon off a small amount of that energy for her own use. If the mystic is targeted by a spell, spell-like ability, psionic power, or psi-like ability that normally would have a lesser effect (such as a partial effect) on a successful Fortitude or Reflex saving throw, she may make a Will saving throw instead of the effect’s normal save. If that Will saving throw is successful, she is completely unaffected by the spell or power, taking no damage and suffering no ill effects. If she fails the saving throw, she suffers the effect as normal, and adds one point of animus to her animus pool.\nThe mystic must be unencumbered and wearing light or no armor to use this ability.This ability does not protect the mystic from traps, extraordinary or supernatural abilities, or any other effects that require a Fortitude or Reflex save.A helpless mystic does not gain the benefit of withstand spell. ", true);
                //TODO ALL


                return config.Configure().ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeInstantEnlightnement()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticInstantEnlightentmentFeature", "Instant Enlightenment", "Starting at 6th level, a mystic is capable of drawing deep on her training to channel the untapped energies within her, granting her an infinitesimal moment of perfect clarity. Once per day as a free action, the mystic may expend one granted maneuver and instantly replace it with another maneuver she knows. This maneuver is added to her currently granted maneuvers and readied for use, replacing the previous maneuver. At 10th level and every four levels thereafter, the mystic can use this ability one additional time per day.", true);
                //TODO ALL


                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeQuellMagic()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticQuellMagicFeature", "Quell Magic", "Starting at 9th level, a mystic can channel her animus to act as a sort of null-magic energy, smothering active magical effects with its flow. As a standard action, she can emulate a dispel magic spell by spending three animus points.", true);
                //TODO ALL


                return config.Configure().ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeFontofAnimus()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticFontOfAnimusFeature", "Font of Animus", "At 15th level, a mystic gains the ability to draw in energy from the world around her, converting it into animus to fuel her own primal power. As a move action, the mystic can add a number of points of animus to her animus pool equal to 1d6 + her mystic initiation modifier. Unlike other animus abilities, this may be used outside of combat to generate a small pool of animus that persists for one minute outside of combat. A mystic cannot use this ability multiple times to accumulate animus; additional attempts only reset her animus pool to 1d6 + her mystic initiation modifier. The mystic may use this a number of times per day equal to her mystic initiation modifier + 1 (minimum of 1).", true);
                //TODO ALL


                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

            BlueprintFeatureBaseReference MakeGlyphMastery()
            {
                var config = MoreFeatTools.MakeFeature(Main.Context, "MysticGlyphMasteryFeature", "Glyph Mastery", "At 20th level, a mystic’s control over her elemental powers are strong enough that she can manifest two elemental glyphs at the same time. As a move action, the mystic may spend two points of animus and manifest an elemental glyph of any two elements, regardless of her active element. These glyphs otherwise function as her elemental glyphs class feature.", true);
                //TODO ALL
                //REMEMBER ALT-CAPSTONE integration!

                var made = config.Configure();
                Main.LogPatch(made);
                return made.ToReference<BlueprintFeatureBaseReference>();
            }

         
            string descTemp = "Born with an untamed magical power buried deep within his soul, the mystic is a warrior who, much like a sorcerer, is filled with untapped energies. This power however, is too primal and unstable, and is difficult to be formed effectively into a spell. By following a martial medium to tame this energy, the mystic is able to shape his wild power into martial maneuvers and this allows him to discover the deeper mysteries of his own inborn power. ";
          
            var mysticConfig = MoreClassTools.MakeClass("Mystic", descTemp, descTemp, 4, Kingmaker.RuleSystem.DiceType.D8, new StatType[] { StatType.SkillMobility, StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion, StatType.SkillStealth, StatType.SkillUseMagicDevice, StatType.SkillPerception }, MoreClassTools.BAB.Medium, false, false, true, 3, groups: new List<Blueprint<BlueprintReference<BlueprintCharacterClassGroup>>>() { "d942741434e7abd4e944872b9d6290c9", "a724728934ca2fd4ab616381bd508a82" }, recommendedAttributes: new StatType[] { StatType.Wisdom, StatType.Dexterity, StatType.Strength, StatType.Constitution }, notRecommendedAttributes: new StatType[] { StatType.Charisma });
            mysticConfig.CloneClassAesthestics("e8f21e5b58e0569468e420ebea456124");
            mysticConfig.SetStartingGold(411);
            mysticConfig.AddToStartingItems("200baf16628d3ab4b993094b51df5891");//Cold Iron Bastard Sword
            mysticConfig.AddToStartingItems("c65f6fc979d5556489b20e478189cbdd");//ChainShirt
            mysticConfig.AddToStartingItems("f4cef3ba1a15b0f4fa7fd66b602ff32b");//Heavy shield;
            mysticConfig.AddToStartingItems("201f6150321e09048bd59e9b7f558cb0");//Longbow
            mysticConfig.AddToStartingItems("d52566ae8cbe8dc4dae977ef51c27d91");//CLW pot
            

            InitiatorProgressionDefine mysticInitatiorDefine = new(Main.Context, "Mystic", grantedType: true);
            mysticInitatiorDefine.ManeuversKnownAtLevel1 = 7;
            mysticInitatiorDefine.ManeuversGrantedAtLevel1 = 2;
            mysticInitatiorDefine.ManeuverSlotsAtLevel1 = 5;
            mysticInitatiorDefine.ChosenManeuvers = 2;
            mysticInitatiorDefine.DefaultInitiatingStat = StatType.Wisdom;
            mysticInitatiorDefine.FixedUnlocks = new string[] { "ElementalFlux", "MithralCurrent", "RivenHourglass", "ShatteredMirror", "SolarWind", "VeiledMoon" };
            mysticInitatiorDefine.MakeFullRecovery("BladeMeditation", "When a mystic finds that her martial power is beginning to wane or that few options remain available for use, she can pause in battle, drawing on her inner well of animus to reinvigorate her body and mind. As a full-round action, a mystic can spend one point of animus to grant herself all her remaining withheld maneuvers, then immediately expend them in a raging cadence of arcane power. As there are no remaining maneuvers to be granted, a new set of maneuvers is granted to the mystic at the end of her turn, as normal.\nIn addition, until the start of her next turn, creatures that target the mystic with melee attacks are engulfed in the explosion of energy, taking 1d6 points of damage of her active element’s associated energy type, plus an additional 1d6 points of damage for every two points of animus remaining in the mystic’s animus pool. ");
           
            mysticInitatiorDefine.ProgressionSpecificSubstitutions = new string[] { "UnquietGrave" };
            mysticInitatiorDefine.NormalSlotsIncreaseAtLevels = new int[] { 3, 6, 9, 12, 15, 18, 20 };
            mysticInitatiorDefine.StancesLearnedAtLevels = new int[] { 1, 2, 5, 9, 11, 15, 18 };
            mysticInitatiorDefine.ManeuversLearnedAtLevels = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 11, 13, 15, 17, 19, 20 };
            
            


            //TODO start tiems and monies

            //TODO equomentEntities

            var mysticBuild = mysticConfig.Configure();

            
            mysticInitatiorDefine.ClassesForClassTemplate.Add(mysticBuild.ToReference<BlueprintCharacterClassReference>());

            var mysticProgression = ProcessProgressionDefinition.BuildInitiatorProgress(mysticInitatiorDefine);
            var prof = MakeMysticProficiencies();
            var mysticAnimusFeature = MakeMysticAnimusFeature();
            var mysticAugments1 = MakeMysticAnimusAugments1Feature();
            var mysticAugments2 = MakeMysticAnimusAugments2Feature();
            var mysticAugments3 = MakeMysticAnimusAugments3Feature();
            var mysticAugments4 = MakeMysticAnimusAugments4Feature();
            var mysticAugments5 = MakeMysticAnimusAugments5Feature();
            var mysticGlyphs = MakeElementalGlyphFeature();
            var elementalAttune = MakeElementalAttunement();
            var bonusfeat = MakeBonusFeat();
            var defense = MakeArcaneDefense();
            var defPlus = BlueprintTool.GetRef<BlueprintFeatureBaseReference>("MysticArcaneDefenseUpgradeFeature");
            var withstand = MakeWithstandSpell();
            var quell = MakeQuellMagic();
            var craft = MakeMysticArtifice();
            var enlightent = MakeInstantEnlightnement();
            var fount = MakeFontofAnimus();
            var glyphMaster = MakeGlyphMastery();
            mysticProgression.AddToProgressionLevels(1, prof);

            mysticProgression.AddToProgressionLevels(1, mysticAnimusFeature);
            
            mysticProgression.AddToProgressionLevels(15, fount);

            mysticProgression.AddToProgressionLevels(1, elementalAttune.Get().ToReference<BlueprintFeatureBaseReference>());
            mysticProgression.AddToProgressionLevels(3, mysticGlyphs);
            mysticProgression.AddToProgressionLevels(20, glyphMaster);//TODO BUILD CAPSTONE SETUP

            mysticProgression.AddToProgressionLevels(1, mysticAugments1.Get().ToReference<BlueprintFeatureBaseReference>());
            mysticProgression.AddToProgressionLevels(4, mysticAugments2.Get().ToReference<BlueprintFeatureBaseReference>());
            mysticProgression.AddToProgressionLevels(9, mysticAugments3.Get().ToReference<BlueprintFeatureBaseReference>());
            mysticProgression.AddToProgressionLevels(14, mysticAugments4.Get().ToReference<BlueprintFeatureBaseReference>());
            mysticProgression.AddToProgressionLevels(19, mysticAugments5.Get().ToReference<BlueprintFeatureBaseReference>());
            
          
            mysticProgression.AddToProgressionLevels(2, bonusfeat);
            mysticProgression.AddToProgressionLevels(7, bonusfeat);
            mysticProgression.AddToProgressionLevels(12, bonusfeat);
            mysticProgression.AddToProgressionLevels(17, bonusfeat);

            mysticProgression.AddToProgressionLevels(4, craft);
            
            mysticProgression.AddToProgressionLevels(5, withstand);
            
            mysticProgression.AddToProgressionLevels(9, quell);

            mysticProgression.AddToProgressionLevels(2, defense);
            mysticProgression.AddToProgressionLevels(6, defPlus);
            mysticProgression.AddToProgressionLevels(11, defPlus);
            mysticProgression.AddToProgressionLevels(16, defPlus);
            mysticProgression.AddToProgressionLevels(20, defPlus);

            mysticProgression.AddToProgressionLevels(6, enlightent);

            mysticBuild.m_Progression = mysticProgression.ToReference<BlueprintProgressionReference>();
            
            var progConfig = ProgressionConfigurator.For(mysticProgression);
            progConfig.AddToUIGroups("MysticAnimusAugments1Feature", "MysticAnimusAugments2Feature", "MysticAnimusAugments3Feature", "MysticAnimusAugments4Feature", "MysticAnimusAugments5Feature");

            progConfig.AddToUIGroups(mysticAnimusFeature, "MysticArcaneDefenseUpgradeFeature");
            progConfig.AddToUIGroups(elementalAttune, mysticGlyphs, glyphMaster);
            progConfig.AddToUIGroups(mysticAnimusFeature, fount);
            progConfig.AddToUIGroups(mysticInitatiorDefine.FullRoundRecovery.m_RestoreFeature.guid, enlightent);
            

            mysticProgression = progConfig.Configure();
             MakeBladeMeditationDeets(mysticInitatiorDefine.FullRoundRecovery.m_RestoreAction.Get());

            mysticConfig = CharacterClassConfigurator.For(mysticBuild);
            mysticConfig.SetSignatureAbilities();
            mysticConfig.SetEquipmentEntities();
            mysticConfig.CloneClassAesthestics("e8f21e5b58e0569468e420ebea456124");
            //mysticConfig.AddToSignatureAbilities(mysticAnimusFeature.guid);
            //mysticConfig.AddToSignatureAbilities(mysticGlyphs.guid);
            mysticConfig.SetDefaultBuild("6cd7d1c27f8ba6441be8f2cdb6afe908");
            mysticConfig.Configure();
            

            ConstructionAssets.Root().Progression.m_CharacterClasses = ConstructionAssets.Root().Progression.m_CharacterClasses.AppendToArray(mysticBuild.ToReference<BlueprintCharacterClassReference>());
            
            void MakeBladeMeditationDeets(BlueprintAbility mediation)
            {
                
            }

        }
    
        public static void Finish()
        {
            var fighter = BlueprintTool.Get<BlueprintFeatureSelection>("41c8486641f7d6d4283ca9dae4147a9f");
            FeatureSelectionConfigurator.For(BonusFeat).AddToAllFeatures(allFeatures: ( fighter.m_AllFeatures.Select(x=>(Blueprint<BlueprintFeatureReference>)x.guid).ToArray())).Configure();
            //TODO ADD ITEM CREATION
        }
    }
}
