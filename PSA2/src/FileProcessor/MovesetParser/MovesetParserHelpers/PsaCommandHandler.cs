using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class PsaCommandHandler
    {
        private PsaCommandParser psaCommandParser;
        private PsaCommandAdder psaCommandAdder;
        private PsaCommandModifier psaCommandModifier;
        private PsaCommandMover psaCommandMover;
        private PsaCommandRemover psaCommandRemover;

        public PsaCommandHandler(PsaFile psaFile, int dataSectionLocation)
        {
            this.psaCommandParser = new PsaCommandParser(psaFile);
            this.psaCommandAdder = new PsaCommandAdder(psaFile, dataSectionLocation);
            this.psaCommandModifier = new PsaCommandModifier(psaFile, dataSectionLocation);
            this.psaCommandMover = new PsaCommandMover(psaFile, dataSectionLocation);
            this.psaCommandRemover = new PsaCommandRemover(psaFile, dataSectionLocation);
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

        public void AddCommand(int codeBlockLocation, int codeBlockCommandsLocation)
        {
            psaCommandAdder.AddCommand(codeBlockLocation, codeBlockCommandsLocation);
        }

        public void ModifyCommand(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            psaCommandModifier.ModifyCommand(commandLocation, oldPsaCommand, newPsaCommand);
        }

        public void MoveCommand(PsaCommand psaCommandToMove, int commandLocation, MoveDirection moveDirection)
        {
            psaCommandMover.MoveCommand(psaCommandToMove, commandLocation, moveDirection);
        }

        public void RemoveCommand(int commandLocation, int codeBlockCommandsLocation, PsaCommand removedPsaCommand, int commandIndex, int codeBlockLocation)
        {
            psaCommandRemover.RemoveCommand(commandLocation, codeBlockCommandsLocation, removedPsaCommand, commandIndex, codeBlockLocation);
        }
    }
}
