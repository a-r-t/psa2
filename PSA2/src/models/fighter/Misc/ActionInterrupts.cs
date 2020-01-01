using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
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
    }
}
