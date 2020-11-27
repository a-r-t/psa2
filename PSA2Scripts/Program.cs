using PSA2MovesetLogic.src.FileProcessor;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Text;

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

            int newNumberOfSpecialActions = newFile.ActionsHandler.GetNumberOfSpecialActions();
            int newNumberOfSubActions = newFile.SubActionsHandler.GetNumberOfSubActions();

            Console.WriteLine("ACTIONS");
            HashSet<(int, int)> actionsWithPointers = new HashSet<(int, int)>();

            // port actions
            for (int i = 0; i < newNumberOfSpecialActions; i++)
            {
                Console.WriteLine("ACTION " + i);
                for (int j = 0; j < 2; j++)
                {
                    Console.WriteLine("CODE BLOCK " + j);

                    List<PsaCommand> oldPsaCommands = newFile.ActionsHandler.GetPsaCommandsInCodeBlock(i, j);
                    for (int k = oldPsaCommands.Count - 1; k >= 0; k--)
                    {
                        Console.WriteLine("REMOVE COMMAND " + k);
                        bool isExternal = false;
                        for (int l = 0; l < oldPsaCommands[k].Parameters.Count; l++)
                        {
                            if (oldPsaCommands[k].Parameters[l].Type == 2 && oldPsaCommands[k].Parameters[l].Value == -1)
                            {
                                actionsWithPointers.Add((i, j));
                                isExternal = true;
                            }
                        }
                        if (!isExternal)
                        {
                            newFile.ActionsHandler.RemoveCommand(i, j, k);
                        }
                    }
                    List<PsaCommand> newPsaCommands = originalFile.ActionsHandler.GetPsaCommandsInCodeBlock(i, j);
                    for (int k = 0; k < newPsaCommands.Count; k++)
                    {
                        Console.WriteLine("INSERT COMMAND " + k);
                        for (int l = 0; l < newPsaCommands[k].Parameters.Count; l++)
                        {
                            if (newPsaCommands[k].Parameters[l].Type == 2)
                            {
                                actionsWithPointers.Add((i, j));
                            }
                        }
                        newFile.ActionsHandler.InsertCommand(i, j, k, newPsaCommands[k]);
                    }

                }
            }
            Console.WriteLine("SUBACTIONS");
            HashSet<(int, int)> subActionsWithPointers = new HashSet<(int, int)>();

            // port subactions
            for (int i = 0; i < newNumberOfSubActions; i++)
            {
                Console.WriteLine("SUB ACTION " + i);
                for (int j = 0; j < 4; j++)
                {
                    Console.WriteLine("CODE BLOCK " + j);
                    List<PsaCommand> oldPsaCommands = newFile.SubActionsHandler.GetPsaCommandsInCodeBlock(i, j);
                    for (int k = oldPsaCommands.Count - 1; k >= 0; k--)
                    {
                        Console.WriteLine("REMOVE COMMAND " + k);
                        bool isExternal = false;
                        for (int l = 0; l < oldPsaCommands[k].Parameters.Count; l++)
                        {
                            if (oldPsaCommands[k].Parameters[l].Type == 2 && oldPsaCommands[k].Parameters[l].Value == -1)
                            {
                                subActionsWithPointers.Add((i, j));
                                isExternal = true;
                            }
                        }
                        if (!isExternal)
                        {
                            newFile.SubActionsHandler.RemoveCommand(i, j, k);
                        }
                    }
                    List<PsaCommand> newPsaCommands = originalFile.SubActionsHandler.GetPsaCommandsInCodeBlock(i, j);
                    for (int k = 0; k < newPsaCommands.Count; k++)
                    {
                        Console.WriteLine("INSERT COMMAND " + k);
                        for (int l = 0; l < newPsaCommands[k].Parameters.Count; l++)
                        {
                            if (newPsaCommands[k].Parameters[l].Type == 2)
                            {
                                subActionsWithPointers.Add((i, j));
                            }
                        }
                        newFile.SubActionsHandler.InsertCommand(i, j, k, newPsaCommands[k]);
                    }
                }
            }
            Console.WriteLine("Writing to file " + outputPath + "...");
            newFile.PsaFile.SaveFile(outputPath + "/FitNew.pac");

            StringBuilder actionsToCheckFormatted = new StringBuilder();
            foreach ((int, int) item in actionsWithPointers)
            {
                actionsToCheckFormatted
                    .Append((item.Item1 + 274).ToString("X"));

                string codeBlockName = "";
                switch(item.Item2)
                {
                    case 0:
                        codeBlockName = "Entry";
                        break;
                    case 1:
                        codeBlockName = "Exit";
                        break;
                }

                actionsToCheckFormatted
                    .Append(" - ")
                    .Append(codeBlockName)
                    .Append("\n");
            }

            StringBuilder subActionsToCheckFormatted = new StringBuilder();
            foreach ((int, int) item in subActionsWithPointers)
            {
                subActionsToCheckFormatted
                    .Append(item.Item1.ToString("X"));

                string codeBlockName = "";
                switch (item.Item2)
                {
                    case 0:
                        codeBlockName = "Main";
                        break;
                    case 1:
                        codeBlockName = "Gfx";
                        break;
                    case 2:
                        codeBlockName = "Sfx";
                        break;
                    case 3:
                        codeBlockName = "Other";
                        break;
                }

                subActionsToCheckFormatted
                    .Append(" - ")
                    .Append(codeBlockName)
                    .Append("\n");
            }

            System.IO.File.WriteAllText(outputPath + "/actionsWithPointers.txt", actionsToCheckFormatted.ToString());
            System.IO.File.WriteAllText(outputPath + "/subActionsWithPointers.txt", subActionsToCheckFormatted.ToString());
            Console.WriteLine("Done!");
        }
    }
}
