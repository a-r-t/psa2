using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class CodeBlockLocationTracker
    {
        public BidirectionalDictionary<int, (SectionType, int, int)> CodeBlockLocations { get; private set; }
        public BidirectionalDictionary<int, (SectionType, int, int, int)> CommandLocations { get; private set; }
        private PsaMovesetHandler psaMovesetHandler;

        public CodeBlockLocationTracker(PsaMovesetHandler psaMovesetHandler)
        {
            CodeBlockLocations = new BidirectionalDictionary<int, (SectionType, int, int)>();
            CommandLocations = new BidirectionalDictionary<int, (SectionType, int, int, int)>();
            this.psaMovesetHandler = psaMovesetHandler;
            PopulateActionLocations();
            PopulateSubActionLocations();
        }

        private void PopulateActionLocations()
        {
            ActionsHandler actionsHandler = psaMovesetHandler.ActionsHandler;
            for (int i = 0; i < actionsHandler.GetNumberOfSpecialActions(); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int codeBlockCommandsPointerLocation = actionsHandler.GetCodeBlockCommandsPointerLocation(i, j);
                    if (codeBlockCommandsPointerLocation != 0)
                    {
                        CodeBlockLocations.AddEntryForward(codeBlockCommandsPointerLocation, (SectionType.ACTION, i, j));
                    }

                    int numberOfPsaCommands = actionsHandler.GetNumberOfPsaCommandsInCodeBlock(i, j);
                    for (int k = 0; k < numberOfPsaCommands; k++)
                    {
                        int commandPointerLocation = codeBlockCommandsPointerLocation + (k * 8); // each command is 8 double words away from one another
                        (SectionType, int, int, int) codeBlock = (SectionType.ACTION, i, j, k);
                        CommandLocations.AddEntryForward(commandPointerLocation, codeBlock);
                    }
                }
            }
        }

        private void PopulateSubActionLocations()
        {
            SubActionsHandler subActionsHandler = psaMovesetHandler.SubActionsHandler;
            for (int i = 0; i < subActionsHandler.GetNumberOfSubActions(); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int codeBlockCommandsPointerLocation = subActionsHandler.GetCodeBlockCommandsPointerLocation(i, j);
                    if (codeBlockCommandsPointerLocation != 0)
                    {
                        CodeBlockLocations.AddEntryForward(codeBlockCommandsPointerLocation, (SectionType.SUBACTION, i, j));
                    }

                    int numberOfPsaCommands = subActionsHandler.GetNumberOfPsaCommandsInCodeBlock(i, j);
                    for (int k = 0; k < numberOfPsaCommands; k++)
                    {
                        int commandPointerLocation = codeBlockCommandsPointerLocation + (k * 8); // each command is 8 double words away from one another
                        (SectionType, int, int, int) codeBlock = (SectionType.SUBACTION, i, j, k);
                        CommandLocations.AddEntryForward(commandPointerLocation, codeBlock);
                    }
                }
            }
        }
    }

    public enum SectionType
    {
        ACTION, SUBACTION, SUBROUTINE
    }

    public static class SectionTypeExtensions
    {
        public static string AsString(this SectionType st)
        {
            switch (st)
            {
                case SectionType.ACTION:
                    return "Action";
                case SectionType.SUBACTION:
                    return "Sub Action";
                case SectionType.SUBROUTINE:
                    return "Subroutine";
                default:
                    return "";
            }
        }
    }
}
