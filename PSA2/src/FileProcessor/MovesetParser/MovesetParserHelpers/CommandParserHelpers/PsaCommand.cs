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
        public int CommandParametersLocation { get; set; }
        public List<PsaCommandParameter> Parameters { get; set; }


        public PsaCommand(int instruction, int commandParametersLocation, List<PsaCommandParameter> parameters)
        {
            Instruction = instruction;
            CommandParametersLocation = commandParametersLocation;
            Parameters = parameters;
        }

        public override string ToString()
        {
            return $"{{{nameof(Instruction)}={Instruction.ToString("X")}, {nameof(CommandParametersLocation)}={CommandParametersLocation.ToString("X")}, {nameof(Parameters)}={string.Join(",", Parameters.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
