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
        public List<int> HopVelocities;

        public MultiJumpHops()
        {
            HopVelocities = new List<int>();
        }
    }
}
