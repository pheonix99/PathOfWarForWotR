using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheInfiniteCrusade.Utilities
{
    public static class LevelEntrySorter 
    {
        public static int SortLEs(LevelEntry x, LevelEntry y)
        {
            return x.Level.CompareTo(y.Level);
        }


    }
}
