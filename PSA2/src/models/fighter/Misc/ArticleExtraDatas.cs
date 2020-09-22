using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class ArticleExtraDatas
    {
        public int Offset { get; set; }
        public int EntriesCount { get; set; }
        public List<ArticleExtraDataEntry> ArticleExtraDataEntries { get; set; }

        public ArticleExtraDatas()
        {
            ArticleExtraDataEntries = new List<ArticleExtraDataEntry>();
        }
    }
}
