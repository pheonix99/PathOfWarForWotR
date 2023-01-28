using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathOfWarForWotR.Backend.NewUnitParts
{
    class UnitPartMartialTraining : OldStyleUnitPart
    {
        public List<UnitFact> RankUpFacts = new();

        public int Rank => RankUpFacts.Count();

        public void RegisterFact(UnitFact fact)
        {
            if (!RankUpFacts.Contains(fact))
                RankUpFacts.Add(fact); 
        }

        public void UnregisterFact(UnitFact fact)
        {
            
                RankUpFacts.Remove(fact);
        }
    }
}
