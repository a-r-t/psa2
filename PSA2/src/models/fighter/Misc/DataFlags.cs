using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class DataFlags
    {
        public int Offset { get; set; }
        public int ActionFlagsCount { get; set; }
        public List<int> ActionFlags { get; set; }

        public DataFlags()
        {
            ActionFlags = new List<int>();
        }
    }
}
