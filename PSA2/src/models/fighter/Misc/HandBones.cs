using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class HandBones
    {
        public int Offset { get; set; }
        public int HandNBoneIndex0 { get; set; }
        public int HandNBoneIndex1 { get; set; }
        public int HandNBoneIndex2 { get; set; }
        public int HandNBoneIndex3 { get; set; }
        public int DataCount { get; set; }
        public int DataOffset { get; set; }
        public List<int> Bones { get; set; }
    }
}
