using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MultiJumpUnknown
    {
        public int Offset { get; set; }
        public List<int> Unknowns { get; set; }

        public MultiJumpUnknown()
        {
            Unknowns = new List<int>();
        }
    }
}
