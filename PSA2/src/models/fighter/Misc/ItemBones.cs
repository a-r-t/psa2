using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class ItemBones
    {
        public int Offset { get; set; }
        public int HaveNBoneIndex0 { get; set; }
        public int HaveNBoneIndex1 { get; set; }
        public int ThrowNBoneIndex { get; set; }
        public int DataCount { get; set; }
        public int DataOffset { get; set; }
        public int Pad { get; set; }

        public int EntriesCount { get; set; }
        public List<ItemBonesEntry> Entries { get; set; }

        public ItemBones()
        {
            Entries = new List<ItemBonesEntry>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(HaveNBoneIndex0)}={HaveNBoneIndex0.ToString("X")}, {nameof(HaveNBoneIndex1)}={HaveNBoneIndex1.ToString("X")}, {nameof(ThrowNBoneIndex)}={ThrowNBoneIndex.ToString("X")}, {nameof(DataCount)}={DataCount.ToString("X")}, {nameof(DataOffset)}={DataOffset.ToString("X")}, {nameof(Pad)}={Pad.ToString("X")}, {nameof(EntriesCount)}={EntriesCount.ToString()}, {nameof(Entries)}={string.Join(",", Entries.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
