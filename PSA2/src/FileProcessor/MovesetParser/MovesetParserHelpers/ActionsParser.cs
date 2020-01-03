using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.ActionsParserHelpers;
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

        // this is the offset where action code starts (displayed in PSAC)
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

        // this is the offset where subaction code starts (displayed in PSAC)
        public int GetSubActionCodeBlockLocation(int subActionId, int codeBlockId)
        {
            int subActionsCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 12 + codeBlockId] / 4; // n
            int subActionCodeBlockLocation = PsaFile.FileContent[subActionsCodeBlockStartingLocation + subActionId];

            return subActionCodeBlockLocation;
        }


        public List<PsaCommand> GetPsaInstructionsForSubAction(int subActionId, int codeBlockId)
        {
            List<PsaCommand> psaInstructions = new List<PsaCommand>();
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId); // i
            int subActionCodeBlockInstructionsStartLocation = subActionCodeBlockLocation / 4; // j
            if (subActionCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextSubActionCodeBlockInstructionLocation = subActionCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextSubActionCodeBlockInstructionLocation] != 0 &&
                    nextSubActionCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaInstructions.Add(PsaCommandParser.GetPsaInstruction(nextSubActionCodeBlockInstructionLocation));
                    nextSubActionCodeBlockInstructionLocation += 2;
                }
            }

            return psaInstructions;
        }

        public string GetSubActionAnimationName(int subActionId)
        {
            // h = subActionId
            // not 100% sure why this chain works but it does
            int animationLocation = PsaFile.FileContent[DataSectionLocation] / 4 + 1 + subActionId * 2;
            if (PsaFile.FileContent[animationLocation] == 0)
            {
                return "NONE";
            }
            else
            {
                int animationNameLocation = PsaFile.FileContent[animationLocation] / 4; // j

                if (animationNameLocation < PsaFile.DataSectionSize) // and animationNameLocation >= stf whatever that means
                {
                    StringBuilder animationName = new StringBuilder();
                    int nameEndByteIndex = 0;
                    while (true)
                    {
                        string nextStringData = Utils.ConvertWordToString(PsaFile.FileContent[animationNameLocation + nameEndByteIndex]);
                        animationName.Append(nextStringData);
                        if (nextStringData.Length == 4)
                        {
                            nameEndByteIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    Console.WriteLine(animationName.ToString());
                    return animationName.ToString();
                }

                return "ERROR";
            }

        }

        public AnimationFlags GetSubActionAnimationFlags(int subActionId)
        {
            // will need to look at this later to figure out why it works
            int animationFlagsLocation = PsaFile.FileContent[DataSectionLocation] / 4 + subActionId * 2;
            int animationFlagsValue = PsaFile.FileContent[animationFlagsLocation];
            int inTransition = (animationFlagsValue >> 24) & 0xFF;
            int noOutTransition = animationFlagsValue & 0x1;
            int loop = ((animationFlagsValue >> 16) & 0xFF) & 0x2;
            int movesCharacter = ((animationFlagsValue >> 16) & 0xFF) & 0x4;
            int unknown3 = ((animationFlagsValue >> 16) & 0xFF) & 0x8;
            int unknown4 = ((animationFlagsValue >> 16) & 0xFF) & 0x10;
            int unknown5 = ((animationFlagsValue >> 16) & 0xFF) & 0x20;
            int transitionOutFromStart = ((animationFlagsValue >> 16) & 0xFF) & 0x40;
            int unknown7 = ((animationFlagsValue >> 16) & 0xFF) & 0x80;
            return new AnimationFlags(inTransition, noOutTransition, loop, movesCharacter, unknown3, unknown4, unknown5, transitionOutFromStart, unknown7);
        }
    }
}
