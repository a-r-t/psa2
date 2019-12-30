using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class FinalSmashAura
    {
        public int Offset { get; set; }
        public int FinalSmashAuraEntryCount { get; set; }
        public List<FinalSmashAuraEntry> Entries { get; set; }

        public FinalSmashAura()
        {
            Entries = new List<FinalSmashAuraEntry>();
        }
    }
}
