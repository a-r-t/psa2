using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class Unknown24
    {
        public int Offset { get; set; }
        public int DataOffset { get; set; }
        public int DataCount { get; set; }
        public List<int> Bones { get; set; }

        public Unknown24()
        {
            Bones = new List<int>();
        }
    }
}
