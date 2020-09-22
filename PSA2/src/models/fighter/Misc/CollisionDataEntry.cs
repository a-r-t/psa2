using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class CollisionDataEntry
    {
        public int Offset { get; set; }
        public int Type { get; set; }
        public int BoneDataOffset { get; set; }
        public int Count { get; set; }
        public List<int> Unknowns { get; set; }
        public int BonesListOffset { get; set; }
        public List<int> Bones { get; set; }

        public CollisionDataEntry()
        {
            Unknowns = new List<int>();
            Bones = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Type)}={Type.ToString("X")}, {nameof(BoneDataOffset)}={BoneDataOffset.ToString("X")}, {nameof(Count)}={Count.ToString("X")}, {nameof(Unknowns)}={string.Join(",", Unknowns.Select(x => x.ToString("X")).ToList())}, {nameof(BonesListOffset)}={BonesListOffset.ToString("X")}, {nameof(Bones)}={string.Join(",", Bones.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
