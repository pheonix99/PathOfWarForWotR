//using Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook.Switchers;
//using Kingmaker.UnitLogic;
//using Kingmaker.Utility;
//using Owlcat.Runtime.UI.MVVM;
//using Owlcat.Runtime.UI.SelectionGroup;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheInfiniteCrusade.Backend.NewUnitDataClasses;
//using TheInfiniteCrusade.Extensions;
//using UniRx;

//namespace TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBook_VM.Switchers
//{
//    public class ManeuverBookSwitcherVM : BaseDisposable, IViewModel, IBaseDisposable, IDisposable
//    {
//		public ManeuverBookSwitcherVM(IReactiveProperty<UnitDescriptor> unitDescriptor, IReactiveProperty<ManeuverBook> currentSpellbook)
//		{
//			this.UnitDescriptor = unitDescriptor;
//			this.m_CurrentManeuverBook = currentSpellbook;
//			base.AddDisposable(unitDescriptor.Subscribe(new Action<UnitDescriptor>(this.RefreshData)));
//			base.AddDisposable(this.m_SelectedEntity.Subscribe(delegate (ManeuverBookSwitcherEntityVM e)
//			{
//				this.m_CurrentManeuverBook.Value = ((e != null) ? e.ManeuverBook : null);
//			}));
//		}
//		private void RefreshData(UnitDescriptor unit)
//		{
//			this.CreateEntities(unit);
//			base.AddDisposable(this.SelectionGroup = new SelectionGroupRadioVM<ManeuverBookSwitcherEntityVM>(this.m_EntitiesList, this.m_SelectedEntity));
//		}

//		private void CreateEntities(UnitDescriptor unit)
//		{
//			this.m_EntitiesList.Clear();
//			foreach (ManeuverBook spellbook in unit.ManeuverBooks())
//			{
//				spellbook.DemandSlotsUpdate();
//				ManeuverBookSwitcherEntityVM spellbookSwitcherEntityVM = new ManeuverBookSwitcherEntityVM(spellbook);
//				base.AddDisposable(spellbookSwitcherEntityVM);
//				this.m_EntitiesList.Add(spellbookSwitcherEntityVM);
//			}
//			this.m_SelectedEntity.Value = this.m_EntitiesList.FirstOrDefault<ManeuverBookSwitcherEntityVM>();
//			this.HasSpellbooks.Value = this.m_EntitiesList.Any<ManeuverBookSwitcherEntityVM>();
//		}

//        public override void DisposeImplementation()
//        {
          
//        }

//        private List<ManeuverBookSwitcherEntityVM> m_EntitiesList = new List<ManeuverBookSwitcherEntityVM>();

//		private ReactiveProperty<ManeuverBookSwitcherEntityVM> m_SelectedEntity = new ReactiveProperty<ManeuverBookSwitcherEntityVM>();

//		private readonly IReactiveProperty<ManeuverBook> m_CurrentManeuverBook;

//		public IReactiveProperty<UnitDescriptor> UnitDescriptor;

//		public SelectionGroupRadioVM<ManeuverBookSwitcherEntityVM> SelectionGroup;

//		public BoolReactiveProperty HasSpellbooks = new BoolReactiveProperty();
//	}
//}
