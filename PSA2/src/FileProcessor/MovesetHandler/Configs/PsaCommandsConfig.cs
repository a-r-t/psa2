using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.Configs
{
    public class PsaCommandsConfig
    {
        public List<PsaCommandConfig> PsaCommands { get; set; }

        public PsaCommandsConfig()
        {
            PsaCommands = new List<PsaCommandConfig>();
        }

        public PsaCommandConfig GetPsaCommandConfigByInstruction(int instruction)
        {
            string instructionHex = instruction.ToString("X8");
            return PsaCommands.Find(command => command.Instruction == instructionHex);
        }
    }

    public class PsaCommandConfig
    {
        public string CommandName { get; set; }
        public string Description { get; set; }
        public string Instruction { get; set; }
        public List<PsaCommandParamConfig> CommandParams { get; set; }

        public PsaCommandConfig(string commandName, string description, string instruction, List<PsaCommandParamConfig> commandParams = null)
        {
            CommandName = commandName;
            Description = description;
            Instruction = instruction;
            CommandParams = commandParams;
        }

        public PsaCommand ToPsaCommand()
        {
            int instructionInt = int.Parse(Instruction, System.Globalization.NumberStyles.HexNumber);

            List<PsaCommandParameter> parameters = new List<PsaCommandParameter>();
            foreach (PsaCommandParamConfig psaCommandParamConfig in CommandParams)
            {
                parameters.Add(psaCommandParamConfig.ToPsaCommandParameter());
            }

            return new PsaCommand(instructionInt, 0, parameters);
        }
    }

    public class PsaCommandParamConfig
    {
        public string ParamName { get; set; }
        public string Description { get; set; }
        public List<string> DataTypes { get; set; }
        public bool CanUseVariable { get; set; }
        public string DefaultValue { get; set; }

        public PsaCommandParamConfig(string paramName, string description, List<string> dataTypes, bool canUseVariable, string defaultValue = null)
        {
            ParamName = paramName;
            Description = description;
            DataTypes = dataTypes;
            CanUseVariable = canUseVariable;

            if (defaultValue != null)
            {
                DefaultValue = defaultValue;
            }
            else
            {
                DefaultValue = GetDefaultValueByType();
            }
        }

        private string GetDefaultValueByType()
        {
            switch (DataTypes[0].ToLower())
            {
                case "scalar": return "0";
                case "boolean": return "false";
                case "variable": return "IC-Basic[0]";
                case "value":
                case "pointer":
                case "(4)":
                case "Any":
                default: return "00000000";
            }
        }

        private int GetTypeIndex()
        {
            switch (DataTypes[0].ToLower())
            {
                case "value": return 0;
                case "scalar": return 1;
                case "pointer": return 2;
                case "boolean": return 3;
                case "(4)": return 4;
                case "variable": return 5;
                case "requirement": return 6;
                default: return 0;
            }
        }

        public PsaCommandParameter ToPsaCommandParameter()
        {
            return new PsaCommandParameter(GetTypeIndex(), 0);
        }
    }
}
