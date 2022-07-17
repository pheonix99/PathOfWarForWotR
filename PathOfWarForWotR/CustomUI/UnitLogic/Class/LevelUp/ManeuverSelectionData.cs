using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.UI;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TheInfiniteCrusade.Backend.NewBlueprints;
using TheInfiniteCrusade.Backend.NewUnitDataClasses;
using TheInfiniteCrusade.Backend.NewUnitParts;
using TheInfiniteCrusade.Extensions;
using UnityEngine;

namespace TheInfiniteCrusade.CustomUI.UnitLogic.Class.LevelUp
{
    public class ManeuverSelectionData : IUIDataProvider
    {
        public ManeuverSelectionData(BlueprintManeuverBook maneuverBook, bool stances)
        {
            this.ManeuverBook = maneuverBook;
            this.SpellList = stances ? BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.Context, "MasterStanceList") : BlueprintTools.GetModBlueprint<BlueprintSpellList>(Main.Context, "MasterManeuverList");
            Stance = stances;
           
        }

        public void SpendSlot(BlueprintAbility spell, int slotIndex)
        {
            
            if (this.ExtraSelected != null &&  slotIndex >= 0 && slotIndex < this.ExtraSelected.Length)
            {
                this.ExtraSelected[slotIndex] = spell;
            }
        }

        public string Name
        {
            get
            {
                if (Stance)
                    return "Stance";
                else
                    return "Maneuvers";
                
            }
        }


        public string Description
        {
            get
            {
                if (Stance)
                    return "Choose Stance";
                else
                    return "Choose Maneuvers";

            }
        }


        public Sprite Icon
        {
            get
            {
                return null;
            }
        }

        public string NameForAcronym
        {
            get
            {
                return null;
            }
        }

        public bool CanSpendSlot(int slotIndex)
        {
          
            return  (this.ExtraSelected != null && slotIndex >= 0 && slotIndex < this.ExtraSelected.Length && this.ExtraSelected[slotIndex] == null);
        }

        public readonly BlueprintManeuverBook ManeuverBook;

        public readonly BlueprintSpellList SpellList;

        public readonly bool Stance;
        public BlueprintAbility[] ExtraSelected;
        

        internal void SetExtraManuevers(int count)
        {
            ExtraSelected = new BlueprintAbility[count];
        }

        internal bool CanSelectAnything(UnitDescriptor unit)
        {
            ManeuverBook spellbook = unit.ManeuverBooks().FirstOrDefault((ManeuverBook s) => s.Blueprint == this.ManeuverBook);
            if (spellbook == null)
            {
                return false;
            }
            
            if ( this.ExtraSelected.Length != 0)
            {
                if (this.ExtraSelected.HasItem((BlueprintAbility i) => i == null))
                {
                    var disciple = unit.Ensure<UnitPartMartialDisciple>();

                    return SpellList.GetAllSpells().Any(x => !disciple.KnowsManeuver(x.ToReference<BlueprintAbilityReference>()));

                   
                }
            }
            
            return false;
        }
    }
}
