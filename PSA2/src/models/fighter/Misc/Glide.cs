using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class Glide
    {
        public int Offset { get; set; }
        public List<int> Datas { get; set; }

        public Glide()
        {
            Datas = new List<int>();
        }
    }
}
