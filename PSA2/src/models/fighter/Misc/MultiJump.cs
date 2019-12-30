using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class MultiJump
    {
        public int Offset { get; set; }
        public int Unknown0 { get; set; }
        public int Unknown1 { get; set; }
        public int Unknown2 { get; set; }
        public int HorizontalBoost { get; set; }
        public int HopsOffset { get; set; }
        public int UnknownDatasOffset { get; set; }
        public int TurnFrames { get; set; }
        public MultiJumpHops Hops { get; set; }
        public MultiJumpUnknown MultiJumpUnknown { get; set; }
    }
}
