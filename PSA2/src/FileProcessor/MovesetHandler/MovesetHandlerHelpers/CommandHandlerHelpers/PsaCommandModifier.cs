using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    /// <summary>
    /// This class is responsible for handling command modification in a code block
    /// <para>This includes changing the command entirely to a new instruction or just changing parameter types/values</para>
    /// </summary>
    public class PsaCommandModifier
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }
        public PsaFileHelperMethods PsaFileHelperMethods { get; private set; }

        public PsaCommandModifier(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation, PsaFileHelperMethods psaFileHelperMethods)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            PsaFileHelperMethods = psaFileHelperMethods;
        }

        /// <summary>
        /// Modify a chosen command in a code block
        /// <para>If the new command has more params than the old command (or the old command did not have params in the first place), the params location space may need to be expanded/relocated</para>
        /// </summary>
        /// <param name="commandLocation">The location of the command in the code block</param>
        /// <param name="oldPsaCommand">The old psa command (the one that is being modified)</param>
        /// <param name="newPsaCommand">The new psa command (the one that is replcaing the old psa command)</param>
        public void ModifyCommand(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            if (PsaFile.DataSection[commandLocation + 1] >= 0 && PsaFile.DataSection[commandLocation + 1] < PsaFile.DataSectionSize)
            {
                // if there were no command params on the previous command
                if (oldPsaCommand.GetCommandParamsSize() == 0)
                {
                    // if there are no command params on new command, it's simply a swap out of command instructions with nothing else needed
                    if (newPsaCommand.GetCommandParamsSize() == 0)
                    {
                        PsaFile.DataSection[commandLocation] = newPsaCommand.Instruction;
                    }

                    // if there are command params on new command, create space for this new params location
                    else
                    {
                        CreateNewCommandParametersLocation(commandLocation, newPsaCommand);
                        PsaFileHelperMethods.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
                    }
                }

                // if there are existing command params, they need to be overwritten, or the entire action may need to be relocated if not enough room for all the params
                else
                {
                    ModifyExistingCommandParametersLocation(commandLocation, oldPsaCommand, newPsaCommand);
                    PsaFileHelperMethods.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
                }
            }
            else
            {
                throw new ApplicationException("Cannot modify command because it is not located within the data section");
            }

        }
        /// <summary>
        /// Creates a new parameters section for a command
        /// <para>This is called if the old psa command does not have any parameters, but the new command does -- that means a parameters section for the command needs to be created and pointed to</para>
        /// </summary>
        /// <param name="commandLocation"></param>
        /// <param name="newPsaCommand"></param>
        private void CreateNewCommandParametersLocation(int commandLocation, PsaCommand newPsaCommand)
        {
            int newCommandParamsValuesSize = newPsaCommand.GetCommandParamsSize();

            // determine where the new parameters values location will be by finding enough free space to fit it
            int newCommandParametersValuesLocation = PsaFileHelperMethods.FindLocationWithAmountOfFreeSpace(CodeBlockDataStartLocation, newCommandParamsValuesSize);

            // if the only place with free space found is after the limits of the data section, expand the data section to make room
            if (newCommandParametersValuesLocation >= PsaFile.DataSectionSizeBytes)
            {
                // TODO: Refactor this code
                for (int i = 0; i < newCommandParamsValuesSize; i++)
                {
                    PsaFile.DataSection.Add(0);
                }
                newCommandParametersValuesLocation = PsaFile.DataSectionSizeBytes;
                if (PsaFile.DataSection[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    newCommandParametersValuesLocation -= 2;
                    PsaFile.DataSection[PsaFile.DataSectionSizeBytes + newCommandParamsValuesSize - 2] = Constants.FADE0D8A;
                    PsaFile.DataSection[PsaFile.DataSectionSizeBytes + newCommandParamsValuesSize - 1] = PsaFile.DataSection[PsaFile.DataSectionSizeBytes - 1];
                }
                PsaFile.DataSectionSizeBytes += newCommandParamsValuesSize;
            }

            // go through each parameter in the new command one at a time and place it in the newly created parameters values location (type, value)
            for (int paramIndex = 0; paramIndex < newPsaCommand.NumberOfParams; paramIndex++)
            {
                int paramTypeLocation = paramIndex * 2;
                int paramValueLocation = paramIndex * 2 + 1;
                // if command param type is "Pointer" and the param value is greater than 0 (meaning it points to something)
                if (newPsaCommand.Parameters[paramIndex].Type == 2 && newPsaCommand.Parameters[paramIndex].Value > 0)
                {
                    int commandParameterPointerLocation = (newCommandParametersValuesLocation + paramTypeLocation) * 4 + 4;
                    PsaFile.OffsetSection.Add(commandParameterPointerLocation);
                }
                PsaFile.DataSection[newCommandParametersValuesLocation + paramTypeLocation] = newPsaCommand.Parameters[paramIndex].Type;
                PsaFile.DataSection[newCommandParametersValuesLocation + paramValueLocation] = newPsaCommand.Parameters[paramIndex].Value;
            }

            // place new command instruction at command location
            PsaFile.DataSection[commandLocation] = newPsaCommand.Instruction;

            // set pointer to command parameters location
            int newCommandParametersLocation = newCommandParametersValuesLocation * 4;
            PsaFile.DataSection[commandLocation + 1] = newCommandParametersLocation;

            // set pointer to command parameters pointer location in the offset interlock tracker
            int newCommandParametersPointerLocation = commandLocation * 4 + 4;
            PsaFile.OffsetSection.Add(newCommandParametersPointerLocation);
        }

        /// <summary>
        /// Modify a command with existing parameters
        /// <para>Case 1: both old and new command have same number of parameters, which is an easy params replace 1 by 1</para>
        /// <para>Case 2: new command has less params than old command, which is the same as case 1, but the unused param space needs to be converted to free space (FADEF00D)</para>
        /// <para>Case 3: new command has more params than old command, which will result in finding a new location for the parameters that has enough space</para>
        /// </summary>
        /// <param name="commandLocation">The location of the command in the code block</param>
        /// <param name="oldPsaCommand">The old psa command (the one that is being modified)</param>
        /// <param name="newPsaCommand">The new psa command (the one that is replcaing the old psa command)</param>
        private void ModifyExistingCommandParametersLocation(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            // If modifying a command that pointed to another location (the "Pointer" param type), the pointer location needs to be removed from the offset interlock tracker
            // If the pointer param was an external subroutine (from the external data table) (such as Mario's Up B, the item ones like the home run bat, etc), 
            // some additional work needs to be done to remove references to it from the external data table
            // This if statement checks if the first parameter is of type Pointer or the second, which matches commands like "goto", "subroutine", and "concurrent subroutine"
            if (PsaFile.DataSection[oldPsaCommand.CommandParametersValuesLocation] == 2 ||
                (PsaFile.DataSection[oldPsaCommand.CommandParametersValuesLocation + 2] == 2 && oldPsaCommand.NumberOfParams == 2))
            {
                // Get the pointer location of the param value that is currently pointing to another location
                int commandParamPointerValueLocation = PsaFile.DataSection[oldPsaCommand.CommandParametersValuesLocation] == 2
                    ? oldPsaCommand.CommandParametersLocation + 4
                    : oldPsaCommand.CommandParametersLocation + 12;

                // Attempt to remove the above offset from the offset interlock tracker
                bool wasOffsetRemoved = PsaFileHelperMethods.RemoveOffsetFromOffsetInterlockTracker(commandParamPointerValueLocation);

                // If offset was not successfully removed, it means it either doesn't exist or is an external subroutine
                // The below method call UpdateExternalPointerLogic will do some necessary work if the offset turns out to be an external subroutine call
                if (!wasOffsetRemoved)
                {
                    UpdateExternalPointerLogic(oldPsaCommand, commandParamPointerValueLocation);
                }
            }

            // Replace old param values with free space (FADEF00D)
            for (int paramIndex = 0; paramIndex < oldPsaCommand.NumberOfParams; paramIndex++)
            {
                int commandParameterTypeLocation = oldPsaCommand.GetCommandParameterTypeLocation(paramIndex);
                int commandParameterValueLocation = oldPsaCommand.GetCommandParameterValueLocation(paramIndex);
                PsaFile.DataSection[commandParameterTypeLocation] = Constants.FADEF00D;
                PsaFile.DataSection[commandParameterValueLocation] = Constants.FADEF00D;
            }

            // set new command instruction
            PsaFile.DataSection[commandLocation] = newPsaCommand.Instruction;

            // if new command has no parameters, set pointer to parameters to nothing (which is 0) and remove the pointer to the parameters from the offset interlock
            if (newPsaCommand.NumberOfParams == 0)
            {
                // remove pointer to params since this command has no params
                PsaFile.DataSection[commandLocation + 1] = 0;

                // remove offset from interlock tracker since it no longer exists
                int commandParametersPointerLocation = commandLocation * 4 + 4; // rmv
                PsaFileHelperMethods.RemoveOffsetFromOffsetInterlockTracker(commandParametersPointerLocation);
            }

            // if new command has parameters
            else
            {
                // if new command has a less or equal parameter count to the old command, the parameter values location does not need to be relocated
                // if the new command has a higher parameter count than the old command, the paramter values location will need to be expanded/relocated to have enough room for all the parameters
                int newCommandParametersValuesLocation = oldPsaCommand.NumberOfParams >= newPsaCommand.NumberOfParams
                    ? oldPsaCommand.CommandParametersValuesLocation
                    : ExpandCommandParametersSection(oldPsaCommand, newPsaCommand);

                PsaFile.DataSection[commandLocation + 1] = newCommandParametersValuesLocation * 4;


                // put param values in new param values location one by one
                for (int paramIndex = 0; paramIndex < newPsaCommand.NumberOfParams; paramIndex++)
                {
                    int paramTypeLocation = paramIndex * 2;
                    int paramValueLocation = paramIndex * 2 + 1;

                    // if command param type is Pointer and it actually points to something
                    if (newPsaCommand.Parameters[paramIndex].Type == 2 && newPsaCommand.Parameters[paramIndex].Value > 0)
                    {
                        // I believe this points to the location of the param value (if the param is a pointer)
                        int commandParameterPointerValueLocation = (newCommandParametersValuesLocation + paramTypeLocation) * 4 + 4;
                        PsaFile.OffsetSection.Add(commandParameterPointerValueLocation);
                    }

                    // place parameter type in value in proper place
                    PsaFile.DataSection[newCommandParametersValuesLocation + paramTypeLocation] = newPsaCommand.Parameters[paramIndex].Type;
                    PsaFile.DataSection[newCommandParametersValuesLocation + paramValueLocation] = newPsaCommand.Parameters[paramIndex].Value;
                }
            }
        }

        /// <summary>
        /// This method is called when attempting to remove a param value that once pointed to another location (Pointer type) that MIGHT be an external subroutine call
        /// <para>If it is determined to be a call to an external subroutine, the external subroutine data table is updated to no longer pointer to it</para>
        /// </summary>
        /// <param name="oldPsaCommand">The psa command being modified</param>
        /// <param name="commandParamPointerValueLocation">The pointer to the param's value location</param>
        private void UpdateExternalPointerLogic(PsaCommand oldPsaCommand, int commandParamPointerValueLocation)
        {
            // iterate through each external subroutine in the external data table
            for (int externalSubRoutineIndex = 0; externalSubRoutineIndex < PsaFile.NumberOfExternalSubRoutines; externalSubRoutineIndex++)
            {
                // get a external subroutine's pointer
                int externalSubRoutineLocationIndex = (PsaFile.NumberOfDataTableEntries + externalSubRoutineIndex) * 2;
                int externalSubRoutineLocation = PsaFile.RemainingSections[externalSubRoutineLocationIndex];
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
                            PsaFile.RemainingSections[externalSubRoutineLocationIndex] = PsaFile.DataSection[commandExternalSubroutineValueLocation];
                        }
                        else
                        {
                            PsaFile.RemainingSections[externalSubRoutineLocationIndex] = -1;
                        }
                        break;
                    }

                    // same deal as above for the most part, but I am NOT sure what location "externalSubRoutineCodeBlockLocation" actually is (I don't think it's right currently)
                    // it's tough to test further because I also can't really figure out how to get this code to trigger at the moment, so I'm just going to leave this as is
                    int externalSubRoutineCodeBlockLocation = externalSubRoutineLocation / 4;

                    int externalSubRoutineCommandsPointerLocation = PsaFile.DataSection[externalSubRoutineCodeBlockLocation];
                    if (externalSubRoutineCommandsPointerLocation >= 8096
                        && externalSubRoutineCommandsPointerLocation < PsaFile.DataSectionSize 
                        && oldPsaCommand.CommandParametersLocation == externalSubRoutineCommandsPointerLocation)
                    {
                        int commandExternalDataPointerValue = oldPsaCommand.GetCommandParameterValueLocation(0);
                        if (PsaFile.DataSection[commandExternalDataPointerValue] >= 8096 
                            && PsaFile.DataSection[commandExternalDataPointerValue] < PsaFile.DataSectionSize 
                            && PsaFile.DataSection[commandExternalDataPointerValue] % 4 == 0)
                        { 
                            PsaFile.RemainingSections[externalSubRoutineCodeBlockLocation] = PsaFile.DataSection[commandExternalDataPointerValue];
                        }
                        else
                        {
                            PsaFile.RemainingSections[externalSubRoutineCodeBlockLocation] = -1;
                        }
                        break;
                    }
                }
            }    
        }

        /// <summary>
        /// This will expand (usually includes relocating) the command's parameters location if the new psa command has more parameters than the old psa command
        /// <para>If possible, it will not relocate the params section if enough free space can be found</para>
        /// <para>If it can't find enough free space in the data section, it resorts to expanding the data section</para>
        /// </summary>
        /// <param name="oldPsaCommand">The psa command being modified</param>
        /// <param name="newPsaCommand">The psa command replacing the old psa command</param>
        /// <returns>The new location of the command parameters section (if section does not end up getting relocated, it will return the original param section location of the old psa command)</returns>
        private int ExpandCommandParametersSection(PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            int oldCommandParamsSize = oldPsaCommand.GetCommandParamsSize();
            int newCommandParamsSize = newPsaCommand.GetCommandParamsSize();

            // determine how much space is currently available for params
            // to start, there is the original amount of space that the old psa command had
            int currentCommandParamSize = oldCommandParamsSize;

            // if adding one command to the current parameters location would cause it to go over the data section limit, expand the data section by the amount needed
            if (oldPsaCommand.CommandParametersValuesLocation + oldCommandParamsSize + 5 > PsaFile.DataSectionSizeBytes)
            {
                // difference between the new size needed and the old size
                int commandSizeDifference = newCommandParamsSize - oldCommandParamsSize;

                // if at the end of the data section, expand data section by the required amount
                if (PsaFile.DataSection[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    // TODO: Refactor this code
                    for (int i = 0; i < commandSizeDifference; i++)
                    {
                        PsaFile.DataSection.Add(0);
                    }
                    PsaFile.DataSection[PsaFile.DataSectionSizeBytes + commandSizeDifference - 2] = Constants.FADE0D8A;
                    PsaFile.DataSection[PsaFile.DataSectionSizeBytes + commandSizeDifference - 1] = PsaFile.DataSection[PsaFile.DataSectionSizeBytes - 1];
                    PsaFile.DataSectionSizeBytes += commandSizeDifference;
                    currentCommandParamSize = newCommandParamsSize;
                }
                // if adding one additional command would cause going over the data section size, just straight up expand the data section size
                // not entirely sure what differs this case from the others...
                else if (oldPsaCommand.CommandParametersValuesLocation + oldCommandParamsSize + 3 > PsaFile.DataSectionSizeBytes)
                {
                    // TODO: Refactor this code
                    for (int i = 0; i < commandSizeDifference; i++)
                    {
                        PsaFile.DataSection.Add(0);
                    }

                    PsaFile.DataSectionSizeBytes += commandSizeDifference;
                    currentCommandParamSize = newCommandParamsSize;
                }
            }

            // add 1 additional block of space for each free space (FADEF00D) that comes afterwards
            while (currentCommandParamSize < newCommandParamsSize
                && PsaFile.DataSection[oldPsaCommand.CommandParametersValuesLocation + currentCommandParamSize] == Constants.FADEF00D)
            {
                currentCommandParamSize++;
            }

            // if expanding the data section was applicable or enough free space was found from the above loop
            // the original location of the old psa command now has enough room to hold all necessary parameters for the new command
            if (currentCommandParamSize >= newCommandParamsSize)
            {
                return oldPsaCommand.CommandParametersValuesLocation;
            }

            // if the amount of space available at the current moment is STILL not enough for all of the new params
            // the parameter location is going to need to be relocated to a place in the file with enough free space
            else
            {
                // find a location in the file with enough free space
                int newCommandParametersValuesLocation = PsaFileHelperMethods.FindLocationWithAmountOfFreeSpace(CodeBlockDataStartLocation, newCommandParamsSize);

                // if new location goes over data section limit, expand data sectoin
                if (newCommandParametersValuesLocation >= PsaFile.DataSectionSizeBytes)
                {
                    // TODO: Refactor this code
                    for (int i = 0; i < newCommandParamsSize; i++)
                    {
                        PsaFile.DataSection.Add(0);
                    }

                    newCommandParametersValuesLocation = PsaFile.DataSectionSizeBytes;
                    if (PsaFile.DataSection[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                    {
                        newCommandParametersValuesLocation -= 2;
                        PsaFile.DataSection[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 2] = Constants.FADE0D8A;
                        PsaFile.DataSection[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 1] = PsaFile.DataSection[PsaFile.DataSectionSizeBytes - 1];
                    }
                    PsaFile.DataSectionSizeBytes += newCommandParamsSize;
                }
                return newCommandParametersValuesLocation;  
            }
        }
    }
}
