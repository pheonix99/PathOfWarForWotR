//using JetBrains.Annotations;
//using Kingmaker.UI.Common;
//using Kingmaker.UI.MVVM._PCView.CharGen.Phases;
//using Kingmaker.UnitLogic.Abilities.Blueprints;
//using Owlcat.Runtime.UniRx;
//using UniRx;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.Chargen.Phases.Maneuvers;
//using UnityEngine;
//using UnityEngine.UI;
//using Newtonsoft.Json;
//using Owlcat.Runtime.UI.Controls.Button;
//using Owlcat.Runtime.UI.Controls.Selectable;
//using TMPro;

//namespace TheInfiniteCrusade.CustomUI.MVVM._PCView.Chargen.Phases.Maneuvers
//{
//    public class CharGenManeuversPhaseRoadmapPCView : CharGenPhaseRoadmapBaseView<CharGenManeuversPhaseVM>
//    {
		
		

//		public void actuallyBuild()
//        {
//			var object1 = new GameObject("RoadMapItemBackground", typeof(RectTransform));
//			object1.transform.SetParent(this.gameObject.transform);
//			base.m_Button = object1.AddComponent<OwlcatMultiButton>();

//			var object2 = new GameObject("RoadmapButtonView", typeof(RectTransform));
//			object2.transform.SetParent(object1.transform);
//			base.m_ButtonBackground = object2.AddComponent<OwlcatMultiButton>();


//			var object3 = new GameObject("RoadmapButtonLabelView", typeof(RectTransform));
//			object3.transform.SetParent(object1.transform);
//			base.m_ButtonLabel = object3.AddComponent<OwlcatMultiSelectable>();

//			var object4 = new GameObject("LabelPlace", typeof(RectTransform));
//			object4.transform.SetParent(object3.transform);

//			var object5 = new GameObject("PhaseName", typeof(RectTransform));
//			object5.transform.SetParent(object4.transform);
//			base.m_Label = object5.AddComponent<TextMeshProUGUI>();
//			base.m_LabelLayoutElement = object5.AddComponent<LayoutElement>();
//			//m_ButtonBackground goes to RoadmapButtonView
//			//m_button goes to Console_RoadMapItemBackground
//			//m_ButtonLabel goes to RoadmapButtonLabelView

//			//m_Label and m_LabelLayoutElement go to phaseName

//			try
//			{
//				base.m_Button = this.gameObject.AddComponent<OwlcatMultiButton>();
//			}
//			catch(Exception e)
//            {
//				Main.Context.Logger.Log($"Aspslosion trying to add m_button {e}");
//            }
//		}

//		public override void BindViewImplementation()
//		{
//			base.BindViewImplementation();
//			if (this.m_Label != null)
//			{
//				this.m_Label.text = "Moves!";
//			}
//			base.AddDisposable(base.ViewModel.SelectedManeuverVMs.ObserveCountChanged(true).ObserveLastValueOnLateUpdate<int>().Subscribe(delegate (int _)
//			{
//				this.DrawEntities();
//			}));
//		}

//		[SerializeField]
//		[UsedImplicitly]
//		private WidgetListMVVM m_WidgetList;

//		// Token: 0x0400444A RID: 17482
//		[SerializeField]
//		[UsedImplicitly]
//		private CharGenRoadmapManeuverView m_WidgetEntityView;

//		// Token: 0x0400444B RID: 17483
//		[SerializeField]
//		[UsedImplicitly]
//		private GridLayoutGroup m_GridLayout;

//		private void DrawEntities()
//		{
//			if (m_GridLayout == null)
//            {
//				Main.Context.Logger.Log($"CharGenManeuversPhaseRoadmapPCView.m_GridLayout is null");
//            }
//			if (m_WidgetEntityView == null)
//			{
//				Main.Context.Logger.Log($"CharGenManeuversPhaseRoadmapPCView.m_WidgetEntityView is null");
//			}
//			if (m_WidgetList == null)
//			{
//				Main.Context.Logger.Log($"CharGenManeuversPhaseRoadmapPCView.m_WidgetList is null");
//			}
//			if (base.ViewModel.MechanicSelectedManeuvers.Length <= 4)
//			{
//				this.m_GridLayout.constraintCount = 1;
//				this.m_GridLayout.cellSize = new Vector2(48f, 48f);
//			}
//			else
//			{
//				this.m_GridLayout.constraintCount = 2;
//				this.m_GridLayout.cellSize = new Vector2(24f, 24f);
//			}
//			this.m_WidgetList.DrawEntries<CharGenRoadmapManeuverView>(base.ViewModel.SelectedManeuverVMs, this.m_WidgetEntityView);
//			this.m_WidgetList.DrawEmptyEntities<CharGenRoadmapManeuverView>(this.m_WidgetEntityView, base.ViewModel.MechanicSelectedManeuvers.Count((BlueprintAbility sp) => sp == null));
//			this.UpdateSelectableState();
//		}

//	}
//}
