﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class BoneFloats2
    {
        public int Offset { get; set; }
        public int EntriesCount { get; set; }
        public List<BoneFloatEntry> BoneFloatEntries { get; set; }

        public BoneFloats2()
        {
            BoneFloatEntries = new List<BoneFloatEntry>();
        }
    }
}