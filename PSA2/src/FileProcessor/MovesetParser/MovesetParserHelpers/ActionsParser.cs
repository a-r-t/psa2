using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers.CommandParserHelpers;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class ActionsParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandParser PsaCommandParser { get; private set; }
        public PsaCommandAdder PsaCommandAdder { get; private set; }
        public PsaCommandModifier PsaCommandModifier { get; private set; }
        public PsaCommandRemover PsaCommandRemover { get; private set; }
        public const int ENTRY_CODE_BLOCK = 0;
        public const int EXIT_CODE_BLOCK = 1;

        public ActionsParser(PsaFile psaFile, int dataSectionLocation, string movesetBaseName)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandParser = new PsaCommandParser(PsaFile);
            PsaCommandAdder = new PsaCommandAdder(psaFile, dataSectionLocation);
            PsaCommandModifier = new PsaCommandModifier(psaFile, dataSectionLocation);
            PsaCommandRemover = new PsaCommandRemover(psaFile, dataSectionLocation);
        }

        public int GetNumberOfSpecialActions()
        {
            //Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        public int GetTotalNumberOfActions()
        {
            return 274 + GetNumberOfSpecialActions();
        }

        /// <summary>
        /// Gets starting location in data section where new actions can be placed
        /// </summary>
        /// <returns>location in data section -- "stf" in psa-c</returns>
        public int GetOpenAreaStartLocation() // stf
        {
            return 2014 + GetNumberOfSpecialActions() * 2;
        }

        public int GetActionCodeBlockLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockStartingLocation;

            switch(codeBlockId)
            {
                case ENTRY_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 9] / 4;
                    break;
                case EXIT_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 10] / 4;
                    break;
                default:
                    throw new ArgumentException("Invalid code block id -- only 0 (Entry) and 1 (Exit) are valid");
            }

            return actionCodeBlockStartingLocation + actionId;
        }

        // this is the offset where action code starts (displayed in PSAC)
        public int GetActionCodeBlockCommandsLocation(int actionId, int codeBlockId)
        {
            int actionsCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);

            int actionCodeBlockCommandsLocation = PsaFile.FileContent[actionsCodeBlockLocation];

            return actionCodeBlockCommandsLocation;
        }

        public int GetActionCodeBlockCommandLocation(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockCommandsStartLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return actionCodeBlockCommandsStartLocation / 4 + commandIndex * 2;
        }

        public List<PsaCommand> GetPsaCommandsForActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return PsaCommandParser.GetPsaCommands(actionCodeBlockLocation);
        }

        public PsaCommand GetPsaCommandForActionCodeBlock(int actionId, int codeBlockId, int commandIndex)
        {
            int commandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex);
            return PsaCommandParser.GetPsaCommand(commandLocation);
        }

        public int GetNumberOfPsaCommandsInActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return PsaCommandParser.GetNumberOfPsaCommands(actionCodeBlockLocation);
        }

        public void AddCommandToAction(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId); // h
            int actionCodeBlockCommandsLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId); // h
            PsaCommandAdder.AddCommandToAction(actionCodeBlockLocation, actionCodeBlockCommandsLocation);
        }

        public void ModifyActionCommand(int actionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int actionCodeBlockCommandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex); // j
            PsaCommand oldPsaCommand = GetPsaCommandForActionCodeBlock(actionId, codeBlockId, commandIndex);
            PsaCommandModifier.ModifyCommand(actionCodeBlockCommandLocation, oldPsaCommand, newPsaCommand);
        }

        public void RemoveCommandFromAction(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockCommandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex); // j
            int actionCodeBlockCommandsLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId); // h
            PsaCommand removedPsaCommand = GetPsaCommandForActionCodeBlock(actionId, codeBlockId, commandIndex);
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            PsaCommandRemover.RemoveCommand(actionCodeBlockCommandLocation, actionCodeBlockCommandsLocation, removedPsaCommand, commandIndex, actionCodeBlockLocation);
        }

    }
}
