using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class StaticArticles
    {
        public int Offset { get; set; }
        public int ArticleListOffset { get; set; }
        public int ArticleCount { get; set; }
        public List<StaticArticleEntry> StaticArticleEntries { get; set; }

        public StaticArticles()
        {
            StaticArticleEntries = new List<StaticArticleEntry>();
        }
    }
}
