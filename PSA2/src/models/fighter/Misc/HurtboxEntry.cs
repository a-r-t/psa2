using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
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

        public HurtBoxEntry(int xOffset, int yOffset, int zOffset, int xStretch, int yStretch, int zStretch, int radius, int data, int bone, int enabled, int zone, int region)
        {
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
    }
}
