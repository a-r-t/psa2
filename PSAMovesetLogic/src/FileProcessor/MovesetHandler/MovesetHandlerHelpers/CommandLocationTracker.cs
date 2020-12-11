using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class CommandLocationTracker
    {
        public BidirectionalDictionary<int, (SectionType, int, int, int)> Locations { get; private set; }
        private PsaMovesetHandler psaMovesetHandler;

        public CommandLocationTracker(PsaMovesetHandler psaMovesetHandler)
        {
            Locations = new BidirectionalDictionary<int, (SectionType, int, int, int)>();
            this.psaMovesetHandler = psaMovesetHandler;
            PopulateActionsInTracker();
            PopulateSubActionsInTracker();
        }

        public enum SectionType
        {
            ACTION, SUBACTION, SUBROUTINE
        }

        private void PopulateActionsInTracker()
        {
            ActionsHandler actionsHandler = psaMovesetHandler.ActionsHandler;
            for (int i = 0; i < actionsHandler.GetNumberOfSpecialActions(); i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    int codeBlockCommandsPointerLocation = actionsHandler.GetCodeBlockCommandsPointerLocation(i, j);
                    int numberOfPsaCommands = actionsHandler.GetNumberOfPsaCommandsInCodeBlock(i, j);
                    for (int k = 0; k < numberOfPsaCommands; k++)
                    {
                        int commandPointerLocation = codeBlockCommandsPointerLocation + (k * 8); // each command is 8 double words away from one another
                        (SectionType, int, int, int) codeBlock = (SectionType.ACTION, i, j, k);
                        Locations.AddEntry(commandPointerLocation, codeBlock);
                    }
                }
            }
        }

        private void PopulateSubActionsInTracker()
        {
            SubActionsHandler subActionsHandler = psaMovesetHandler.SubActionsHandler;
            for (int i = 0; i < subActionsHandler.GetNumberOfSubActions(); i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int codeBlockCommandsPointerLocation = subActionsHandler.GetCodeBlockCommandsPointerLocation(i, j);
                    int numberOfPsaCommands = subActionsHandler.GetNumberOfPsaCommandsInCodeBlock(i, j);
                    for (int k = 0; k < numberOfPsaCommands; k++)
                    {
                        int commandPointerLocation = codeBlockCommandsPointerLocation + (k * 8); // each command is 8 double words away from one another
                        (SectionType, int, int, int) codeBlock = (SectionType.SUBACTION, i, j, k);
                        Locations.AddEntry(commandPointerLocation, codeBlock);
                    }
                }
            }
        }
    }
}
