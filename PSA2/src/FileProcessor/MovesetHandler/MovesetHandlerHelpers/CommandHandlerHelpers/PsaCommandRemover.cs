using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    public class PsaCommandRemover
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int OpenAreaStartLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandRemover(PsaFile psaFile, int dataSectionLocation, int openAreaStartLocation, PsaCommandParser psaCommandParser)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            OpenAreaStartLocation = openAreaStartLocation;
            PsaCommandParser = psaCommandParser;
        }

        public void RemoveCommand(int commandLocation, int codeBlockCommandsLocation, PsaCommand removedPsaCommand, int commandIndex, int codeBlockLocation)
        {
            int numberOfCommandsAlreadyInCodeBlock = PsaCommandParser.GetNumberOfPsaCommands(codeBlockCommandsLocation); // g

            if (numberOfCommandsAlreadyInCodeBlock > 1)
            {
                RemoveOneCommand(commandLocation, codeBlockCommandsLocation, removedPsaCommand, commandIndex);
            }
            // aka removing this command will remove the last command that was in the action
            else
            {
                RemoveLastCommand(commandLocation, codeBlockCommandsLocation, removedPsaCommand, commandIndex, codeBlockLocation);
            }
        }

        public void RemoveOneCommand(int commandLocation, int codeBlockCommandsLocation, PsaCommand removedPsaCommand, int commandIndex)
        {
            // commandLocation is j, codeBlockCommandsLocation is h
            int numberOfCommandsAlreadyInCodeBlock = PsaCommandParser.GetNumberOfPsaCommands(codeBlockCommandsLocation); // g

            int removedCommandParamsValuesLocation = removedPsaCommand.CommandParametersValuesLocation; // m

            int numberOfParams = removedPsaCommand.NumberOfParams; // k

            if (numberOfParams != 0)
            {
                int removedCommandsParamsSize = removedPsaCommand.GetCommandParamsSize(); // n
                if (removedCommandParamsValuesLocation >= OpenAreaStartLocation && removedCommandParamsValuesLocation < PsaFile.DataSectionSizeBytes)
                {
                    int pointerToCommandLocation = commandLocation * 4 + 4; // rmv

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
                    for (int i = 0; i < removedCommandsParamsSize; i += 2)
                    {
                        // crazy nested stuff from modify

                        // this only comes into play if the old psa command's param type at index i is "Pointer" (which is 2)
                        // it does some crazy relocating stuff
                        if (removedPsaCommand.Parameters[parameterIndex].Type == 2)
                        {
                            // I believe this is the location of the actual value of a particular pointer command param
                            int commandParamValueLocation = (removedPsaCommand.CommandParametersValuesLocation + i) * 4 + 4; // rmv

                            // Delasc method -- this deletes an offset entry in the interlock tracker because it no longer needs to hold on to this pointer that is being modified/replaced
                            int iterator2 = 0;
                            bool existingOffsetFound2 = false;

                            while (iterator2 < PsaFile.NumberOfOffsetEntries)
                            {
                                if (PsaFile.OffsetInterlockTracker[iterator2] == commandParamValueLocation)
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
                                if (i == 0)
                                {
                                    for (int j = 0; j < PsaFile.NumberOfExternalSubRoutines; j++) // j is mov
                                    {
                                        i = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + j) * 2];
                                        if (i > 8096 && i < PsaFile.DataSectionSize)
                                        {
                                            if (commandParamValueLocation == i)
                                            {
                                                int temp = (PsaFile.NumberOfDataTableEntries + j) * 2; // not entirely sure what this is yet  :/
                                                if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation + 1] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersLocation + 1] < PsaFile.DataSectionSize)
                                                {
                                                    if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation + 1] % 4 == 0)
                                                    {
                                                        PsaFile.CompressionTracker[temp] = PsaFile.FileContent[removedPsaCommand.CommandParametersLocation + 1];
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
                                                break;
                                            }
                                            if (i >= 8096 && i < PsaFile.DataSectionSize)
                                            {
                                                for (int k = 0; k < 100; k++) // k is an1
                                                {
                                                    // clearly I'm not sure what location this represents
                                                    int somethingLocation = i / 4; // n

                                                    i = PsaFile.FileContent[somethingLocation];
                                                    if (i < 8096 || i >= PsaFile.DataSectionSize)
                                                    {
                                                        break;
                                                    }
                                                    if (commandParamValueLocation == i)
                                                    {
                                                        if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation + 1] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersLocation + 1] < PsaFile.DataSectionSize)
                                                        {
                                                            if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation + 1] % 4 == 0)
                                                            {
                                                                PsaFile.CompressionTracker[somethingLocation] = PsaFile.FileContent[removedPsaCommand.CommandParametersLocation + 1];
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
                                                        break;
                                                    }
                                                }
                                                if (commandParamValueLocation == i)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    i = 0;
                                    //  n = ((alm[j + i * 2] >> 8) & 0xFF) * 2;
                                }
                                else if (removedPsaCommand.GetCommandParamsSize() == 4 && i == 2)
                                {
                                    for (int j = 0; j < PsaFile.NumberOfExternalSubRoutines; j++) // j is mov
                                    {
                                        i = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + j) * 2];
                                        if (i > 8096 && i < PsaFile.DataSectionSize)
                                        {
                                            if (commandParamValueLocation == i)
                                            {
                                                int temp = (PsaFile.NumberOfDataTableEntries + j) * 2; // not entirely sure what this is yet  :/
                                                if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] < PsaFile.DataSectionSize)
                                                {
                                                    if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] % 4 == 0)
                                                    {
                                                        PsaFile.CompressionTracker[temp] = PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3];
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
                                                    if (commandParamValueLocation == i)
                                                    {
                                                        if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] < PsaFile.DataSectionSize)
                                                        {
                                                            if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] % 4 == 0)
                                                            {
                                                                PsaFile.CompressionTracker[somethingLocation] = PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3];
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
                                                        break;
                                                    }
                                                }
                                                if (commandParamValueLocation == i)
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    i = 2;
                                    // n += 4 (I don't think I need this)
                                }
                            }

                        }
                        PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + i] = Constants.FADEF00D;
                        PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + i + 1] = Constants.FADEF00D;
                        parameterIndex++;
                    }
                }
            }

            while (PsaFile.FileContent[commandLocation + 2] != 0)
            {
                if ((PsaFile.FileContent[commandLocation + 2] >> 8 & 0xFF) != 0)
                {
                    int something = commandLocation * 4 + 12;
                    for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
                    {
                        if (PsaFile.OffsetInterlockTracker[i] == something)
                        {
                            PsaFile.OffsetInterlockTracker[i] -= 8;
                            break;
                        }
                    }
                }
                PsaFile.FileContent[commandLocation] = PsaFile.FileContent[commandLocation + 2];
                PsaFile.FileContent[commandLocation + 1] = PsaFile.FileContent[commandLocation + 3];
                commandLocation += 2;
            }
            PsaFile.FileContent[commandLocation] = 0;
            PsaFile.FileContent[commandLocation + 1] = 0;
            PsaFile.FileContent[commandLocation + 2] = Constants.FADEF00D;
            PsaFile.FileContent[commandLocation + 3] = Constants.FADEF00D;


            // event offset interlock logic
            // not sure what these variables mean

            int k1 = codeBlockCommandsLocation + numberOfCommandsAlreadyInCodeBlock * 8;
            int h = k1 - (numberOfCommandsAlreadyInCodeBlock - 1) * 8;
            for (int i = OpenAreaStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
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

        public void RemoveLastCommand(int commandLocation, int codeBlockCommandsLocation, PsaCommand removedPsaCommand, int commandIndex, int codeBlockLocation)
        {
            // commandLocation is j, codeBlockCommandsLocation is h

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

                    if (removedCommandParamsValuesLocation >= OpenAreaStartLocation && removedCommandParamsValuesLocation < PsaFile.DataSectionSizeBytes)
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
                                            k = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + k) * 2];
                                            if (k > 8096 && k < PsaFile.DataSectionSize)
                                            {
                                                if (commandParamValueLocation == k)
                                                {
                                                    int temp = (PsaFile.NumberOfDataTableEntries + k) * 2; // not entirely sure what this is yet  :/
                                                    if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] < PsaFile.DataSectionSize)
                                                    {
                                                        if (PsaFile.FileContent[removedPsaCommand.CommandParametersLocation] % 4 == 0)
                                                        {
                                                            PsaFile.CompressionTracker[temp] = PsaFile.FileContent[removedPsaCommand.CommandParametersLocation];
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
                                                                    PsaFile.CompressionTracker[somethingLocation] = PsaFile.FileContent[removedPsaCommand.CommandParametersLocation];
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
                                            k = PsaFile.CompressionTracker[(PsaFile.NumberOfDataTableEntries + k) * 2];
                                            if (k > 8096 && k < PsaFile.DataSectionSize)
                                            {
                                                if (commandParamValueLocation == k)
                                                {
                                                    int temp = (PsaFile.NumberOfDataTableEntries + k) * 2; // not entirely sure what this is yet  :/
                                                    if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] >= 8096 && PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] < PsaFile.DataSectionSize)
                                                    {
                                                        if (PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3] % 4 == 0)
                                                        {
                                                            PsaFile.CompressionTracker[temp] = PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3];
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
                                                                    PsaFile.CompressionTracker[somethingLocation] = PsaFile.FileContent[removedPsaCommand.CommandParametersValuesLocation + 3];
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
