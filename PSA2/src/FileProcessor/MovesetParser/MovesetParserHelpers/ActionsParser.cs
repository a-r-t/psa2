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
        public const int NOP = 131072; // 131072 is the nop command in psa (20000)
        public const int FADEF00D = -86052851; // -86052851 is FFFF FFFF FADE F00D
        public const int FADE0D8A = -86110838; // -86110838 is FFFF FFFF FADE 0D8A
        public const int ENTRY_CODE_BLOCK = 0;
        public const int EXIT_CODE_BLOCK = 1;

        public ActionsParser(PsaFile psaFile, int dataSectionLocation, string movesetBaseName)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandParser = new PsaCommandParser(PsaFile);
        }

        public int GetNumberOfSpecialActions()
        {
            //Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        public int GetTotalNumberOfActions()
        {
            return 274 + GetNumberOfSpecialActions();
        }

        /// <summary>
        /// Gets starting location in data section where new actions can be placed
        /// </summary>
        /// <returns>location in data section -- "stf" in psa-c</returns>
        public int GetOpenAreaStartLocation() // stf
        {
            return 2014 + GetNumberOfSpecialActions() * 2;
        }

        // this is the offset where action code starts (displayed in PSAC)
        public int GetActionCodeBlockCommandsLocation(int actionId, int codeBlockId)
        {
            int actionsCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);

            int actionCodeBlockCommandsLocation = PsaFile.FileContent[actionsCodeBlockLocation];

            return actionCodeBlockCommandsLocation;
        }

        public int GetActionCodeBlockLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockStartingLocation;

            switch(codeBlockId)
            {
                case ENTRY_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 9] / 4;
                    break;
                case EXIT_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 10] / 4;
                    break;
                default:
                    throw new ArgumentException("Invalid code block id -- only 0 (Entry) and 1 (Exit) are valid");
            }

            return actionCodeBlockStartingLocation + actionId;
        }

        public List<PsaCommand> GetPsaCommandsForActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return PsaCommandParser.GetPsaCommands(actionCodeBlockLocation);
        }

        public int GetNumberOfPsaCommandsInActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return PsaCommandParser.GetNumberOfPsaCommands(actionCodeBlockLocation);
        }

        public void AddCommandToAction(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId); // h
            int openAreaStartLocation = GetOpenAreaStartLocation();

            if (actionCodeBlockLocation == 0 || (actionCodeBlockLocation >= openAreaStartLocation * 4 && actionCodeBlockLocation < PsaFile.DataSectionSize))
            {
                // no commands yet exist for action
                if (actionCodeBlockLocation == 0)
                {
                    CreateActionCodeBlock(actionId, codeBlockId, openAreaStartLocation);
                }

                // if there are already existing commands for action
                else
                {
                    AddCommandToExistingActionCodeBlock(actionId, codeBlockId, actionCodeBlockLocation);
                }

                ApplyFileUpdatesToAccountForActionChanges();
            }
            else
            {
                throw new DataMisalignedException("Action code block location is not valid");
            }
        }

        public void CreateActionCodeBlock(int actionId, int codeBlockId, int openAreaStartLocation)
        {
            // I'm pretty sure this section finds the next available location to put a psa command, but not sure...
            int stoppingPoint = openAreaStartLocation;
            int bitStoppingPoint = 0;

            while (stoppingPoint < PsaFile.DataSectionSizeBytes && bitStoppingPoint != stoppingPoint + 4)
            {
                if (PsaFile.FileContent[stoppingPoint] == FADEF00D)
                {
                    bitStoppingPoint = stoppingPoint + 1;
                    while (bitStoppingPoint <= stoppingPoint + 3 && PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                    {
                        if (PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                        {
                            stoppingPoint = bitStoppingPoint;
                            break;
                        }
                        bitStoppingPoint++;
                    }
                }
                stoppingPoint++;
            }

            // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
            if (stoppingPoint >= PsaFile.DataSectionSizeBytes)
            {
                IncreaseDataSectionSizeForActionWithNoExistingCommands();
            }

            // this creates the first "nop" command to start off the new location for the action's commands
            SetFileLocationToNop(stoppingPoint);

            // this increases the offset entires table by 1 for the new action offset entry
            int codeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);

            PsaFile.FileContent[codeBlockLocation] = stoppingPoint * 4;
            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = codeBlockLocation * 4;
            PsaFile.NumberOfOffsetEntries++;

            int newOffsetLocation = stoppingPoint * 4;
        }

        public void AddCommandToExistingActionCodeBlock(int actionId, int codeBlockId, int actionCodeBlockLocation)
        {
            int commandStartLocation = actionCodeBlockLocation / 4;

            int numberOfCommandsAlreadyInAction = GetNumberOfPsaCommandsInActionCodeBlock(actionId, codeBlockId); // g

            // TODO: Is this needed if I already get the count above?
            if (PsaFile.FileContent[commandStartLocation] == FADEF00D) // -86052851 is FFFF FFFF FADE F00D
            {
                numberOfCommandsAlreadyInAction = 0;
            }

            // if data section size needs to be resized, first check if it doesn't by seeing if there's any free space at the end (FADE0D8As) and if so that's free real estate to add a new command to
            // I think this is relpacing FADE0D8As with FADEF00DS if possible, and if this happens it means there is enough space to add the new command without needing to relocate the action
            bool isFreeSpaceAvailable = false;
            if (commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 5 > PsaFile.DataSectionSizeBytes)
            {
                isFreeSpaceAvailable = CheckForDataSectionTrailingFreeSpace(commandStartLocation, numberOfCommandsAlreadyInAction);
            }

            // I think this part checks if there is room for new command, and if there is, it doesn't need to move the existing commands all over to a new location so it dips early and just adds the nop
            // basically if either of the above if statemetns are true, this will be true
            if (isFreeSpaceAvailable)
            {
                SetFileLocationToNop(commandStartLocation + numberOfCommandsAlreadyInAction * 2);

                int newOffsetLocation = actionCodeBlockLocation / 4;

            }

            // The below code actually moves the action and its existing commands to a new location in the file if there's no room to add the new one
            // this part actually adds the new nop command
            else
            {
                int newOffsetLocation = RelocateAction(actionId, codeBlockId, numberOfCommandsAlreadyInAction, commandStartLocation);
                ApplyOffsetInterlockLogic(actionCodeBlockLocation, numberOfCommandsAlreadyInAction, newOffsetLocation);
            }
        }

        /// <summary>
        /// relocates action to a new location in file
        /// this happens when trying to add a command to an action and there is no more room to add the command
        /// returns new offset location for commands in action
        /// </summary>
        /// <param name="actionId">id of action (0 = 112)</param>
        /// <param name="codeBlockId">id of code block (0 is Entry, 1 is Exit)</param>
        /// <param name="numberOfCommandsAlreadyInAction">number of commands already in action</param>
        /// <param name="commandStartLocation">location where action's commands start</param>
        /// <returns>new offset location for commands in action (relocated offset)</returns>
        public int RelocateAction(int actionId, int codeBlockId, int numberOfCommandsAlreadyInAction, int commandStartLocation)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();

            // I don't think "commandOffset" is the right name for this...
            int commandOffset = numberOfCommandsAlreadyInAction * 2 + 4; // k
            int stoppingPoint = openAreaStartLocation;
            int bitStoppingPoint = 0;

            while (stoppingPoint < PsaFile.DataSectionSizeBytes && bitStoppingPoint != stoppingPoint + commandOffset)
            {
                if (PsaFile.FileContent[stoppingPoint] == FADEF00D)
                {
                    bitStoppingPoint = stoppingPoint + 1;
                    while (bitStoppingPoint <= stoppingPoint && PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                    {
                        if (PsaFile.FileContent[bitStoppingPoint] != FADEF00D)
                        {
                            stoppingPoint = bitStoppingPoint;
                            break;
                        }
                        bitStoppingPoint++;
                    }
                }
                stoppingPoint++;
            }

            // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
            // stopping point is modified here if necessary
            if (stoppingPoint >= PsaFile.DataSectionSizeBytes)
            {
                stoppingPoint = IncreaseDataSectionSizeForActionWithExistingCommand(commandOffset);
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
                        if (PsaFile.OffsetInterlockTracker[j] == rmv)
                        {
                            PsaFile.OffsetInterlockTracker[j] = (stoppingPoint + i) * 4 + 4;
                            break;
                        }
                    }
                }
                relocatingOffset = i;
                PsaFile.FileContent[stoppingPoint + i] = PsaFile.FileContent[commandStartLocation + i];
                PsaFile.FileContent[stoppingPoint + i + 1] = PsaFile.FileContent[commandStartLocation + i + 1];
                PsaFile.FileContent[commandStartLocation + i] = FADEF00D; 
                PsaFile.FileContent[commandStartLocation + i + 1] = FADEF00D; 
                relocatingOffset += 2;
            }

            PsaFile.FileContent[commandStartLocation + relocatingOffset] = FADEF00D; 
            PsaFile.FileContent[commandStartLocation + relocatingOffset + 1] = FADEF00D;

            SetFileLocationToNop(stoppingPoint + relocatingOffset);

            int codeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            int newOffsetLocation = stoppingPoint * 4;

            PsaFile.FileContent[codeBlockLocation] = newOffsetLocation;

            return newOffsetLocation;
        }

        public void SetFileLocationToNop(int location)
        {
            PsaFile.FileContent[location] = NOP;
            PsaFile.FileContent[location + 1] = 0;
            PsaFile.FileContent[location + 2] = 0;
            PsaFile.FileContent[location + 3] = 0;
        }

        public void ApplyOffsetInterlockLogic(int actionCodeBlockLocation, int numberOfCommandsAlreadyInAction, int newOffsetLocation)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();

            // this is the "EvOffsetInterlock" stuff, not entirely sure what this does, maybe updates existing command offsets so they don't break (like gotos)?
            // actionCodeBlockEndLocation is probably not the right name for this variable...
            int actionCodeBlockEndLocation = actionCodeBlockLocation + numberOfCommandsAlreadyInAction * 8;
            for (int i = openAreaStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
            {
                if (PsaFile.FileContent[i] >= actionCodeBlockLocation && PsaFile.FileContent[i] <= actionCodeBlockEndLocation)
                {
                    // DEFINITELY not a good name for this variable, NO idea what it's for
                    int locationRoot = i * 4;

                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        if (PsaFile.OffsetInterlockTracker[j] == locationRoot)
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
        }

        public int IncreaseDataSectionSizeForActionWithNoExistingCommands()
        {
            int stoppingPoint = PsaFile.DataSectionSizeBytes;
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
            {
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 2] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 3] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                stoppingPoint -= 2;
            }
            PsaFile.DataSectionSizeBytes += 4;

            return stoppingPoint;
        }

        // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
        public int IncreaseDataSectionSizeForActionWithExistingCommand(int commandOffset)
        {
            int stoppingPoint = PsaFile.DataSectionSizeBytes;
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
            {
                stoppingPoint -= 2;
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandOffset - 2] = FADE0D8A;
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandOffset - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
            }
            PsaFile.DataSectionSizeBytes += commandOffset;

            return stoppingPoint;
        }

        /// <summary>
        /// This will check if the data section has free space after it, and if so "claims" this free space as part of the data section
        /// returns if there is free space found after doing this process
        /// </summary>
        public bool CheckForDataSectionTrailingFreeSpace(int commandStartLocation, int numberOfCommandsAlreadyInAction)
        {
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A)
            {
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = FADEF00D; 
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = FADEF00D;
                PsaFile.DataSectionSizeBytes += 2;
                return true;
            }
            else if (commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3 > PsaFile.DataSectionSizeBytes)
            {
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = FADEF00D;
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = FADEF00D;
                PsaFile.DataSectionSizeBytes += 2;
                return true;
            }

            return false;
        }

        /// <summary>
        /// fixam
        /// </summary>
        public void ApplyFileUpdatesToAccountForActionChanges()
        {
            // Fixam (in PSA-C) method here -- I think this is what updates the global headers like filesize and stuff
            // TODO: Fixam logic has to be added for adding new command to action that had no previous commands as well
            // I don't really understand this part yet
            // bad variable name...
            // last data section value is where FADE 0D8A is found?
            int lastDataSectionValidValue = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == FADE0D8A 
                ? PsaFile.DataSectionSizeBytes - 3 
                : PsaFile.DataSectionSizeBytes - 1;

            // decrease last data section value while no fade foods are found ??
            while (lastDataSectionValidValue >= PsaFile.DataSectionSizeBytes && PsaFile.FileContent[lastDataSectionValidValue] == FADEF00D)
            {
                lastDataSectionValidValue--;
            }

            // not a clue what this does, maybe moves all the FADE F00DS over if there's free space?
            int newDataSectionSize = lastDataSectionValidValue;
            while (newDataSectionSize < PsaFile.DataSectionSizeBytes)
            {
                if (PsaFile.FileContent[lastDataSectionValidValue] != FADEF00D)
                {
                    PsaFile.FileContent[newDataSectionSize] = PsaFile.FileContent[lastDataSectionValidValue];
                    newDataSectionSize++;
                }
            }

            // change size of data section to match new size, which did change since a new command was added
            PsaFile.DataSectionSize = newDataSectionSize * 4;

            // this just threw me for a loop, WHAT is going on
            Array.Sort(PsaFile.OffsetInterlockTracker);

            for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
            {
                PsaFile.FileContent[newDataSectionSize] = PsaFile.OffsetInterlockTracker[i];
                newDataSectionSize++;
            }

            int movesetFileSizeLeftoverSpace = PsaFile.MovesetFileSize % 4;
            if (movesetFileSizeLeftoverSpace == 0)
            {
                movesetFileSizeLeftoverSpace = 4;
            }

            for (int i = 0; i < PsaFile.CompressedSize; i++)
            {
                PsaFile.FileContent[newDataSectionSize] = PsaFile.CompressionTracker[i];
                newDataSectionSize++;
            }

            PsaFile.MovesetFileSize = newDataSectionSize * 4 + movesetFileSizeLeftoverSpace + 28;

            // NO CLUE what FileHeader[17] is, this is the only place in the entire PSAC that it's used
            // I'm guessing it just always needs to be the same as the MovesetFileSize at FileHeader[24]
            PsaFile.FileHeader[17] = PsaFile.MovesetFileSize;

            // this checks if moveset is now over 544kb, a limitation of PSAC that I will later remove
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
