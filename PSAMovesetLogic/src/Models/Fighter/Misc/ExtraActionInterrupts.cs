using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class ExtraActionInterrupts
    {
        public int Offset { get; set; }
        public int Unknown0 { get; set; }
        public int Unknown1 { get; set; }
        public int DataOffset { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Unknown0)}={Unknown0.ToString("X")}, {nameof(Unknown1)}={Unknown1.ToString("X")}, {nameof(DataOffset)}={DataOffset.ToString("X")}}}";
        }
    }
}
