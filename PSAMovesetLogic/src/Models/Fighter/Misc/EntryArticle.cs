using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class EntryArticle
    {
        public int Offset { get; set; }
        public int ArticleGroupId { get; set; }
        public int ArcEntryGroup { get; set; }
        public int Bone { get; set; }
        public int ActionFlags { get; set; }
        public int SubActionFlags { get; set; }
        public int Actions { get; set; }
        public int SubActionMain { get; set; }
        public int SubActionGfx { get; set; }
        public int SubActionSfx { get; set; }
        public int ModelVisibility { get; set; }
        public int CollisionData { get; set; }
        public int Data2 { get; set; }
        public int Data3 { get; set; }
        public int SubActionCount { get; set; }
        public EntryArticleSubAction EntryArticleSubAction { get; set; }
        public EntryArticleData3 EntryArticleData3 { get; set; }

        public EntryArticle()
        {
            EntryArticleSubAction = new EntryArticleSubAction();
            EntryArticleData3 = new EntryArticleData3();
        }
    }
}
