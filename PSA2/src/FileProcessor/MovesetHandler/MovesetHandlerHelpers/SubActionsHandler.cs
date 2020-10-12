using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Models.Fighter;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class SubActionsHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public CodeBlocksHandler CodeBlocksHandler { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public const int MAIN_CODE_BLOCK = 0;
        public const int GFX_CODE_BLOCK = 1;
        public const int SFX_CODE_BLOCK = 2;
        public const int OTHER_CODE_BLOCK = 3;

        public SubActionsHandler(PsaFile psaFile, int dataSectionLocation, CodeBlocksHandler codeBlocksHandler, PsaCommandHandler psaCommandHandler)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlocksHandler = codeBlocksHandler;
            PsaCommandHandler = psaCommandHandler;
        }

        public SubAction GetSubAction(int subActionId)
        {
            CodeBlock main = GetCodeBlock(subActionId, MAIN_CODE_BLOCK);
            CodeBlock gfx = GetCodeBlock(subActionId, GFX_CODE_BLOCK);
            CodeBlock sfx = GetCodeBlock(subActionId, SFX_CODE_BLOCK);
            CodeBlock other = GetCodeBlock(subActionId, OTHER_CODE_BLOCK);
            CodeBlock[] codeBlocks = new CodeBlock[] { main, gfx, sfx, other };
            Animation animation = new Animation(GetSubActionAnimationName(subActionId), GetSubActionAnimationFlags(subActionId));
            return new SubAction(subActionId, codeBlocks, animation);
        }

        public CodeBlock GetCodeBlock(int subActionId, int codeBlockId)
        {
            int codeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlock(codeBlockLocation);
        }

        public int GetNumberOfSubActions()
        {
            //Console.WriteLine(string.Format("Number of Sub Actions: {0}", (PsaFile.DataSection[DataSectionLocation + 13] - PsaFile.DataSection[DataSectionLocation + 12]) / 4));
            return (PsaFile.DataSection[DataSectionLocation + 13] - PsaFile.DataSection[DataSectionLocation + 12]) / 4;
        }

        // this is the offset where subaction code starts (displayed in PSAC)
        public int GetSubActionCodeBlockLocation(int subActionId, int codeBlockId)
        {
            int subActionsCodeBlockStartingLocation = PsaFile.DataSection[DataSectionLocation + 12 + codeBlockId] / 4; // n
            int subActionCodeBlockLocation = subActionsCodeBlockStartingLocation + subActionId;

            return subActionCodeBlockLocation;
        }

        public int GetSubActionCodeBlockCommandsPointerLocation(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandsPointerLocation(subActionCodeBlockLocation);
        }

        public int GetSubActionCodeBlockCommandsLocation(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandsLocation(subActionCodeBlockLocation);
        }

        public int GetSubActionCodeBlockCommandLocation(int subActionId, int codeBlockId, int commandIndex)
        {
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetCodeBlockCommandLocation(subActionCodeBlockLocation, commandIndex);
        }


        public List<PsaCommand> GetPsaCommandsForSubAction(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetPsaCommandsForCodeBlock(subActionCodeBlockLocation);
        }

        public PsaCommand GetPsaCommandForSubActionCodeBlock(int subActionId, int codeBlockId, int commandIndex)
        {
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetPsaCommandForCodeBlock(subActionCodeBlockLocation, commandIndex);
        }

        public int GetNumberOfPsaCommandsInSubActionCodeBlock(int subActionId, int codeBlockId)
        {
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            return CodeBlocksHandler.GetNumberOfPsaCommandsInCodeBlock(subActionCodeBlockLocation);
        }

        public Animation GetSubActionAnimationData(int subActionId)
        {
            return new Animation(GetSubActionAnimationName(subActionId), GetSubActionAnimationFlags(subActionId));
        }

        public string GetSubActionAnimationName(int subActionId)
        {
            int animationLocation = PsaFile.DataSection[DataSectionLocation] / 4 + 1 + subActionId * 2;
            if (PsaFile.DataSection[animationLocation] == 0)
            {
                return "NONE";
            }
            else
            {
                int animationNameLocation = PsaFile.DataSection[animationLocation] / 4; // j

                // TODO: Double check this, I think it should be PsaFile.DataSection.Count
                if (animationNameLocation < PsaFile.DataSectionSize) // and animationNameLocation >= stf whatever that means
                {
                    StringBuilder animationName = new StringBuilder();
                    int nameEndByteIndex = 0;
                    while (true) // originally i < 47 -- 48 char limit
                    {
                        string nextStringData = Utils.ConvertDoubleWordToString(PsaFile.DataSection[animationNameLocation + nameEndByteIndex]);
                        animationName.Append(nextStringData);
                        if (nextStringData.Length == 4)
                        {
                            nameEndByteIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    //Console.WriteLine(animationName.ToString());
                    return animationName.ToString();
                }

                return "ERROR";
            }

        }

        public AnimationFlags GetSubActionAnimationFlags(int subActionId)
        {
            // will need to look at this later to figure out why it works
            int animationFlagsLocation = PsaFile.DataSection[DataSectionLocation] / 4 + subActionId * 2;
            int animationFlagsValue = PsaFile.DataSection[animationFlagsLocation];
            int animationFlagsOptions = (animationFlagsValue >> 16) & 0xFF;
            int inTransition = (animationFlagsValue >> 24) & 0xFF;
            int noOutTransition = animationFlagsOptions & 0x1;
            int loop = animationFlagsOptions & 0x2;
            int movesCharacter = animationFlagsOptions & 0x4;
            int unknown3 = animationFlagsOptions & 0x8;
            int unknown4 = animationFlagsOptions & 0x10;
            int unknown5 = animationFlagsOptions & 0x20;
            int transitionOutFromStart = animationFlagsOptions & 0x40;
            int unknown7 = animationFlagsOptions & 0x80;
            return new AnimationFlags(inTransition, noOutTransition, loop, movesCharacter, unknown3, unknown4, unknown5, transitionOutFromStart, unknown7);
        }

        public void AddCommand(int subActionId, int codeBlockId)
        {
            int actionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.AddCommand(actionCodeBlockLocation);
        }

        public void ModifyCommand(int subActionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int actionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.ModifyCommand(actionCodeBlockLocation, commandIndex, newPsaCommand);
        }

        public void RemoveCommand(int subActionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.RemoveCommand(actionCodeBlockLocation, commandIndex);
        }

        public void MoveCommand(int subActionId, int codeBlockId, int commandIndex, MoveDirection moveDirection)
        {
            int actionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.MoveCommand(actionCodeBlockLocation, commandIndex, moveDirection);
        }
    }
}
