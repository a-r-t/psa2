using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Models.Fighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class CodeBlocksHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }

        public CodeBlocksHandler(PsaFile psaFile, int dataSectionLocation, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
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
            int actionCodeBlockCommandsLocation = GetCodeBlockCommandsLocation(codeBlockLocation);
            return actionCodeBlockCommandsLocation + commandIndex * 2;
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
            PsaCommandHandler.AddCommand(codeBlock);
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
