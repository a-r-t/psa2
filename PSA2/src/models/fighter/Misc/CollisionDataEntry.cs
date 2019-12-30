using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class CollisionDataEntry
    {
        public int Offset { get; set; }
        public int Type { get; set; }
        public int BoneDataOffset { get; set; }
        public int Count { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }
        public int Unknown5 { get; set; }
        public int BonesListOffset { get; set; }
        public List<int> Bones { get; set; }

        public CollisionDataEntry(int offset, int type, int boneDataOffset, int count, int unknown3, int unknown4, int unknown5, int bonesListOffset, List<int> bones)
        {
            Offset = offset;
            Type = type;
            BoneDataOffset = boneDataOffset;
            Count = count;
            Unknown3 = unknown3;
            Unknown4 = unknown4;
            Unknown5 = unknown5;
            BonesListOffset = bonesListOffset;
            Bones = new List<int>();
        }
    }
}
