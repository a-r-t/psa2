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

        public int GetTotalNumberOfActions()
        {
            return 274 + GetNumberOfSpecialActions();
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


        public List<PsaCommand> GetPsaCommandsForAction(int actionId, int codeBlockId)
        {
            List <PsaCommand> psaCommands = new List<PsaCommand>();
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            int actionCodeBlockInstructionsStartLocation = actionCodeBlockLocation / 4;
            if (actionCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextActionCodeBlockInstructionLocation = actionCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextActionCodeBlockInstructionLocation] != 0 && 
                    nextActionCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextActionCodeBlockInstructionLocation));
                    nextActionCodeBlockInstructionLocation += 2;
                }
            }


            return psaCommands;
        }

        // this is the offset where subaction code starts (displayed in PSAC)
        public int GetSubActionCodeBlockLocation(int subActionId, int codeBlockId)
        {
            int subActionsCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 12 + codeBlockId] / 4; // n
            int subActionCodeBlockLocation = PsaFile.FileContent[subActionsCodeBlockStartingLocation + subActionId];

            return subActionCodeBlockLocation;
        }


        public List<PsaCommand> GetPsaCommandsForSubAction(int subActionId, int codeBlockId)
        {
            List<PsaCommand> psaCommands = new List<PsaCommand>();
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId); // i
            int subActionCodeBlockInstructionsStartLocation = subActionCodeBlockLocation / 4; // j
            if (subActionCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextSubActionCodeBlockInstructionLocation = subActionCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextSubActionCodeBlockInstructionLocation] != 0 &&
                    nextSubActionCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextSubActionCodeBlockInstructionLocation));
                    nextSubActionCodeBlockInstructionLocation += 2;
                }
            }

            return psaCommands;
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

        public List<PsaCommand> GetPsaCommandsForSubRoutine(int subRoutineLocation)
        {
            List<PsaCommand> psaCommands = new List<PsaCommand>();
            int subRoutineCodeStartLocation = subRoutineLocation / 4;
            if (subRoutineCodeStartLocation > 0 && subRoutineCodeStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextSubRoutineInstructionLocation = subRoutineCodeStartLocation;
                while (PsaFile.FileContent[nextSubRoutineInstructionLocation] != 0 &&
                    nextSubRoutineInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextSubRoutineInstructionLocation));
                    nextSubRoutineInstructionLocation += 2;
                }
            }
            return psaCommands;
        }

        public List<int> GetAllSubRoutines()
        {
            HashSet<int> subRoutines = new HashSet<int>();
            int numberOfSpecialActions = GetNumberOfSpecialActions();
            int numberOfSubActions = GetNumberOfSubActions();

            // find all subroutines in all actions
            for (int i = 0; i < numberOfSpecialActions; i++)
            {
                for (int j = 0; j < 2; j++) {
                    List<PsaCommand> commands = GetPsaCommandsForAction(i, j);
                    foreach (PsaCommand command in commands)
                    {
                        if (command != null)
                        {
                            if (command.Instruction.ToString("X8") == "00070100" && command.Parameters[0].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[1].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[1].Value);
                            }
                        }
                    }
                }
            }

            // find all subroutines in all sub actions
            for (int i = 0; i < numberOfSubActions; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    List<PsaCommand> commands = GetPsaCommandsForSubAction(i, j);
                    foreach (PsaCommand command in commands)
                    {
                        if (command != null)
                        {
                            if (command.Instruction.ToString("X8") == "00070100" && command.Parameters[0].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[1].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[1].Value);
                            }
                        }
                    }
                }
            }

            // "recursively" without using recursion find all subroutines from other subroutines
            HashSet<int> emptySubRoutines = new HashSet<int>();
            List<int> subRoutinesList = subRoutines.ToList();
            for (int i = 0; i < subRoutinesList.Count; i++)
            {
                int subRoutineLocation = subRoutinesList[i];

                List<PsaCommand> commands = GetPsaCommandsForSubRoutine(subRoutineLocation);
                if (commands.Count == 0)
                {
                    emptySubRoutines.Add(subRoutineLocation);
                }
                else
                {
                    foreach (PsaCommand command in commands)
                    {
                        if (command != null)
                        {
                            if (command.Instruction.ToString("X8") == "00070100" && command.Parameters[0].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[0].Value);
                                subRoutinesList.Add(command.Parameters[0].Value);
                            }
                            else if (command.Instruction.ToString("X8") == "0D000200" && command.Parameters[0].Value < PsaFile.DataSectionSize)
                            {
                                subRoutines.Add(command.Parameters[1].Value);
                                subRoutinesList.Add(command.Parameters[i].Value);
                            }
                        }
                    }
                }
            }

            HashSet<int> subRoutinesNested = subRoutinesList.ToHashSet<int>();
            subRoutinesNested.RemoveWhere(emptySubRoutines.Contains);
            //subRoutinesNested.ToList().OrderBy(x => x.ToString("X8")).ToList().ForEach(s => Console.WriteLine(s.ToString("X8")));
            return subRoutinesNested.ToList().OrderBy(x => x.ToString("X8")).ToList();
        }

        public List<int> GetAllActionOverrides(int codeBlockId)
        {
            List<int> actionOverrideIds = new List<int>();
            if (codeBlockId == 3)
            {
                int totalNumberOfActions = GetTotalNumberOfActions();
                for (int i = 0; i < totalNumberOfActions; i++)
                {
                    actionOverrideIds.Add(i);
                }
                // pre action overrides
                return actionOverrideIds;
            }
            else
            {

                int actionOverridesLocation;
                if (codeBlockId == 0)
                {
                    actionOverridesLocation = PsaFile.FileContent[DataSectionLocation + 20] / 4;
                }
                else // if (codeBlockId == 1)
                {
                    actionOverridesLocation = PsaFile.FileContent[DataSectionLocation + 21] / 4;
                }

                int nextActionOverridesLocation = actionOverridesLocation;
                while (PsaFile.FileContent[nextActionOverridesLocation] >= 0)
                {
                    actionOverrideIds.Add(PsaFile.FileContent[nextActionOverridesLocation]);
                    nextActionOverridesLocation += 2;
                }
                actionOverrideIds.ForEach(x => Console.WriteLine(x.ToString("X")));
                return actionOverrideIds;
            }

        }

        // this is the offset where action override code starts (displayed in PSAC)
        public int GetActionOverrideCodeBlockLocation(int actionOverrideId, int codeBlockId) // issue -- actionOverrideId needs to be in order of existence
        {
            int actionOverridesCodeBlockStartingLocation = codeBlockId == 0 ?
                PsaFile.FileContent[DataSectionLocation + 20] / 4 :
                PsaFile.FileContent[DataSectionLocation + 21] / 4;

            int actionOverrideCodeBlockLocation = PsaFile.FileContent[actionOverridesCodeBlockStartingLocation + actionOverrideId * 2 + 1];

            return actionOverrideCodeBlockLocation;
        }

        public List<PsaCommand> GetPsaCommandsForActionOverride(int codeBlockId, int actionOverrideId)
        {
            List<PsaCommand> psaCommands = new List<PsaCommand>();
            int actionOverrideCodeBlockLocation = GetActionOverrideCodeBlockLocation(actionOverrideId, codeBlockId);
            int actionOverrideCodeBlockInstructionsStartLocation = actionOverrideCodeBlockLocation / 4;
            if (actionOverrideCodeBlockInstructionsStartLocation < PsaFile.DataSectionSize) // and greater than "stf" whatever that means"
            {
                int nextActionOverrideCodeBlockInstructionLocation = actionOverrideCodeBlockInstructionsStartLocation;
                while (PsaFile.FileContent[nextActionOverrideCodeBlockInstructionLocation] != 0 &&
                    nextActionOverrideCodeBlockInstructionLocation < PsaFile.DataSectionSize)
                {
                    psaCommands.Add(PsaCommandParser.GetPsaInstruction(nextActionOverrideCodeBlockInstructionLocation));
                    nextActionOverrideCodeBlockInstructionLocation += 2;
                }
            }
            return psaCommands;
        }

        
    }
}
