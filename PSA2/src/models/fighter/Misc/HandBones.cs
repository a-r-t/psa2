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
        public int BonesListDataOffset { get; set; }
        public List<int> Bones { get; set; }

        public HandBones()
        {
            Bones = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(HandNBoneIndex0)}={HandNBoneIndex0.ToString("X")}, {nameof(HandNBoneIndex1)}={HandNBoneIndex1.ToString("X")}, {nameof(HandNBoneIndex2)}={HandNBoneIndex2.ToString("X")}, {nameof(HandNBoneIndex3)}={HandNBoneIndex3.ToString("X")}, {nameof(DataCount)}={DataCount.ToString("X")}, {nameof(DataOffset)}={DataOffset.ToString("X")}, {nameof(BonesListDataOffset)}={BonesListDataOffset.ToString("X")}, {nameof(Bones)}={string.Join(",", Bones.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
