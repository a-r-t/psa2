using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter.Misc
{
    public class ExtraActionFlags
    {
        public int Offset { get; set; }
        public int ActionFlagsCount { get; set; }
        public List<ActionFlag> ActionFlags { get; set; }
    }
}
