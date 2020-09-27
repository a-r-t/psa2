using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.Utility;
using PSA2.src.Models.Fighter;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class ActionsHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public const int ENTRY_CODE_BLOCK = 0;
        public const int EXIT_CODE_BLOCK = 1;

        public ActionsHandler(PsaFile psaFile, int dataSectionLocation, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandHandler = psaCommandHandler;
        }

        public Action GetAction(int actionId)
        {
            CodeBlock entry = GetCodeBlock(actionId, 0);
            CodeBlock exit = GetCodeBlock(actionId, 1);
            CodeBlock[] codeBlocks = new CodeBlock[] { entry, exit };
            return new Action(actionId, codeBlocks);
        }

        public CodeBlock GetCodeBlock(int actionId, int codeBlockId)
        {
            int codeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            int codeBlockCommandsPointerLocation = GetActionCodeBlockCommandsPointerLocation(actionId, codeBlockId);
            int codeBlockCommandsLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            List<PsaCommand> psaCommands = GetPsaCommandsForActionCodeBlock(actionId, codeBlockId);
            return new CodeBlock(codeBlockLocation, codeBlockCommandsPointerLocation, codeBlockCommandsLocation, psaCommands);
        }

        public int GetNumberOfSpecialActions()
        {
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

            switch (codeBlockId)
            {
                case ENTRY_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 9] / 4;
                    break;
                case EXIT_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.FileContent[DataSectionLocation + 10] / 4;
                    break;
                default:
                    throw new System.ArgumentException("Invalid code block id -- only 0 (Entry) and 1 (Exit) are valid");
            }

            return actionCodeBlockStartingLocation + actionId;
        }

        // this is the offset where action code starts (displayed in PSAC)
        public int GetActionCodeBlockCommandsPointerLocation(int actionId, int codeBlockId)
        {
            int actionsCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);

            int actionCodeBlockCommandsLocation = PsaFile.FileContent[actionsCodeBlockLocation];

            return actionCodeBlockCommandsLocation;
        }

        // this is the offset where action code starts (displayed in PSAC)
        public int GetActionCodeBlockCommandsLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockCommandsPointerLocation = GetActionCodeBlockCommandsPointerLocation(actionId, codeBlockId);
            return actionCodeBlockCommandsPointerLocation / 4;
        }

        public int GetActionCodeBlockCommandLocation(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockCommandsLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return actionCodeBlockCommandsLocation + commandIndex * 2;
        }

        public List<PsaCommand> GetPsaCommandsForActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return PsaCommandHandler.GetPsaCommands(actionCodeBlockLocation);
        }

        public PsaCommand GetPsaCommandForActionCodeBlock(int actionId, int codeBlockId, int commandIndex)
        {
            int commandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex);
            return PsaCommandHandler.GetPsaCommand(commandLocation);
        }

        public int GetNumberOfPsaCommandsInActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockCommandsLocation(actionId, codeBlockId);
            return PsaCommandHandler.GetNumberOfPsaCommands(actionCodeBlockLocation);
        }

        public void AddCommandToAction(int actionId, int codeBlockId)
        {
            CodeBlock codeBlock = GetCodeBlock(actionId, codeBlockId);
            PsaCommandHandler.AddCommand(codeBlock);
        }

        public void ModifyActionCommand(int actionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int actionCodeBlockCommandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex); // j
            PsaCommand oldPsaCommand = GetPsaCommandForActionCodeBlock(actionId, codeBlockId, commandIndex);
            PsaCommandHandler.ModifyCommand(actionCodeBlockCommandLocation, oldPsaCommand, newPsaCommand);
        }

        public void RemoveCommandFromAction(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockCommandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex); // j
            int actionCodeBlockCommandsPointerLocation = GetActionCodeBlockCommandsPointerLocation(actionId, codeBlockId); // h
            PsaCommand removedPsaCommand = GetPsaCommandForActionCodeBlock(actionId, codeBlockId, commandIndex);
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            PsaCommandHandler.RemoveCommand(actionCodeBlockCommandLocation, actionCodeBlockCommandsPointerLocation, removedPsaCommand, commandIndex, actionCodeBlockLocation);
        }

        public void MoveActionCommand(int actionId, int codeBlockId, int commandIndex, MoveDirection moveDirection)
        {
            int actionCodeBlockCommandLocation = GetActionCodeBlockCommandLocation(actionId, codeBlockId, commandIndex); // j
            PsaCommand psaCommandToMove = GetPsaCommandForActionCodeBlock(actionId, codeBlockId, commandIndex);
            PsaCommandHandler.MoveCommand(psaCommandToMove, actionCodeBlockCommandLocation, moveDirection);
        }

    }
}
