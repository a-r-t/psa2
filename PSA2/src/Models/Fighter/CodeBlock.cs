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

        public int GetPsaCommandPointerLocation(int commandIndex)
        {
            return (CommandsLocation + (commandIndex * 2)) * 4 + 4;
        }

        public int GetPsaCommandLocation(int commandIndex)
        {
            return CommandsLocation + (commandIndex * 2);
        }

        public override string ToString()
        {
            return $"{{{nameof(Location)}={Location.ToString("X")} ({Location}), {nameof(CommandsPointerLocation)}={CommandsPointerLocation.ToString("X")} ({CommandsPointerLocation}), {nameof(CommandsLocation)}={CommandsLocation.ToString("X")} ({CommandsLocation}), {nameof(PsaCommands)}={string.Join(",", PsaCommands.Select(x => x.ToString()).ToList())}}}";
        }
    }
}
