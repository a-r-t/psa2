using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.ActionsHelpers;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class ActionsParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public Dictionary<string, PsaCommandConfig> PsaCommands { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public ActionsParser(PsaFile psaFile, int dataSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandsConfig PsaCommandsConfig = Utils.LoadJson<PsaCommandsConfig>("data/psa_command_data.json");
            PsaCommands = PsaCommandsConfig.PsaCommands.ToDictionary(command => command.Instruction, command => command);
            PsaCommandParser = new PsaCommandParser(PsaFile);
        }

        public int GetNumberOfSpecialActions()
        {
            Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        public int GetNumberOfSubActions()
        {
            Console.WriteLine(String.Format("Number of Sub Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 13] - PsaFile.FileContent[DataSectionLocation + 12]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 13] - PsaFile.FileContent[DataSectionLocation + 12]) / 4;
        }

        public int GetActionCodeBlockLocation(int actionId, int codeBlockId)
        {
            int actionsCodeBlockStartingLocation = codeBlockId == 0 ?
                PsaFile.FileContent[DataSectionLocation + 9] / 4 :
                PsaFile.FileContent[DataSectionLocation + 10] / 4;

            int actionCodeBlockLocation = PsaFile.FileContent[actionsCodeBlockStartingLocation + actionId];

            return actionCodeBlockLocation;
        }


        public List<PsaCommand> GetPsaInstructionsForAction(int actionId, int codeBlockId)
        {
            List <PsaCommand> psaInstructions = new List<PsaCommand>();

            // actionId = h
            // codeBlockLocation = n
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            int actionCodeBlockInstructionsStartLocation = actionCodeBlockLocation / 4;
            if (actionCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextActionCodeBlockInstructionLocation = actionCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextActionCodeBlockInstructionLocation] != 0 && 
                    nextActionCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaInstructions.Add(PsaCommandParser.GetPsaInstruction(nextActionCodeBlockInstructionLocation));
                    nextActionCodeBlockInstructionLocation += 2;
                }
            }


            return psaInstructions;
        }

        public List<PsaCommand> GetPsaInstructionsForSubAction(int subActionId)
        {
            List<PsaCommand> psaInstructions = new List<PsaCommand>();



            return psaInstructions;
        }
    }
}
