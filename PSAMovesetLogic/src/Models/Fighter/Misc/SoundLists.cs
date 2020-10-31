using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
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

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(SoundListOffset)}={SoundListOffset.ToString("X")}, {nameof(SoundListCount)}={SoundListCount.ToString("X")}, {nameof(Entries)}={string.Join(",", Entries.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
