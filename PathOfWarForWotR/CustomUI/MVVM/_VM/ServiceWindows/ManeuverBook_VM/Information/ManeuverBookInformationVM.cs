//using Kingmaker.UnitLogic;
//using Owlcat.Runtime.UI.MVVM;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UniRx;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBoom_VM;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBook_VM.Switchers;
//using TheInfiniteCrusade.Backend.NewUnitDataClasses;
//using Kingmaker.Blueprints.Root;
//using Kingmaker.EntitySystem.Stats;
//using Kingmaker.UI.MVVM._VM.ServiceWindows.CharacterInfo.Sections.Abilities;
//using Kingmaker.UI.ServiceWindow.CharacterScreen;
//using Kingmaker.UI.Common;
//using Kingmaker.Blueprints.Classes;

//namespace TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBoom_VM.Information
//{
//    public class ManeuverBookInformationVM : BaseDisposable, IViewModel, IBaseDisposable, IDisposable
//    {
//        public IReactiveProperty<UnitDescriptor> Unit;

//		public IReactiveProperty<ManeuverBook> Spellbook;

		
//        public string SpellbookName;

//		public string CasterStat;
//		public int? CasterStatValue;
		
//		public ManeuverBookInformationVM(IReactiveProperty<UnitDescriptor> unit, IReactiveProperty<ManeuverBook> spellbook)
//		{
//			this.Unit = unit;
//			this.Spellbook = spellbook;
			
//			base.AddDisposable(this.Spellbook.Subscribe(delegate (ManeuverBook _)
//			{
//				this.RefreshData();
//			}));
//			base.AddDisposable(this.Unit.Skip(1).Subscribe(delegate (UnitDescriptor _)
//			{
//				this.SetupHeaderFeatures();
//			}));
//		}

	

//        public void RefreshData()
//		{
//			if (this.Spellbook.Value == null)
//			{
//				return;
//			}
//			this.SpellbookName = this.Spellbook.Value.Blueprint.DisplayName;
			
//			this.CasterStat = LocalizedTexts.Instance.Stats.GetText(Spellbook.Value.InitiatingStat);
//			ModifiableValueAttributeStat modifiableValueAttributeStat = this.Spellbook.Value.Owner.Stats.GetStat(Spellbook.Value.InitiatingStat) as ModifiableValueAttributeStat;
//			this.CasterStatValue = ((modifiableValueAttributeStat != null) ? new int?(modifiableValueAttributeStat.PermanentValue) : null);
//			this.SetupHeaderFeatures();
//		}

//		private void SetupHeaderFeatures()
//		{
			
//		}

//		private void ClearFeatures(List<CharInfoFeatureVM> features)
//		{
//			features.ForEach(delegate (CharInfoFeatureVM f)
//			{
//				f.Dispose();
//			});
//			features.Clear();
//		}

//        public override void DisposeImplementation()
//        {
            
//        }
//    }
//}
