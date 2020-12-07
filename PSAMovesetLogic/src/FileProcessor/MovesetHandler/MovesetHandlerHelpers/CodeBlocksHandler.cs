using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.Models.Fighter;
using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class CodeBlocksHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }

        public CodeBlocksHandler(PsaFile psaFile, int dataSectionLocation, PsaCommandHandler psaCommandHandler, int codeBlockDataStartLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
        }

        public CodeBlock GetCodeBlock(int codeBlockLocation)
        {
            int codeBlockCommandsPointerLocation = GetCodeBlockCommandsPointerLocation(codeBlockLocation);
            int codeBlockCommandsLocation = GetCodeBlockCommandsLocation(codeBlockLocation);
            List<PsaCommand> psaCommands = GetPsaCommandsForCodeBlock(codeBlockLocation);
            return new CodeBlock(codeBlockLocation, codeBlockCommandsPointerLocation, codeBlockCommandsLocation, psaCommands);
        }

        public int GetCodeBlockCommandsPointerLocation(int codeBlockLocation)
        {
            return PsaFile.DataSection[codeBlockLocation];
        }

        public int GetCodeBlockCommandsLocation(int codeBlockLocation)
        {
            return GetCodeBlockCommandsPointerLocation(codeBlockLocation) / 4;
        }

        public int GetCodeBlockCommandLocation(int codeBlockLocation, int commandIndex)
        {
            int codeBlockCommandsLocation = GetCodeBlockCommandsLocation(codeBlockLocation);
            return codeBlockCommandsLocation + commandIndex * 2;
        }

        public List<PsaCommand> GetPsaCommandsForCodeBlock(int codeBlockLocation)
        {
            int codeBlockCommandsLocation = GetCodeBlockCommandsLocation(codeBlockLocation);
            return PsaCommandHandler.GetPsaCommands(codeBlockCommandsLocation);
        }

        public PsaCommand GetPsaCommandForCodeBlock(int codeBlockLocation, int commandIndex)
        {
            int codeBlockCommandLocation = GetCodeBlockCommandLocation(codeBlockLocation, commandIndex);
            return PsaCommandHandler.GetPsaCommand(codeBlockCommandLocation);
        }

        public int GetNumberOfPsaCommandsInCodeBlock(int codeBlockLocation)
        {
            int codeBlockCommandsLocation = GetCodeBlockCommandsLocation(codeBlockLocation);
            return PsaCommandHandler.GetNumberOfPsaCommands(codeBlockCommandsLocation);
        }

        public void AddCommand(int codeBlockLocation)
        {
            CodeBlock codeBlock = GetCodeBlock(codeBlockLocation);

            // if commands pointer location is 0, code block needs to be set up
            // it will be 0 if it currently has zero commands
            if (codeBlock.CommandsPointerLocation == 0)
            {
                SetupCodeBlockForCommands(codeBlock);
            }
            else
            {
                PsaCommandHandler.AddCommand(codeBlock);
            }
        }

        /// <summary>
        /// Sets up a code block to have commands, and then adds a "nop" command to the code block
        /// <para>This "setup" is needed when a code block does not have any commands currently</para>
        /// </summary>
        /// <param name="codeBlock">The codeblock to setup</param>
        private void SetupCodeBlockForCommands(CodeBlock codeBlock)
        {
            // Get a new location to put commands in that has the required amount of free space
            int newCodeBlockCommandsLocation = PsaFile.HelperMethods.FindLocationWithAmountOfFreeSpace(CodeBlockDataStartLocation, 4);

            // if there is no free space found, increase size of data section by the required amount of space needed and use that as the new code block commands location
            if (newCodeBlockCommandsLocation >= PsaFile.DataSection.Count)
            {
                newCodeBlockCommandsLocation = PsaFile.DataSection.Count;
            }

            // add a NOP command to the action code block
            PsaFile.HelperMethods.SetDataSectionValue(newCodeBlockCommandsLocation, Constants.NOP);
            PsaFile.HelperMethods.SetDataSectionValue(newCodeBlockCommandsLocation + 1, 0);
            PsaFile.HelperMethods.SetDataSectionValue(newCodeBlockCommandsLocation + 2, 0);
            PsaFile.HelperMethods.SetDataSectionValue(newCodeBlockCommandsLocation + 3, 0);

            // set code block to point to new commands location
            int newCodeBlockCommandsPointerLocation = newCodeBlockCommandsLocation * 4;
            PsaFile.DataSection[codeBlock.Location] = newCodeBlockCommandsPointerLocation;

            // update offset tracker to include code block commands pointer (since it now has commands again)
            PsaFile.OffsetSection.Add(codeBlock.Location * 4);

            PsaFile.HelperMethods.UpdateMovesetHeaders(DataSectionLocation);
        }

        public void ModifyCommand(int codeBlockLocation, int commandIndex, PsaCommand newPsaCommand)
        {
            int codeBlockCommandLocation = GetCodeBlockCommandLocation(codeBlockLocation, commandIndex);
            PsaCommand oldPsaCommand = GetPsaCommandForCodeBlock(codeBlockLocation, commandIndex);
            PsaCommandHandler.ModifyCommand(codeBlockCommandLocation, oldPsaCommand, newPsaCommand);
        }

        public void RemoveCommand(int codeBlockLocation, int commandIndex)
        {
            PsaCommand removedPsaCommand = GetPsaCommandForCodeBlock(codeBlockLocation, commandIndex);
            CodeBlock codeBlock = GetCodeBlock(codeBlockLocation);
            int codeBlockCommandLocation = codeBlock.GetPsaCommandLocation(commandIndex);
            PsaCommandHandler.RemoveCommand(codeBlock, codeBlockCommandLocation, removedPsaCommand);

            codeBlock = GetCodeBlock(codeBlockLocation);
            if (codeBlock.PsaCommands.Count == 0)
            {
                TearDownCodeBlock(codeBlock);
            }
        }

        /// <summary>
        /// "Tear down" codeblock aka it no longer holds any psa commands/data
        /// <para>This is called when last command in a codeblock is removed in order to save on space</para>
        /// </summary>
        /// <param name="codeBlock"></param>
        private void TearDownCodeBlock(CodeBlock codeBlock)
        {
            // code block location is set to 0 since a code block with no commands is pointless anyway
            PsaFile.DataSection[codeBlock.Location] = 0;

            // remove any existing pointer references to the code block
            int pointerToCodeBlockLocation = codeBlock.Location * 4;
            PsaFile.HelperMethods.RemoveOffsetFromOffsetInterlockTracker(pointerToCodeBlockLocation);
        } 

        public void MoveCommandUp(int codeBlockLocation, int commandIndex)
        {
            if (commandIndex > 0)
            {
                int codeBlockCommandLocation = GetCodeBlockCommandLocation(codeBlockLocation, commandIndex);
                PsaCommand psaCommandToMove = GetPsaCommandForCodeBlock(codeBlockLocation, commandIndex);
                CodeBlock codeBlock = GetCodeBlock(codeBlockLocation);
                PsaCommandHandler.MoveCommand(codeBlock, psaCommandToMove, codeBlockCommandLocation, MoveDirection.UP);
            }
        }

        public void MoveCommandDown(int codeBlockLocation, int commandIndex)
        {
            CodeBlock codeBlock = GetCodeBlock(codeBlockLocation);
            if (commandIndex < codeBlock.NumberOfCommands - 1)
            {
                int codeBlockCommandLocation = GetCodeBlockCommandLocation(codeBlockLocation, commandIndex);
                PsaCommand psaCommandToMove = GetPsaCommandForCodeBlock(codeBlockLocation, commandIndex);
                PsaCommandHandler.MoveCommand(codeBlock, psaCommandToMove, codeBlockCommandLocation, MoveDirection.DOWN);
            }
        }

        public void InsertCommand(int codeBlockLocation, int commandIndex, PsaCommand newPsaCommand)
        {
            // add command (NOP) to end of code block
            AddCommand(codeBlockLocation);

            CodeBlock codeBlock = GetCodeBlock(codeBlockLocation);

            // if command index is greater than the total number of commands, set it equal to the last command index (which is where a new command was just added in the line above)
            // this allows for inserting below the last command of a code block (e.g. if there are 5 commands, a command can be "inserted" as command 6)
            commandIndex = commandIndex > codeBlock.NumberOfCommands - 1
                ? codeBlock.NumberOfCommands - 1
                : commandIndex;

            // move command upwards to desired index
            for (int i = codeBlock.NumberOfCommands - 1; i > commandIndex; i--)
            {
                PsaCommand psaCommandToMove = GetPsaCommandForCodeBlock(codeBlockLocation, i);
                int codeBlockCommandLocation = GetCodeBlockCommandLocation(codeBlockLocation, i);
                PsaCommandHandler.MoveCommand(codeBlock, psaCommandToMove, codeBlockCommandLocation, MoveDirection.UP);
            }

            // replace NOP command with desired command
            PsaCommand oldPsaCommand = GetPsaCommandForCodeBlock(codeBlockLocation, commandIndex);
            int oldPsaCommandLocation = codeBlock.GetPsaCommandLocation(commandIndex);
            PsaCommandHandler.ModifyCommand(oldPsaCommandLocation, oldPsaCommand, newPsaCommand);
        }
    }
}
