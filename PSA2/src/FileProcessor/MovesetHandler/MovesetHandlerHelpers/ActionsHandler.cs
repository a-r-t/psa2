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
        public CodeBlockHandler CodeBlockHandler { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public const int ENTRY_CODE_BLOCK = 0;
        public const int EXIT_CODE_BLOCK = 1;

        public ActionsHandler(PsaFile psaFile, int dataSectionLocation, CodeBlockHandler codeBlockHandler, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockHandler = codeBlockHandler;
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
            return CodeBlockHandler.GetCodeBlock(codeBlockLocation);
        }

        public int GetNumberOfSpecialActions()
        {
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        public int GetTotalNumberOfActions()
        {
            return 274 + GetNumberOfSpecialActions();
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

        public int GetActionCodeBlockCommandsPointerLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            return CodeBlockHandler.GetCodeBlockCommandsPointerLocation(actionCodeBlockLocation);
        }

        public int GetActionCodeBlockCommandsLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            return CodeBlockHandler.GetCodeBlockCommandsLocation(actionCodeBlockLocation);
        }

        public int GetActionCodeBlockCommandLocation(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            return CodeBlockHandler.GetCodeBlockCommandLocation(actionCodeBlockLocation, commandIndex);
        }

        public List<PsaCommand> GetPsaCommandsForActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            return CodeBlockHandler.GetPsaCommandsForCodeBlock(actionCodeBlockLocation);
        }

        public PsaCommand GetPsaCommandForActionCodeBlock(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            return CodeBlockHandler.GetPsaCommandForCodeBlock(actionCodeBlockLocation, commandIndex);
        }

        public int GetNumberOfPsaCommandsInActionCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            return CodeBlockHandler.GetNumberOfPsaCommandsInCodeBlock(actionCodeBlockLocation);
        }

        public void AddCommandToAction(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            CodeBlockHandler.AddCommand(actionCodeBlockLocation);
        }

        public void ModifyActionCommand(int actionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            CodeBlockHandler.ModifyCommand(actionCodeBlockLocation, commandIndex, newPsaCommand);
        }

        public void RemoveCommandFromAction(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            CodeBlockHandler.RemoveCommand(actionCodeBlockLocation, commandIndex);    
        }

        public void MoveActionCommand(int actionId, int codeBlockId, int commandIndex, MoveDirection moveDirection)
        {
            int actionCodeBlockLocation = GetActionCodeBlockLocation(actionId, codeBlockId);
            CodeBlockHandler.MoveCommand(actionCodeBlockLocation, commandIndex, moveDirection);
        }

    }
}
