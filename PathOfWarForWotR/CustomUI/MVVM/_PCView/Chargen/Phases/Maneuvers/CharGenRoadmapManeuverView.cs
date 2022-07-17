//using Kingmaker.UI.Common;
//using Kingmaker.UI.MVVM._VM.Tooltip.Utils;
//using Owlcat.Runtime.UI.MVVM;
//using Owlcat.Runtime.UI.Tooltips;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.Chargen.Phases.Maneuvers;
//using UnityEngine;
//using UnityEngine.UI;

//namespace TheInfiniteCrusade.CustomUI.MVVM._PCView.Chargen.Phases.Maneuvers
//{
//    internal class CharGenRoadmapManeuverView : ViewBase<CharGenManeuverSelectorItemVM>, IWidgetView
//    {
//		public override void BindViewImplementation()
//		{
//			this.SpriteIcon.gameObject.SetActive(true);
//			this.SpriteIcon.sprite = base.ViewModel.Icon;
//			base.AddDisposable(this.SetTooltip(base.ViewModel.TooltipTemplate(), new TooltipConfig(InfoCallPCMethod.None, InfoCallConsoleMethod.LongRightStickButton, false, false, null, 0, 0, 0)));
//		}

//		public override void DestroyViewImplementation()
//		{
//			this.SpriteIcon.sprite = null;
//			this.SpriteIcon.gameObject.SetActive(false);
//		}

//		public void BindWidgetVM(IViewModel vm)
//		{
//			base.Bind(vm as CharGenManeuverSelectorItemVM);
//		}
//		public bool CheckType(IViewModel viewModel)
//		{
//			return viewModel is CharGenManeuverSelectorItemVM;
//		}
//		public MonoBehaviour MonoBehaviour
//		{
//			get
//			{
//				return this;
//			}
//		}

//		[SerializeField]
//		private Image SpriteIcon;
//	}
//}