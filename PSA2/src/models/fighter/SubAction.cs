using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter
{
    public class SubAction
    {
        public int SubActionNumber { get; private set; }
        public CodeBlock[] CodeBlocks { get; private set; }
        public Animation Animation { get; set; }

        public SubAction(int subActionNumber, CodeBlock[] codeBlocks, Animation animation)
        {
            SubActionNumber = subActionNumber;
            CodeBlocks = codeBlocks;
            Animation = animation;
        }
    }
}
