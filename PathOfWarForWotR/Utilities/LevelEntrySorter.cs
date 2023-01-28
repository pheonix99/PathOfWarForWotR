﻿using Kingmaker.Blueprints.Classes;

namespace PathOfWarForWotR.Utilities
{
    public static class LevelEntrySorter 
    {
        public static int SortLEs(LevelEntry x, LevelEntry y)
        {
            return x.Level.CompareTo(y.Level);
        }


    }
}
