using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class PsaInstruction
    {
        public int Instruction { get; set; }
        public List<(int Type, int Value)> ParamValues { get; set; }

        public PsaInstruction(int instruction, List<(int Type, int Value)> paramValues)
        {
            Instruction = instruction;
            ParamValues = paramValues;
        }
    }
}
