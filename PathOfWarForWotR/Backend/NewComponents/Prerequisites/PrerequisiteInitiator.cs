﻿using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheInfiniteCrusade.Extensions;
using TheInfiniteCrusade.NewComponents.ManeuverBookSystem;

namespace TheInfiniteCrusade.Backend.NewComponents.Prerequisites
{
    class PrerequisiteInitiator : Prerequisite
    {
        public override bool CheckInternal([CanBeNull] FeatureSelectionState selectionState, [NotNull] UnitDescriptor unit, [CanBeNull] LevelUpState state)
        {
            if (IncludeMartialTraining)
                return unit.ManeuverBooks().Any();
            else
                return unit.ManeuverBooks().Any(x => !x.Blueprint.IsMartialTraining);
            
        }

        public override string GetUITextInternal(UnitDescriptor unit)
        {
            if (IncludeMartialTraining)
                return "Can Initiate Maneuvers";
            else
                return "Has Initiator Levels";
        }

        public bool IncludeMartialTraining;
    }
}
