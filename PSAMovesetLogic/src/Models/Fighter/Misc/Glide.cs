using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class Glide
    {
        public int Offset { get; set; }
        public List<int> Datas { get; set; }

        public Glide()
        {
            Datas = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Datas)}={string.Join(",", Datas.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
