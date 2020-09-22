using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class MiscSection2
    {
        public int Offset { get; set; }
        public int EntriesCount { get; set; }
        public List<MiscSection2Entry> Entries { get; set; }

        public MiscSection2()
        {
            Entries = new List<MiscSection2Entry>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(EntriesCount)}={EntriesCount.ToString()}, {nameof(Entries)}={string.Join(",", Entries.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
