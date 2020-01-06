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

        public MultiJump()
        {
            Hops = new MultiJumpHops();
            MultiJumpUnknown = new MultiJumpUnknown();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString("X")}, {nameof(Unknown0)}={Unknown0.ToString("X")}, {nameof(Unknown1)}={Unknown1.ToString("X")}, {nameof(Unknown2)}={Unknown2.ToString("X")}, {nameof(HorizontalBoost)}={HorizontalBoost.ToString("X")}, {nameof(HopsOffset)}={HopsOffset.ToString("X")}, {nameof(UnknownDatasOffset)}={UnknownDatasOffset.ToString("X")}, {nameof(TurnFrames)}={TurnFrames.ToString("X")}, {nameof(Hops)}={Hops}, {nameof(MultiJumpUnknown)}={MultiJumpUnknown}}}";
        }
    }
}
