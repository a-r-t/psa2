using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
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

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(ArticleListOffset)}={ArticleListOffset.ToString("X")}, {nameof(ArticleCount)}={ArticleCount.ToString()}, {nameof(StaticArticleEntries)}={string.Join(",", StaticArticleEntries.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
