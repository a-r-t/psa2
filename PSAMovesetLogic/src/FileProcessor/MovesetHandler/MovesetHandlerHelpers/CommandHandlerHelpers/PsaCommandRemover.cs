using PSA2MovesetLogic.src.Models.Fighter;
using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    /// <summary>
    /// This class is responsible for handling command removal in a code block
    /// <para>If the last command in a code block is removed, a bit of extra work is done to remove command allocation for that code block in order to save room</para>
    /// </summary>
    public class PsaCommandRemover
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }
        public PsaFileHelperMethods PsaFileHelperMethods { get; private set; }

        public PsaCommandRemover(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation, PsaFileHelperMethods psaFileHelperMethods)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            PsaFileHelperMethods = psaFileHelperMethods;
        }

        /// <summary>
        /// Removes a command from a code block
        /// <para>If the removed command is the last command in the code block, the command space allocation is removed as well</para>
        /// </summary>
        /// <param name="codeBlock">The code block that contains the command to remove</param>
        /// <param name="commandLocation">The location of the command to remove in the code block</param>
        /// <param name="removedPsaCommand">The psa command that is going to be removed</param>
        public void RemoveCommand(CodeBlock codeBlock, int commandLocation, PsaCommand removedPsaCommand)
        {
            // if command to remove has params
            if (removedPsaCommand.NumberOfParams != 0)
            {
                // delete the command's parameters
                DeleteCommandParameters(removedPsaCommand, commandLocation);
            }

            // if code block will still have commands left in it after removing this command
            if (codeBlock.NumberOfCommands > 1)
            {
                // delete the command from the code
                DeleteCommandAndCloseGap(codeBlock, commandLocation);
            }

            // if removing this command will result in code block having no commands (essentially the last command in the block is being removed)
            else
            {
                // set removed command location to free space
                PsaFile.DataSection[commandLocation] = Constants.FADEF00D;
                PsaFile.DataSection[commandLocation + 1] = Constants.FADEF00D;

                // set end of code block commands space to free space
                // this location is normally what marks the "end" of the commands in a code block (using 0s)
                // since it no longer represents a code block's commands space, it can be changed to free space
                int codeBlockCommandsEnd = codeBlock.GetCommandsEndLocation();
                PsaFile.DataSection[codeBlockCommandsEnd] = Constants.FADEF00D;
                PsaFile.DataSection[codeBlockCommandsEnd + 1] = Constants.FADEF00D;

                // code block location is set to 0 since a code block with no commands is pointless anyway
                PsaFile.DataSection[codeBlock.Location] = 0;

                // remove any existing pointer references to the cdoe block
                int pointerToCodeBlockLocation = codeBlock.Location * 4;
                PsaFileHelperMethods.RemoveOffsetFromOffsetInterlockTracker(pointerToCodeBlockLocation);
            }

            // look through the rest of the PSA commands for something that points to a removed command or any of its params and remove that pointer
            int codeBlockCommandsEndLocation = codeBlock.CommandsPointerLocation + codeBlock.NumberOfCommands * 8;
            int codeBlockCommandsStartLocation = codeBlockCommandsEndLocation - 8;
            for (int i = CodeBlockDataStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
            {
                if (PsaFile.DataSection[i] >= codeBlockCommandsStartLocation && PsaFile.DataSection[i] <= codeBlockCommandsEndLocation)
                {
                    DeleteOffsetInterlockData(i);
                }
            }

            PsaFileHelperMethods.UpdateMovesetHeaders();
        }

        /// <summary>
        /// Deletes all of a command's parameters
        /// </summary>
        /// <param name="removedPsaCommand">command being removed (which will have its params deleted)</param>
        /// <param name="commandLocation">location of the command being removed in the code block</param>
        private void DeleteCommandParameters(PsaCommand removedPsaCommand, int commandLocation)
        {
            // if command parameters location is within the valid space for code block commands
            if (removedPsaCommand.CommandParametersValuesLocation >= CodeBlockDataStartLocation && removedPsaCommand.CommandParametersValuesLocation < PsaFile.DataSectionSizeBytes)
            {
                // remove any pointers to the parameters location (since it won't exist anymore)
                int commandParametersPointerLocation = commandLocation * 4 + 4;
                PsaFileHelperMethods.RemoveOffsetFromOffsetInterlockTracker(commandParametersPointerLocation);

                // iterates through each param the command had
                for (int i = 0; i < removedPsaCommand.NumberOfParams * 2; i += 2)
                {
                    int parameterType = removedPsaCommand.Parameters[i / 2].Type;
                    int parameterValue = removedPsaCommand.Parameters[i / 2].Value;

                    // If the pointer param was an external subroutine (from the external data table) (such as Mario's Up B, the item ones like the home run bat, etc), 
                    // some additional work needs to be done to remove references to it from the external data table
                    if (parameterType == 2)
                    {
                        // Get the pointer location of the param value that is currently pointing to another location
                        int commandParamPointerValueLocation = removedPsaCommand.CommandParametersLocation + 4;


                        // Attempt to remove the above offset from the offset interlock tracker
                        bool wasOffsetRemoved = PsaFileHelperMethods.RemoveOffsetFromOffsetInterlockTracker(commandParamPointerValueLocation);

                        // If offset was not successfully removed, it means it either doesn't exist or is an external subroutine
                        // The below method call UpdateExternalPointerLogic will do some necessary work if the offset turns out to be an external subroutine call
                        if (!wasOffsetRemoved)
                        {
                            UpdateExternalPointerLogic(removedPsaCommand, commandParamPointerValueLocation);
                        }

                    }

                    // replace removed param values with free space (FADEF00D)
                    // first replaces the param type, second replaces the param value
                    PsaFile.DataSection[removedPsaCommand.CommandParametersValuesLocation + i] = Constants.FADEF00D;
                    PsaFile.DataSection[removedPsaCommand.CommandParametersValuesLocation + i + 1] = Constants.FADEF00D;
                }
            }
        }

        /// <summary>
        /// Deletes a command from a code block
        /// <para>all other commands in the code block after the removed command are shifted backwards by one to close the gap</para>
        /// <para>think of it like a list data structure where removing an item causes the list to adjust by shifting indexes</para>
        /// </summary>
        /// <param name="codeBlock">The code block of the command being removed</param>
        /// <param name="commandLocation">The location in the code block of the command being removed</param>
        private void DeleteCommandAndCloseGap(CodeBlock codeBlock, int commandLocation)
        {
            // get the command to be removed's index in the code block (i.e. first command is index 0, second command is index 1, etc)
            int removedCommandIndex = codeBlock.GetPsaCommandIndexByLocation(commandLocation);

            // starting from the removed command, iterate through each command in the code block
            // this loop will shift the commands all over by 1, closing the gap where the removed command used to exist
            for (int commandIndex = removedCommandIndex; commandIndex < codeBlock.PsaCommands.Count - 1; commandIndex++)
            {
                // get the next psa command that comes after the current one
                PsaCommand nextPsaCommand = codeBlock.PsaCommands[commandIndex + 1];

                // get the command location of the current command
                int currentCommandLocation = codeBlock.GetPsaCommandLocation(commandIndex);

                // if next command that comes after the current one has params
                if (nextPsaCommand.NumberOfParams > 0)
                {
                    // adjust pointer location for next command to point to the command before it (since all commands are shifted over now)
                    int nextCommandPointerLocation = currentCommandLocation * 4 + 12;

                    for (int j = 0; j < PsaFile.OffsetSection.Count; j++)
                    {
                        if (PsaFile.OffsetSection[j] == nextCommandPointerLocation)
                        {
                            PsaFile.OffsetSection[j] -= 8;
                            break;
                        }
                    }
                }

                // this is what actually shifts the command - the instruction and param pointer is shifted to the previous command location
                PsaFile.DataSection[currentCommandLocation] = PsaFile.DataSection[currentCommandLocation + 2];
                PsaFile.DataSection[currentCommandLocation + 1] = PsaFile.DataSection[currentCommandLocation + 3];
            }

            // mark the old last command location (that was shifted up in the the loop above) as the end of the code block
            int lastCommandLocation = codeBlock.GetPsaCommandLocation(codeBlock.NumberOfCommands - 1);
            PsaFile.DataSection[lastCommandLocation] = 0;
            PsaFile.DataSection[lastCommandLocation + 1] = 0;

            // this space is left over from all of the commands shifting -- it is now unused so it can be set to free space
            PsaFile.DataSection[lastCommandLocation + 2] = Constants.FADEF00D;
            PsaFile.DataSection[lastCommandLocation + 3] = Constants.FADEF00D;
        }

        /// <summary>
        /// Deletes offset interlock tracker pointers that pointed to the removed command
        /// <para>DelILData method in PSA-C</para>
        /// </summary>
        /// <param name="fileContentIndex">the index of the pointer in the PSA File's File Content</param>
        private void DeleteOffsetInterlockData(int fileContentIndex)
        {
            // check if file index location is being pointed to by an offset entry
            // this will look at all commands that have a pointer 
            int fileContentPointerLocation = fileContentIndex * 4;
            bool offsetFound = false;
            int offsetIndex = 0;
            for (int i = 0; i < PsaFile.OffsetSection.Count; i++)
            {
                if (PsaFile.OffsetSection[i] == fileContentPointerLocation)
                {
                    offsetFound = true;
                    offsetIndex = i;
                    break;
                }
            }

            // if offset is being pointed to
            if (offsetFound)
            {
                // remove parm value since location was removed and offset pointing to that param value
                PsaFile.DataSection[fileContentIndex] = 0;
                //PsaFile.OffsetSection[offsetIndex] = 16777216; // 100 0000
                PsaFile.OffsetSection.RemoveAt(offsetIndex);

                // if param type is pointer
                if (PsaFile.DataSection[fileContentIndex - 1] == 2)
                {
                    // remove param type
                    PsaFile.DataSection[fileContentIndex - 1] = 0;
                    fileContentPointerLocation -= 4;

                    // find if anything was pointing to the command itself that had the pointer
                    int commandParamValueLocation = 2025;
                    offsetFound = false;
                    for (int i = 0; i < PsaFile.OffsetSection.Count; i++)
                    {
                        if (PsaFile.OffsetSection[i] == fileContentPointerLocation)
                        {
                            offsetFound = true;
                            commandParamValueLocation = i;
                            break;
                        }
                    }

                    // if offset found pointing to the command
                    if (offsetFound)
                    {
                        // if command was subroutine or goto
                        // 459008 is subroutine instruction, 590080 is goto instruction
                        if (PsaFile.DataSection[commandParamValueLocation - 1] == 459008 || PsaFile.DataSection[commandParamValueLocation - 1] == 590080)
                        {
                            // replace command to a "nop" instruction 
                            // TODO: I don't actually agree with doing this
                            PsaFile.DataSection[commandParamValueLocation - 1] = Constants.NOP;
                            PsaFile.DataSection[commandParamValueLocation] = 0;

                            // set to freespace
                            PsaFile.DataSection[fileContentIndex] = Constants.FADEF00D;
                            PsaFile.DataSection[fileContentIndex - 1] = Constants.FADEF00D;

                            // remove any offset pointer to the command's param value location (since all params were removed when changing to NOP)
                            int commandParamValuePointerLocation = commandParamValueLocation * 4;

                            PsaFileHelperMethods.RemoveOffsetFromOffsetInterlockTracker(commandParamValuePointerLocation);
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
                int externalSubRoutineLocation = PsaFile.DataTableSections[externalSubRoutineLocationIndex];
                if (externalSubRoutineLocation >= 8096 && externalSubRoutineLocation < PsaFile.DataSectionSize)
                {
                    // if the param being removed was pointing to an external subroutine
                    if (commandParamPointerValueLocation == externalSubRoutineLocation)
                    {
                        // get the location of the value of the param
                        int commandExternalSubroutineValueLocation = commandParamPointerValueLocation / 4;

                        // This stops the external data table from pointing to the command param pointer value location
                        if (PsaFile.DataSection[commandExternalSubroutineValueLocation] >= 8096
                            && PsaFile.DataSection[commandExternalSubroutineValueLocation] < PsaFile.DataSectionSize
                            && PsaFile.DataSection[commandExternalSubroutineValueLocation] % 4 == 0)
                        {
                            PsaFile.DataTableSections[externalSubRoutineLocationIndex] = PsaFile.DataSection[commandExternalSubroutineValueLocation];
                        }
                        else
                        {
                            PsaFile.DataTableSections[externalSubRoutineLocationIndex] = -1;
                        }
                        break;
                    }

                    // same deal as above for the most part, but I am NOT sure what location "externalSubRoutineCodeBlockLocation" actually is (I don't think it's right currently)
                    // it's tough to test further because I also can't really figure out how to get this code to trigger at the moment, so I'm just going to leave this as is
                    int externalSubRoutineCodeBlockLocation = externalSubRoutineLocation / 4;

                    int externalSubRoutineCommandsPointerLocation = PsaFile.DataSection[externalSubRoutineCodeBlockLocation];
                    if (externalSubRoutineCommandsPointerLocation >= 8096
                        && externalSubRoutineCommandsPointerLocation < PsaFile.DataSectionSize
                        && removedPsaCommand.CommandParametersLocation == externalSubRoutineCommandsPointerLocation)
                    {
                        int commandExternalDataPointerValue = removedPsaCommand.GetCommandParameterValueLocation(0);
                        if (PsaFile.DataSection[commandExternalDataPointerValue] >= 8096
                            && PsaFile.DataSection[commandExternalDataPointerValue] < PsaFile.DataSectionSize
                            && PsaFile.DataSection[commandExternalDataPointerValue] % 4 == 0)
                        {
                            PsaFile.DataTableSections[externalSubRoutineCodeBlockLocation] = PsaFile.DataSection[commandExternalDataPointerValue];
                        }
                        else
                        {
                            PsaFile.DataTableSections[externalSubRoutineCodeBlockLocation] = -1;
                        }
                        break;
                    }
                }
            }
        }
    }
}
