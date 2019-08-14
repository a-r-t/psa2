using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class Command
    {
        public string CommandName { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public List<CommandParam> CommandParams { get; set; }
        public List<string> CommandNameAliases { get; set; }

        public Command(string commandName, string description, string instruction, List<CommandParam> commandParams=null, List<string> commandNameAliases=null)
        {
            CommandName = commandName;
            CommandNameAliases = commandNameAliases;
            Description = description;
            Instruction = instruction;
            CommandParams = commandParams;
            CommandNameAliases = commandNameAliases;
        }
    }
}
