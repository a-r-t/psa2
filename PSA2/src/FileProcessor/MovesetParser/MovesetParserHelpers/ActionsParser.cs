using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class ActionsParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }

        public ActionsParser(PsaFile psaFile, int dataSectionLocation, string movesetBaseName)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandParser = new PsaCommandParser(PsaFile);
        }

        public int GetNumberOfSpecialActions()
        {
            Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        public int GetTotalNumberOfActions()
        {
            return 274 + GetNumberOfSpecialActions();
        }

        // this is the offset where action code starts (displayed in PSAC)
        public int GetActionCodeBlockLocation(int actionId, int codeBlockId)
        {
            int actionsCodeBlockStartingLocation = codeBlockId == 0 ?
                PsaFile.FileContent[DataSectionLocation + 9] / 4 :
                PsaFile.FileContent[DataSectionLocation + 10] / 4;

            int actionCodeBlockLocation = PsaFile.FileContent[actionsCodeBlockStartingLocation + actionId];

            return actionCodeBlockLocation;
        }


        public List<PsaCommand> GetPsaCommandsForAction(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            return PsaCommandParser.GetPsaCommands(actionCodeBlockLocation);
        }
    }
}
