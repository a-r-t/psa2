using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class MiscSection5
    {
        public int Offset { get; set; }
        public List<int> Entries { get; set; }

        public MiscSection5()
        {
            Entries = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Entries)}={string.Join(",", Entries.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
