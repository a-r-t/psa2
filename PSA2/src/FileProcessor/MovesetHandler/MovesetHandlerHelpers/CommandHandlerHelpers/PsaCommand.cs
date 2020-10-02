using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    public class PsaCommand
    {
        public int Instruction { get; set; }
        public int CommandParametersLocation { get; set; }
        public int CommandParametersValuesLocation { get; set; }
        public List<PsaCommandParameter> Parameters { get; set; }
        public int NumberOfParams
        {
            get
            {
                return Instruction >> 8 & 0xFF;
            }
        }

        public PsaCommand(int instruction, int commandParametersLocation, List<PsaCommandParameter> parameters)
        {
            Instruction = instruction;
            CommandParametersLocation = commandParametersLocation;
            Parameters = parameters;
            CommandParametersValuesLocation = CommandParametersLocation / 4;
        }

        public override string ToString()
        {
            return $"{{{nameof(Instruction)}={Instruction.ToString("X")} ({Instruction}), {nameof(CommandParametersLocation)}={CommandParametersLocation.ToString("X")} ({CommandParametersLocation}), {nameof(CommandParametersValuesLocation)}={CommandParametersValuesLocation.ToString("X")} ({CommandParametersValuesLocation}), {nameof(Parameters)}={string.Join(",", Parameters.Select(x => x.ToString()).ToList())}}}";
        }

        /// <summary>
        /// Gets how much space is required for each param
        /// each param requires two doublewords -- one for the type (e.g. value, boolean) and one for the actual value
        /// </summary>
        /// <param name="psaCommandInstruction"></param>
        /// <returns>how many doublewords are required to hold the params for the given instruction</returns>
        public int GetCommandParamsSize()
        {
            return (Instruction >> 8 & 0xFF) * 2;
        }

        public int GetCommandParameterTypeLocation(int paramIndex)
        {
            return CommandParametersValuesLocation + (paramIndex * 2);
        }

        public int GetCommandParameterValueLocation(int paramIndex)
        {
            return CommandParametersValuesLocation + (paramIndex * 2) + 1;
        }
    }
}

