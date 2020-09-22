using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class ArticleCollisionDataEntry
    {
        public int Offset { get; set; }
        public int Type { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int Unknown3 { get; set; }

        public ArticleCollisionDataEntry(int offset, int type, int unknown1, int unknown2, int unknown3)
        {
            Offset = offset;
            Type = type;
            Unknown1 = unknown1;
            Unknown2 = unknown2;
            Unknown3 = unknown3;
        }
    }
}
