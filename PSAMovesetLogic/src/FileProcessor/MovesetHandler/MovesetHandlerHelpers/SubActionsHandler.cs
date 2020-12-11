using PSA2MovesetLogic.src.ExtentionMethods;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.Models.Fighter;
using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class SubActionsHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public CommandHandler CodeBlocksHandler { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public AnimationsHandler AnimationsHandler { get; private set; }
        public const int MAIN_CODE_BLOCK = 0;
        public const int GFX_CODE_BLOCK = 1;
        public const int SFX_CODE_BLOCK = 2;
        public const int OTHER_CODE_BLOCK = 3;
        public int CodeBlockDataStartLocation { get; private set; }

        public SubActionsHandler(PsaFile psaFile, int dataSectionLocation, CommandHandler codeBlocksHandler, PsaCommandHandler psaCommandHandler, AnimationsHandler animationsHandler, int codeBlockDataStartLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlocksHandler = codeBlocksHandler;
            PsaCommandHandler = psaCommandHandler;
            AnimationsHandler = animationsHandler;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
        }

        public SubAction GetSubAction(int subActionId)
        {
            CodeBlock main = GetCodeBlock(subActionId, MAIN_CODE_BLOCK);
            CodeBlock gfx = GetCodeBlock(subActionId, GFX_CODE_BLOCK);
            CodeBlock sfx = GetCodeBlock(subActionId, SFX_CODE_BLOCK);
            CodeBlock other = GetCodeBlock(subActionId, OTHER_CODE_BLOCK);
            CodeBlock[] codeBlocks = new CodeBlock[] { main, gfx, sfx, other };
            Animation animation = new Animation(GetAnimationName(subActionId), GetAnimationFlags(subActionId));
            return new SubAction(subActionId, codeBlocks, animation);
        }

        public CodeBlock GetCodeBlock(int subActionId, int codeBlockId)
        {
            int codeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlock(codeBlockLocation);
        }

        public int GetNumberOfSubActions()
        {
            //Console.WriteLine(string.Format("Number of Sub Actions: {0}", (PsaFile.DataSection[DataSectionLocation + 13] - PsaFile.DataSection[DataSectionLocation + 12]) / 4));
            return (PsaFile.DataSection[DataSectionLocation + 13] - PsaFile.DataSection[DataSectionLocation + 12]) / 4;
        }

        // this is the offset where subaction code starts (displayed in PSAC)
        public int GetCodeBlockLocation(int subActionId, int codeBlockId)
        {
            int subActionsCodeBlockStartingLocation = PsaFile.DataSection[DataSectionLocation + 12 + codeBlockId] / 4; // n
            int subActionCodeBlockLocation = subActionsCodeBlockStartingLocation + subActionId;

            return subActionCodeBlockLocation;
        }

        public int GetCodeBlockCommandsPointerLocation(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandsPointerLocation(subActionCodeBlockLocation);
        }

        public int GetCodeBlockCommandsLocation(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandsLocation(subActionCodeBlockLocation);
        }

        public int GetCodeBlockCommandLocation(int subActionId, int codeBlockId, int commandIndex)
        {
            int subActionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandLocation(subActionCodeBlockLocation, commandIndex);
        }

        public List<PsaCommand> GetPsaCommandsInCodeBlock(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetPsaCommandsForCodeBlock(subActionCodeBlockLocation);
        }

        public PsaCommand GetPsaCommandInCodeBlock(int subActionId, int codeBlockId, int commandIndex)
        {
            int subActionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetPsaCommandForCodeBlock(subActionCodeBlockLocation, commandIndex);
        }

        public int GetNumberOfPsaCommandsInCodeBlock(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetNumberOfPsaCommandsInCodeBlock(subActionCodeBlockLocation);
        }

        public void AddCommand(int subActionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.AddCommand(actionCodeBlockLocation);
        }

        public void ModifyCommand(int subActionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.ModifyCommand(actionCodeBlockLocation, commandIndex, newPsaCommand);
        }

        public void RemoveCommand(int subActionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.RemoveCommand(actionCodeBlockLocation, commandIndex);
        }

        public void MoveCommandUp(int subActionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.MoveCommandUp(actionCodeBlockLocation, commandIndex);
        }

        public void MoveCommandDown(int subActionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.MoveCommandDown(actionCodeBlockLocation, commandIndex);
        }

        public void InsertCommand(int subActionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int subActionCodeBlockLocation = GetCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.InsertCommand(subActionCodeBlockLocation, commandIndex, newPsaCommand);
        }

        public Animation GetSubActionAnimationData(int subActionId)
        {
            return new Animation(GetAnimationName(subActionId), GetAnimationFlags(subActionId));
        }

        public string GetAnimationName(int subActionId)
        {
            int animationNamePointerLocation = PsaFile.DataSection[DataSectionLocation] / 4 + 1 + subActionId * 2;
            return AnimationsHandler.GetAnimationName(animationNamePointerLocation);
        }

        public AnimationFlags GetAnimationFlags(int subActionId)
        {
            int animationLocation = PsaFile.DataSection[DataSectionLocation] / 4 + subActionId * 2;
            return AnimationsHandler.GetAnimationFlags(animationLocation);
        }

        public void ModifyAnimationName(int subActionId, string newAnimationName)
        {
            int animationLocation = PsaFile.DataSection[DataSectionLocation] / 4 + subActionId * 2;
            AnimationsHandler.ModifyAnimationName(animationLocation, newAnimationName);
        }

        public void RemoveAnimationData(int subActionId)
        {
            int animationLocation = PsaFile.DataSection[DataSectionLocation] / 4 + subActionId * 2;
            AnimationsHandler.RemoveAnimationData(animationLocation);
        }

        public void ModifyAnimationFlags(int subActionId, AnimationFlags animationFlags)
        {
            int animationLocation = PsaFile.DataSection[DataSectionLocation] / 4 + subActionId * 2;
            AnimationsHandler.ModifyAnimationFlags(animationLocation, animationFlags);
        }
    }
}
