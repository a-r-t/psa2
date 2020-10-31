using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter.Misc
{
    public class ActionInterruptEntry
    {
        public int Value { get; set; }

        public override string ToString()
        {
            return $"{{{nameof(Value)}={Value.ToString("X")}}}";
        }
    }
}
