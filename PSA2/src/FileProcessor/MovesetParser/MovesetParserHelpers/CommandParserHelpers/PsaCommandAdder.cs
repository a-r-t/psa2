using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers
{
    public class PsaCommandAdder
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandAdder(PsaFile psaFile, int dataSectionLocation)
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

        /// <summary>
        /// Gets starting location in data section where new actions can be placed
        /// </summary>
        /// <returns>location in data section -- "stf" in psa-c</returns>
        public int GetOpenAreaStartLocation() // stf
        {
            return 2014 + GetNumberOfSpecialActions() * 2;
        }

        /// <summary>
        /// Searches through data section for the desired amount of free space
        /// </summary>
        /// <param name="amountOfFreeSpace">amount of free space desired (as doubleword, e.g. 4 would look for 4 doublewords)</param>
        /// <returns>starting location where the desired amount of free space has been found</returns>
        public int FindLocationWithAmountOfFreeSpace(int amountOfFreeSpace)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();
            int stoppingPoint = openAreaStartLocation;

            while (stoppingPoint < PsaFile.DataSectionSizeBytes)
            {
                if (PsaFile.FileContent[stoppingPoint] == Constants.FADEF00D)
                {
                    bool hasEnoughSpace = true;
                    for (int i = 0; i < amountOfFreeSpace; i++)
                    {
                        if (PsaFile.FileContent[stoppingPoint + 1 + i] != Constants.FADEF00D)
                        {
                            hasEnoughSpace = false;
                            break;
                        }
                    }
                    if (hasEnoughSpace)
                    {
                        return stoppingPoint;
                    }
                }
                stoppingPoint++;
            }
            return stoppingPoint;
        }


        /********************************
         * ADDING NEW COMMAND TO ACTION *
         * ******************************/
        public void AddCommandToAction(int codeBlockLocation, int codeBlockCommandsLocation)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();

            if (codeBlockCommandsLocation == 0 || (codeBlockCommandsLocation >= openAreaStartLocation * 4 && codeBlockCommandsLocation < PsaFile.DataSectionSize))
            {
                // no commands yet exist for action
                if (codeBlockCommandsLocation == 0)
                {
                    CreateActionCodeBlock(codeBlockLocation);
                }

                // if there are already existing commands for action
                else
                {
                    AddCommandToExistingActionCodeBlock(codeBlockCommandsLocation);
                }

                PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
            }
            else
            {
                throw new DataMisalignedException("Action code block location is not valid");
            }
        }

        public void CreateActionCodeBlock(int codeBlockLocation)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();

            // I'm pretty sure this section finds the next available location to put a psa command, but not sure...
            int stoppingPoint = FindLocationWithAmountOfFreeSpace(4);

            // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
            if (stoppingPoint >= PsaFile.DataSectionSizeBytes)
            {
                IncreaseDataSectionSizeForActionWithNoExistingCommands();
            }

            // this creates the first "nop" command to start off the new location for the action's commands
            SetFileLocationToNop(stoppingPoint);

            // this increases the offset entires table by 1 for the new action offset entry
            PsaFile.FileContent[codeBlockLocation] = stoppingPoint * 4;
            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = codeBlockLocation * 4;
            PsaFile.NumberOfOffsetEntries++;

            int newOffsetLocation = stoppingPoint * 4;
        }

        public void AddCommandToExistingActionCodeBlock(int codeBlockCommandsLocation)
        {
            int commandStartLocation = codeBlockCommandsLocation / 4;

            int numberOfCommandsAlreadyInCodeBlock = PsaCommandParser.GetNumberOfPsaCommands(codeBlockCommandsLocation); // g

            // TODO: Is this needed if I already get the count above?
            if (PsaFile.FileContent[commandStartLocation] == Constants.FADEF00D) // -86052851 is FFFF FFFF FADE F00D
            {
                numberOfCommandsAlreadyInCodeBlock = 0;
            }

            // if data section size needs to be resized, first check if it doesn't by seeing if there's any free space at the end (FADE0D8As) and if so that's free real estate to add a new command to
            // I think this is relpacing FADE0D8As with FADEF00DS if possible, and if this happens it means there is enough space to add the new command without needing to relocate the action
            bool isFreeSpaceAvailable = false;
            if (commandStartLocation + numberOfCommandsAlreadyInCodeBlock * 2 + 5 > PsaFile.DataSectionSizeBytes)
            {
                isFreeSpaceAvailable = CheckForDataSectionTrailingFreeSpace(commandStartLocation, numberOfCommandsAlreadyInCodeBlock);
            }

            // I think this part checks if there is room for new command, and if there is, it doesn't need to move the existing commands all over to a new location so it dips early and just adds the nop
            // basically if either of the above if statemetns are true, this will be true
            if (isFreeSpaceAvailable)
            {
                SetFileLocationToNop(commandStartLocation + numberOfCommandsAlreadyInCodeBlock * 2);

                int newOffsetLocation = codeBlockCommandsLocation / 4;

            }

            // The below code actually moves the action and its existing commands to a new location in the file if there's no room to add the new one
            // this part actually adds the new nop command
            else
            {
                int newOffsetLocation = RelocateAction(codeBlockCommandsLocation, numberOfCommandsAlreadyInCodeBlock, commandStartLocation);
                ApplyOffsetInterlockLogic(codeBlockCommandsLocation, numberOfCommandsAlreadyInCodeBlock, newOffsetLocation);
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
        public int RelocateAction(int codeBlockCommandsLocation, int numberOfCommandsAlreadyInAction, int commandStartLocation)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();

            // I don't think "commandOffset" is the right name for this...
            int commandsParamsSpaceRequired = numberOfCommandsAlreadyInAction * 2 + 4; // k
            int stoppingPoint = FindLocationWithAmountOfFreeSpace(commandsParamsSpaceRequired);

            // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
            // stopping point is modified here if necessary
            if (stoppingPoint >= PsaFile.DataSectionSizeBytes)
            {
                stoppingPoint = IncreaseDataSectionSizeForActionWithExistingCommand(commandsParamsSpaceRequired);
            }

            commandsParamsSpaceRequired = numberOfCommandsAlreadyInAction * 2;

            // commandOffset might be the number of bits (or bytes?) taken up by all of the commands' pointers currently in the action
            // also NGL I have NO clue what this is doing
            int relocatingOffset = 0;
            for (int i = 0; i < commandsParamsSpaceRequired; i += 2)
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
                PsaFile.FileContent[commandStartLocation + i] = Constants.FADEF00D;
                PsaFile.FileContent[commandStartLocation + i + 1] = Constants.FADEF00D;
                relocatingOffset += 2;
            }

            PsaFile.FileContent[commandStartLocation + relocatingOffset] = Constants.FADEF00D;
            PsaFile.FileContent[commandStartLocation + relocatingOffset + 1] = Constants.FADEF00D;

            SetFileLocationToNop(stoppingPoint + relocatingOffset);

            int newOffsetLocation = stoppingPoint * 4;

            PsaFile.FileContent[codeBlockCommandsLocation] = newOffsetLocation;

            return newOffsetLocation;
        }

        public void SetFileLocationToNop(int location)
        {
            PsaFile.FileContent[location] = Constants.NOP;
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
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
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
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
            {
                stoppingPoint -= 2;
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandOffset - 2] = Constants.FADE0D8A;
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
            if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
            {
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = Constants.FADEF00D;
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = Constants.FADEF00D;
                PsaFile.DataSectionSizeBytes += 2;
                return true;
            }
            else if (commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3 > PsaFile.DataSectionSizeBytes)
            {
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 2] = Constants.FADEF00D;
                PsaFile.FileContent[commandStartLocation + numberOfCommandsAlreadyInAction * 2 + 3] = Constants.FADEF00D;
                PsaFile.DataSectionSizeBytes += 2;
                return true;
            }

            return false;
        }

    }
}
