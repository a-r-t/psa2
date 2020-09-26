﻿using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    public class PsaCommandAdder
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int OpenAreaStartLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandAdder(PsaFile psaFile, int dataSectionLocation, int openAreaStartLocation, PsaCommandParser psaCommandParser)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            OpenAreaStartLocation = openAreaStartLocation;
            PsaCommandParser = psaCommandParser;
        }

        public void AddCommand(int codeBlockLocation, int codeBlockCommandsPointerLocation)
        {
            if (codeBlockCommandsPointerLocation == 0 || codeBlockCommandsPointerLocation >= OpenAreaStartLocation * 4 && codeBlockCommandsPointerLocation < PsaFile.DataSectionSize)
            {
                // no commands yet exist for action
                if (codeBlockCommandsPointerLocation == 0)
                {
                    CreateCodeBlock(codeBlockLocation);
                }

                // if there are already existing commands for action
                else
                {
                    AddCommandToExistingCodeBlock(codeBlockCommandsPointerLocation);
                }

                PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
            }
            else
            {
                throw new DataMisalignedException("Action code block location is not valid");
            }
        }

        public void CreateCodeBlock(int codeBlockLocation)
        {
            // I'm pretty sure this section finds the next available location to put a psa command, but not sure...
            int newCodeBlockCommandsLocation = PsaFile.FindLocationWithAmountOfFreeSpace(OpenAreaStartLocation, 4);

            // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
            if (newCodeBlockCommandsLocation >= PsaFile.DataSectionSizeBytes)
            {
                newCodeBlockCommandsLocation = PsaFile.DataSectionSizeBytes;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 2] = Constants.FADE0D8A; // PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 3] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                    newCodeBlockCommandsLocation -= 2;
                }
                PsaFile.DataSectionSizeBytes += 4;                
            }

            // this creates the first "nop" command to start off the new location for the action's commands
            SetFileLocationToNop(newCodeBlockCommandsLocation);
            int newCodeBlockCommandsPointerLocation = newCodeBlockCommandsLocation * 4;

            // this increases the offset entires table by 1 for the new action offset entry
            PsaFile.FileContent[codeBlockLocation] = newCodeBlockCommandsPointerLocation;
            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = codeBlockLocation * 4;
            PsaFile.NumberOfOffsetEntries++;
        }

        public void AddCommandToExistingCodeBlock(int codeBlockCommandsPointerLocation)
        {
            int codeBlockCommandsLocation = codeBlockCommandsPointerLocation / 4;

            int numberOfCommandsAlreadyInCodeBlock = PsaCommandParser.GetNumberOfPsaCommands(codeBlockCommandsLocation); // g

            // TODO: Is this needed if I already get the count above?
            if (PsaFile.FileContent[codeBlockCommandsLocation] == Constants.FADEF00D)
            {
                numberOfCommandsAlreadyInCodeBlock = 0;
            }

            // if data section size needs to be resized, first check if it doesn't by seeing if there's any free space at the end (FADE0D8As) and if so that's free real estate to add a new command to
            // I think this is relpacing FADE0D8As with FADEF00DS if possible, and if this happens it means there is enough space to add the new command without needing to relocate the action
            if (codeBlockCommandsLocation + numberOfCommandsAlreadyInCodeBlock * 2 + 5 > PsaFile.DataSectionSizeBytes)
            {
                int futureEndCodeBlockLocation = codeBlockCommandsLocation + numberOfCommandsAlreadyInCodeBlock * 2 + 3;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A || futureEndCodeBlockLocation > PsaFile.DataSectionSizeBytes)
                {
                    if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                    {
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                    }
                    PsaFile.FileContent[codeBlockCommandsLocation + numberOfCommandsAlreadyInCodeBlock * 2 + 2] = Constants.FADEF00D;
                    PsaFile.FileContent[codeBlockCommandsLocation + numberOfCommandsAlreadyInCodeBlock * 2 + 3] = Constants.FADEF00D;
                    PsaFile.DataSectionSizeBytes += 2;
                    SetFileLocationToNop(codeBlockCommandsLocation + numberOfCommandsAlreadyInCodeBlock * 2);
                }
            }

            // The below code actually moves the action and its existing commands to a new location in the file if there's no room to add the new one
            // this part actually adds the new nop command
            else
            {
                int newCodeBlockCommandsLocation = RelocateCodeBlock(codeBlockCommandsPointerLocation, numberOfCommandsAlreadyInCodeBlock);
                ApplyOffsetInterlockLogic(codeBlockCommandsPointerLocation, numberOfCommandsAlreadyInCodeBlock, newCodeBlockCommandsLocation);
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
        /// <returns>new offset location for commands in action (relocated offset)</returns>
        public int RelocateCodeBlock(int codeBlockCommandsPointerLocation, int numberOfCommandsAlreadyInAction)
        {
            int codeBlockCommandsLocation = codeBlockCommandsPointerLocation / 4;

            int commandsParamsSpaceRequired = numberOfCommandsAlreadyInAction * 2 + 4; // k
            int newCodeBlockLocation = PsaFile.FindLocationWithAmountOfFreeSpace(OpenAreaStartLocation, commandsParamsSpaceRequired);

            // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section
            // stopping point is modified here if necessary
            if (newCodeBlockLocation >= PsaFile.DataSectionSizeBytes)
            {
                // this increases the size of the data section, I guess this happens if you try to add a new command and there's no room left in the data section  
                newCodeBlockLocation = PsaFile.DataSectionSizeBytes;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    newCodeBlockLocation -= 2;
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandsParamsSpaceRequired - 2] = Constants.FADE0D8A;
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandsParamsSpaceRequired - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                }
                PsaFile.DataSectionSizeBytes += commandsParamsSpaceRequired;
            }
            
            commandsParamsSpaceRequired = numberOfCommandsAlreadyInAction * 2;

            // commandOffset might be the number of bits (or bytes?) taken up by all of the commands' pointers currently in the action
            // also NGL I have NO clue what this is doing
            int relocatingOffset = 0;
            for (int i = 0; i < commandsParamsSpaceRequired; i += 2)
            {
                // asc stuff (PointerInterlock)
                // NO clue what this does exactly or how it works
                int numberOfParams = PsaFile.FileContent[codeBlockCommandsLocation + i] >> 8 & 0xFF;
                if (numberOfParams != 0)
                {
                    // psac uses rmv for this name, no idea what this variable means
                    int rmv = (codeBlockCommandsLocation + i) * 4 + 4;
                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        if (PsaFile.OffsetInterlockTracker[j] == rmv)
                        {
                            PsaFile.OffsetInterlockTracker[j] = (newCodeBlockLocation + i) * 4 + 4;
                            break;
                        }
                    }
                }
                relocatingOffset = i;
                PsaFile.FileContent[newCodeBlockLocation + i] = PsaFile.FileContent[codeBlockCommandsLocation + i];
                PsaFile.FileContent[newCodeBlockLocation + i + 1] = PsaFile.FileContent[codeBlockCommandsLocation + i + 1];
                PsaFile.FileContent[codeBlockCommandsLocation + i] = Constants.FADEF00D;
                PsaFile.FileContent[codeBlockCommandsLocation + i + 1] = Constants.FADEF00D;
                relocatingOffset += 2;
            }

            PsaFile.FileContent[codeBlockCommandsLocation + relocatingOffset] = Constants.FADEF00D;
            PsaFile.FileContent[codeBlockCommandsLocation + relocatingOffset + 1] = Constants.FADEF00D;

            SetFileLocationToNop(newCodeBlockLocation + relocatingOffset);

            int newOffsetLocation = newCodeBlockLocation * 4;

            PsaFile.FileContent[codeBlockCommandsPointerLocation] = newOffsetLocation;

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
            // this is the "EvOffsetInterlock" stuff, not entirely sure what this does, maybe updates existing command offsets so they don't break (like gotos)?
            // actionCodeBlockEndLocation is probably not the right name for this variable...
            int actionCodeBlockEndLocation = actionCodeBlockLocation + numberOfCommandsAlreadyInAction * 8;
            for (int i = OpenAreaStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
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