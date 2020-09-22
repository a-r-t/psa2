using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
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

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Bone)}={Bone.ToString("X")}, {nameof(Data)}={string.Join(",", Data.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
