using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class FinalSmashAuraEntry
    {
        public int Offset { get; set; }
        public int Bone { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public FinalSmashAuraEntry(int offset, int bone, int x, int y, int width, int height)
        {
            Offset = offset;
            Bone = bone;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Bone)}={Bone.ToString("X")}, {nameof(X)}={X.ToString("X")}, {nameof(Y)}={Y.ToString("X")}, {nameof(Width)}={Width.ToString("X")}, {nameof(Height)}={Height.ToString("X")}}}";
        }
    }
}
