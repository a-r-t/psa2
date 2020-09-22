using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class ArticleExtraDataEntry
    {
        public int Offset { get; set; }
        public int ActionFlagsCount { get; set; }
        public List<ActionFlag> ActionFlags { get; set; }
        public ArticleCollisionData ArticleCollisionData { get; set; }
        public ModelVisibility ModelVisibility { get; set; }
        public ArticleExtraDataEntryData2 ArticleExtraDataEntryData2 { get; set; }

        public ArticleExtraDataEntry()
        {
            ActionFlags = new List<ActionFlag>();
            ArticleCollisionData = new ArticleCollisionData();
            ModelVisibility = new ModelVisibility();
            ArticleExtraDataEntryData2 = new ArticleExtraDataEntryData2();
        }
    }
}
