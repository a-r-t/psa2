using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class MiscSection1
    {
        public int Offset { get; set; }
        public List<MiscSection1Param> Params { get; set; }

        public MiscSection1()
        {
            Params = new List<MiscSection1Param>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Params)}={string.Join(",", Params.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
