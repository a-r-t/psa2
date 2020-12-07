using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.Models.Fighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class PsaCommandHandler
    {
        private PsaCommandParser psaCommandParser;
        private PsaCommandAdder psaCommandAdder;
        private PsaCommandModifier psaCommandModifier;
        private PsaCommandMover psaCommandMover;
        private PsaCommandRemover psaCommandRemover;

        public PsaCommandHandler(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation)
        {
            psaCommandParser = new PsaCommandParser(psaFile, codeBlockDataStartLocation);
            psaCommandAdder = new PsaCommandAdder(psaFile, dataSectionLocation, codeBlockDataStartLocation);
            psaCommandModifier = new PsaCommandModifier(psaFile, dataSectionLocation, codeBlockDataStartLocation);
            psaCommandMover = new PsaCommandMover(psaFile, dataSectionLocation, codeBlockDataStartLocation);
            psaCommandRemover = new PsaCommandRemover(psaFile, dataSectionLocation, codeBlockDataStartLocation);
        }

        public int GetNumberOfPsaCommands(int psaCodeLocation)
        {
            return psaCommandParser.GetNumberOfPsaCommands(psaCodeLocation);
        }

        public List<PsaCommand> GetPsaCommands(int psaCodeLocation)
        {
            return psaCommandParser.GetPsaCommands(psaCodeLocation);
        }

        public PsaCommand GetPsaCommand(int commandLocation)
        {
            return psaCommandParser.GetPsaCommand(commandLocation);
        }

        public int AddCommand(CodeBlock codeBlock)
        {
            return psaCommandAdder.AddCommand(codeBlock);
        }

        public void ModifyCommand(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            psaCommandModifier.ModifyCommand(commandLocation, oldPsaCommand, newPsaCommand);
        }

        public void MoveCommand(CodeBlock codeBlock, PsaCommand psaCommandToMove, int commandLocation, MoveDirection moveDirection)
        {
            psaCommandMover.MoveCommand(codeBlock, psaCommandToMove, commandLocation, moveDirection);
        }

        public void MoveCommandUp(CodeBlock codeBlock, PsaCommand psaCommandToMove, int commandLocation)
        {
            psaCommandMover.MoveCommand(codeBlock, psaCommandToMove, commandLocation, MoveDirection.UP);
        }

        public void MoveCommandDown(CodeBlock codeBlock, PsaCommand psaCommandToMove, int commandLocation)
        {
            psaCommandMover.MoveCommand(codeBlock, psaCommandToMove, commandLocation, MoveDirection.DOWN);
        }

        public void RemoveCommand(CodeBlock codeBlock, int commandLocation, PsaCommand removedPsaCommand)
        {
            psaCommandRemover.RemoveCommand(codeBlock, commandLocation, removedPsaCommand);
        }
    }
}
