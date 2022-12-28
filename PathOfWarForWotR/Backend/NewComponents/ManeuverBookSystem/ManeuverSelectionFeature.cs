namespace TheInfiniteCrusade.Backend.NewComponents.ManeuverBookSystem
{
    /*
    [AllowMultipleComponents]
    partial class ManeuverSelectionFeature : UnitFactComponentDelegate, IUnitCompleteLevelUpHandler, IGlobalSubscriber, ISubscriber
    {



      
        public BlueprintManeuverBookReference targetSpellbook;
        private ManeuverBook SpellBookToAddTo => Owner.DemandManeuverBook(targetSpellbook.Get());

        public ManeuverSelectionMode mode;

        public bool stance;




        private List<ManeuverSelectionData> spellSelections = new List<ManeuverSelectionData>();
        public BlueprintSpellListReference m_SpellList;


        public int Count = 1;

        public int AdjustedMaxLevel
        {
            get
            {
                if (mode == ManeuverSelectionMode.Standard && targetSpellbook.Get().BookType == BlueprintManeuverBook.ManeuverBookType.Level6Archetype)
                    return 6;

                else return 9;


            }
        }

        public override void OnActivate()
        {

            Owner.Ensure<UnitPartMartialDisciple>();
            Main.Context.Logger.Log($"Manuever Selector Activated From: {OwnerBlueprint.name}");
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;



            if (controller == null) { return; }

            var selectedclass = controller.State?.SelectedClass;
            



            if (SpellBookToAddTo == null)
            {

                return;
            }
            var selectionCount = controller
                .State?
                .Selections?
                .Select(s => s.SelectedItem?.Feature)
                .Where(f => f == Fact.Blueprint)
                .Count();
            int i = 0;
            if (selectionCount == 0)
                selectionCount = 1;
            if (controller.State.SelectedClass != null)
            {
                Main.Context.Logger.Log($"Class At Selection Is: {controller.State.SelectedClass.Name}");
            }
            BlueprintSpellList SpellList = ProxyList(selectedclass);

            Main.Context.Logger.Log($"Spell List is {SpellList.NameSafe()}, spells included:{SpellList.SpellsByLevel.SelectMany(x => x.m_Spells).Count()}");
            for (; i < spellSelections.Count && i < selectionCount; i++)
            {
                controller.State.ManeuverSelections().Add(spellSelections[i]);
                spellSelections[i].SetExtraManuevers(Count);
            }
            for (; i < selectionCount; i++)
            {

                if (i >= selectionCount) { continue; }
                var selection = controller.State.DemandManeuverSelection(SpellBookToAddTo.Blueprint, SpellList);
                selection.SetExtraManuevers(Count);

                spellSelections.Add(selection);
            }
            
            if (selectionCount > 0)
            {
                Main.ModContextPathOfTheCrusade.Logger.Log($"SelectionCount > 0");
                for (; i < spellSelections.Count && i < selectionCount; i++)
                {
                    controller.State.SpellSelections.Add(spellSelections[i]);
                    spellSelections[i].SetExtraSpells(Count, AdjustedMaxLevel);
                }
                for (; i < selectionCount; i++)
                {

                    if (i >= selectionCount) { continue; }
                    var selection = controller.State.DemandSpellSelection(SpellBookToAddTo.Blueprint, SpellList);
                    selection.SetExtraSpells(Count, AdjustedMaxLevel);
                    spellSelections.Add(selection);
                }
            }
            else
            {
                Main.ModContextPathOfTheCrusade.Logger.Log($"SelectionCount <= 0");
                var selection = controller.State.DemandSpellSelection(SpellBookToAddTo.Blueprint, SpellList);
                selection.SetExtraSpells(Count, AdjustedMaxLevel);
                spellSelections.Add(selection);
            }
            

        }
        public override void OnTurnOff()
        {
            if (spellSelections.Empty()) { return; }
            LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
            if (controller == null) { return; }
            if (SpellBookToAddTo == null) { return; }
            spellSelections.ForEach(selection => controller.State.ManeuverSelections().Remove(selection));
        }

        public void HandleUnitCompleteLevelup(UnitEntityData unit)
        {
            spellSelections.Clear();
        }

        private BlueprintSpellList ProxyList(BlueprintCharacterClass currentClass)
        {

            
            var bookPart = Owner.Parts.Ensure<UnitPartMartialDisciple>();
            BlueprintSpellList relevantMasterList = stance ? BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.Context, "MasterStanceList") : BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.Context, "MasterManeuverList");


            return Helpers.CreateCopy(relevantMasterList, bp =>
            {

                bp.name = $"{bp.name}Proxy";
                

                
                foreach (SpellLevelList spellLevelList in bp.SpellsByLevel)
                {
                    Main.Context.Logger.Log($"Entered spell level list loop at {spellLevelList.SpellLevel}, there are {spellLevelList.m_Spells.Count} entries");
                    int placeInLevelList = 0;
                    while (placeInLevelList < spellLevelList.m_Spells.Count)
                    {

                        var manuever = spellLevelList.m_Spells[placeInLevelList];

                        if (bookPart.CanLearnManeuver(manuever, mode, SpellBookToAddTo, currentClass))
                        {
                            placeInLevelList++;
                        }
                        else
                        {
                            spellLevelList.m_Spells.Remove(manuever);
                        }

                    
                    }



                }


            });
        }





    }*/

}
