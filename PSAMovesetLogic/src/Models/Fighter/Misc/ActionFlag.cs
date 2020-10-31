using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
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

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Flags)}={string.Join(",", Flags.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
