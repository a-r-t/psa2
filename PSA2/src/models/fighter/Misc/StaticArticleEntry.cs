using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class StaticArticleEntry
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
        public List<string> SubActionNames { get; set; }

        public StaticArticleEntry(int offset, int articleGroupId, int arcEntryGroup, int bone, int actionFlags, 
            int subActionFlags, int actions, int subActionMain, int subActionGfx, int subActionSfx, int modelVisibility, 
            int collisionData, int data2, int data3, int subActionCount)
        {
            Offset = offset;
            ArticleGroupId = articleGroupId;
            ArcEntryGroup = arcEntryGroup;
            Bone = bone;
            ActionFlags = actionFlags;
            SubActionFlags = subActionFlags;
            Actions = actions;
            SubActionMain = subActionMain;
            SubActionGfx = subActionGfx;
            SubActionSfx = subActionSfx;
            ModelVisibility = modelVisibility;
            CollisionData = collisionData;
            Data2 = data2;
            Data3 = data3;
            SubActionCount = subActionCount;
            SubActionNames = new List<string>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(ArticleGroupId)}={ArticleGroupId.ToString("X")}, {nameof(ArcEntryGroup)}={ArcEntryGroup.ToString("X")}, {nameof(Bone)}={Bone.ToString("X")}, {nameof(ActionFlags)}={ActionFlags.ToString("X")}, {nameof(SubActionFlags)}={SubActionFlags.ToString("X")}, {nameof(Actions)}={Actions.ToString("X")}, {nameof(SubActionMain)}={SubActionMain.ToString("X")}, {nameof(SubActionGfx)}={SubActionGfx.ToString("X")}, {nameof(SubActionSfx)}={SubActionSfx.ToString("X")}, {nameof(ModelVisibility)}={ModelVisibility.ToString("X")}, {nameof(CollisionData)}={CollisionData.ToString("X")}, {nameof(Data2)}={Data2.ToString("X")}, {nameof(Data3)}={Data3.ToString("X")}, {nameof(SubActionCount)}={SubActionCount.ToString("X")}}}";
        }
    }
}
