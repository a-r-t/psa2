using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MiscSection12
    {
        public int Offset { get; set; }
        public int DataOffset { get; set; }
        public int DataCount { get; set; }
        public int ItemsListOffset { get; set; }
        public List<int> Items { get; set; }

        public MiscSection12()
        {
            Items = new List<int>();
        }
    }
}
