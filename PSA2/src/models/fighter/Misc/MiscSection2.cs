using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MiscSection2
    {
        public List<MiscSection2Entry> Entries { get; set; }

        public MiscSection2()
        {
            Entries = new List<MiscSection2Entry>();
        }
    }
}
