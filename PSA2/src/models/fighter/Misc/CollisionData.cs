using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class CollisionData
    {
        public int Offset { get; set; }
        public int CollisionDataOffset { get; set; }
        public int EntryOffset { get; set; }
        public int Count { get; set; }
        public int DataOffset { get; set; }
        public CollisionDataEntry CollisionDataEntry { get; set; }

        public CollisionData()
        {
            CollisionDataEntry = new CollisionDataEntry();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(CollisionDataOffset)}={CollisionDataOffset.ToString("X")}, {nameof(EntryOffset)}={EntryOffset.ToString("X")}, {nameof(Count)}={Count.ToString("X")}, {nameof(DataOffset)}={DataOffset.ToString("X")}, {nameof(CollisionDataEntry)}={CollisionDataEntry}}}";
        }
    }
}
