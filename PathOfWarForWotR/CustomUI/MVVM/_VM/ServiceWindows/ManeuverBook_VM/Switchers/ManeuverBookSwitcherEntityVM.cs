//using Owlcat.Runtime.UI.SelectionGroup;
//using TheInfiniteCrusade.Backend.NewUnitDataClasses;

//namespace TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBook_VM.Switchers
//{
//    public class ManeuverBookSwitcherEntityVM : SelectionGroupEntityVM
//    {
//        public ManeuverBookSwitcherEntityVM(ManeuverBook spellbook) : base(false)
//        {
//            this.ManeuverBook = spellbook;
//            this.BookName = spellbook.Blueprint.DisplayName;
//            this.BookLevel = spellbook.GetRealInitiatorLevel();
//        }

//        public ManeuverBook ManeuverBook { get; internal set; }

//        public readonly string BookName;

//        // Token: 0x04002B8F RID: 11151
//        public readonly int BookLevel;

//        public override void DoSelectMe()
//        {
           
//        }
//    }
//}