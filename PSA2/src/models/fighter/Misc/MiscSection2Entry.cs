using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MiscSection2Entry
    {
        public int Offset { get; set; }
        public int Unknown0 { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }
        public int Unknown5 { get; set; }
        public int Unknown6 { get; set; }
        public int Unknown7 { get; set; }

        public MiscSection2Entry(int offset, int unknown0, int unknown1, int unknown2, int unknown3, int unknown4, int unknown5, int unknown6, int unknown7)
        {
            Offset = offset;
            Unknown0 = unknown0;
            Unknown1 = unknown1;
            Unknown2 = unknown2;
            Unknown3 = unknown3;
            Unknown4 = unknown4;
            Unknown5 = unknown5;
            Unknown6 = unknown6;
            Unknown7 = unknown7;
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Unknown0)}={Unknown0.ToString("X")}, {nameof(Unknown1)}={Unknown1.ToString("X")}, {nameof(Unknown2)}={Unknown2.ToString("X")}, {nameof(Unknown3)}={Unknown3.ToString("X")}, {nameof(Unknown4)}={Unknown4.ToString("X")}, {nameof(Unknown5)}={Unknown5.ToString("X")}, {nameof(Unknown6)}={Unknown6.ToString("X")}, {nameof(Unknown7)}={Unknown7.ToString("X")}}}";
        }
    }
}
