//using Kingmaker.Blueprints;
//using Kingmaker.UI.MVVM._VM.InfoWindow;
//using Kingmaker.UI.MVVM._VM.Other;
//using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
//using Kingmaker.UnitLogic.Abilities.Blueprints;
//using Owlcat.Runtime.UI.SelectionGroup;
//using Owlcat.Runtime.UI.Tooltips;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheInfiniteCrusade.NewComponents.MartialAbilityInformation;
//using UnityEngine;

//namespace TheInfiniteCrusade.CustomUI.MVVM._VM.Chargen.Phases.Maneuvers
//{
//    public class CharGenManeuverSelectorItemVM : SelectionGroupEntityVM, IHasTooltipTemplate
//    {
//        public override void DoSelectMe()
//        {
            
//        }

//        public TooltipBaseTemplate TooltipTemplate()
//        {

//            return this.m_TooltipTemplate;
//        }

//        public void SetForbidenState(bool state)
//        {
//            base.SetAvailableState(!state && !this.HasInSpellbook);
//        }

//        public CharGenManeuverSelectorItemVM(BlueprintAbility maenuver, InfoSectionVM infoVM,  int recommendation, bool hasInSpellbook) : base(true)
//        {
//            this.Spell = maenuver;
//            var infocomp = maenuver.GetComponent<ManeuverInformation>();

            
//            this.m_InfoVM = infoVM;
//            this.Level = infocomp.ManeuverLevel;
//            this.DisplayName = maenuver.Name;
//            this.Icon = maenuver.Icon;
//            this.SchoolName = infocomp.GetManeuverSchoolString();
//            this.HasInSpellbook = hasInSpellbook;
//            base.AddDisposable(this.Recommendation = new RecommendationMarkerVM(recommendation));
//            base.SetAvailableState(!this.HasInSpellbook);
//            this.SetForbidenState(!this.HasInSpellbook);
//            this.m_TooltipTemplate = new TooltipTemplateAbility(maenuver);
//        }
//        private TooltipBaseTemplate m_TooltipTemplate;
//        private readonly InfoSectionVM m_InfoVM;
//        public BlueprintAbility Spell;
//        public readonly Sprite Icon;
//        public readonly string DisplayName;
//        public readonly int Level;
//        public readonly string SchoolName;
//        public readonly bool HasInSpellbook;
//        public readonly RecommendationMarkerVM Recommendation;

//        internal void UpdateRecommendation(int getRecommendationPriority)
//        {
//            this.Recommendation.ChangeRecommendation(getRecommendationPriority);
//        }
//    }
//}
