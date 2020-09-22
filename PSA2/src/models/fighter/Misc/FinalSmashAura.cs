using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
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

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(FinalSmashAuraEntryCount)}={FinalSmashAuraEntryCount.ToString()}, {nameof(Entries)}={string.Join(",", Entries.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
