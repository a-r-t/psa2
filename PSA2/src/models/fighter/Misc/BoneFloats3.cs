using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class BoneFloats3
    {
        public int Offset { get; set; }
        public int EntriesCount { get; set; }
        public List<BoneFloatEntry> BoneFloatEntries { get; set; }

        public BoneFloats3()
        {
            BoneFloatEntries = new List<BoneFloatEntry>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(EntriesCount)}={EntriesCount.ToString()}, {nameof(BoneFloatEntries)}={string.Join(",", BoneFloatEntries.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
