﻿using PSA2.src.Models.Fighter;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    /// <summary>
    /// This class is responsible for handling commands being added to a code block
    /// <para>Works for all code blocks such as action entry/exit and subaction main/gfx/sfx/other</para>
    /// </summary>
    public class PsaCommandAdder
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandAdder(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation, PsaCommandParser psaCommandParser)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            PsaCommandParser = psaCommandParser;
        }

        /// <summary>
        /// Adds a command to a code block
        /// <para>A "nop" command will be placed at the end of the given code block</para>
        /// <para>If code block has no more room for commands, it will be relocated</para>
        /// <para>If code block has no current allocation for commands, it will be setup to have it</para>
        /// </summary>
        /// <param name="codeBlock">The codeblock to add the command to</param>
        public void AddCommand(CodeBlock codeBlock)
        {
            if (codeBlock.CommandsPointerLocation == 0 || codeBlock.CommandsPointerLocation >= CodeBlockDataStartLocation * 4 && codeBlock.CommandsPointerLocation < PsaFile.DataSectionSize)
            {
                // if there are currently no existing commands (pointer to commands is 0, meaning it is not pointing to anything)
                if (codeBlock.CommandsPointerLocation == 0)
                {
                    // setup codeblock to have commands, and then adds a nop command
                    SetupCodeBlockForCommands(codeBlock);
                }

                // if there are already existing commands in code block
                else
                {
                    // add nop command to end of code block commands
                    AddCommandToCodeBlock(codeBlock);
                }

                // update psa file headings
                PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
            }
            else
            {
                throw new DataMisalignedException("Code block location is not valid");
            }
        }

        /// <summary>
        /// Sets up a code block to have commands, and then adds a "nop" command to the code block
        /// <para>This "setup" is needed when a code block does not have any commands currently</para>
        /// </summary>
        /// <param name="codeBlock">The codeblock to add the command to</param>
        private void SetupCodeBlockForCommands(CodeBlock codeBlock)
        {
            // Get a new location to put commands in that has the required amount of free space
            int newCodeBlockCommandsLocation = PsaFile.FindLocationWithAmountOfFreeSpace(CodeBlockDataStartLocation, 4);

            // if there is no free space found, increase size of data section by the required amount of space needed and use that as the new code block commands location
            if (newCodeBlockCommandsLocation >= PsaFile.DataSectionSizeBytes)
            {
                newCodeBlockCommandsLocation = PsaFile.DataSectionSizeBytes;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 2] = Constants.FADE0D8A;
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 3] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                    newCodeBlockCommandsLocation -= 2;
                }
                PsaFile.DataSectionSizeBytes += 4;                
            }

            // add nop psa command
            PsaFile.FileContent[newCodeBlockCommandsLocation] = Constants.NOP;
            PsaFile.FileContent[newCodeBlockCommandsLocation + 1] = 0;

            // signifies the end of the code block commands list
            PsaFile.FileContent[newCodeBlockCommandsLocation + 2] = 0;
            PsaFile.FileContent[newCodeBlockCommandsLocation + 3] = 0;

            // set code block to point to new commands location
            int newCodeBlockCommandsPointerLocation = newCodeBlockCommandsLocation * 4;
            PsaFile.FileContent[codeBlock.Location] = newCodeBlockCommandsPointerLocation;

            // update offset tracker to include code block commands pointer (since it now has commands again)
            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = codeBlock.Location * 4;
            PsaFile.NumberOfOffsetEntries++;
        }

        /// <summary>
        /// Adds a "nop" command to the end of a code block
        /// <para>If there is no room for another command to be added, the entire commands section gets relocated to a place with more room</para>
        /// </summary>
        /// <param name="codeBlock">The codeblock to add the command to</param>
        private void AddCommandToCodeBlock(CodeBlock codeBlock)
        {
            // if commands list is at the edge of the data section and adding a new command would send it over the data section's limit, expand the data section to add the new command
            if (codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2 + 5 > PsaFile.DataSectionSizeBytes)
            {
                // increase size of data section and then add nop command
                int futureEndCodeBlockLocation = codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2 + 3;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A || futureEndCodeBlockLocation > PsaFile.DataSectionSizeBytes)
                {
                    // push back the end of the data section (signified with the FADE0D8A)
                    if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                    {
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                    }
                    PsaFile.FileContent[codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2 + 2] = Constants.FADEF00D;
                    PsaFile.FileContent[codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2 + 3] = Constants.FADEF00D;
                    PsaFile.DataSectionSizeBytes += 2;

                    int newCommandLocation = codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2;

                    // add nop psa command
                    PsaFile.FileContent[newCommandLocation] = Constants.NOP;
                    PsaFile.FileContent[newCommandLocation + 1] = 0;

                    // signifies the end of the action
                    PsaFile.FileContent[newCommandLocation + 2] = 0;
                    PsaFile.FileContent[newCommandLocation + 3] = 0;
                }
            }
            // if commands location is somewhere in the middle of the data section
            else
            {
                // if code block commands list has free space at the end of it, replace free space with nop command and call it a day
                int codeBlockEndLocation = codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2;
                if (PsaFile.FileContent[codeBlockEndLocation + 2] == Constants.FADEF00D || PsaFile.FileContent[codeBlockEndLocation + 3] == Constants.FADEF00D)
                {
                    // add nop psa command
                    PsaFile.FileContent[codeBlockEndLocation] = Constants.NOP;
                    PsaFile.FileContent[codeBlockEndLocation + 1] = 0;

                    // signifies the end of the action
                    PsaFile.FileContent[codeBlockEndLocation + 2] = 0;
                    PsaFile.FileContent[codeBlockEndLocation + 3] = 0;
                }
                // if code block commands list has no free space at the end of it, code block commands will need to be relocated to a new place with adequate space
                else
                {
                    // relocates code block commands to a new place with more room and adds nop command to the end
                    int newCodeBlockCommandsLocation = RelocateCodeBlockCommands(codeBlock);

                    // updates pointers in other commands to point to the new location where the code block commands were moved to
                    UpdateCommandPointers(codeBlock, newCodeBlockCommandsLocation);
                }
            }
        }

        /// <summary>
        /// Relocates code block commands to a new place with enough room to support its current commands plus one additional command (the one to be added)
        /// <para>Then it adds a "nop" command to the end of the code block command list</para>
        /// </summary>
        /// <param name="codeBlock">The code block to have its commands relocate</param>
        /// <returns>The new code block commands location</returns>
        private int RelocateCodeBlockCommands(CodeBlock codeBlock)
        {
            // get new location for commands to be moved to that has enough free space
            int commandsParamsSpaceRequired = codeBlock.NumberOfCommands * 2 + 4;
            int newCodeBlockCommandsLocation = PsaFile.FindLocationWithAmountOfFreeSpace(CodeBlockDataStartLocation, commandsParamsSpaceRequired);

            // if the new location is past the data section limit, expand the data section by the amount of space required
            if (newCodeBlockCommandsLocation >= PsaFile.DataSectionSizeBytes)
            {
                newCodeBlockCommandsLocation = PsaFile.DataSectionSizeBytes;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                {
                    newCodeBlockCommandsLocation -= 2;
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandsParamsSpaceRequired - 2] = Constants.FADE0D8A;
                    PsaFile.FileContent[PsaFile.DataSectionSizeBytes + commandsParamsSpaceRequired - 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                }
                PsaFile.DataSectionSizeBytes += commandsParamsSpaceRequired;
            }

            // move commands one at a time from old location to new location
            for (int commandIndex = 0; commandIndex < codeBlock.NumberOfCommands; commandIndex++)
            {
                // if command has params
                if (codeBlock.PsaCommands[commandIndex].NumberOfParams != 0)
                {
                    // update offset tracker to replace any references to the old command pointer to the new one
                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        if (PsaFile.OffsetInterlockTracker[j] == codeBlock.GetPsaCommandPointerLocation(commandIndex))
                        {
                            int newCodeBlockCommandPointerLocation = (newCodeBlockCommandsLocation + (commandIndex * 2)) * 4 + 4;
                            PsaFile.OffsetInterlockTracker[j] = newCodeBlockCommandPointerLocation;
                            break;
                        }
                    }
                }

                // move the old command over to the new command location
                // replace old command location with free space (FADEF00D)
                int oldCodeBlockCommandLocation = codeBlock.GetPsaCommandLocation(commandIndex);
                int newCodeBlockCommandLocation = newCodeBlockCommandsLocation + (commandIndex * 2);
                PsaFile.FileContent[newCodeBlockCommandLocation] = PsaFile.FileContent[oldCodeBlockCommandLocation];
                PsaFile.FileContent[newCodeBlockCommandLocation + 1] = PsaFile.FileContent[oldCodeBlockCommandLocation + 1];
                PsaFile.FileContent[oldCodeBlockCommandLocation] = Constants.FADEF00D;
                PsaFile.FileContent[oldCodeBlockCommandLocation + 1] = Constants.FADEF00D;
            }

            // replace end of old commands list with free space (FADEF00D) since it is no longer being used
            int commandPointerSpace = codeBlock.NumberOfCommands * 2;
            PsaFile.FileContent[codeBlock.CommandsLocation + commandPointerSpace] = Constants.FADEF00D;
            PsaFile.FileContent[codeBlock.CommandsLocation + commandPointerSpace + 1] = Constants.FADEF00D;

            // add nop psa command to end of commands list
            int newCommandLocation = newCodeBlockCommandsLocation + commandPointerSpace;
            PsaFile.FileContent[newCommandLocation] = Constants.NOP;
            PsaFile.FileContent[newCommandLocation + 1] = 0;

            // signifies the end of the action
            PsaFile.FileContent[newCommandLocation + 2] = 0;
            PsaFile.FileContent[newCommandLocation + 3] = 0;

            // update commands pointer to point to new location 
            int newCodeBlockCommandsPointerLocation = newCodeBlockCommandsLocation * 4;
            PsaFile.FileContent[codeBlock.CommandsPointerLocation] = newCodeBlockCommandsPointerLocation;

            return newCodeBlockCommandsPointerLocation;
        }

        /// <summary>
        /// This is called after a code block has its commands relocated
        /// <para>Scans through every command in every other code block in the file and checks if the old location is being pointed to (such as in a goto or subroutine param value)</para>
        /// <para>If any are found, those now out-of-date location references are updated to point to the new location</para>
        /// </summary>
        /// <param name="codeBlock">The old code block (before any relocation was done)</param>
        /// <param name="newCodeBlockCommandsPointerLocation">The location of the pointer that points to the new commands location</param>
        private void UpdateCommandPointers(CodeBlock codeBlock, int newCodeBlockCommandsPointerLocation)
        {
            int codeBlockEndLocation = codeBlock.CommandsPointerLocation + codeBlock.NumberOfCommands * 8;
            for (int i = CodeBlockDataStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
            {
                // if pointer value found falls within the code block commands' old location
                if (PsaFile.FileContent[i] >= codeBlock.CommandsPointerLocation && PsaFile.FileContent[i] <= codeBlockEndLocation)
                {
                    int pointerToOffsetLocation = i * 4;

                    // check if any current pointers reference that old location
                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        // if pointer to offset is found in offset interlock tracker
                        if (PsaFile.OffsetInterlockTracker[j] == pointerToOffsetLocation)
                        {
                            // if pointer in file location is currently equal to the old code block, replace it with new code block location 
                            if (PsaFile.FileContent[i] == codeBlock.CommandsPointerLocation)
                            {
                                PsaFile.FileContent[i] = newCodeBlockCommandsPointerLocation;
                            }
                            else
                            {
                                // if offset was pointing to a particular location in the code block, make sure it continues to point to that spot in the new code block
                                int offsetDifference = PsaFile.FileContent[i] - codeBlock.CommandsPointerLocation;
                                if (offsetDifference % 8 == 0)
                                {
                                    PsaFile.FileContent[i] = newCodeBlockCommandsPointerLocation + offsetDifference;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}
