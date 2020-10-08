using PSA2.src.Models.Fighter;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers
{
    /// <summary>
    /// This class is responsible for moving commands in a code block
    /// <para>Essentially adjacent commands can be swapped, which allows a command to be moved "upwards" or "downwards" by one index</para>
    /// </summary>
    public class PsaCommandMover
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }
        public PsaFileHelperMethods PsaFileHelperMethods { get; private set; }

        public PsaCommandMover(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation, PsaFileHelperMethods psaFileHelperMethods)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            PsaFileHelperMethods = psaFileHelperMethods;
        }

        /// <summary>
        /// Moves a command either upwards or downwards
        /// </summary>
        /// <param name="codeBlock">The code block the command to move is in</param>
        /// <param name="psaCommandToMove">The psa command to move</param>
        /// <param name="commandLocation">The location of the psa command to move</param>
        /// <param name="moveDirection">The direction to move the command in (MoveDirection enum -- UP or DOWN)</param>
        public void MoveCommand(CodeBlock codeBlock, PsaCommand psaCommandToMove, int commandLocation, MoveDirection moveDirection)
        {
            switch (moveDirection)
            {
                // move command upwards by one index (swap command with command above it)
                case MoveDirection.UP:
                    MoveCommandUp(codeBlock, psaCommandToMove, commandLocation);
                    break;
                // move command downwards by one index (swap command with command below it)
                case MoveDirection.DOWN:
                    MoveCommandDown(codeBlock, psaCommandToMove, commandLocation);
                    break;
                default:
                    throw new InvalidEnumArgumentException("Direction type does not exist");
            }
        }

        /// <summary>
        /// Moves a command upwards
        /// <para>swaps command with the command above it</para>
        /// </summary>
        /// <param name="codeBlock">The code block the command to move is in</param>
        /// <param name="psaCommandToMove">The psa command to move</param>
        /// <param name="commandLocation">The location of the psa command to move</param>
        private void MoveCommandUp(CodeBlock codeBlock, PsaCommand psaCommandToMove, int commandLocation)
        {
            int commandIndex = codeBlock.GetPsaCommandIndexByLocation(commandLocation);
            PsaCommand psaCommandAbove = codeBlock.PsaCommands[commandIndex - 1];

            // if moving a command with no params
            if (psaCommandToMove.NumberOfParams == 0)
            {
                // if the command above has params
                if (psaCommandAbove.NumberOfParams != 0)
                {
                    // since command is being moved upwards, the command above will switch with it and as a result moves downwards
                    // this will increase the offset table's pointer to the command above's param location by 8 to adjust for the command moving downwards
                    int commandAboveParamsPointerLocation = codeBlock.GetPsaCommandParamsLocation(commandIndex - 1) * 4;
                    for (int i = 0; i < PsaFile.OffsetInterlockTracker.Count; i++)
                    {
                        if (PsaFile.OffsetInterlockTracker[i] == commandAboveParamsPointerLocation)
                        {
                            // Update actual offset pointer in both offset tracker and file content (since the apply header updates method is never called for moving commands)
                            PsaFile.OffsetInterlockTracker[i] += 8;
                            PsaFile.FileContent[PsaFile.DataSectionSizeBytes + i] += 8;
                            break;
                        }
                    }
                }
            }

            // if the command above has no parameters
            else if (psaCommandAbove.NumberOfParams == 0)
            {
                // since command is being moved upwards, anything pointing to its params pointer will need to move upwards by 8
                int commandParamsPointerLocation = codeBlock.GetPsaCommandParametersPointerLocation(commandIndex);
                for (int i = 0; i < PsaFile.OffsetInterlockTracker.Count; i++)
                {
                    if (PsaFile.OffsetInterlockTracker[i] == commandParamsPointerLocation)
                    {
                        // Update actual offset pointer in both offset tracker and file content (since the apply header updates method is never called for moving commands)
                        PsaFile.OffsetInterlockTracker[i] -= 8;
                        PsaFile.FileContent[PsaFile.DataSectionSizeBytes + i] -= 8;
                        break;
                    }
                }
            }

            // replace the command to move with the command above
            PsaFile.FileContent[commandLocation] = psaCommandAbove.Instruction;
            PsaFile.FileContent[commandLocation + 1] = psaCommandAbove.CommandParametersLocation;

            // replace the command above with the command to move
            PsaFile.FileContent[commandLocation - 2] = psaCommandToMove.Instruction;
            PsaFile.FileContent[commandLocation - 1] = psaCommandToMove.CommandParametersLocation;
        }

        /// <summary>
        /// Moves a command downwards
        /// <para>swaps command with the command below it</para>
        /// </summary>
        /// <param name="codeBlock">The code block the command to move is in</param>
        /// <param name="psaCommandToMove">The psa command to move</param>
        /// <param name="commandLocation">The location of the psa command to move</param>
        private void MoveCommandDown(CodeBlock codeBlock, PsaCommand psaCommandToMove, int commandLocation)
        {
            int commandIndex = codeBlock.GetPsaCommandIndexByLocation(commandLocation);
            PsaCommand psaCommandBelow = codeBlock.PsaCommands[commandIndex + 1];

            // if moving a command with no params
            if (psaCommandToMove.NumberOfParams == 0)
            {
                // if the command below has params
                if (psaCommandBelow.NumberOfParams != 0)
                {

                    // since command is being moved downwards, the command above will switch with it and as a result moves upwards
                    // this will decrease the offset table's pointer to the command above's param location by 8 to adjust for the command moving upwards
                    int commandBelowParamsPointerLocation = codeBlock.GetPsaCommandParamsLocation(commandIndex + 1) * 4;
                    for (int i = 0; i < PsaFile.OffsetInterlockTracker.Count; i++)
                    {
                        if (PsaFile.OffsetInterlockTracker[i] == commandBelowParamsPointerLocation)
                        {
                            // Update actual offset pointer in both offset tracker and file content (since the apply header updates method is never called for moving commands)
                            PsaFile.OffsetInterlockTracker[i] -= 8;
                            PsaFile.FileContent[i + PsaFile.DataSectionSizeBytes] -= 8;
                            break;
                        }
                    }
                }
            }

            // if the command below has no parameters
            else if (psaCommandBelow.NumberOfParams == 0)
            {
                // since command is being moved downwards, anything pointing to its params pointer will need to move downwards by 8
                int commandParamsPointerLocation = codeBlock.GetPsaCommandParametersPointerLocation(commandIndex);
                for (int i = 0; i < PsaFile.OffsetInterlockTracker.Count; i++)
                {
                    if (PsaFile.OffsetInterlockTracker[i] == commandParamsPointerLocation)
                    {
                        // Update actual offset pointer in both offset tracker and file content (since the apply header updates method is never called for moving commands)
                        PsaFile.OffsetInterlockTracker[i] += 8;
                        PsaFile.FileContent[i + PsaFile.DataSectionSizeBytes] += 8;
                        break;
                    }
                }
            }

            // replace the command to move with the command below
            PsaFile.FileContent[commandLocation] = psaCommandBelow.Instruction;
            PsaFile.FileContent[commandLocation + 1] = psaCommandBelow.CommandParametersLocation;

            // replace the command below with the command to move
            PsaFile.FileContent[commandLocation + 2] = psaCommandToMove.Instruction;
            PsaFile.FileContent[commandLocation + 3] = psaCommandToMove.CommandParametersLocation;
        }
    }
}
