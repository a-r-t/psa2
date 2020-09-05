using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class ActionsParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public ActionsParser(PsaFile psaFile, int dataSectionLocation, string movesetBaseName)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandParser = new PsaCommandParser(PsaFile);
        }

        public int GetNumberOfSpecialActions()
        {
            Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
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
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            return PsaCommandParser.GetPsaCommands(actionCodeBlockLocation);
        }


        public void AddCommandToAction(int actionId, int codeBlockId, int command)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId); // h
            Console.WriteLine("Action start offset: " + actionCodeBlockLocation.ToString("X"));
            int dataSectionSizeBytes = PsaFile.DataSectionSize / 4;

            int openAreaStartLocation = 2014 + GetNumberOfSpecialActions() * 2; // stf

            int newOffsetLocation = 0;

            if (actionCodeBlockLocation == 0 || (actionCodeBlockLocation >= openAreaStartLocation * 4 && actionCodeBlockLocation < PsaFile.DataSectionSize))
            {
                int numberOfCommandsAlreadyInAction = GetPsaCommandsForAction(actionId, codeBlockId).Count; // g

                // EventAdd method

                // no commands yet exist for action
                if (actionCodeBlockLocation == 0)
                {
                    // I'm pretty sure this section finds the next available location to put a psa command, but not sure...
                    int stoppingPoint = openAreaStartLocation;
                    int bitStoppingPoint = 0;

                    for (int i = openAreaStartLocation; i < dataSectionSizeBytes; i++)
                    {
                        stoppingPoint++;
                        if (PsaFile.FileContent[i] == -86052851) // -86052851 is equal to FFFF FFFF FADE F00D in hex, which means empty space that PSA created...I think
                        {
                            for (int j = i + 1; j <= i + 3; j++)
                            {
                                bitStoppingPoint = j;
                                if (PsaFile.FileContent[j] != -86052851)
                                {
                                    i = j;
                                    stoppingPoint = i;
                                    break;
                                }
                            }
                            if (bitStoppingPoint == i + 4)
                            {
                                break;
                            }
                        }
                    }

                    // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
                    if (stoppingPoint >= dataSectionSizeBytes)
                    {
                        stoppingPoint = dataSectionSizeBytes;
                        if (PsaFile.FileContent[dataSectionSizeBytes - 2] == -86110838) // -86110838 is FFFF FFFF FADE 0D8A
                        {
                            PsaFile.FileContent[dataSectionSizeBytes + 2] = PsaFile.FileContent[dataSectionSizeBytes - 2];
                            PsaFile.FileContent[dataSectionSizeBytes + 3] = PsaFile.FileContent[dataSectionSizeBytes - 1];
                            stoppingPoint -= 2;
                        }
                        dataSectionSizeBytes += 4;
                        PsaFile.DataSectionSize = dataSectionSizeBytes * 4;
                    }

                    // this creates the first "nop" command to start off the new location for the action's commands
                    PsaFile.FileContent[stoppingPoint] = 131072; // 131072 = nop
                    PsaFile.FileContent[stoppingPoint + 1] = 0;
                    PsaFile.FileContent[stoppingPoint + 2] = 0;
                    PsaFile.FileContent[stoppingPoint + 3] = 0;

                    // I believe this part increases the global header counter for the number of offsets in the file and adds the new action's pointer to the pointer table
                    int codeBlockLocation = 0;
                    if (codeBlockId == 0)
                    {
                        codeBlockLocation = PsaFile.FileContent[DataSectionLocation + 9] / 4;
                    }
                    else if (codeBlockId == 1)
                    {
                        codeBlockLocation = PsaFile.FileContent[DataSectionLocation + 10] / 4;
                    }
                    PsaFile.FileContent[codeBlockLocation + codeBlockId] = stoppingPoint * 4;
                    PsaFile.NumberOfOffsetEntries++;

                    newOffsetLocation = stoppingPoint * 4;
                }

                // if there are already existing commands for action, add new command to this
                else
                {
                    int commandStartLocation = actionCodeBlockLocation / 4;
                    if (PsaFile.FileContent[commandStartLocation] == -86052851) // -86052851 is FFFF FFFF FADE F00D
                    {
                        // number of commands that already exist is 0
                    }

                    // if adding a new command when there's no more room in data section, increase size of data section
                    if (commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 5 > dataSectionSizeBytes)
                    {
                        if (PsaFile.FileContent[dataSectionSizeBytes - 2] == -86110838) // -86110838 is FFFF FFFF FADE 0D8A
                        {
                            PsaFile.FileContent[dataSectionSizeBytes] = PsaFile.FileContent[dataSectionSizeBytes - 2];
                            PsaFile.FileContent[dataSectionSizeBytes + 1] = PsaFile.FileContent[dataSectionSizeBytes - 1];
                            PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = -86052851; // -86052851 is FFFF FFFF FADE F00D
                            PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = -86052851; // -86052851 is FFFF FFFF FADE F00D
                            dataSectionSizeBytes += 2;
                            PsaFile.DataSectionSize = dataSectionSizeBytes * 4;
                        }
                        else if (commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3 > dataSectionSizeBytes)
                        {
                            PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = -86052851; // -86052851 is FFFF FFFF FADE F00D
                            PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = -86052851; // -86052851 is FFFF FFFF FADE F00D
                            dataSectionSizeBytes += 2;
                            PsaFile.DataSectionSize = dataSectionSizeBytes * 4;
                        }
                    }

                    // this part actually adds the new nop command

                    // checks if location for command to go currently is FFFF FFFF FADE F00D I think and adds nop
                    // I guess this is like for if you had previous deleted a command, it would replace it to FADE F00D so this code can recognize it and easily replace it...I think
                    if (PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] == -86052851 && PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] == -86052851)
                    {
                        PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2] = 131072; // 131072 = nop
                        PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 1] = 0;
                        PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = 0;
                        PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = 0;
                    }

                    // adding new command regularly?
                    // finding correct location to put it in
                    else
                    {
                        // I don't think "commandOffset" is the right name for this...
                        int commandOffset = numberOfCommandsAlreadyInAction * 2 + 4; // k
                        int stoppingPoint = openAreaStartLocation;
                        int bitStoppingPoint = 0;

                        for (int i = openAreaStartLocation; i < dataSectionSizeBytes; i++)
                        {
                            stoppingPoint++;
                            if (PsaFile.FileContent[i] == -86052851) // -86052851 is equal to FFFF FFFF FADE F00D in hex, which means empty space that PSA created...I think
                            {
                                bitStoppingPoint = i + 1;
                                for (int j = i + 1; j < i + commandOffset; j++)
                                {
                                    bitStoppingPoint = j;
                                    if (PsaFile.FileContent[j] != -86052851)
                                    {
                                        i = j;
                                        stoppingPoint = i;
                                        break;
                                    }
                                }
                                if (bitStoppingPoint == i + commandOffset)
                                {
                                    break;
                                }
                            }
                        }

                        // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
                        if (stoppingPoint >= dataSectionSizeBytes)
                        {
                            stoppingPoint = dataSectionSizeBytes;
                            if (PsaFile.FileContent[dataSectionSizeBytes - 2] == -86110838) // -86110838 is FFFF FFFF FADE 0D8A
                            {
                                stoppingPoint -= 2;
                                PsaFile.FileContent[dataSectionSizeBytes + commandOffset - 2] = -86110838;
                                PsaFile.FileContent[dataSectionSizeBytes + commandOffset - 1] = PsaFile.FileContent[dataSectionSizeBytes - 1];
                            }
                            dataSectionSizeBytes += commandOffset;
                            PsaFile.DataSectionSize = dataSectionSizeBytes * 4;

                        }

                        commandOffset = numberOfCommandsAlreadyInAction * 2;

                        // commandOffset might be the number of bits (or bytes?) taken up by all of the commands' pointers currently in the action
                        // also NGL I have NO clue what this is doing
                        int relocatingOffset = 0;
                        for (int i = 0; i < commandOffset; i += 2)
                        {
                            // asc stuff (PointerInterlock)
                            // NO clue what this does exactly or how it works
                            if (((PsaFile.FileContent[commandStartLocation + i] >> 8) & 0xFF) != 0)
                            {
                                // psac uses rmv for this name, no idea what this variable means
                                int rmv = (commandStartLocation + i) * 4 + 4;
                                for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                                {
                                    if (PsaFile.PointerInterlockTracker[j] == rmv)
                                    {
                                        PsaFile.PointerInterlockTracker[j] = (stoppingPoint + i) * 4 + 4;
                                        break;
                                    }
                                }
                            }
                            relocatingOffset = i;
                            PsaFile.FileContent[stoppingPoint + i] = PsaFile.FileContent[commandStartLocation + i];
                            PsaFile.FileContent[commandStartLocation + i] = -86052851; // -86052851 is FFFF FFFF FADE F00D
                            PsaFile.FileContent[stoppingPoint + i + 1] = PsaFile.FileContent[commandStartLocation + i + 1];
                            PsaFile.FileContent[commandStartLocation + i + 1] = -86052851; // -86052851 is FFFF FFFF FADE F00D
                        }

                        PsaFile.FileContent[stoppingPoint + relocatingOffset] = 131072; // 131072 is nop
                        PsaFile.FileContent[commandStartLocation + relocatingOffset] = -86052851; // -86052851 is FFFF FFFF FADE F00D
                        // relocatingOffset++;
                        PsaFile.FileContent[stoppingPoint + relocatingOffset + 1] = 0;
                        PsaFile.FileContent[commandStartLocation + relocatingOffset] = -86052851; // -86052851 is FFFF FFFF FADE F00D

                        PsaFile.FileContent[stoppingPoint + relocatingOffset + 2] = 0;
                        PsaFile.FileContent[stoppingPoint + relocatingOffset + 3] = 0;


                        // I believe this part increases the global header counter for the number of offsets in the file and adds the new action's pointer to the pointer table
                        int codeBlockLocation = 0;
                        if (codeBlockId == 0)
                        {
                            codeBlockLocation = PsaFile.FileContent[DataSectionLocation + 9] / 4;
                        }
                        else if (codeBlockId == 1)
                        {
                            codeBlockLocation = PsaFile.FileContent[DataSectionLocation + 10] / 4;
                        }
                        PsaFile.FileContent[codeBlockLocation + codeBlockId] = stoppingPoint * 4;

                        Console.WriteLine("END RESULT: " + (stoppingPoint * 4).ToString("X"));
                        newOffsetLocation = stoppingPoint * 4;

                        // this is the "EvOffsetInterlock" stuff, not entirely sure what this does, maybe updates existing command offsets so they don't break (like gotos)?
                        // actionCodeBlockEndLocation is probably not the right name for this variable...
                        int actionCodeBlockEndLocation = actionCodeBlockLocation + numberOfCommandsAlreadyInAction * 8;
                        for (int i = openAreaStartLocation; i < dataSectionSizeBytes; i++)
                        {
                            if (PsaFile.FileContent[i] >= actionCodeBlockLocation && PsaFile.FileContent[i] <= actionCodeBlockEndLocation)
                            {
                                // DEFINITELY not a good name for this variable, NO idea what it's for
                                int locationRoot = i * 4;

                                for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                                {
                                    if (PsaFile.PointerInterlockTracker[j] == locationRoot)
                                    {
                                        if (PsaFile.FileContent[i] == actionCodeBlockLocation)
                                        {
                                            PsaFile.FileContent[i] = newOffsetLocation;
                                            break;
                                        }
                                        int offsetDifference = PsaFile.FileContent[i] - actionCodeBlockLocation;
                                        if (offsetDifference % 8 == 0)
                                        {
                                            PsaFile.FileContent[i] = newOffsetLocation + offsetDifference;
                                        }
                                        break;
                                    }
                                }
                            }
                        }

                        // Fixam (in PSA-C) method here -- I think this is what updates the global headers like filesize and stuff
                        // TODO: Fixam logic has to be added for adding new command to action that had no previous commands as well
                        // I don't really understand this part yet
                        // bad variable name...
                        int lastDataSectionValidValue = 0;

                        // last data section value is where FADE 0D8A is found?
                        if (PsaFile.FileContent[dataSectionSizeBytes - 2] == -86110838) // -86110838 is FFFF FFFF FADE 0D8A
                        {
                            lastDataSectionValidValue = dataSectionSizeBytes - 3;
                        }
                        else
                        {
                            lastDataSectionValidValue = dataSectionSizeBytes - 1;
                        }

                        // decrease last data section value while no fade foods are found ??
                        while (lastDataSectionValidValue >= dataSectionSizeBytes && PsaFile.FileContent[lastDataSectionValidValue] == -86052851) // -86052851 is FFFF FFFF FADE F00D
                        {
                            lastDataSectionValidValue--;
                        }

                        // not a clue what this does, maybe moves all the FADE F00DS over if there's free space?
                        int newDataSectionSize;
                        for (newDataSectionSize = lastDataSectionValidValue; lastDataSectionValidValue <= dataSectionSizeBytes - 1; lastDataSectionValidValue++)
                        {
                            if (PsaFile.FileContent[lastDataSectionValidValue] != -86052851) // -86052851 is FFFF FFFF FADE F00D
                            {
                                PsaFile.FileContent[newDataSectionSize] = PsaFile.FileContent[lastDataSectionValidValue];
                                newDataSectionSize++;
                            }
                        }

                        // change size of data section to match new size, which did change since a new command was added
                        PsaFile.DataSectionSize = newDataSectionSize * 4;
                        dataSectionSizeBytes = PsaFile.DataSectionSize / 4;

                        // this just threw me for a loop, WHAT is going on
                        Array.Sort(PsaFile.PointerInterlockTracker);

                        for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
                        {
                            PsaFile.FileContent[newDataSectionSize] = PsaFile.PointerInterlockTracker[i];
                            newDataSectionSize++;
                        }


                        int movesetTotalFileSize = (PsaFile.MovesetFileSize - 32) / 4;
                        int movesetFileSizeLeftoverSpace = PsaFile.MovesetFileSize % 4;
                        if (movesetFileSizeLeftoverSpace == 0)
                        {
                            movesetFileSizeLeftoverSpace = 4;
                        }
                        else
                        {
                            movesetTotalFileSize++;
                        }

                        for (int i = 0; i < PsaFile.CompressedSize; i++)
                        {
                            PsaFile.FileContent[newDataSectionSize] = PsaFile.CompressionTracker[i];
                            newDataSectionSize++;
                        }

                        PsaFile.MovesetFileSize = newDataSectionSize * 4 + movesetFileSizeLeftoverSpace + 28;
                        Console.WriteLine("NEW DATA SECTOINSIZE: " + newDataSectionSize);
                        Console.WriteLine("MOVESET FILE SIZE: " + PsaFile.MovesetFileSize);

                        // NO CLUE what FileHeader[17] is, this is the only place in the entire PSAC that it's used
                        // I'm guessing it just always needs to be the same as the MovesetFileSize at FileHeader[24]
                        PsaFile.FileHeader[17] = PsaFile.MovesetFileSize;

                        // this checks if moveset is now over 544kb, a limitation of PSAC that I want to remove
                        int newMovesetFileSizeBytes = (PsaFile.MovesetFileSize + 3) / 4;
                        if (newMovesetFileSizeBytes % 8 != 0)
                        {
                            newDataSectionSize = 8 - newMovesetFileSizeBytes % 8;
                            newMovesetFileSizeBytes += newDataSectionSize;
                        }
                        newMovesetFileSizeBytes += PsaFile.ExtraSpace - 8;
                        if (newMovesetFileSizeBytes > 139264)
                        {
                            Console.WriteLine("Current data size over 544kb");
                        }
                    }

                }


            }
            PsaFile.SaveFile("result.pac");
        }


    }
}
