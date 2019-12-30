using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class LedgeGrabEntry
    {
        public int Offset { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public LedgeGrabEntry(int offset, int x, int y, int width, int height)
        {
            Offset = offset;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}
