using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class Unknown24
    {
        public int Offset { get; set; }
        public int DataOffset { get; set; }
        public int DataCount { get; set; }
        public int BonesListOffset { get; set; }
        public List<int> Bones { get; set; }

        public Unknown24()
        {
            Bones = new List<int>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(DataOffset)}={DataOffset.ToString("X")}, {nameof(DataCount)}={DataCount.ToString("X")}, {nameof(BonesListOffset)}={BonesListOffset.ToString("X")}, {nameof(Bones)}={string.Join(",", Bones.Select(x => x.ToString("X")).ToList())}}}";
        }
    }
}
