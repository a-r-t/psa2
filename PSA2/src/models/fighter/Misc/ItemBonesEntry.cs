using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class ItemBonesEntry
    {
        public int Offset { get; set; }
        public int Unknown0 { get; set; }
        public int Unknown1 { get; set; }
        public int Pad0 { get; set; }
        public int Pad1 { get; set; }

        public ItemBonesEntry(int offset, int unknown0, int unknown1, int pad0, int pad1)
        {
            Offset = offset;
            Unknown0 = unknown0;
            Unknown1 = unknown1;
            Pad0 = pad0;
            Pad1 = pad1;
        }
    }
}
