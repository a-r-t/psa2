using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class HurtBoxes
    {
        public int Offset { get; set; }
        public int HurtBoxEntryCount { get; set; }
        public List<HurtBoxEntry> Entries { get; set; }

        public HurtBoxes()
        {
            Entries = new List<HurtBoxEntry>();
        }
    }
}
