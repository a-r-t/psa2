using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class ArticleExtraDataEntryData2Entry
    {
        public int Offset { get; set; }
        public int UnknownFlags0 { get; set; }
        public int UnknownFlags1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }
        public int Unknown4 { get; set; }
        public int Unknown5 { get; set; }
        public int Unknown6 { get; set; }
        public int Unknown7 { get; set; }

        public ArticleExtraDataEntryData2Entry(int offset, int unknownFlags0, int unknownFlags1, int unknown2, int unknown3, int unknown4, int unknown5, int unknown6, int unknown7)
        {
            Offset = offset;
            UnknownFlags0 = unknownFlags0;
            UnknownFlags1 = unknownFlags1;
            Unknown2 = unknown2;
            Unknown3 = unknown3;
            Unknown4 = unknown4;
            Unknown5 = unknown5;
            Unknown6 = unknown6;
            Unknown7 = unknown7;
        }
    }
}
