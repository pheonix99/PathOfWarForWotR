using Kingmaker.UI;
using Kingmaker.UI.MVVM._PCView.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Owlcat.Runtime.UI.Controls.Other;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PathOfWarForWotR.CustomUI.ManeuverBook
{
    class ActionBarManeuverGroupPCView : ActionBarGroupPCView
    {
		/*
		public override void BindViewImplementation()
		{
			this.SetGroup();
			base.AddDisposable(base.ViewModel.OnUnitUpdated.Subscribe(delegate (Unit _)
			{
				this.SetGroup();
			}));
			base.AddDisposable(this.m_SwitchButton.OnLeftClickAsObservable().Subscribe(delegate (Unit _)
			{
				this.SetVisible(!this.VisibleState, false);
			}));
		}
		*/
	}
}
