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
            int codeBlockCommandsPointerLocation = codeBlock.CommandsPointerLocation;
            int codeBlockLocation = codeBlock.Location;

            // codeBlock.NumberOfCommands is g in PSA-C

            if (codeBlock.NumberOfCommands > 1)
            {
                RemoveOneCommand(codeBlock, commandLocation, removedPsaCommand);
            }
            // aka removing this command will remove the last command that was in the action
            else
            {
                RemoveLastCommand(commandLocation, codeBlockCommandsPointerLocation, removedPsaCommand, codeBlockLocation);
            }
        }

        public void RemoveOneCommand(CodeBlock codeBlock, int commandLocation, PsaCommand removedPsaCommand)
        {
            // commandLocation is j, codeBlockCommandsLocation is h, removedCommandParmasValuesLocation is m

            if (removedPsaCommand.NumberOfParams != 0)
            {
                RemoveCommandParameters(removedPsaCommand, commandLocation);
            }

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

            // event offset interlock logic
            // not sure what these variables mean

            int k1 = codeBlock.CommandsPointerLocation + codeBlock.NumberOfCommands * 8;
            int h = k1 - (codeBlock.NumberOfCommands - 1) * 8;
            for (int i = CodeBlockDataStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
            {
                if (PsaFile.FileContent[i] >= h && PsaFile.FileContent[i] <= k1)
                {
                    // DelILData method
                    // maybe IL stands for "interlock"?
                    int n = i * 4;
                    int g = 0;
                    bool offsetFound = false;
                    while (true)
                    {
                        if (g < PsaFile.NumberOfOffsetEntries)
                        {
                            if (PsaFile.OffsetInterlockTracker[g] == n)
                            {
                                offsetFound = true;
                                break;
                            }
                            g++;
                            continue;
                        }
                        break;
                    }
                    if (offsetFound)
                    {
                        PsaFile.FileContent[i] = 0;
                        PsaFile.OffsetInterlockTracker[g] = 16777216; // 100 0000
                        PsaFile.NumberOfOffsetEntries--;

                        // something here is a pointer i guess
                        if (PsaFile.FileContent[i - 1] == 2)
                        {
                            PsaFile.FileContent[i - 1] = 0;
                            n -= 4;
                            // this is a bad name def
                            int startOffsetOpenArea = 2025;
                            bool offsetFound2 = false;
                            while (true)
                            {
                                if (startOffsetOpenArea < PsaFile.DataSectionSizeBytes)
                                {
                                    if (PsaFile.FileContent[startOffsetOpenArea] == n)
                                    {
                                        offsetFound2 = true;
                                        break;
                                    }
                                    startOffsetOpenArea++;
                                    continue;
                                }
                                break;
                            }
                            if (offsetFound2)
                            {
                                // wtf is going on aghh
                                if (PsaFile.FileContent[startOffsetOpenArea - 1] == 459008 || PsaFile.FileContent[startOffsetOpenArea - 1] == 590080)
                                {
                                    PsaFile.FileContent[startOffsetOpenArea - 1] = Constants.NOP;
                                    PsaFile.FileContent[startOffsetOpenArea] = 0;
                                    PsaFile.FileContent[i] = Constants.FADEF00D;
                                    PsaFile.FileContent[i - 1] = Constants.FADEF00D;

                                    int somethingLocation = startOffsetOpenArea * 4; // rmv
                                    // delasc method

                                    int iterator = 0;
                                    bool existingOffsetFound = false;

                                    while (iterator < PsaFile.NumberOfOffsetEntries)
                                    {
                                        if (PsaFile.OffsetInterlockTracker[iterator] == somethingLocation) // rmv
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

                                    // end delasc method
                                }
                            }
                        }
                    }
                }
            }
            // end event offset interlock logic
            PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
        }

        public void RemoveCommandParameters(PsaCommand removedPsaCommand, int commandLocation)
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
                    // it does some crazy relocating stuff
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

        public void RemoveLastCommand(int commandLocation, int codeBlockCommandsPointerLocation, PsaCommand removedPsaCommand, int codeBlockLocation)
        {
            // commandLocation is j, codeBlockCommandsLocation is h
            int codeBlockCommandsLocation = codeBlockCommandsPointerLocation / 4;
            int numberOfCommandsAlreadyInCodeBlock = PsaCommandParser.GetNumberOfPsaCommands(codeBlockCommandsLocation); // g

            int something = commandLocation + numberOfCommandsAlreadyInCodeBlock * 2;
            if (PsaFile.FileContent[something - 2] == Constants.FADEF00D)
            {
                if (PsaFile.FileContent[something - 1] == Constants.FADEF00D)
                {
                    numberOfCommandsAlreadyInCodeBlock = 0;
                }
            }
            else
            {
                PsaFile.FileContent[something] = Constants.FADEF00D;
                PsaFile.FileContent[something + 1] = Constants.FADEF00D;
            }

            int removedCommandParamsValuesLocation = removedPsaCommand.CommandParametersValuesLocation; // m

            for (int i = 0; i < numberOfCommandsAlreadyInCodeBlock; i++)
            {
                int numberOfParams = removedPsaCommand.NumberOfParams; // k

                // crazy nested part here
                if (numberOfParams != 0)
                {
                    int removedCommandsParamsSize = removedPsaCommand.GetCommandParamsSize(); // n
                                                                                              //m is removedCommandParamsValuesLocation

                    if (removedCommandParamsValuesLocation >= CodeBlockDataStartLocation && removedCommandParamsValuesLocation < PsaFile.DataSectionSizeBytes)
                    {
                        int pointerToCommandLocation = (commandLocation + i * 2) * 4 + 4; // rmv

                        // delasc method
                        int iterator = 0;
                        bool existingOffsetFound = false;

                        while (iterator < PsaFile.NumberOfOffsetEntries)
                        {
                            if (PsaFile.OffsetInterlockTracker[iterator] == pointerToCommandLocation) // rmv
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

                        // end delasc method

                        // iterates through each param the command had
                        int parameterIndex = 0;
                        for (int j = 0; j < removedCommandsParamsSize; j += 2)
                        {
                            // crazy nested stuff from modify

                            // this only comes into play if the old psa command's param type at index i is "Pointer" (which is 2)
                            // it does some crazy relocating stuff
                            if (removedPsaCommand.Parameters[parameterIndex].Type == 2)
                            {
                                // I believe this is the location of the actual value of a particular pointer command param
                                int commandParamValueLocation = (removedPsaCommand.CommandParametersValuesLocation + j) * 4 + 4; // rmv

                                // Delasc method -- this deletes an offset entry in the interlock tracker because it no longer needs to hold on to this pointer that is being modified/replaced
                                int iterator2 = 0;
                                bool existingOffsetFound2 = false;

                                while (iterator2 < PsaFile.NumberOfOffsetEntries)
                                {
                                    if (PsaFile.OffsetInterlockTracker[iterator] == commandParamValueLocation)
                                    {
                                        existingOffsetFound2 = true;
                                        break;
                                    }
                                    iterator2++;
                                }

                                if (existingOffsetFound2)
                                {
                                    PsaFile.OffsetInterlockTracker[iterator2] = 16777216; // 100 0000
                                    PsaFile.NumberOfOffsetEntries--;
                                }

                                // end delasc

                                // this part is a long series of nested if statements...
                                // I can't figure out exactly what it does and can't get it to consistently trigger
                                if (iterator2 >= PsaFile.NumberOfOffsetEntries)
                                {
                                    // something to do with external subroutines
                                    if (j == 0)
                                    {
                                        for (int k = 0; k < PsaFile.NumberOfExternalSubRoutines; k++) // j is mov
                                        {
                                            k = PsaFile.FileOtherData[(PsaFile.NumberOfDataTableEntries + k) * 2];
                                            if (k > 8096 && k < PsaFile.DataSectionSize)
                                            {
                                                if (commandParamValueLocation == k)
                                                {
                                                    int temp = (PsaFile.NumberOfDataTableEntries + k) * 2; // not entirely sure what this is yet  :/
                                                    if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                    {
                                                        if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] % 4 == 0)
                                                        {
                                                            PsaFile.FileOtherData[temp] = PsaFile.FileContent[removedPsaCommand.CommandParametersLocation];
                                                        }
                                                        else
                                                        {
                                                            PsaFile.FileOtherData[temp] = -1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        PsaFile.FileOtherData[temp] = -1;
                                                    }
                                                    break;
                                                }
                                                if (k >= 8096 && k < PsaFile.DataSectionSize)
                                                {
                                                    for (int l = 0; l < 100; l++) // k is an1
                                                    {
                                                        // clearly I'm not sure what location this represents
                                                        int somethingLocation = l / 4; // n

                                                        l = PsaFile.FileContent[somethingLocation];
                                                        if (l < 8096 || l >= PsaFile.DataSectionSize)
                                                        {
                                                            break;
                                                        }
                                                        if (commandParamValueLocation == l)
                                                        {
                                                            if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                            {
                                                                if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] % 4 == 0)
                                                                {
                                                                    PsaFile.FileOtherData[somethingLocation] = PsaFile.FileContent[removedPsaCommand.CommandParametersLocation];
                                                                }
                                                                else
                                                                {
                                                                    PsaFile.FileOtherData[somethingLocation] = -1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                PsaFile.FileOtherData[somethingLocation] = -1;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                    if (commandParamValueLocation == k)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        j = 0;
                                    }
                                    else if (removedCommandsParamsSize == 4 && j == 2)
                                    {
                                        for (int k = 0; k < PsaFile.NumberOfExternalSubRoutines; k++) // j is mov
                                        {
                                            k = PsaFile.FileOtherData[(PsaFile.NumberOfDataTableEntries + k) * 2];
                                            if (k > 8096 && k < PsaFile.DataSectionSize)
                                            {
                                                if (commandParamValueLocation == k)
                                                {
                                                    int temp = (PsaFile.NumberOfDataTableEntries + k) * 2; // not entirely sure what this is yet  :/
                                                    if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] < PsaFile.DataSectionSize)
                                                    {
                                                        if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] % 4 == 0)
                                                        {
                                                            PsaFile.FileOtherData[temp] = PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3];
                                                        }
                                                        else
                                                        {
                                                            PsaFile.FileOtherData[temp] = -1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        PsaFile.FileOtherData[temp] = -1;
                                                    }
                                                    break;
                                                }
                                                if (k >= 8096 && k < PsaFile.DataSectionSize)
                                                {
                                                    for (int l = 0; l < 100; l++) // k is an1
                                                    {
                                                        // clearly I'm not sure what location this represents
                                                        int somethingLocation = l / 4;

                                                        l = PsaFile.FileContent[somethingLocation];
                                                        if (l < 8096 || l >= PsaFile.DataSectionSize)
                                                        {
                                                            break;
                                                        }
                                                        if (commandParamValueLocation == l)
                                                        {
                                                            if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] < PsaFile.DataSectionSize)
                                                            {
                                                                if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] % 4 == 0)
                                                                {
                                                                    PsaFile.FileOtherData[somethingLocation] = PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3];
                                                                }
                                                                else
                                                                {
                                                                    PsaFile.FileOtherData[somethingLocation] = -1;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                PsaFile.FileOtherData[somethingLocation] = -1;
                                                            }
                                                            break;
                                                        }
                                                    }
                                                    if (commandParamValueLocation == k)
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        j = 2;
                                        // n += 4 (I don't think I need this)
                                    }
                                }

                            }
                            PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + j] = Constants.FADEF00D;
                            PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + j + 1] = Constants.FADEF00D;
                            parameterIndex++;
                        }
                    }
                }
                PsaFile.FileContent[commandLocation + i * 2] = Constants.FADEF00D;
                PsaFile.FileContent[commandLocation + i * 2 + 1] = Constants.FADEF00D;
            }

            // more delasc stuff after removeallenv

            // codeBlockLocation is n + i
            PsaFile.FileContent[codeBlockLocation] = 0;
            int pointerToCodeBlockLocation = codeBlockLocation * 4;

            // delasc method

            int iterator3 = 0;
            bool existingOffsetFound3 = false;

            while (iterator3 < PsaFile.NumberOfOffsetEntries)
            {
                if (PsaFile.OffsetInterlockTracker[iterator3] == pointerToCodeBlockLocation) // rmv
                {
                    existingOffsetFound3 = true;
                    break;
                }
                iterator3++;
            }

            if (existingOffsetFound3)
            {
                PsaFile.OffsetInterlockTracker[iterator3] = 16777216; // 100 0000
                PsaFile.NumberOfOffsetEntries--;
            }

            // end delasc method

            PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
        }

    }
}
