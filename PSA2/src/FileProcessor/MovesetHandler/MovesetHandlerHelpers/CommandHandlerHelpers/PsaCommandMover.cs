using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    public class PsaCommandMover
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int OpenAreaStartLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandMover(PsaFile psaFile, int dataSectionLocation, int openAreaStartLocation, PsaCommandParser psaCommandParser)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            OpenAreaStartLocation = openAreaStartLocation;
            PsaCommandParser = psaCommandParser;
        }

        public void MoveCommand(PsaCommand psaCommandToMove, int commandLocation, MoveDirection moveDirection)
        {
            // h is codeBlockLocation
            // j + i * 2 is commandLocation
            // g is commandIndex
            switch (moveDirection)
            {
                case MoveDirection.UP:
                    MoveCommandUp(psaCommandToMove, commandLocation);
                    break;
                case MoveDirection.DOWN:
                    MoveCommandDown(psaCommandToMove, commandLocation);
                    break;
            }
        }

        public void MoveCommandUp(PsaCommand psaCommandToMove, int commandLocation)
        {
            // h is codeBlockLocation
            // j + i * 2 is commandLocation
            // g is commandIndex

            PsaCommand psaCommandAbove = PsaCommandParser.GetPsaCommand(commandLocation - 2);

            if (psaCommandToMove.NumberOfParams == 0)
            {
                // if the command above this command's param number is not 0
                //if (((PsaFile.FileContent[commandLocation - 2] >> 8) & 0xFF) != 0))
                if (psaCommandAbove.NumberOfParams != 0)
                {
                    // command above command location --- commandLocation - 4 I'm pretty sure
                    int commandAboveLocation = (commandLocation - 1) * 4;
                    for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
                    {
                        if (PsaFile.OffsetInterlockTracker[i] == commandAboveLocation)
                        {
                            PsaFile.OffsetInterlockTracker[i] += 8;
                            PsaFile.FileContent[i + PsaFile.DataSectionSizeBytes] += 8;
                            break;
                        }
                    }
                }
            }
            else if (psaCommandAbove.NumberOfParams == 0)
            {
                int something = commandLocation * 4 + 4;
                for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
                {
                    if (PsaFile.OffsetInterlockTracker[i] == something)
                    {
                        PsaFile.OffsetInterlockTracker[i] -= 8;
                        PsaFile.FileContent[i + PsaFile.DataSectionSizeBytes] -= 8;
                        break;
                    }
                }
            }


            // h is instruction and then command parameters location

            // swap the command above and the command to move
            PsaFile.FileContent[commandLocation] = psaCommandAbove.Instruction;
            PsaFile.FileContent[commandLocation + 1] = psaCommandAbove.CommandParametersLocation;
            PsaFile.FileContent[commandLocation - 2] = psaCommandToMove.Instruction;
            PsaFile.FileContent[commandLocation - 1] = psaCommandToMove.CommandParametersLocation;
        }

        public void MoveCommandDown(PsaCommand psaCommandToMove, int commandLocation)
        {
            // h is codeBlockLocation
            // j + i * 2 is commandLocation
            // g is commandIndex

            PsaCommand psaCommandBelow = PsaCommandParser.GetPsaCommand(commandLocation + 2);

            if (psaCommandToMove.NumberOfParams == 0)
            {
                // if the command below this command's param number is not 0
                //if (((PsaFile.FileContent[commandLocation - 2] >> 8) & 0xFF) != 0))
                if (psaCommandBelow.NumberOfParams != 0)
                {
                    // command below command location???
                    int commandBelowLocation = commandLocation * 4 + 12;
                    for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
                    {
                        if (PsaFile.OffsetInterlockTracker[i] == commandBelowLocation)
                        {
                            PsaFile.OffsetInterlockTracker[i] -= 8;
                            PsaFile.FileContent[i + PsaFile.DataSectionSizeBytes] -= 8;
                            break;
                        }
                    }
                }
            }
            else if (psaCommandBelow.NumberOfParams == 0)
            {
                int something = commandLocation * 4 + 4;
                for (int i = 0; i < PsaFile.NumberOfOffsetEntries; i++)
                {
                    if (PsaFile.OffsetInterlockTracker[i] == something)
                    {
                        PsaFile.OffsetInterlockTracker[i] += 8;
                        PsaFile.FileContent[i + PsaFile.DataSectionSizeBytes] += 8;
                        break;
                    }
                }
            }


            // h is instruction and then command parameters location

            // swap the command below and the command to move
            PsaFile.FileContent[commandLocation] = psaCommandBelow.Instruction;
            PsaFile.FileContent[commandLocation + 1] = psaCommandBelow.CommandParametersLocation;
            PsaFile.FileContent[commandLocation + 2] = psaCommandToMove.Instruction;
            PsaFile.FileContent[commandLocation + 3] = psaCommandToMove.CommandParametersLocation;
        }
    }
}
