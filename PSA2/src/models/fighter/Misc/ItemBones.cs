using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
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
        public List<ItemBonesEntry> Entries { get; set; }

        public ItemBones()
        {
            Entries = new List<ItemBonesEntry>();
        }
    }
}
