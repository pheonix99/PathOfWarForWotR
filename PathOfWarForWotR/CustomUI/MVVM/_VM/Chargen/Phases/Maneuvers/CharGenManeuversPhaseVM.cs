//using JetBrains.Annotations;
//using Kingmaker.Blueprints;
//using Kingmaker.Blueprints.Classes;
//using Kingmaker.Blueprints.Classes.Spells;
//using Kingmaker.Blueprints.Root.Strings;
//using Kingmaker.EntitySystem.Entities;
//using Kingmaker.UI.Common;
//using Kingmaker.UI.MVVM._VM.CharGen.Phases;
//using Kingmaker.UI.MVVM._VM.InfoWindow;
//using Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook;
//using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
//using Kingmaker.UnitLogic;
//using Kingmaker.UnitLogic.Abilities.Blueprints;
//using Kingmaker.UnitLogic.Class.LevelUp;
//using Kingmaker.Utility;
//using Owlcat.Runtime.Core.Logging;
//using Owlcat.Runtime.UI.SelectionGroup;
//using Owlcat.Runtime.UI.Tooltips;
//using Owlcat.Runtime.UniRx;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TheInfiniteCrusade.Backend.NewUnitDataClasses;
//using TheInfiniteCrusade.Backend.NewUnitParts;
//using TheInfiniteCrusade.CustomUI.MVVM._VM.ServiceWindows.ManeuverBoom_VM;
//using TheInfiniteCrusade.CustomUI.UnitLogic.Class.LevelUp;
//using TheInfiniteCrusade.Extensions;
//using UniRx;

//namespace TheInfiniteCrusade.CustomUI.MVVM._VM.Chargen.Phases.Maneuvers
//{
//    public class CharGenManeuversPhaseVM : CharGenPhaseBaseVM
//    {
//        public override int OrderPriority
//        {
//            get
//            {
//                return base.GetBaseOrderPriority(CharGenPhaseBaseVM.ChargenPhasePriority.Spells);
//            }
//        }

//        public CharGenManeuversPhaseVM([NotNull] LevelUpController levelUpController, [NotNull] ManeuverSelectionData selectionData) : base(levelUpController)
//        {
//            SetPhaseName("Maneuvers");//TODO redirct to localized

//            this.m_SelectionData = selectionData;

//            this.OriginalManeuverBook = levelUpController.Unit.Descriptor.GetManeuverBook(selectionData.ManeuverBook);


//            base.AddDisposable(levelUpController.GetReactiveProperty((LevelUpController c) => c.Preview, true).Subscribe(delegate (UnitEntityData value)
//            {
//                this.m_UnitDescriptor.Value = levelUpController.Preview.Descriptor;
//                this.PreviewManeuverBook = levelUpController.Preview.Descriptor.GetManeuverBook(selectionData.ManeuverBook);
//                if (this.ManeuverBookVM != null)

//                {

//                    this.ManeuverBookVM.CurrentManeuverBook.Value = this.PreviewManeuverBook;
//                }
//            }));
//            this.ManeuverList = selectionData.SpellList;

//            base.AddDisposable(this.InfoVM = new InfoSectionVM());
//            this.SelectionDataChanged(this.m_SelectionData);

//            base.AddDisposable(this.SelectedManeuverVMs.ObserveCountChanged(true).ObserveLastValueOnLateUpdate<int>().Subscribe(new Action<int>(this.OnChangedSelectedSpellsCount)));
//            base.AddDisposable(this.SelectedManeuverVMs.ObserveAdd().Subscribe(new Action<CollectionAddEvent<CharGenManeuverSelectorItemVM>>(this.OnManeuverChosen)));
//            base.AddDisposable(this.SelectedManeuverVMs.ObserveRemove().Subscribe(new Action<CollectionRemoveEvent<CharGenManeuverSelectorItemVM>>(this.OnManeuverRemoved)));
//            IObservable<CharGenManeuverSelectorItemVM> item = this.ManeuversSelector.SelectManyOrDefault((SelectionGroupCheckVM<CharGenManeuverSelectorItemVM> selector) => selector.LastChangedEntity);
//            base.AddDisposable(this.SelectedIsChosen = this.SelectedManeuverVMs.ObserveContains(item).ToReadOnlyReactiveProperty<bool>());
//            UILog.VMCreated("CharGenFeatureSelectorVM");
//        }

//        private void CreateManeuverBookVM()
//        {
//            base.AddDisposable(this.ManeuverBookVM = new ManeuverBookVM(this.m_UnitDescriptor));
//            this.ManeuverBookVM.CurrentManeuverBook.Value = this.PreviewManeuverBook;
//        }


//        public override void OnBeginDetailedView()
//        {
//            this.SelectionDataChanged(this.m_SelectionData);
//            if (!this.m_SpellListIsCreated)
//            {
//                this.CreateManeuverBookVM();
//                this.SelectedManeuverVMs.Clear();
//                List<CharGenManeuverSelectorItemVM> visibleCollection = this.CreateManeuverVMsCollection(base.LevelUpController, this.m_SelectionData.SpellList);

//                base.AddDisposable(this.ManeuversSelector.Value = new SelectionGroupCheckVM<CharGenManeuverSelectorItemVM>(visibleCollection, this.SelectedManeuverVMs));
//                base.AddDisposable(this.IsAllSpellsSelected.Subscribe(delegate (bool state)
//                {
//                    this.ManeuversSelector.Value.EntitiesCollection.ForEach(delegate (CharGenManeuverSelectorItemVM spellVM)
//                    {
//                        spellVM.SetForbidenState(state && !this.MechanicSelectedManeuvers.Contains(spellVM.Spell));
//                    });
//                }));

//                this.MechanicSelectedManeuversChanged(this.MechanicSelectedManeuvers);
//                this.m_SpellListIsCreated = true;

//            }

//            this.m_UnitDescriptor.Value = base.LevelUpController.Preview.Descriptor;
//            this.TryShowTooltip();
//        }

//        public void TryShowTooltip()
//        {
//            if (this.SelectedManeuverVMs.Any<CharGenManeuverSelectorItemVM>())
//            {
//                InfoSectionVM infoVM = this.InfoVM;
//                CharGenManeuverSelectorItemVM charGenSpellSelectorItemVM = this.SelectedManeuverVMs.FirstOrDefault<CharGenManeuverSelectorItemVM>();
//                infoVM.SetTemplate((charGenSpellSelectorItemVM != null) ? charGenSpellSelectorItemVM.TooltipTemplate() : null);
//                return;
//            }
//            SelectionGroupCheckVM<CharGenManeuverSelectorItemVM> value = this.ManeuversSelector.Value;
//            if (value != null && value.EntitiesCollection.Any<CharGenManeuverSelectorItemVM>())
//            {
//                InfoSectionVM infoVM2 = this.InfoVM;
//                SelectionGroupCheckVM<CharGenManeuverSelectorItemVM> value2 = this.ManeuversSelector.Value;
//                TooltipBaseTemplate template;
//                if (value2 == null)
//                {
//                    template = null;
//                }
//                else
//                {
//                    CharGenManeuverSelectorItemVM charGenSpellSelectorItemVM2 = value2.EntitiesCollection.FirstOrDefault<CharGenManeuverSelectorItemVM>();
//                    template = ((charGenSpellSelectorItemVM2 != null) ? charGenSpellSelectorItemVM2.TooltipTemplate() : null);
//                }
//                infoVM2.SetTemplate(template);
//            }
//        }

//        private List<CharGenManeuverSelectorItemVM> CreateManeuverVMsCollection(LevelUpController levelUpController, BlueprintSpellList spellSelectionSpellList)
//        {
//            this.m_ManeuverVMsCollection = new List<CharGenManeuverSelectorItemVM>();
//            if (spellSelectionSpellList == null)
//            {
//                return this.m_ManeuverVMsCollection;
//            }

//            using (List<BlueprintAbility>.Enumerator enumerator = spellSelectionSpellList.GetAllSpells().ToList<BlueprintAbility>().GetEnumerator())
//            {
//                while (enumerator.MoveNext())
//                {
//                    BlueprintAbility spell = enumerator.Current;
//                    if (spell == null)
//                    {
//                        UberDebug.LogError(string.Format("{0} has empty spell", spellSelectionSpellList), Array.Empty<object>());
//                    }
//                    else
//                    {
//                        CharGenManeuverSelectorItemVM charGenSpellSelectorItemVM = this.m_ManeuverVMsCollection.FirstOrDefault((CharGenManeuverSelectorItemVM spellVM) => spellVM.Spell == spell);

//                        int recommendationPriority = spell.GetRecommendationPriority(levelUpController.State);
//                        bool hasInSpellbook = this.PreviewManeuverBook != null && (this.PreviewManeuverBook.Knows(spell.ToReference<BlueprintAbilityReference>()) || levelUpController.Unit.Ensure<UnitPartMartialDisciple>().KnowsManeuver(spell.ToReference<BlueprintAbilityReference>())) && !this.MechanicSelectedManeuvers.Contains(spell);//TODO make sure this works properly when mutlicalssing
//                        this.m_ManeuverVMsCollection.Add(new CharGenManeuverSelectorItemVM(spell, this.InfoVM, recommendationPriority, hasInSpellbook));

//                    }
//                }

//            }
//            return this.m_ManeuverVMsCollection;

//        }

//        private void UpdateSelection([NotNull] ManeuverSelectionData selectionData)
//        {
//            this.m_SelectionData = selectionData;
//            this.SelectionDataChanged(this.m_SelectionData);
//        }

//        public void SelectionDataChanged(ManeuverSelectionData data)
//        {

//            this.MechanicSelectedManeuvers = data.ExtraSelected;

//            this.MechanicSelectedManeuversChanged(this.MechanicSelectedManeuvers);
//        }

//        public void MechanicSelectedManeuversChanged(BlueprintAbility[] selectedSpells)
//        {
//            if (this.MechanicSelectedManeuvers == null)
//            {
//                return;
//            }
//            if (this.m_ManeuverVMsCollection == null)
//            {
//                this.CreateManeuverVMsCollection(base.LevelUpController, this.m_SelectionData.SpellList);
//            }
//            List<CharGenManeuverSelectorItemVM> list = (from vm in this.m_ManeuverVMsCollection
//                                                        where selectedSpells.Contains(vm.Spell) && !this.SelectedManeuverVMs.Contains(vm)
//                                                        select vm).ToList<CharGenManeuverSelectorItemVM>();
//            foreach (CharGenManeuverSelectorItemVM item in (from vm in this.SelectedManeuverVMs
//                                                            where !selectedSpells.Contains(vm.Spell)
//                                                            select vm).ToList<CharGenManeuverSelectorItemVM>())
//            {
//                this.SelectedManeuverVMs.Remove(item);
//            }
//            foreach (CharGenManeuverSelectorItemVM item2 in list)
//            {
//                this.SelectedManeuverVMs.Add(item2);
//            }
//            foreach (CharGenManeuverSelectorItemVM charGenSpellSelectorItemVM in this.m_ManeuverVMsCollection)
//            {
//                charGenSpellSelectorItemVM.UpdateRecommendation(charGenSpellSelectorItemVM.Spell.GetRecommendationPriority(base.LevelUpController.State));
//            }
//        }
//        private void OnManeuverRemoved(CollectionRemoveEvent<CharGenManeuverSelectorItemVM> spellItem)
//        {
//            int slotIndex = this.MechanicSelectedManeuvers.IndexOf(spellItem.Value.Spell);

//            base.LevelUpController.UnselectManeuver(this.PreviewManeuverBook.Blueprint, this.ManeuverList, slotIndex);

//            this.PreviewManeuverBook = base.LevelUpController.Preview.Descriptor.GetManeuverBook(this.m_SelectionData.ManeuverBook);
//            this.m_UnitDescriptor.Value = base.LevelUpController.Preview.Descriptor;
//            this.ManeuverBookVM.Refresh();
//        }

//        // Token: 0x06004DFB RID: 19963 RVA: 0x0013BA9C File Offset: 0x00139C9C
//        private void OnManeuverChosen(CollectionAddEvent<CharGenManeuverSelectorItemVM> spellItem)
//        {

//            CharGenManeuverSelectorItemVM itemVM = spellItem.Value;
//            if (this.MechanicSelectedManeuvers.Any((BlueprintAbility spell) => itemVM.Spell == spell))
//            {
//                return;
//            }
//            int num = this.MechanicSelectedManeuvers.FindIndex((BlueprintAbility spell) => spell == null);
//            if (num < 0)
//            {
//                return;
//            }
//            base.LevelUpController.SelectManeuver(this.PreviewManeuverBook.Blueprint, this.ManeuverList, spellItem.Value.Spell, num);
//            this.ManeuverBookVM.Refresh();
//        }
//        private void OnChangedSelectedSpellsCount(int count)
//        {
//            this.IsAllSpellsSelected.Value = (count >= this.MechanicSelectedManeuvers.Length);
//            this.AvailableSpellCount.Value = this.MechanicSelectedManeuvers.Count((BlueprintAbility s) => s == null);
//        }

//        public bool TryUpdateSamePhase(ManeuverSelectionData selectionData)
//        {
//            if (this.m_SelectionData == null)
//            {
//                return false;
//            }
//            if (selectionData == null)
//            {
//                return false;
//            }

//            this.UpdateSelection(selectionData);


//            this.MechanicSelectedManeuversChanged(this.MechanicSelectedManeuvers);
//            return true;
//        }

//        public override void DisposeImplementation()
//        {
//            SelectionGroupCheckVM<CharGenManeuverSelectorItemVM> value = this.ManeuversSelector.Value;
//            if (value != null)
//            {
//                value.Dispose();
//            }
//            UILog.VMDisposed("CharGenFeatureSelectorVM");
//        }
//        public static bool SelectionStateIsCompleted(UnitDescriptor unit, ManeuverSelectionData data)
//        {
            
//            return data.ExtraSelected.All((BlueprintAbility spell) => spell != null) || !data.CanSelectAnything(unit);

//        }

//        public static bool SelectionStateHasSelections(ManeuverSelectionData data)
//        {
            
               
//                    return  data.ExtraSelected.Any<BlueprintAbility>();
             
//        }

//        public override TooltipBaseTemplate NotCompletedReasonTooltip
//        {
//            get
//            {
//                if (!this.IsCompletedAndAvailible.Value)
//                {
//                    return new TooltipTemplateSimple(UIStrings.Instance.CharGen.SpellsNotSelected, null);
//                }
//                return null;
//            }
//        }



//        public BlueprintSpellList ManeuverList;
//        public override bool CheckIsCompleted()
//        {
//            return (CharGenManeuversPhaseVM.SelectionStateIsCompleted(base.LevelUpController.Preview, this.m_SelectionData) && base.IsInDetailedView.Value) || base.LevelUpController.IsAutoLevelup;
//        }


//        public IntReactiveProperty AvailableSpellCount = new IntReactiveProperty();
//        public ManeuverBookVM ManeuverBookVM;
//        private bool m_SpellListIsCreated;
//        private ManeuverBook OriginalManeuverBook;
//        public BlueprintAbility[] MechanicSelectedManeuvers;
//        private ManeuverBook PreviewManeuverBook;
//        public readonly ReadOnlyReactiveProperty<bool> SelectedIsChosen;
//        public BoolReactiveProperty IsAllSpellsSelected = new BoolReactiveProperty(false);
//        public ReactiveCollection<CharGenManeuverSelectorItemVM> SelectedManeuverVMs = new ReactiveCollection<CharGenManeuverSelectorItemVM>();
//        private readonly ReactiveProperty<UnitDescriptor> m_UnitDescriptor = new ReactiveProperty<UnitDescriptor>();
//        public ReadOnlyReactiveProperty<bool> IsCompletedAndAvailible;
//        public ReactiveProperty<SelectionGroupCheckVM<CharGenManeuverSelectorItemVM>> ManeuversSelector = new ReactiveProperty<SelectionGroupCheckVM<CharGenManeuverSelectorItemVM>>();
//        private ManeuverSelectionData m_SelectionData;
//        private List<CharGenManeuverSelectorItemVM> m_ManeuverVMsCollection;
//        public InfoSectionVM InfoVM;
//    }
//}
