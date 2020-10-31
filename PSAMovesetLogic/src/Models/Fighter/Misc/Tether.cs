using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class Tether
    {
        public int Offset { get; set; }
        public int HangFrameCount { get; set; }
        public int Unknown { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString()}, {nameof(HangFrameCount)}={HangFrameCount.ToString()}, {nameof(Unknown)}={Unknown.ToString()}}}";
        }
    }
}
