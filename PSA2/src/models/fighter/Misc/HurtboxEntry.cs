using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class HurtBoxEntry
    {
        public int Offset { get; set; }
        public int XOffset { get; set; }
        public int YOffset { get; set; }
        public int ZOffset { get; set; }
        public int XStretch { get; set; }
        public int YStretch { get; set; }
        public int ZStretch { get; set; }
        public int Radius { get; set; }
        public int Data { get; set; }
        public int Bone { get; set; }
        public int Enabled { get; set; }
        public int Zone { get; set; }
        public int Region { get; set; }

        public HurtBoxEntry(int offset, int xOffset, int yOffset, int zOffset, int xStretch, int yStretch, int zStretch, int radius, int data, int bone, int enabled, int zone, int region)
        {
            Offset = offset;
            XOffset = xOffset;
            YOffset = yOffset;
            ZOffset = zOffset;
            XStretch = xStretch;
            YStretch = yStretch;
            ZStretch = zStretch;
            Radius = radius;
            Data = data;
            Bone = bone;
            Enabled = enabled;
            Zone = zone;
            Region = region;
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(XOffset)}={XOffset.ToString("X")}, {nameof(YOffset)}={YOffset.ToString("X")}, {nameof(ZOffset)}={ZOffset.ToString("X")}, {nameof(XStretch)}={XStretch.ToString("X")}, {nameof(YStretch)}={YStretch.ToString("X")}, {nameof(ZStretch)}={ZStretch.ToString("X")}, {nameof(Radius)}={Radius.ToString("X")}, {nameof(Data)}={Data.ToString("X")}, {nameof(Bone)}={Bone.ToString("X")}, {nameof(Enabled)}={Enabled.ToString("X")}, {nameof(Zone)}={Zone.ToString("X")}, {nameof(Region)}={Region.ToString("X")}}}";
        }
    }
}
