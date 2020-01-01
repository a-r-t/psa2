using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class ArticleExtraDataEntryData2
    {
        public int Offset { get; set; }
        public int Count { get; set; }
        public List<ArticleExtraDataEntryData2Entry> ArticleExtraDataEntryData2Entries { get; set; }

        public ArticleExtraDataEntryData2()
        {
            ArticleExtraDataEntryData2Entries = new List<ArticleExtraDataEntryData2Entry>();
        }
    }
}
