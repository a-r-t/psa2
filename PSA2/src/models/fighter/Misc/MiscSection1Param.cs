﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MiscSection1Param
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public MiscSection1Param(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}