using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class BoneFloatEntry
    {
        public int Offset { get; set; }
        public int Bone { get; set; }
        public int[] Data { get; set; }

        public BoneFloatEntry(int offset, int bone, int[] data)
        {
            Offset = offset;
            Bone = bone;
            Data = data;
        }
    }
}
