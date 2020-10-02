using PSA2.src.Models.Fighter;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    public class PsaCommandRemover
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandRemover(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation, PsaCommandParser psaCommandParser)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            PsaCommandParser = psaCommandParser;
        }

        public void RemoveCommand(CodeBlock codeBlock, int commandLocation, PsaCommand removedPsaCommand)
        {
            // commandLocation is j, codeBlockCommandsLocation is h, removedCommandParmasValuesLocation is m

            if (removedPsaCommand.NumberOfParams != 0)
            {
                DeleteCommandParameters(removedPsaCommand, commandLocation);
            }

            if (codeBlock.NumberOfCommands > 1)
            {
                DeleteCommandAndCloseGap(codeBlock, commandLocation);
            }
            else
            {
                int codeBlockCommandsEnd = codeBlock.GetCommandsEndLocation();
                PsaFile.FileContent[codeBlockCommandsEnd] = Constants.FADEF00D;
                PsaFile.FileContent[codeBlockCommandsEnd + 1] = Constants.FADEF00D;

                PsaFile.FileContent[commandLocation] = Constants.FADEF00D;
                PsaFile.FileContent[commandLocation + 1] = Constants.FADEF00D;

                PsaFile.FileContent[codeBlock.Location] = 0;
                int pointerToCodeBlockLocation = codeBlock.Location * 4;
                PsaFile.RemoveOffsetFromOffsetInterlockTracker(pointerToCodeBlockLocation);
            }

            int codeBlockCommandsEndLocation = codeBlock.CommandsPointerLocation + codeBlock.NumberOfCommands * 8;
            int codeBlockCommandsStartLocation = codeBlockCommandsEndLocation - (codeBlock.NumberOfCommands - 1) * 8;
            for (int i = CodeBlockDataStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
            {
                if (PsaFile.FileContent[i] >= codeBlockCommandsStartLocation && PsaFile.FileContent[i] <= codeBlockCommandsEndLocation)
                {
                    DeleteOffsetInterlockData(i);
                }
            }

            PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
        }

        public void DeleteCommandParameters(PsaCommand removedPsaCommand, int commandLocation)
        {
            if (removedPsaCommand.CommandParametersValuesLocation >= CodeBlockDataStartLocation && removedPsaCommand.CommandParametersValuesLocation < PsaFile.DataSectionSizeBytes)
            {
                int commandParametersPointerLocation = commandLocation * 4 + 4; // rmv
                PsaFile.RemoveOffsetFromOffsetInterlockTracker(commandParametersPointerLocation);

                // iterates through each param the command had
                for (int i = 0; i < removedPsaCommand.NumberOfParams * 2; i += 2)
                {
                    int parameterType = removedPsaCommand.Parameters[i / 2].Type;
                    int parameterValue = removedPsaCommand.Parameters[i / 2].Value;

                    // this only comes into play if the old psa command's param type at index i is "Pointer" (which is 2)
                    if (parameterType == 2)
                    {
                        // I believe this is the location of the actual value of a particular pointer command param
                        int commandParamPointerValueLocation = PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation] == 2
                            ? removedPsaCommand.CommandParametersLocation + 4
                            : removedPsaCommand.CommandParametersLocation + 12;

                        bool wasOffsetRemoved = PsaFile.RemoveOffsetFromOffsetInterlockTracker(commandParamPointerValueLocation);

                        // this part is a long series of nested if statements...
                        // I can't figure out exactly what it does and can't get it to consistently trigger
                        if (!wasOffsetRemoved)
                        {
                            // something to do with external subroutines
                            UpdateExternalPointerLogic(removedPsaCommand, commandParamPointerValueLocation);
                        }

                    }
                    PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + i] = Constants.FADEF00D;
                    PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + i + 1] = Constants.FADEF00D;
                }
            }
        }

        public void DeleteCommandAndCloseGap(CodeBlock codeBlock, int commandLocation)
        {
            int removedCommandIndex = codeBlock.GetPsaCommandIndexByLocation(commandLocation);

            // starting from the removed command, iterate through each command in the code block
            // this loop will shift the commands all over by 1, closing the gap where the removed command used to exist
            for (int commandIndex = removedCommandIndex; commandIndex < codeBlock.PsaCommands.Count - 1; commandIndex++)
            {
                PsaCommand nextPsaCommand = codeBlock.PsaCommands[commandIndex + 1];
                int currentCommandLocation = codeBlock.GetPsaCommandLocation(commandIndex);

                // if next command has params
                if (nextPsaCommand.NumberOfParams > 0)
                {
                    // adjust pointer location for next command to point to the command before it (since all commands are shifted over now)
                    int nextCommandPointerLocation = currentCommandLocation * 4 + 12;

                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        if (PsaFile.OffsetInterlockTracker[j] == nextCommandPointerLocation)
                        {
                            PsaFile.OffsetInterlockTracker[j] -= 8;
                            break;
                        }
                    }
                }

                // this is what actually shifts the command - the instruction and param pointer is shifted to the previous command location
                PsaFile.FileContent[currentCommandLocation] = PsaFile.FileContent[currentCommandLocation + 2];
                PsaFile.FileContent[currentCommandLocation + 1] = PsaFile.FileContent[currentCommandLocation + 3];
            }
            // mark the old last command location (that was shifted up in the the loop above) as the end of the code block
            int lastCommandLocation = codeBlock.GetPsaCommandLocation(codeBlock.NumberOfCommands - 1);
            PsaFile.FileContent[lastCommandLocation] = 0;
            PsaFile.FileContent[lastCommandLocation + 1] = 0;
            PsaFile.FileContent[lastCommandLocation + 2] = Constants.FADEF00D;
            PsaFile.FileContent[lastCommandLocation + 3] = Constants.FADEF00D;
        }

        /// <summary>
        /// DelILData method in PSA-C
        /// </summary>
        /// <param name="fileContentIndex"></param>
        public void DeleteOffsetInterlockData(int fileContentIndex)
        {
            // check if file index location is being pointed to
            int fileContentPointerLocation = fileContentIndex * 4;
            bool offsetFound = false;
            int offsetIndex = 0;
            for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
            {
                if (PsaFile.OffsetInterlockTracker[i] == fileContentPointerLocation)
                {
                    offsetFound = true;
                    offsetIndex = i;
                    break;
                }
            }

            if (offsetFound)
            {
                PsaFile.FileContent[fileContentIndex] = 0;
                PsaFile.OffsetInterlockTracker[offsetIndex] = 16777216; // 100 0000
                PsaFile.NumberOfOffsetEntries--;

                // something here is a pointer i guess
                if (PsaFile.FileContent[fileContentIndex - 1] == 2)
                {
                    PsaFile.FileContent[fileContentIndex - 1] = 0;
                    fileContentPointerLocation -= 4;

                    // this is a bad name def
                    int commandParamValueLocation = 2025;
                    offsetFound = false;
                    for (int i = 0; i < PsaFile.DataSectionSizeBytes; i++)
                    {
                        if (PsaFile.OffsetInterlockTracker[i] == fileContentPointerLocation)
                        {
                            offsetFound = true;
                            commandParamValueLocation = i;
                            break;
                        }
                    }
                    if (offsetFound)
                    {
                        // 459008 is subroutine instruction, 590080 is goto instruction
                        if (PsaFile.FileContent[commandParamValueLocation - 1] == 459008 || PsaFile.FileContent[commandParamValueLocation - 1] == 590080)
                        {
                            // replace command that was pointing to removed command to "nop" instruction
                            PsaFile.FileContent[commandParamValueLocation - 1] = Constants.NOP;
                            PsaFile.FileContent[commandParamValueLocation] = 0;

                            // set to freespace
                            PsaFile.FileContent[fileContentIndex] = Constants.FADEF00D;
                            PsaFile.FileContent[fileContentIndex - 1] = Constants.FADEF00D;

                            int commandParamValuePointerLocation = commandParamValueLocation * 4; // rmv

                            PsaFile.RemoveOffsetFromOffsetInterlockTracker(commandParamValuePointerLocation);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is called when attempting to remove a param value that once pointed to another location (Pointer type) that MIGHT be an external subroutine call
        /// <para>If it is determined to be a call to an external subroutine, the external subroutine data table is updated to no longer pointer to it</para>
        /// </summary>
        /// <param name="removedPsaCommand">The psa command being removed</param>
        /// <param name="commandParamPointerValueLocation">The pointer to the param's value location</param>
        private void UpdateExternalPointerLogic(PsaCommand removedPsaCommand, int commandParamPointerValueLocation)
        {
            // iterate through each external subroutine in the external data table
            for (int externalSubRoutineIndex = 0; externalSubRoutineIndex < PsaFile.NumberOfExternalSubRoutines; externalSubRoutineIndex++)
            {
                // get a external subroutine's pointer
                int externalSubRoutineLocationIndex = (PsaFile.NumberOfDataTableEntries + externalSubRoutineIndex) * 2;
                int externalSubRoutineLocation = PsaFile.FileOtherData[externalSubRoutineLocationIndex];
                if (externalSubRoutineLocation >= 8096 && externalSubRoutineLocation < PsaFile.DataSectionSize)
                {
                    // if the param being removed was pointing to an external subroutine
                    if (commandParamPointerValueLocation == externalSubRoutineLocation)
                    {
                        // get the location of the value of the param
                        int commandExternalSubroutineValueLocation = commandParamPointerValueLocation / 4;

                        // This stops the external data table from pointing to the command param pointer value location
                        if (PsaFile.FileContent[commandExternalSubroutineValueLocation] >= 8096
                            && PsaFile.FileContent[commandExternalSubroutineValueLocation] < PsaFile.DataSectionSize
                            && PsaFile.FileContent[commandExternalSubroutineValueLocation] % 4 == 0)
                        {
                            PsaFile.FileOtherData[externalSubRoutineLocationIndex] = PsaFile.FileContent[commandExternalSubroutineValueLocation];
                        }
                        else
                        {
                            PsaFile.FileOtherData[externalSubRoutineLocationIndex] = -1;
                        }
                        break;
                    }

                    // same deal as above for the most part, but I am NOT sure what location "externalSubRoutineCodeBlockLocation" actually is (I don't think it's right currently)
                    // it's tough to test further because I also can't really figure out how to get this code to trigger at the moment, so I'm just going to leave this as is
                    int externalSubRoutineCodeBlockLocation = externalSubRoutineLocation / 4;

                    int externalSubRoutineCommandsPointerLocation = PsaFile.FileContent[externalSubRoutineCodeBlockLocation];
                    if (externalSubRoutineCommandsPointerLocation >= 8096
                        && externalSubRoutineCommandsPointerLocation < PsaFile.DataSectionSize
                        && removedPsaCommand.CommandParametersLocation == externalSubRoutineCommandsPointerLocation)
                    {
                        int commandExternalDataPointerValue = removedPsaCommand.GetCommandParameterValueLocation(0);
                        if (PsaFile.FileContent[commandExternalDataPointerValue] >= 8096
                            && PsaFile.FileContent[commandExternalDataPointerValue] < PsaFile.DataSectionSize
                            && PsaFile.FileContent[commandExternalDataPointerValue] % 4 == 0)
                        {
                            PsaFile.FileOtherData[externalSubRoutineCodeBlockLocation] = PsaFile.FileContent[commandExternalDataPointerValue];
                        }
                        else
                        {
                            PsaFile.FileOtherData[externalSubRoutineCodeBlockLocation] = -1;
                        }
                        break;
                    }
                }
            }
        }
    }
}
