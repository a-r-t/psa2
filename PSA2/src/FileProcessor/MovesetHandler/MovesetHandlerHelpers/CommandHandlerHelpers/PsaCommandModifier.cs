﻿using PSA2.src.Utility;
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
                    int valuePointerOffset = (newCommandParametersValuesLocation + paramTypeLocation) * 4 + 4;
                    PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = valuePointerOffset;
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
            int oldCommandParamsSize = oldPsaCommand.GetCommandParamsSize();

            if (PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation] == 2)
            {
                UpdatePointerLogicForExternalSubroutineCall(oldPsaCommand, commandLocation);
            }

            if (PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + 2] == 2 && oldPsaCommand.NumberOfParams == 2)
            {
                UpdatePointerLogicForExternalConcurrentInfiniteLoopCall(oldPsaCommand, commandLocation);
            }

            // Replace old param values with free space (FADEF00D)
            for (int paramIndex = 0; paramIndex < oldPsaCommand.NumberOfParams; paramIndex++)
            {
                PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + (paramIndex * 2)] = Constants.FADEF00D;
                PsaFile.FileContent[oldPsaCommand.CommandParametersValuesLocation + (paramIndex * 2) + 1] = Constants.FADEF00D;
            }

            int newCommandParamsSize = newPsaCommand.GetCommandParamsSize(); // m

            // if new command has no params (such as a "nop" command)
            if (newCommandParamsSize == 0)
            {
                // set instruction
                PsaFile.FileContent[commandLocation] = newPsaCommand.Instruction;

                // remove pointer to params since this command has no params
                PsaFile.FileContent[commandLocation + 1] = 0;

                // remove offset from interlock tracker since it no longer exists
                int commandParamValuePointerLocation = commandLocation * 4 + 4; // rmv
                RemoveOffsetFromOffsetInterlockTracker(commandParamValuePointerLocation);
            }

            // resize command params if new command has more params than the one it is replacing
            else if (newCommandParamsSize > oldCommandParamsSize)
            {
                ExpandCommandParametersSection(commandLocation, oldPsaCommand, newCommandParamsSize, oldCommandParamsSize);
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
        }

        // delasc method in psac
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

        public void UpdatePointerLogicForExternalSubroutineCall(PsaCommand oldPsaCommand, int commandLocation)
        {
            int commandParameterLocation = oldPsaCommand.CommandParametersLocation + 4;
            //int commandParameterLocation = (oldPsaCommand.CommandParametersValuesLocation + i) * 4 + 4; // rmv --- OLD (works though)

            bool wasOffsetRemoved = RemoveOffsetFromOffsetInterlockTracker(commandParameterLocation);

            // This will trigger if command was pointing to an external subroutine (like Mario's Up B has one, the home run bat has one, etc)
            if (!wasOffsetRemoved)
            {
                // something to do with external subroutines

                for (int j = 0; j < PsaFile.NumberOfExternalSubRoutines; j++) // j is mov
                {
                    int something1 = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + j) * 2];
                    if (something1 > 8096 && something1 < PsaFile.DataSectionSize)
                    {
                        if (commandParameterLocation == something1)
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
                        if (something1 >= 8096 && something1 < PsaFile.DataSectionSize)
                        {
                            for (int k = 0; k < 100; k++) // k is an1
                            {
                                // clearly I'm not sure what location this represents
                                int somethingLocation = something1 / 4;

                                int something2 = PsaFile.FileContent[somethingLocation];
                                if (something2 < 8096 || something2 >= PsaFile.DataSectionSize)
                                {
                                    break;
                                }
                                if (oldPsaCommand.CommandParametersLocation == something2)
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
            }
        }

        public void UpdatePointerLogicForExternalConcurrentInfiniteLoopCall(PsaCommand oldPsaCommand, int commandLocation)
        {
            int commandParameterLocation = oldPsaCommand.CommandParametersLocation + 12;
            //int commandParameterLocation = (oldPsaCommand.CommandParametersValuesLocation + i) * 4 + 4; // rmv --- OLD (works though)

            bool wasOffsetRemoved = RemoveOffsetFromOffsetInterlockTracker(commandParameterLocation);

            // This will trigger if command was pointing to an external subroutine (like Mario's Up B has one, the home run bat has one, etc)
            if (!wasOffsetRemoved)
            {
                for (int j = 0; j < PsaFile.NumberOfExternalSubRoutines; j++) // j is mov
                {
                    int something1 = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + j) * 2];
                    if (something1 > 8096 && something1 < PsaFile.DataSectionSize)
                    {
                        if (commandParameterLocation == something1)
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
                        if (something1 >= 8096 && something1 < PsaFile.DataSectionSize)
                        {
                            for (int k = 0; k < 100; k++) // k is an1
                            {
                                // clearly I'm not sure what location this represents
                                int somethingLocation = something1 / 4;

                                int something2 = PsaFile.FileContent[somethingLocation];
                                if (something2 < 8096 || something2 >= PsaFile.DataSectionSize)
                                {
                                    break;
                                }
                                if (oldPsaCommand.CommandParametersLocation == something2)
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
            }
        }

        public void ExpandCommandParametersSection(int commandLocation, PsaCommand oldPsaCommand, int newCommandParamsSize, int oldCommandParamsSize)
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
                oldPsaCommand.CommandParametersValuesLocation = CodeBlockDataStartLocation;
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
    }
}
