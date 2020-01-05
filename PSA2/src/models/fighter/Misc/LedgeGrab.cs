using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class LedgeGrab
    {
        public int Offset { get; set; }
        public int LedgeGrabeEntriesCount { get; set; }
        public List<LedgeGrabEntry> Entries { get; set; }

        public LedgeGrab()
        {
            Entries = new List<LedgeGrabEntry>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(LedgeGrabeEntriesCount)}={LedgeGrabeEntriesCount.ToString()}, {nameof(Entries)}={string.Join(",", Entries.Select(x => x.ToString()).ToList())}}}}}";
        }
    }
}
