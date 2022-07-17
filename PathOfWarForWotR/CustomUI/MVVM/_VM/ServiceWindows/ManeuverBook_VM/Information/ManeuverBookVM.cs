//using Kingmaker.PubSubSystem;
//using Kingmaker.UI.MVVM._VM.InfoWindow;
//using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.LevelClassScores.Experience;
//using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.NameAndPortrait;
//using Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook;
//using Kingmaker.UnitLogic;
//using Owlcat.Runtime.UI.MVVM;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheInfiniteCrusade.Backend.NewUnitDataClasses;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBook_VM.Switchers;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBoom_VM.Information;
//using UniRx;

//namespace TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBoom_VM
//{
//    public class ManeuverBookVM : BaseDisposable, IViewModel, IBaseDisposable, IDisposable, IManeuverBookHandler, IGlobalSubscriber, ISubscriber
//    {

//        public IReactiveProperty<ManeuverBook> CurrentManeuverBook = new ReactiveProperty<ManeuverBook>();
//        public IReactiveProperty<UnitDescriptor> UnitDescriptor;
//        public CharInfoNameAndPortraitVM NameAndPortraitVM;
//        public CharInfoExperienceVM ExperienceVM;
//        public InfoSectionVM InfoSectionVM;

//        public ManeuverBookVM(IReactiveProperty<UnitDescriptor> unitDescriptor)
//        {
//            this.UnitDescriptor = unitDescriptor;
//            base.AddDisposable(EventBus.Subscribe(this));
//            base.AddDisposable(this.CurrentManeuverBook.Subscribe(new Action<ManeuverBook>(this.OnManeuverBookSelected)));
//            base.AddDisposable(this.NameAndPortraitVM = new CharInfoNameAndPortraitVM(this.UnitDescriptor));
//            base.AddDisposable(this.ExperienceVM = new CharInfoExperienceVM(this.UnitDescriptor));
//            base.AddDisposable(this.ManeuverBookSwitcherVM = new ManeuverBookSwitcherVM(this.UnitDescriptor, this.CurrentManeuverBook));
           

//            base.AddDisposable(this.InfoSectionVM = new InfoSectionVM());
//        }

//        public ManeuverBookSwitcherVM ManeuverBookSwitcherVM;

//        private void OnManeuverBookSelected(ManeuverBook spellbook)
//        {
//            EventBus.RaiseEvent<IUIEventHandler>(delegate (IUIEventHandler h)
//            {
//                ManeuverBook spellbook2 = spellbook;
//                h.HandleUIEvent(UIEventType.SpellbookOpen);
//            }, true);
//        }



//        internal void Refresh()
//        {
//            Main.Context.Logger.Log($"Maunever Book VM Refresh");
//        }

//        public override void DisposeImplementation()
//        {
           
//        }
//    }
//}
