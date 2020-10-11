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

        public void MoveCommand(int codeBlockLocation, int commandIndex, MoveDirection moveDirection)
        {
            int actionCodeBlockCommandLocation = GetCodeBlockCommandLocation(codeBlockLocation, commandIndex);
            PsaCommand psaCommandToMove = GetPsaCommandForCodeBlock(codeBlockLocation, commandIndex);
            CodeBlock codeBlock = GetCodeBlock(codeBlockLocation);
            PsaCommandHandler.MoveCommand(codeBlock, psaCommandToMove, actionCodeBlockCommandLocation, moveDirection);
        }

    }
}
