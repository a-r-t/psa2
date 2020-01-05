﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MiscSection1
    {
        public int Offset { get; set; }
        public List<MiscSection1Param> Params { get; set; }

        public MiscSection1()
        {
            Params = new List<MiscSection1Param>();
        }
    }
}