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
        public CodeBlocksHandler CodeBlocksHandler { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public const int ENTRY_CODE_BLOCK = 0;
        public const int EXIT_CODE_BLOCK = 1;

        public ActionsHandler(PsaFile psaFile, int dataSectionLocation, CodeBlocksHandler codeBlocksHandler, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlocksHandler = codeBlocksHandler;
            PsaCommandHandler = psaCommandHandler;
        }

        public Action GetAction(int actionId)
        {
            CodeBlock entry = GetCodeBlock(actionId, ENTRY_CODE_BLOCK);
            CodeBlock exit = GetCodeBlock(actionId, EXIT_CODE_BLOCK);
            CodeBlock[] codeBlocks = new CodeBlock[] { entry, exit };
            return new Action(actionId, codeBlocks);
        }

        public CodeBlock GetCodeBlock(int actionId, int codeBlockId)
        {
            int codeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlock(codeBlockLocation);
        }

        public int GetNumberOfSpecialActions()
        {
            return (PsaFile.DataSection[DataSectionLocation + 10] - PsaFile.DataSection[DataSectionLocation + 9]) / 4;
        }

        public int GetTotalNumberOfActions()
        {
            return 274 + GetNumberOfSpecialActions();
        }

        public int GetCodeBlockLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockStartingLocation;

            switch (codeBlockId)
            {
                case ENTRY_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.DataSection[DataSectionLocation + 9] / 4;
                    break;
                case EXIT_CODE_BLOCK:
                    actionCodeBlockStartingLocation = PsaFile.DataSection[DataSectionLocation + 10] / 4;
                    break;
                default:
                    throw new System.ArgumentException("Invalid code block id -- only 0 (Entry) and 1 (Exit) are valid");
            }

            return actionCodeBlockStartingLocation + actionId;
        }

        public int GetCodeBlockCommandsPointerLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandsPointerLocation(actionCodeBlockLocation);
        }

        public int GetCodeBlockCommandsLocation(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandsLocation(actionCodeBlockLocation);
        }

        public int GetCodeBlockCommandLocation(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandLocation(actionCodeBlockLocation, commandIndex);
        }

        public List<PsaCommand> GetPsaCommandsInCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            return CodeBlocksHandler.GetPsaCommandsForCodeBlock(actionCodeBlockLocation);
        }

        public PsaCommand GetPsaCommandInCodeBlock(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            return CodeBlocksHandler.GetPsaCommandForCodeBlock(actionCodeBlockLocation, commandIndex);
        }

        public int GetNumberOfPsaCommandsInCodeBlock(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            return CodeBlocksHandler.GetNumberOfPsaCommandsInCodeBlock(actionCodeBlockLocation);
        }

        public void AddCommand(int actionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            CodeBlocksHandler.AddCommand(actionCodeBlockLocation);
        }

        public void ModifyCommand(int actionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            CodeBlocksHandler.ModifyCommand(actionCodeBlockLocation, commandIndex, newPsaCommand);
        }

        public void RemoveCommand(int actionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            CodeBlocksHandler.RemoveCommand(actionCodeBlockLocation, commandIndex);    
        }

        public void MoveCommand(int actionId, int codeBlockId, int commandIndex, MoveDirection moveDirection)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            CodeBlocksHandler.MoveCommand(actionCodeBlockLocation, commandIndex, moveDirection);
        }

        public void InsertCommand(int actionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(actionId, codeBlockId);
            CodeBlocksHandler.InsertCommand(actionCodeBlockLocation, commandIndex, newPsaCommand);
        }

    }
}
