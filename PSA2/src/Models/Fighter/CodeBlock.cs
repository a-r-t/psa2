using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter
{
    public class CodeBlock
    {
        public int Location { get; set; }
        public int CommandsPointerLocation { get; set; }
        public int CommandsLocation { get; set; }
        public List<PsaCommand> PsaCommands { get; set; }

        public int NumberOfCommands 
        { 
            get 
            { 
                return PsaCommands.Count;
            }
        }

        public CodeBlock(int location, int commandsPointerLocation, int commandsLocation, List<PsaCommand> psaCommands)
        {
            Location = location;
            CommandsPointerLocation = commandsPointerLocation;
            CommandsLocation = commandsLocation;
            PsaCommands = psaCommands;
        }

        public PsaCommand GetPsaCommand(int index)
        {
            return PsaCommands[index];
        }

        public int GetPsaCommandParametersPointerLocation(int commandIndex)
        {
            return (CommandsLocation + (commandIndex * 2)) * 4 + 4;
        }

        public int GetPsaCommandLocation(int commandIndex)
        {
            return CommandsLocation + (commandIndex * 2);
        }

        public int GetPsaCommandParamsLocation(int commandIndex)
        {
            return CommandsLocation + (commandIndex * 2) + 1;
        }

        public int GetCommandsEndLocation()
        {
            return CommandsLocation + NumberOfCommands * 2;
        }

        public int GetPsaCommandIndexByLocation(int commandLocation)
        {
            for (int commandIndex = 0; commandIndex < PsaCommands.Count; commandIndex++)
            {
                if (GetPsaCommandLocation(commandIndex) == commandLocation)
                {
                    return commandIndex;
                }
            }
            throw new ArgumentException("Invalid command location");
        }

        public override string ToString()
        {
            return $"{{{nameof(Location)}={Location.ToString("X")} ({Location}), {nameof(CommandsPointerLocation)}={CommandsPointerLocation.ToString("X")} ({CommandsPointerLocation}), {nameof(CommandsLocation)}={CommandsLocation.ToString("X")} ({CommandsLocation}), {nameof(PsaCommands)}={string.Join(",", PsaCommands.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
