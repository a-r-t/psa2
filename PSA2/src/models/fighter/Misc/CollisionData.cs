using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class CollisionData
    {
        public int Offset { get; set; }
        public int CollisionDataOffset { get; set; }
        public int EntryOffset { get; set; }
        public int Count { get; set; }
        public int DataOffset { get; set; }
        public List<CollisionDataEntry> Entries { get; set; }

        public CollisionData()
        {
            Entries = new List<CollisionDataEntry>();
        }
    }
}
