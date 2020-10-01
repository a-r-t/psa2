﻿using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Models.Fighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
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
            psaCommandMover = new PsaCommandMover(psaFile, dataSectionLocation, codeBlockDataStartLocation, psaCommandParser);
            psaCommandRemover = new PsaCommandRemover(psaFile, dataSectionLocation, codeBlockDataStartLocation, psaCommandParser);
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

        public void AddCommand(CodeBlock codeBlock)
        {
            psaCommandAdder.AddCommand(codeBlock);
        }

        public void ModifyCommand(int commandLocation, PsaCommand oldPsaCommand, PsaCommand newPsaCommand)
        {
            psaCommandModifier.ModifyCommand(commandLocation, oldPsaCommand, newPsaCommand);
        }

        public void MoveCommand(PsaCommand psaCommandToMove, int commandLocation, MoveDirection moveDirection)
        {
            psaCommandMover.MoveCommand(psaCommandToMove, commandLocation, moveDirection);
        }

        public void RemoveCommand(int commandLocation, int codeBlockCommandsPointerLocation, PsaCommand removedPsaCommand, int commandIndex, int codeBlockLocation)
        {
            psaCommandRemover.RemoveCommand(commandLocation, codeBlockCommandsPointerLocation, removedPsaCommand, commandIndex, codeBlockLocation);
        }
    }
}
