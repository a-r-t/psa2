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

        public void AddCommand(CodeBlock codeBlock)
        {
            if (codeBlock.CommandsPointerLocation == 0 || codeBlock.CommandsPointerLocation >= OpenAreaStartLocation * 4 && codeBlock.CommandsPointerLocation < PsaFile.DataSectionSize)
            {
                // no commands yet exist for action
                if (codeBlock.CommandsPointerLocation == 0)
                {
                    SetupCodeBlockForCommands(codeBlock);
                }

                // if there are already existing commands for action
                else
                {
                    AddCommandToCodeBlock(codeBlock);
                }

                PsaFile.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
            }
            else
            {
                throw new DataMisalignedException("Action code block location is not valid");
            }
        }

        public void SetupCodeBlockForCommands(CodeBlock codeBlock)
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

            // nop psa command with no parameters
            PsaFile.FileContent[newCodeBlockCommandsLocation] = Constants.NOP;
            PsaFile.FileContent[newCodeBlockCommandsLocation + 1] = 0;

            // signifies the end of the action
            PsaFile.FileContent[newCodeBlockCommandsLocation + 2] = 0;
            PsaFile.FileContent[newCodeBlockCommandsLocation + 3] = 0;

            int newCodeBlockCommandsPointerLocation = newCodeBlockCommandsLocation * 4;

            // this increases the offset entires table by 1 for the new action offset entry
            PsaFile.FileContent[codeBlock.Location] = newCodeBlockCommandsPointerLocation;
            PsaFile.OffsetInterlockTracker[PsaFile.NumberOfOffsetEntries] = codeBlock.Location * 4;
            PsaFile.NumberOfOffsetEntries++;
        }

        public void AddCommandToCodeBlock(CodeBlock codeBlock)
        {
            // TODO: Is this needed if I already have the count?
            
            /*
            if (PsaFile.FileContent[codeBlock.CommandsLocation] == Constants.FADEF00D)
            {
                Console.WriteLine("HI");
                //numberOfCommandsAlreadyInCodeBlock = 0;
            }
            */

            // if data section size needs to be resized, first check if it doesn't by seeing if there's any free space at the end (FADE0D8As) and if so that's free real estate to add a new command to
            // I think this is relpacing FADE0D8As with FADEF00DS if possible, and if this happens it means there is enough space to add the new command without needing to relocate the action
            if (codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2 + 5 > PsaFile.DataSectionSizeBytes)
            {
                int futureEndCodeBlockLocation = codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2 + 3;
                if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A || futureEndCodeBlockLocation > PsaFile.DataSectionSizeBytes)
                {
                    if (PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A)
                    {
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 2];
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes + 1] = PsaFile.FileContent[PsaFile.DataSectionSizeBytes - 1];
                    }
                    PsaFile.FileContent[codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2 + 2] = Constants.FADEF00D;
                    PsaFile.FileContent[codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2 + 3] = Constants.FADEF00D;
                    PsaFile.DataSectionSizeBytes += 2;

                    int newCommandLocation = codeBlock.CommandsLocation + codeBlock.NumberOfCommands * 2;

                    // nop psa command with no parameters
                    PsaFile.FileContent[newCommandLocation] = Constants.NOP;
                    PsaFile.FileContent[newCommandLocation + 1] = 0;

                    // signifies the end of the action
                    PsaFile.FileContent[newCommandLocation + 2] = 0;
                    PsaFile.FileContent[newCommandLocation + 3] = 0;
                }
            }

            // The below code actually moves the action and its existing commands to a new location in the file if there's no room to add the new one
            // this part actually adds the new nop command
            else
            {
                int newCodeBlockCommandsLocation = RelocateCodeBlock(codeBlock);
                UpdateCommandPointers(codeBlock, newCodeBlockCommandsLocation);
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
        public int RelocateCodeBlock(CodeBlock codeBlock)
        {
            int commandsParamsSpaceRequired = codeBlock.NumberOfCommands * 2 + 4; // k
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

            // commandOffset might be the number of bits (or bytes?) taken up by all of the commands' pointers currently in the action
            // also NGL I have NO clue what this is doing
            for (int i = 0; i < codeBlock.NumberOfCommands; i++)
            {
                Console.WriteLine("I: " + i);
                int commandLocationOffset = i * 2;
                // asc stuff (PointerInterlock)
                // NO clue what this does exactly or how it works
                int numberOfParams = codeBlock.PsaCommands[i].NumberOfParams;

                if (numberOfParams != 0)
                {
                    // psac uses rmv for this name, no idea what this variable means
                    int commandPointerLocation = (codeBlock.CommandsLocation + commandLocationOffset) * 4 + 4;

                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        if (PsaFile.OffsetInterlockTracker[j] == commandPointerLocation)
                        {
                            int newCommandPointerLocation = (newCodeBlockLocation + commandLocationOffset) * 4 + 4;
                            PsaFile.OffsetInterlockTracker[j] = newCommandPointerLocation;
                            break;
                        }
                    }
                }
                // replace old locations with new locations, replace old with FADEF00Ds
                PsaFile.FileContent[newCodeBlockLocation + commandLocationOffset] = PsaFile.FileContent[codeBlock.CommandsLocation + commandLocationOffset];
                PsaFile.FileContent[newCodeBlockLocation + commandLocationOffset + 1] = PsaFile.FileContent[codeBlock.CommandsLocation + commandLocationOffset + 1];
                PsaFile.FileContent[codeBlock.CommandsLocation + commandLocationOffset] = Constants.FADEF00D;
                PsaFile.FileContent[codeBlock.CommandsLocation + commandLocationOffset + 1] = Constants.FADEF00D;
            }

            // replace end of commands list with FADEF00D
            int commandPointerSpace = codeBlock.NumberOfCommands * 2;
            PsaFile.FileContent[codeBlock.CommandsLocation + commandPointerSpace] = Constants.FADEF00D;
            PsaFile.FileContent[codeBlock.CommandsLocation + commandPointerSpace + 1] = Constants.FADEF00D;

            int newCommandLocation = newCodeBlockLocation + commandPointerSpace;

            // add nop psa command with no parameters
            PsaFile.FileContent[newCommandLocation] = Constants.NOP;
            PsaFile.FileContent[newCommandLocation + 1] = 0;

            // signifies the end of the action
            PsaFile.FileContent[newCommandLocation + 2] = 0;
            PsaFile.FileContent[newCommandLocation + 3] = 0;

            int newOffsetLocation = newCodeBlockLocation * 4;

            PsaFile.FileContent[codeBlock.CommandsPointerLocation] = newOffsetLocation;

            return newOffsetLocation;
        }

        /// <summary>
        /// Called after codeblock has been relocated
        /// Goes through all the commands in the entire PSA
        /// If any command has a pointer that pointed to this codeblock, it will be updated to point to its new location
        /// </summary>
        /// <param name="codeBlockLocation"></param>
        /// <param name="numberOfCommandsAlreadyInCodeBlock"></param>
        /// <param name="newOffsetLocation"></param>
        public void UpdateCommandPointers(CodeBlock codeBlock, int newOffsetLocation)
        {
            int codeBlockEndLocation = codeBlock.CommandsPointerLocation + codeBlock.NumberOfCommands * 8;
            for (int i = OpenAreaStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
            {
                // if value falls within code block location
                if (PsaFile.FileContent[i] >= codeBlock.CommandsPointerLocation && PsaFile.FileContent[i] <= codeBlockEndLocation)
                {
                    int pointerToOffsetLocation = i * 4;

                    for (int j = 0; j < PsaFile.NumberOfOffsetEntries; j++)
                    {
                        // if pointer to offset is found in offset interlock tracker
                        if (PsaFile.OffsetInterlockTracker[j] == pointerToOffsetLocation)
                        {
                            // if pointer in file location is currently equal to the old code block, replace it with new code block location 
                            if (PsaFile.FileContent[i] == codeBlock.CommandsPointerLocation)
                            {
                                PsaFile.FileContent[i] = newOffsetLocation;
                            }
                            else
                            {
                                // if offset was pointing to a particular location in the code block, make sure it continues to point to that spot in the new code block
                                int offsetDifference = PsaFile.FileContent[i] - codeBlock.CommandsPointerLocation;
                                if (offsetDifference % 8 == 0)
                                {
                                    PsaFile.FileContent[i] = newOffsetLocation + offsetDifference;
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
