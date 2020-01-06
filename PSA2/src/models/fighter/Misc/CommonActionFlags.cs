﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class CommonActionFlags
    {
        public int Offset { get; set; }
        public int ActionFlagsCount { get; set; }
        public List<ActionFlag> ActionFlags { get; set; }

        public CommonActionFlags()
        {
            ActionFlags = new List<ActionFlag>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(ActionFlagsCount)}={ActionFlagsCount.ToString("X")}, {nameof(ActionFlags)}={string.Join(",", ActionFlags.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
