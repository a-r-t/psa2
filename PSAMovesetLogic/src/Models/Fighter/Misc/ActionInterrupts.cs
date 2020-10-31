using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class ActionInterrupts
    {
        public int Offset { get; set; }
        public int ActionInterruptsOffset { get; set; }
        public int DataOffset { get; set; }
        public int DataCount { get; set; }
        public List<ActionInterruptEntry> ActionInterruptEntries { get; set; }

        public ActionInterrupts()
        {
            ActionInterruptEntries = new List<ActionInterruptEntry>();
        }

        public override string ToString()
        {
            return $"{{{nameof(Offset)}={Offset.ToString()}, {nameof(ActionInterruptsOffset)}={ActionInterruptsOffset.ToString()}, {nameof(DataOffset)}={DataOffset.ToString()}, {nameof(DataCount)}={DataCount.ToString()}, {nameof(ActionInterruptEntries)}={string.Join(",", ActionInterruptEntries.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
