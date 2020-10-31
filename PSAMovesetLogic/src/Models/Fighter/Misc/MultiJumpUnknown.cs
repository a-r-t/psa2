using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class MultiJumpUnknown
    {
        public int Offset { get; set; }
        public List<int> Unknowns { get; set; }

        public MultiJumpUnknown()
        {
            Unknowns = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Unknowns)}={string.Join(",", Unknowns.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
