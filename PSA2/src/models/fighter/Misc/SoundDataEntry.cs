using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class SoundDataEntry
    {
        public int Offset { get; set; }
        public int SoundListOffset { get; set; }
        public int SoundListCount { get; set; }
        public List<int> SfxIds { get; set; }

        public SoundDataEntry(int offset, int soundListOffset, int soundListCount)
        {
            Offset = offset;
            SoundListOffset = soundListOffset;
            SoundListCount = soundListCount;
            SfxIds = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(SoundListOffset)}={SoundListOffset.ToString("X")}, {nameof(SoundListCount)}={SoundListCount.ToString("X")}, {nameof(SfxIds)}={string.Join(",", SfxIds.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
