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

        public SoundDataEntry(int offset, int soundListOffset)
        {
            Offset = offset;
            SoundListOffset = soundListOffset;
            SfxIds = new List<int>();
        }
    }
}
