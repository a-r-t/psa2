﻿using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class ActionOverridesHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public ActionsHandler ActionsParser { get; private set; }

        public ActionOverridesHandler(PsaFile psaFile, int dataSectionLocation, ActionsHandler actionsParser, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
            ActionsParser = actionsParser;
        }

        public List<int> GetAllActionOverrides(int codeBlockId)
        {
            List<int> actionOverrideIds = new List<int>();
            if (codeBlockId == 3)
            {
                int totalNumberOfActions = ActionsParser.GetTotalNumberOfActions();
                for (int i = 0; i < totalNumberOfActions; i++)
                {
                    actionOverrideIds.Add(i);
                }
                // pre action overrides
                return actionOverrideIds;
            }
            else
            {

                int actionOverridesLocation;
                if (codeBlockId == 0)
                {
                    actionOverridesLocation = PsaFile.DataSection[DataSectionLocation + 20] / 4;
                }
                else // if (codeBlockId == 1)
                {
                    actionOverridesLocation = PsaFile.DataSection[DataSectionLocation + 21] / 4;
                }

                int nextActionOverridesLocation = actionOverridesLocation;
                while (PsaFile.DataSection[nextActionOverridesLocation] >= 0)
                {
                    actionOverrideIds.Add(PsaFile.DataSection[nextActionOverridesLocation]);
                    nextActionOverridesLocation += 2;
                }
                actionOverrideIds.ForEach(x => Console.WriteLine(x.ToString("X")));
                return actionOverrideIds;
            }

        }

        // this is the offset where action override code starts (displayed in PSAC)
        public int GetActionOverrideCodeBlockLocation(int actionOverrideId, int codeBlockId) // issue -- actionOverrideId needs to be in order of existence
        {
            int actionOverridesCodeBlockStartingLocation = codeBlockId == 0 ?
                PsaFile.DataSection[DataSectionLocation + 20] / 4 :
                PsaFile.DataSection[DataSectionLocation + 21] / 4;

            int actionOverrideCodeBlockLocation = PsaFile.DataSection[actionOverridesCodeBlockStartingLocation + actionOverrideId * 2 + 1];

            return actionOverrideCodeBlockLocation;
        }

        public List<PsaCommand> GetPsaCommandsForActionOverride(int codeBlockId, int actionOverrideId)
        {
            int actionOverrideCodeBlockLocation = GetActionOverrideCodeBlockLocation(actionOverrideId, codeBlockId);
            return PsaCommandHandler.GetPsaCommands(actionOverrideCodeBlockLocation);
        }
    }
}
