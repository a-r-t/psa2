using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class ArticleCollisionData
    {
        public int Offset { get; set; }
        public int CollisionDataOffset { get; set; }
        public int EntryOffset { get; set; }
        public int DataOffset { get; set; }

        public int EntriesCount { get; set; }
        public List<ArticleCollisionDataEntry> ArticleCollisionDataEntries { get; set; }

        public ArticleCollisionData()
        {
            ArticleCollisionDataEntries = new List<ArticleCollisionDataEntry>();
        }
    }
}
