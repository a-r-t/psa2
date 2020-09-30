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
    public class PsaCommandModifier
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandModifier(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation, PsaCommandParser psaCommandParser)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            PsaCommandParser = psaCommandParser;
        }

        /********************************
         * MODIFYING COMMAND IN ACTION  *
         * ******************************/
        /// <summary>
        /// Modify a psa command to a new psa command
        /// </summary>
        /// <param name="commandLocation">location of command</param>
        /// <param name="oldPsaCommand">old psa command that is to be replaced</param>
        /// <param name="newPsaCommand">new psa command that is replacing the old command</param>
        public void ModifyCommand(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            if (PsaFile.FileContent[commandLocation + 1] >= 0 && PsaFile.FileContent[commandLocation + 1] < PsaFile.DataSectionSize)
            {
                // event modify method
                int oldCommandParamsSize = oldPsaCommand.GetCommandParamsSize(); // k

                // if there were no command params on the previous command
                if (oldCommandParamsSize == 0)
                {
                    int newCommandParamsSize = newPsaCommand.GetCommandParamsSize(); // m

                    // if there are no command params on new command, it's simply a swap out of command instructions with nothing else needed
                    if (newCommandParamsSize == 0)
                    {
                        PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;
                    }

                    // if there are command params on new command, create space for this new params location
                    else
                    {
                        CreateNewCommandParametersLocation(commandLocation, newPsaCommand);
                        PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
                    }
                }

                // if there are existing command params, they need to be overwritten, or the entire action may need to be relocated if not enough room for all the params
                else
                {
                    ModifyExistingCommandParametersLocation(commandLocation, oldPsaCommand, newPsaCommand);
                    PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
                }
            }
            else
            {
                throw new ApplicationException("Cannot modify event...not sure why just can't okay");
            }

        }

        public void CreateNewCommandParametersLocation(int commandLocation, PsaCommand newPsaCommand)
        {
            int newCommandParamsValuesSize = newPsaCommand.GetCommandParamsSize();

            // determine stopping point, which is where new command params will be added (finds room where number of params can all fit)
            int newCommandParametersValuesLocation = PsaFile.FindLocationWithAmountOfFreeSpace(CodeBlockDataStartLocation, newCommandParamsValuesSize);

            // if adding command params location causes data to go beyond data section limit, increase data section size
            if (newCommandParametersValuesLocation >= PsaFile.DataSectionSizeBytes)
            {
                newCommandParametersValuesLocation = PsaFile.DataSectionSizeBytes;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    newCommandParametersValuesLocation -= 2;
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsValuesSize - 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsValuesSize - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                }
                PsaFile.DataSectionSizeBytes += newCommandParamsValuesSize;
            }

            for (int paramIndex = 0; paramIndex < newPsaCommand.NumberOfParams; paramIndex++)
            {
                int paramTypeLocation = paramIndex * 2;
                int paramValueLocation = paramIndex * 2 + 1;
                // if command param type is "Pointer" and the param value is greater than 0 (meaning it points to something)
                if (newPsaCommand.Parameters[paramIndex].Type == 2 && newPsaCommand.Parameters[paramIndex].Value > 0)
                {
                    int commandParameterPointerLocation = (newCommandParametersValuesLocation + paramTypeLocation) * 4 + 4;
                    PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = commandParameterPointerLocation;
                    PsaFile.NumberOfOffsetEntries++;
                }
                PsaFile.FileContent[newCommandParametersValuesLocation + paramTypeLocation] = newPsaCommand.Parameters[paramIndex].Type;
                PsaFile.FileContent[newCommandParametersValuesLocation + paramValueLocation] = newPsaCommand.Parameters[paramIndex].Value;
            }

            PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;
            int newCommandParametersLocation = newCommandParametersValuesLocation * 4;
            PsaFile.FileContent[commandLocation + 1] = newCommandParametersLocation;

            int newCommandParametersPointerLocation = commandLocation * 4 + 4;
            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = newCommandParametersPointerLocation;
            PsaFile.NumberOfOffsetEntries++;
        }

        /// <summary>
        /// Modify a command with existing parameters
        /// </summary>
        /// <param name="commandLocation">location of command</param>
        /// <param name="oldPsaCommand">old psa command that is going to be changed</param>
        /// <param name="newPsaCommand">new psa command that is replacing the old one</param>
        public void ModifyExistingCommandParametersLocation(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            if (PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation] == 2 ||
                (PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + 2] == 2 && oldPsaCommand.NumberOfParams == 2))
            {
                // I think this is what it is, not positive
                int commandParamExternalSubRoutineLocation = PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation] == 2
                    ? oldPsaCommand.CommandParametersLocation + 4
                    : oldPsaCommand.CommandParametersLocation + 12;

                bool wasOffsetRemoved = RemoveOffsetFromOffsetInterlockTracker(commandParamExternalSubRoutineLocation);

                // This will trigger if command was pointing to an external subroutine (like Mario's Up B has one, the home run bat has one, etc)
                if (!wasOffsetRemoved)
                {
                    UpdateExternalPointerLogic(oldPsaCommand, commandLocation, commandParamExternalSubRoutineLocation);
                }
            }

            // Replace old param values with free space (FADEF00D)
            for (int paramIndex = 0; paramIndex < oldPsaCommand.NumberOfParams; paramIndex++)
            {
                PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + (paramIndex * 2)] = Constants.FADEF00D;
                PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + (paramIndex * 2) + 1] = Constants.FADEF00D;
            }

            // set new command instruction
            PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;

            // if new command has no parameters, set pointer to parameters to nothing and remove the pointer to the parameters from the offset interlock
            if (newPsaCommand.NumberOfParams == 0)
            {
                // remove pointer to params since this command has no params
                PsaFile.FileContent[commandLocation + 1] = 0;

                // remove offset from interlock tracker since it no longer exists
                int commandParamValuePointerLocation = commandLocation * 4 + 4; // rmv
                RemoveOffsetFromOffsetInterlockTracker(commandParamValuePointerLocation);
            }
            // if new command has parameters
            else
            {
                // if new command has a less or equal parameter count to the old command, the parameter values location does not need to be relocated
                // if the new command has a higher parameter count than the old comman,d the paramter values location will need to be expanded/relocated to have enough room for all the parameters
                int newCommandParametersvaluesLocation = oldPsaCommand.NumberOfParams >= newPsaCommand.NumberOfParams
                    ? oldPsaCommand.CommandParametersValuesLocation
                    : ExpandCommandParametersSection(commandLocation, oldPsaCommand, newPsaCommand);

                // put param values in new param values location one by one
                for (int paramIndex = 0; paramIndex < newPsaCommand.NumberOfParams; paramIndex++)
                {
                    int paramTypeLocation = paramIndex * 2;
                    int paramValueLocation = paramIndex * 2 + 1;

                    // if command param type is Pointer and it actually points to something
                    if (newPsaCommand.Parameters[paramIndex].Type == 2 && newPsaCommand.Parameters[paramIndex].Value > 0)
                    {
                        int commandParameterPointerLocation = (newCommandParametersvaluesLocation + paramTypeLocation) * 4 + 4;
                        PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = commandParameterPointerLocation;
                        PsaFile.NumberOfOffsetEntries++;
                    }

                    // place parameter type in value in proper place
                    PsaFile.FileContent[newCommandParametersvaluesLocation + paramTypeLocation] = newPsaCommand.Parameters[paramIndex].Type;
                    PsaFile.FileContent[newCommandParametersvaluesLocation + paramValueLocation] = newPsaCommand.Parameters[paramIndex].Value;
                }
            }
        }

        /// <summary>
        /// This will remove an offset location from the tracker that is no longer being pointed to by a command
        /// <para>Delasc method in PSA-C</para>
        /// </summary>
        /// <param name="locationToRemove">The offset to remove from the tracker</param>
        /// <returns>If the offset existed in the tracker and was removed or not</returns>
        public bool RemoveOffsetFromOffsetInterlockTracker(int locationToRemove)
        {
            for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
            {
                if (PsaFile.OffsetInterlockTracker[i] == locationToRemove)
                {
                    PsaFile.OffsetInterlockTracker[i] = 16777216; // 100 0000
                    PsaFile.NumberOfOffsetEntries--;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// This method will update the external data table to account for the removed pointer that pointed to an external data table item
        /// <para>It goes through each external data table pointer and checks if it or the item it points to is the current command parameter value that is being removed</para>
        /// <para>If so, it will...update the external data table?? Honestly I'm pretty unsure of this methods exact purpose as of now. I'm just positive it's external data subroutine related</para>
        /// </summary>
        /// <param name="oldPsaCommand"></param>
        /// <param name="commandLocation"></param>
        public void UpdateExternalPointerLogic(PsaCommand oldPsaCommand, int commandLocation, int commandParamExternalSubRoutineLocation)
        {
            for (int externalSubRoutineIndex = 0; externalSubRoutineIndex < PsaFile.NumberOfExternalSubRoutines; externalSubRoutineIndex++) // j is mov
            {
                int externalSubRoutineLocationIndex = (PsaFile.NumberOfDataTableEntries + externalSubRoutineIndex) * 2; // not entirely sure what this is yet  :/
                int externalSubRoutineLocation = PsaFile.FileOtherData[externalSubRoutineLocationIndex];
                if (externalSubRoutineLocation >= 8096 && externalSubRoutineLocation < PsaFile.DataSectionSize)
                {
                    if (commandParamExternalSubRoutineLocation == externalSubRoutineLocation)
                    {
                        int commandExternalDataPointerValue = PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation] == 2
                            ? PsaFile.FileContent[commandLocation + 1] / 4 + 1
                            : PsaFile.FileContent[commandLocation + 1] / 4 + 3;

                        if (PsaFile.FileContent[commandExternalDataPointerValue] >= 8096 
                            && PsaFile.FileContent[commandExternalDataPointerValue] < PsaFile.DataSectionSize 
                            && PsaFile.FileContent[commandExternalDataPointerValue] % 4 == 0)
                        {
                            PsaFile.FileOtherData[externalSubRoutineLocationIndex] = PsaFile.FileContent[commandExternalDataPointerValue];
                        }
                        else
                        {
                            PsaFile.FileOtherData[externalSubRoutineLocationIndex] = -1;
                        }
                        break;
                    }

                    int externalSubRoutineCodeBlockLocation = externalSubRoutineLocation / 4;

                    int externalSubRoutineCommandsPointerLocation = PsaFile.FileContent[externalSubRoutineCodeBlockLocation];
                    if (externalSubRoutineCommandsPointerLocation >= 8096
                        && externalSubRoutineCommandsPointerLocation < PsaFile.DataSectionSize 
                        && oldPsaCommand.CommandParametersLocation == externalSubRoutineCommandsPointerLocation)
                    {
                        int commandExternalDataPointerValue = PsaFile.FileContent[commandLocation + 1] / 4 + 1;
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

        public int ExpandCommandParametersSection(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            // determine how much space is currently available for params
            // to start, there is the original amount of space
            // add 1 additional block of space for each free space that comes afterwards (FADEF00D)
            int oldCommandParamsSize = oldPsaCommand.GetCommandParamsSize();
            int newCommandParamsSize = newPsaCommand.GetCommandParamsSize();

            int currentCommandParamSize = oldCommandParamsSize;

            while (currentCommandParamSize < newCommandParamsSize && PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + currentCommandParamSize] == Constants.FADEF00D)
            {
                currentCommandParamSize++;
            }

            // if current parameters location is at the "edge" of the data section
            // and if the current parameters location doesn't even have enough room in the data section to add one more parameter comfortably, 
            // increase the data section size by the amount needed
            if (oldPsaCommand.CommandParametersValuesLocation + oldCommandParamsSize + 5 > PsaFile.DataSectionSizeBytes)
            {
                // difference between the new size needed and the old size
                int commandSizeDifference = newCommandParamsSize - oldCommandParamsSize;

                // if at the end of the data section, expand data section by the required amount
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandSizeDifference - 2] = Constants.FADE0D8A;
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandSizeDifference - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                    PsaFile.DataSectionSizeBytes += commandSizeDifference;
                    currentCommandParamSize = newCommandParamsSize;
                }
                // if adding one additional command would cause going over the data section size, just straight up expand the data section size 
                else if (oldPsaCommand.CommandParametersValuesLocation + oldCommandParamsSize + 3 > PsaFile.DataSectionSizeBytes)
                {
                    PsaFile.DataSectionSizeBytes += commandSizeDifference;
                    currentCommandParamSize = newCommandParamsSize;
                }
            }

            // if the amount of space available at the current moment is STILL not enough for all of the new params
            if (currentCommandParamSize < newCommandParamsSize)
            {
                int newCommandParametersValuesLocation = PsaFile.FindLocationWithAmountOfFreeSpace(CodeBlockDataStartLocation, newCommandParamsSize);
                if (newCommandParametersValuesLocation >= PsaFile.DataSectionSizeBytes)
                {
                    newCommandParametersValuesLocation = PsaFile.DataSectionSizeBytes;
                    if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                    {
                        newCommandParametersValuesLocation -= 2;
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 2] = Constants.FADE0D8A;
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                    }
                    PsaFile.DataSectionSizeBytes += newCommandParamsSize;
                }
                PsaFile.FileContent[commandLocation + 1] = newCommandParametersValuesLocation * 4;
                return newCommandParametersValuesLocation;
            } 
            else
            {
                return oldPsaCommand.CommandParametersValuesLocation;
            }
        }
    }
}
