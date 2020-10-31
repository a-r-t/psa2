using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter
{
    public class Action
    {
        public int ActionNumber { get; private set; }
        public CodeBlock[] CodeBlocks { get; private set; }

        public Action(int actionNumber, CodeBlock[] codeBlocks)
        {
            ActionNumber = actionNumber;
            CodeBlocks = codeBlocks;
        }

        public override string ToString()
        {
            return $"{{{nameof(ActionNumber)}={ActionNumber}, {nameof(CodeBlocks)}={string.Join(",", CodeBlocks.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
