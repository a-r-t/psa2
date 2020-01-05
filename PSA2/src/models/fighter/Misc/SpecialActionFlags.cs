﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class SpecialActionFlags
    {
        public int Offset { get; set; }
        public int ActionFlagsCount { get; set; }
        public List<ActionFlag> ActionFlags { get; set; }

        public SpecialActionFlags()
        {
            ActionFlags = new List<ActionFlag>();
        }
    }
}