using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers
{
    public class PsaCommand
    {
        public int Instruction { get; set; }
        public List<PsaCommandParameter> Parameters { get; set; }


        public PsaCommand(int instruction, List<PsaCommandParameter> parameters)
        {
            Instruction = instruction;
            Parameters = parameters;
        }
    }
}
