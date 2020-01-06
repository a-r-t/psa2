using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class BoneReferences
    {
        public int Offset { get; set; }
        public int BonesCount { get; set; }
        public List<int> Bones { get; set; }

        public BoneReferences()
        {
            Bones = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(BonesCount)}={BonesCount.ToString()}, {nameof(Bones)}={string.Join(",", Bones.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
