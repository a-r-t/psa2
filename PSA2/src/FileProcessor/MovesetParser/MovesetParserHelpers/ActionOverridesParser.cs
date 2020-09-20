using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class ActionOverridesParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public ActionsParser ActionsParser { get; private set; }

        public ActionOverridesParser(PsaFile psaFile, int dataSectionLocation, ActionsParser actionsParser, PsaCommandHandler psaCommandHandler)
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
                    actionOverridesLocation = PsaFile.FileContent[DataSectionLocation + 20] / 4;
                }
                else // if (codeBlockId == 1)
                {
                    actionOverridesLocation = PsaFile.FileContent[DataSectionLocation + 21] / 4;
                }

                int nextActionOverridesLocation = actionOverridesLocation;
                while (PsaFile.FileContent[nextActionOverridesLocation] >= 0)
                {
                    actionOverrideIds.Add(PsaFile.FileContent[nextActionOverridesLocation]);
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
                PsaFile.FileContent[DataSectionLocation + 20] / 4 :
                PsaFile.FileContent[DataSectionLocation + 21] / 4;

            int actionOverrideCodeBlockLocation = PsaFile.FileContent[actionOverridesCodeBlockStartingLocation + actionOverrideId * 2 + 1];

            return actionOverrideCodeBlockLocation;
        }

        public List<PsaCommand> GetPsaCommandsForActionOverride(int codeBlockId, int actionOverrideId)
        {
            int actionOverrideCodeBlockLocation = GetActionOverrideCodeBlockLocation(actionOverrideId, codeBlockId);
            return PsaCommandHandler.GetPsaCommands(actionOverrideCodeBlockLocation);
        }
    }
}
