using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MultiJumpHops
    {
        public int Offset { get; set; }
        public List<int> HopVelocities { get; set; }

        public MultiJumpHops()
        {
            HopVelocities = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(HopVelocities)}={string.Join(",", HopVelocities.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
