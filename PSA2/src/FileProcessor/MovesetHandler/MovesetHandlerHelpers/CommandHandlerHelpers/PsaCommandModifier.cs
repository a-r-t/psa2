using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    public class PsaCommandModifier
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int OpenAreaStartLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandModifier(PsaFile psaFile, int dataSectionLocation, int openAreaStartLocation, PsaCommandParser psaCommandParser)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            OpenAreaStartLocation = openAreaStartLocation;
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
                    }
                }

                // if there are existing command params, they need to be overwritten, or the entire action may need to be relocated if not enough room for all the params
                else
                {
                    ModifyExistingCommandParametersLocation(commandLocation, oldPsaCommand, newPsaCommand);
                }


                // end EventModify method
                if (oldPsaCommand.CommandParametersLocation == -1)
                {
                    // I don't think I need any of this
                }
            }
            else
            {
                throw new ApplicationException("Cannot modify event...not sure why just can't okay");
            }

        }

        public void CreateNewCommandParametersLocation(int commandLocation, PsaCommand newPsaCommand)
        {
            int newCommandParamsSize = newPsaCommand.GetCommandParamsSize();

            // determine stopping point, which is where new command params will be added (finds room where number of params can all fit)
            int newCommandParametersLocation = PsaFile.FindLocationWithAmountOfFreeSpace(OpenAreaStartLocation, newCommandParamsSize);

            // if adding command params location causes data to go beyond data section limit, increase data section size
            if (newCommandParametersLocation >= PsaFile.DataSectionSizeBytes)
            {
                newCommandParametersLocation = PsaFile.DataSectionSizeBytes;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    newCommandParametersLocation -= 2;
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                }
                PsaFile.DataSectionSizeBytes += newCommandParamsSize;
            }

            int paramsIndex = 0;
            for (int i = 0; i < newCommandParamsSize; i += 2)
            {
                // if command param type is "Pointer" and the param value is greater than 0 (meaning it points to something)
                if (newPsaCommand.Parameters[paramsIndex].Type == 2 && newPsaCommand.Parameters[paramsIndex].Value > 0)
                {
                    int valuePointerOffset = (newCommandParametersLocation + i) * 4 + 4;
                    PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = valuePointerOffset;
                    PsaFile.NumberOfOffsetEntries++;
                }
                PsaFile.FileContent[newCommandParametersLocation + i] = newPsaCommand.Parameters[paramsIndex].Type;
                PsaFile.FileContent[newCommandParametersLocation + i + 1] = newPsaCommand.Parameters[paramsIndex].Value;
                paramsIndex++;
            }

            PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;
            int newCommandParamsLocation = newCommandParametersLocation * 4;
            PsaFile.FileContent[commandLocation + 1] = newCommandParamsLocation;
            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = commandLocation * 4 + 4;
            PsaFile.NumberOfOffsetEntries++;
            PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
        }

        /// <summary>
        /// Modify a command with existing parameters
        /// </summary>
        /// <param name="commandLocation">location of command</param>
        /// <param name="oldPsaCommand">old psa command that is going to be changed</param>
        /// <param name="newPsaCommand">new psa command that is replacing the old one</param>
        public void ModifyExistingCommandParametersLocation(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            int oldCommandParamsSize = oldPsaCommand.GetCommandParamsSize();

            // basically iterates once for each param in the old command
            for (int i = 0; i < oldCommandParamsSize; i += 2)
            {
                // this only comes into play if the old psa command's param type at index i is "Pointer" (which is 2)
                // it does some crazy relocating stuff
                if (PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i] == 2)
                {
                    // I believe this is the location of the actual value of a particular pointer command param
                    int commandParamValueLocation = (oldPsaCommand.CommandParametersValuesLocation + i) * 4 + 4; // rmv

                    // Delasc method -- this deletes an offset entry in the interlock tracker because it no longer needs to hold on to this pointer that is being modified/replaced
                    int iterator = 0;
                    bool existingOffsetFound = false;

                    while (iterator < PsaFile.NumberOfOffsetEntries)
                    {
                        if (PsaFile.OffsetInterlockTracker[iterator] == commandParamValueLocation)
                        {
                            existingOffsetFound = true;
                            break;
                        }
                        iterator++;
                    }

                    if (existingOffsetFound)
                    {
                        PsaFile.OffsetInterlockTracker[iterator] = 16777216; // 100 0000
                        PsaFile.NumberOfOffsetEntries--;
                    }

                    // end delasc

                    // this part is a long series of nested if statements...
                    // I can't figure out exactly what it does and can't get it to consistently trigger
                    if (iterator >= PsaFile.NumberOfOffsetEntries)
                    {
                        // something to do with external subroutines
                        if (i == 0)
                        {
                            for (int j = 0; j < PsaFile.NumberOfExternalSubRoutines; j++) // j is mov
                            {
                                i = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + j) * 2];
                                if (i > 8096 && i < PsaFile.DataSectionSize)
                                {
                                    if (commandParamValueLocation == i)
                                    {
                                        oldPsaCommand.CommandParametersLocation = PsaFile.FileContent[commandLocation + 1] / 4 + 1; // rmv
                                        int temp = (PsaFile.NumberOfDataTableEntries + j) * 2; // not entirely sure what this is yet  :/
                                        if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                        {
                                            if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] % 4 == 0)
                                            {
                                                PsaFile.CompressionTracker[temp] = PsaFile.FileContent[oldPsaCommand.CommandParametersLocation];
                                            }
                                            else
                                            {
                                                PsaFile.CompressionTracker[temp] = -1;
                                            }
                                        }
                                        else
                                        {
                                            PsaFile.CompressionTracker[temp] = -1;
                                        }
                                        oldPsaCommand.CommandParametersLocation = 0;
                                        break;
                                    }
                                    if (i >= 8096 && i < PsaFile.DataSectionSize)
                                    {
                                        for (int k = 0; k < 100; k++) // k is an1
                                        {
                                            // clearly I'm not sure what location this represents
                                            int somethingLocation = i / 4;

                                            i = PsaFile.FileContent[somethingLocation];
                                            if (i < 8096 || i >= PsaFile.DataSectionSize)
                                            {
                                                break;
                                            }
                                            if (oldPsaCommand.CommandParametersLocation == i)
                                            {
                                                oldPsaCommand.CommandParametersLocation = PsaFile.FileContent[commandLocation + 1] / 4 + 1;
                                                if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                {
                                                    if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] % 4 == 0)
                                                    {
                                                        PsaFile.CompressionTracker[somethingLocation] = PsaFile.FileContent[oldPsaCommand.CommandParametersLocation];
                                                    }
                                                    else
                                                    {
                                                        PsaFile.CompressionTracker[somethingLocation] = -1;
                                                    }
                                                }
                                                else
                                                {
                                                    PsaFile.CompressionTracker[somethingLocation] = -1;
                                                }
                                                oldPsaCommand.CommandParametersLocation = 0;
                                                break;
                                            }
                                        }
                                        if (oldPsaCommand.CommandParametersLocation == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            i = 0;
                        }
                        else if (oldCommandParamsSize == 4 && i == 2)
                        {
                            for (int j = 0; j < PsaFile.NumberOfExternalSubRoutines; j++) // j is mov
                            {
                                i = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + j) * 2];
                                if (i > 8096 && i < PsaFile.DataSectionSize)
                                {
                                    if (commandParamValueLocation == i)
                                    {
                                        oldPsaCommand.CommandParametersLocation = PsaFile.FileContent[commandLocation + 1] / 4 + 3; // rmv
                                        int temp = (PsaFile.NumberOfDataTableEntries + j) * 2; // not entirely sure what this is yet  :/
                                        if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                        {
                                            if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] % 4 == 0)
                                            {
                                                PsaFile.CompressionTracker[temp] = PsaFile.FileContent[oldPsaCommand.CommandParametersLocation];
                                            }
                                            else
                                            {
                                                PsaFile.CompressionTracker[temp] = -1;
                                            }
                                        }
                                        else
                                        {
                                            PsaFile.CompressionTracker[temp] = -1;
                                        }
                                        oldPsaCommand.CommandParametersLocation = 0;
                                        break;
                                    }
                                    if (i >= 8096 && i < PsaFile.DataSectionSize)
                                    {
                                        for (int k = 0; k < 100; k++) // k is an1
                                        {
                                            // clearly I'm not sure what location this represents
                                            int somethingLocation = i / 4;

                                            i = PsaFile.FileContent[somethingLocation];
                                            if (i < 8096 || i >= PsaFile.DataSectionSize)
                                            {
                                                break;
                                            }
                                            if (oldPsaCommand.CommandParametersLocation == i)
                                            {
                                                oldPsaCommand.CommandParametersLocation = PsaFile.FileContent[commandLocation + 1] / 4 + 1;
                                                if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                {
                                                    if (PsaFile.FileContent[oldPsaCommand.CommandParametersLocation] % 4 == 0)
                                                    {
                                                        PsaFile.CompressionTracker[somethingLocation] = PsaFile.FileContent[oldPsaCommand.CommandParametersLocation];
                                                    }
                                                    else
                                                    {
                                                        PsaFile.CompressionTracker[somethingLocation] = -1;
                                                    }
                                                }
                                                else
                                                {
                                                    PsaFile.CompressionTracker[somethingLocation] = -1;
                                                }
                                                oldPsaCommand.CommandParametersLocation = 0;
                                                break;
                                            }
                                        }
                                        if (oldPsaCommand.CommandParametersLocation == 0)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            i = 2;
                        }
                    }

                }
                PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i] = Constants.FADEF00D;
                PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i + 1] = Constants.FADEF00D;
            }
            int newCommandParamsSize = newPsaCommand.GetCommandParamsSize(); // m
            if (newCommandParamsSize == 0)
            {
                PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;
                PsaFile.FileContent[commandLocation + 1] = 0;

                // I believe this is the location of the actual value of a particular command param
                int commandParamValueLocation = commandLocation * 4 + 4; // rmv

                // Delasc method -- this checks if an offset already exists I think? Not quite sure
                int iterator = 0;
                bool existingOffsetFound = false;

                while (iterator < PsaFile.NumberOfOffsetEntries)
                {
                    if (PsaFile.OffsetInterlockTracker[iterator] == commandParamValueLocation) // rmv
                    {
                        existingOffsetFound = true;
                        break;
                    }
                    iterator++;
                }

                if (existingOffsetFound)
                {
                    PsaFile.OffsetInterlockTracker[iterator] = 16777216; // 100 0000
                    PsaFile.NumberOfOffsetEntries--;
                }

                // end delasc

                // fixam
                PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
            }
            // resize command params if new command has more params than the one it is replacing
            else if (newCommandParamsSize > oldCommandParamsSize)
            {
                // figure out how much space is in the current location for command params (increases if it finds some trailing FADEF00D spots)
                int currentCommandParamSize = oldCommandParamsSize; // i
                while (currentCommandParamSize < newCommandParamsSize && PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + currentCommandParamSize] == Constants.FADEF00D)
                {
                    currentCommandParamSize++;
                }
                // resize data section if no more room for command params
                if (oldPsaCommand.CommandParametersValuesLocation + oldCommandParamsSize + 5 > PsaFile.DataSectionSize)
                {
                    int commandSizeDifference = newCommandParamsSize - oldCommandParamsSize;
                    if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                    {
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandSizeDifference - 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandSizeDifference - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                        PsaFile.DataSectionSizeBytes += commandSizeDifference;
                        currentCommandParamSize = newCommandParamsSize;
                    }
                    else if (oldPsaCommand.CommandParametersValuesLocation + oldCommandParamsSize + 3 > PsaFile.DataSectionSizeBytes)
                    {
                        PsaFile.DataSectionSizeBytes += commandSizeDifference;
                        currentCommandParamSize = newCommandParamsSize;
                    }
                }
                // if new command params size is less than what's already there, create some FADEF00DS to represent free space that can later be used if needed
                if (currentCommandParamSize < newCommandParamsSize)
                {
                    oldPsaCommand.CommandParametersValuesLocation = OpenAreaStartLocation;
                    int bitStoppingPoint = 0;

                    while (oldPsaCommand.CommandParametersValuesLocation < PsaFile.DataSectionSizeBytes && bitStoppingPoint != oldPsaCommand.CommandParametersValuesLocation + newCommandParamsSize)
                    {
                        if (PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation] == Constants.FADEF00D)
                        {
                            bitStoppingPoint = oldPsaCommand.CommandParametersValuesLocation + 1;
                            while (bitStoppingPoint <= oldPsaCommand.CommandParametersValuesLocation + newCommandParamsSize && PsaFile.FileContent[bitStoppingPoint] != Constants.FADEF00D)
                            {
                                if (PsaFile.FileContent[bitStoppingPoint] != Constants.FADEF00D)
                                {
                                    oldPsaCommand.CommandParametersValuesLocation = bitStoppingPoint;
                                    break;
                                }
                                bitStoppingPoint++;
                            }
                        }
                        oldPsaCommand.CommandParametersValuesLocation++;
                    }
                    if (oldPsaCommand.CommandParametersValuesLocation >= PsaFile.DataSectionSizeBytes)
                    {
                        oldPsaCommand.CommandParametersValuesLocation = PsaFile.DataSectionSizeBytes;
                        if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                        {
                            oldPsaCommand.CommandParametersValuesLocation -= 2;
                            PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                            PsaFile.FileContent[PsaFile.DataSectionSizeBytes + newCommandParamsSize - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                        }
                        PsaFile.DataSectionSizeBytes += newCommandParamsSize;
                    }
                    PsaFile.FileContent[commandLocation + 1] = oldPsaCommand.CommandParametersValuesLocation * 4;
                }
            }
            PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;

            int parameterIndex = 0;
            for (int i = 0; i < newCommandParamsSize; i += 2)
            {
                // if command param type is Pointer and it actually points to something
                if (newPsaCommand.Parameters[parameterIndex].Type == 2 && newPsaCommand.Parameters[parameterIndex].Value > 0)
                {
                    int something = (oldPsaCommand.CommandParametersValuesLocation + i) * 4 + 4;
                    PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = something;
                    PsaFile.NumberOfOffsetEntries++;
                }

                PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i] = newPsaCommand.Parameters[parameterIndex].Type;
                PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + i + 1] = newPsaCommand.Parameters[parameterIndex].Value;
                parameterIndex++;
            }

            // in psac, this is only called if "fnt" is 1, which means something was changed -- will see if this will break things or not to just call every time
            PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
        }
    }
}
