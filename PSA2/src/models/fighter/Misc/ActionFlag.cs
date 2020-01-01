using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class ActionFlag
    {
        public int Offset { get; set; }
        public int[] Flags { get; set; }

        public ActionFlag(int offset, int[] flags)
        {
            Offset = offset;
            Flags = flags;
        }
    }
}
