using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers
{
    public class PsaCommandMover
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public PsaCommandMover(PsaFile psaFile, int dataSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandParser = new PsaCommandParser(PsaFile);
        }

        public int GetNumberOfSpecialActions()
        {
            //Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        /// <summary>
        /// Gets starting location in data section where new actions can be placed
        /// </summary>
        /// <returns>location in data section -- "stf" in psa-c</returns>
        public int GetOpenAreaStartLocation() // stf
        {
            return 2014 + GetNumberOfSpecialActions() * 2;
        }

        /// <summary>
        /// Searches through data section for the desired amount of free space
        /// </summary>
        /// <param name="amountOfFreeSpace">amount of free space desired (as doubleword, e.g. 4 would look for 4 doublewords)</param>
        /// <returns>starting location where the desired amount of free space has been found</returns>
        public int FindLocationWithAmountOfFreeSpace(int amountOfFreeSpace)
        {
            int openAreaStartLocation = GetOpenAreaStartLocation();
            int stoppingPoint = openAreaStartLocation;

            while (stoppingPoint < PsaFile.DataSectionSizeBytes)
            {
                if (PsaFile.FileContent[stoppingPoint] == Constants.FADEF00D)
                {
                    bool hasEnoughSpace = true;
                    for (int i = 0; i < amountOfFreeSpace; i++)
                    {
                        if (PsaFile.FileContent[stoppingPoint + 1 + i] != Constants.FADEF00D)
                        {
                            hasEnoughSpace = false;
                            break;
                        }
                    }
                    if (hasEnoughSpace)
                    {
                        return stoppingPoint;
                    }
                }
                stoppingPoint++;
            }
            return stoppingPoint;
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
                int something = (commandLocation * 4) + 4;
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
                int something = (commandLocation * 4) + 4;
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
