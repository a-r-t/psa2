using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class SoundLists
    {
        public int Offset { get; set; }
        public int SoundListOffset { get; set; }
        public int SoundListCount { get; set; }
        public List<SoundDataEntry> Entries { get; set; }

        public SoundLists()
        {
            Entries = new List<SoundDataEntry>();
        }
    }
}
