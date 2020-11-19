using PSA2MovesetLogic.src.FileProcessor;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;

namespace PSA2Scripts
{
    /// <summary>
    /// Ports a moveset from one file to another
    /// Eventually this will be a feature in PSA2 and will be a lot better, 
    /// but for now it just copies and pastes action/sub action commands from one file to the other, which saves a lot of time
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter file path of file being ported: ");
            string originalFilePath = Console.ReadLine();
            Console.WriteLine("Enter file path of new file to be ported to: ");
            string newFilePath = Console.ReadLine();
            Console.WriteLine("Enter an output path for new file: ");
            string outputPath = Console.ReadLine();

            PsaMovesetHandler originalFile = new PsaFileParser(originalFilePath).ParseMovesetFile();
            PsaMovesetHandler newFile = new PsaFileParser(newFilePath).ParseMovesetFile();

            int oldNumberOfSpecialActions = originalFile.ActionsHandler.GetNumberOfSpecialActions();
            int oldNumberOfSubActions = originalFile.SubActionsHandler.GetNumberOfSubActions();

            for (int i = 0; i < oldNumberOfSpecialActions; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int numberOfPsaCommands = newFile.ActionsHandler.GetNumberOfPsaCommandsInCodeBlock(i, j);
                    for (int k = numberOfPsaCommands; i >= 0; k--)
                    {
                        newFile.ActionsHandler.RemoveCommand(i, j, k);
                    }
                    List<PsaCommand> psaCommands = originalFile.ActionsHandler.GetPsaCommandsInCodeBlock(i, j);
                    for (int k = 0; k < psaCommands.Count; k++)
                    {
                        newFile.ActionsHandler.InsertCommand(i, j, k, psaCommands[k]);
                    }

                }
            }

            for (int i = 0; i < oldNumberOfSubActions; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int numberOfPsaCommands = newFile.SubActionsHandler.GetNumberOfPsaCommandsInSubActionCodeBlock(i, j);
                    for (int k = numberOfPsaCommands; i >= 0; k--)
                    {
                        newFile.SubActionsHandler.RemoveCommand(i, j, k);
                    }
                    List<PsaCommand> psaCommands = originalFile.SubActionsHandler.GetPsaCommandsForSubAction(i, j);
                    for (int k = 0; k < psaCommands.Count; k++)
                    {
                        newFile.SubActionsHandler.InsertCommand(i, j, k, psaCommands[k]);
                    }
                }
            }
            newFile.PsaFile.SaveFile(outputPath);
        }
    }
}
