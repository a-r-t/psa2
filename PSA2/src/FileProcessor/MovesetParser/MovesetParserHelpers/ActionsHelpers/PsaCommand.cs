using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.ActionsHelpers
{
    public class PsaCommand
    {
        public int Instruction { get; set; }
        public List<(int Type, int Value)> Parameters { get; set; }


        public PsaCommand(int instruction, List<(int Type, int Value)> parameters)
        {
            Instruction = instruction;
            Parameters = parameters;
        }
    }
}
