using PSA2.src.FileProcessor.MovesetParser.Configs;
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
        public Dictionary<string, PsaCommand> PsaCommands { get; private set; }

        public ActionsParser(PsaFile psaFile, int dataSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandsConfig PsaCommandsConfig = Utils.LoadJson<PsaCommandsConfig>("data/psa_command_data.json");
            PsaCommands = PsaCommandsConfig.PsaCommands.ToDictionary(command => command.Instruction, command => command);
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

        public PsaInstruction GetPsaInstruction(int instructionLocation) // instructionLocation = j
        {
            Console.WriteLine("Get PSA Instruction");
            int instruction = 0;
            List<(int Type, int Value)> paramValues = new List<(int Type, int Value)>();

            if (PsaFile.FileContent[instructionLocation] == -86052851)
            {
                // FADEF00D
            }
            else if (PsaFile.FileContent[instructionLocation + 1] < 0 || PsaFile.FileContent[instruction] >= PsaFile.DataSectionSize)
            {
                // Error Data x8 something something
            }

            int rawPsaInstruction = PsaFile.FileContent[instructionLocation];
            Console.WriteLine(rawPsaInstruction);
            Console.WriteLine(rawPsaInstruction.ToString("X8"));

            // this ain't even needed, whyyyy
            int firstHalfPsaInstruction = ((rawPsaInstruction >> 16) & 0xFFFF); // this gets you the first half of it? so like if a command is 120B0100, it'll give back 120B

            PsaCommand psaCommand;
            if (!PsaCommands.ContainsKey(rawPsaInstruction.ToString("X8"))) {
                // psa command not found...
                //Console.WriteLine("NOT FOUND");
            }
            else
            {
                //Console.WriteLine("FOUND!");
                psaCommand = PsaCommands[rawPsaInstruction.ToString("X8")];
            }
            instruction = rawPsaInstruction;

            // gets number of params in instruction based on 3rd byte in word for instruction's location
            // e.g. 120B0100's 3rd byte is 01, which means it only has 1 param
            int numberOfParams = ((rawPsaInstruction >> 8) & 0xFF);

            // guessing
            int commandParamsLocation = PsaFile.FileContent[instructionLocation + 1] / 4;
            for (int i = 0; i < numberOfParams * 2; i+=2)
            {
                Console.WriteLine(String.Format("Param Type: {0}", PsaFile.FileContent[commandParamsLocation + i]));
                Console.WriteLine(String.Format("Param Value: {0}", PsaFile.FileContent[commandParamsLocation + i + 1]));
                paramValues.Add((Type: PsaFile.FileContent[commandParamsLocation + i], Value: PsaFile.FileContent[commandParamsLocation + i + 1]));
            }

            Console.WriteLine(String.Format("Instruction: {0}", instruction));
            paramValues.ForEach(t => Console.WriteLine(String.Format("Param Type: {0}, Param Value: {1}", t.Type, t.Value)));
            return new PsaInstruction(instruction, paramValues);
        }

        public List<PsaInstruction> GetPsaInstructionsForAction(int actionId, int codeBlockId)
        {
            List <PsaInstruction> psaInstructions = new List<PsaInstruction>();

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
                    psaInstructions.Add(GetPsaInstruction(nextActionCodeBlockInstructionLocation));
                    nextActionCodeBlockInstructionLocation += 2;
                }
            }


            return psaInstructions;
        }

        public List<PsaInstruction> GetPsaInstructionsForSubAction(int subActionId)
        {
            List<PsaInstruction> psaInstructions = new List<PsaInstruction>();



            return psaInstructions;
        }
    }
}
