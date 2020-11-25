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
        public CodeBlocksHandler CodeBlocksHandler { get; private set; }
        public PsaCommandHandler PsaCommandHandler { get; private set; }
        public int AnimationSectionStartLocation { get; private set; } // this is a guess for: snstr
        public const int MAIN_CODE_BLOCK = 0;
        public const int GFX_CODE_BLOCK = 1;
        public const int SFX_CODE_BLOCK = 2;
        public const int OTHER_CODE_BLOCK = 3;
        public int CodeBlockDataStartLocation { get; private set; }
        private PsaFileHelperMethods psaFileHelperMethods;

        public SubActionsHandler(PsaFile psaFile, int dataSectionLocation, CodeBlocksHandler codeBlocksHandler, PsaCommandHandler psaCommandHandler, int numberOfSpecialActions, int codeBlockDataStartLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlocksHandler = codeBlocksHandler;
            PsaCommandHandler = psaCommandHandler;
            AnimationSectionStartLocation = PsaFile.DataSection[DataSectionLocation + 11] / 4 + 274 + numberOfSpecialActions;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            psaFileHelperMethods = new PsaFileHelperMethods(PsaFile, DataSectionLocation);
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

        public void MoveCommandUp(int subActionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.MoveCommandUp(actionCodeBlockLocation, commandIndex);
        }

        public void MoveCommandDown(int subActionId, int codeBlockId, int commandIndex)
        {
            int actionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.MoveCommandDown(actionCodeBlockLocation, commandIndex);
        }

        public void InsertCommand(int subActionId, int codeBlockId, int commandIndex, PsaCommand newPsaCommand)
        {
            int subActionCodeBlockLocation = GetSubActionCodeBlockLocation(subActionId, codeBlockId);
            CodeBlocksHandler.InsertCommand(subActionCodeBlockLocation, commandIndex, newPsaCommand);
        }

        public void SetAnimationName(int subActionId, string newAnimationName)
        {
            // int animationLocation = PsaFile.DataSection[DataSectionLocation] / 4 + 1 + subActionId * 2;

            int animationSectionEndLocation = PsaFile.DataSection[DataSectionLocation] / 4; // an2 --- total guess
            int animationLocation = PsaFile.DataSection[DataSectionLocation] / 4 + subActionId * 2; // k

            // rd1 = newAnimationName

            // SubaRename
            int animationNamePointerLocation = PsaFile.DataSection[animationLocation + 1]; // n
            int animationNameLocation = animationNamePointerLocation / 4; // i

            Console.WriteLine("K: " + animationLocation);
            Console.WriteLine("N: " + animationNamePointerLocation);
            Console.WriteLine("I: " + animationNameLocation);
            Console.WriteLine("AN2: " + animationSectionEndLocation);

            /*
            // if no animation exists
            if (animationNamePointerLocation == 0)
            {
                
            }

            // animation data exists
            else
            {
            */

            byte[] animationNameBytes = Encoding.UTF8.GetBytes(newAnimationName); // bytes
            int animationNameLength = newAnimationName.Length; // m

            // if animation string length doesn't match byte length, throw exception...
            // I THINK this is from like if you were to try to insert an emoji as a character since they represent more bytes, but unsure
            // leaving this for now in case I eventually figure it out
            if (animationNameLength != animationNameBytes.Length)
            {
                throw new ArgumentException("Typing error???");
            }

            int animationNameByteLength = animationNameLength / 4;

            // length of array is number of bytes required to hold all characters in animation name
            // each byte can hold 4 characters max

            int numberOfDoubleWords = (animationNameLength / 4) + 1;
            int[] animationNameDoubleWords = new int[numberOfDoubleWords];
            for (int i = 0; i < animationNameLength; i++) // i == g
            {
                if (i % 4 == 0)
                {
                    animationNameDoubleWords[i / 4] = animationNameBytes[i] * 16777216;
                }
                else if (i % 4 == 1)
                {
                    animationNameDoubleWords[i / 4] += animationNameBytes[i] * 65536;
                }
                else if (i % 4 == 2)
                {
                    animationNameDoubleWords[i / 4] += animationNameBytes[i] * 256;
                }
                else
                {
                    animationNameDoubleWords[i / 4] += animationNameBytes[i];
                }
            }
            
            // Console.WriteLine(string.Join(", ", animationNameDoubleWords));
            Console.WriteLine("AN1: " + numberOfDoubleWords);

            // This part looks through the strings section of the file to see if there's an existing string that already exists
            int animationSectionLocation; // g
            for (animationSectionLocation = AnimationSectionStartLocation; animationSectionLocation < animationSectionEndLocation; animationSectionLocation++) // the naming of this for loop is all guess work
            {
                if (PsaFile.DataSection[animationSectionLocation] == animationNameDoubleWords[0] && (PsaFile.DataSection[animationSectionLocation - 1] & 0xFF) < 15)
                {
                    int currentWord;
                    for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                    {
                        if (PsaFile.DataSection[animationSectionLocation + currentWord] != animationNameDoubleWords[currentWord])
                        {
                            animationSectionLocation += currentWord;
                            break;
                        }
                    }
                    if (numberOfDoubleWords == currentWord)
                    {
                        if (animationSectionLocation != animationNameLocation)
                        {
                            PsaFile.DataSection[animationLocation + 1] = animationSectionLocation * 4;
                        }
                        break;
                    }
                }
            }
            Console.WriteLine("ANIMATION SECTION LOCATION 1: " + animationSectionLocation);

            // if there previously was no animation name offset, add one here to the offset section
            if (animationNamePointerLocation == 0)
            {
                PsaFile.OffsetSection.Add(animationLocation * 4 + 4);
            }

            // there was previously animation data
            else
            {
                // if animation section found is greater than animation section end, it doesn't exist yet
                if (animationSectionLocation >= animationSectionEndLocation)
                {
                    PsaFile.DataSection[animationLocation + 1] = 0;
                }

                // if pointer for animation already exists
                int chosenLocation = CodeBlockDataStartLocation;
                while (chosenLocation < PsaFile.DataSectionSizeBytes && PsaFile.DataSection[chosenLocation] != animationNamePointerLocation)
                {
                    chosenLocation++;
                }

                // I think this means the pointer was not found
                if (chosenLocation == PsaFile.DataSectionSizeBytes)
                {
                    if (animationNameLocation >= AnimationSectionStartLocation && animationNameLocation < animationSectionEndLocation)
                    {
                        while((PsaFile.DataSection[animationNameLocation] & 0xFF) > 13)
                        {
                            PsaFile.DataSection[animationNameLocation] = 0;
                            animationNameLocation++;
                        }
                        PsaFile.DataSection[animationNameLocation] = 0;
                    }
                    else
                    {
                        while((PsaFile.DataSection[animationNameLocation] & 0xFF) > 15)
                        {
                            PsaFile.DataSection[animationNameLocation] = Constants.FADEF00D;
                            animationNameLocation++;
                        }
                        PsaFile.DataSection[animationNameLocation] = Constants.FADEF00D;
                    }
                }
            }

            if (animationSectionLocation >= animationSectionEndLocation)
            {
                for (animationSectionLocation = AnimationSectionStartLocation; animationSectionLocation < animationSectionEndLocation; animationSectionLocation++)
                {
                    if ((PsaFile.DataSection[animationSectionLocation] == 0 || PsaFile.DataSection[animationSectionLocation] == Constants.FADEF00D) && (PsaFile.DataSection[animationSectionLocation - 1] & 0xFF) < 15)
                    {
                        int currentWord;
                        for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                        {
                            if (PsaFile.DataSection[animationSectionLocation + currentWord] != 0 && PsaFile.DataSection[animationSectionLocation + currentWord] != Constants.FADEF00D)
                            {
                                animationSectionLocation += currentWord;
                                break;
                            }
                        }
                        if (currentWord == numberOfDoubleWords && currentWord < animationSectionEndLocation)
                        {
                            for (int i = 0; i < numberOfDoubleWords; i++)
                            {
                                PsaFile.DataSection[animationSectionLocation + i] = animationNameDoubleWords[i];
                            }
                            PsaFile.DataSection[animationLocation + 1] = animationSectionLocation * 4;
                            break;
                        }
                    }
                }

                Console.WriteLine("ANIMATION SECTION LOCATION 2: " + animationSectionLocation);


                if (animationSectionLocation >= animationSectionEndLocation)
                {
                    for (animationSectionLocation = CodeBlockDataStartLocation; animationSectionLocation < AnimationSectionStartLocation; animationSectionLocation++)
                    {
                        if (PsaFile.DataSection[animationSectionLocation] == Constants.FADEF00D)
                        {
                            int currentWord;
                            for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                            {
                                if (PsaFile.DataSection[animationSectionLocation + currentWord] != Constants.FADEF00D)
                                {
                                    animationSectionLocation += currentWord;
                                    break;
                                }
                            }
                            if (numberOfDoubleWords == currentWord)
                            {
                                for (int i = 0; i < numberOfDoubleWords; i++)
                                {
                                    PsaFile.DataSection[animationSectionLocation + i] = animationNameDoubleWords[i];
                                }
                                PsaFile.DataSection[animationLocation + 1] = animationSectionLocation * 4;
                                break;
                            }
                        }
                    }

                    Console.WriteLine("ANIMATION SECTION LOCATION 3: " + animationSectionLocation);


                    if (animationSectionLocation >= AnimationSectionStartLocation)
                    {
                        for (animationSectionLocation = PsaFile.DataSection[DataSectionLocation] / 4 + GetNumberOfSubActions() * 2; animationSectionLocation < PsaFile.DataSectionSizeBytes; animationSectionLocation++)
                        {
                            if (PsaFile.DataSection[animationSectionLocation] == Constants.FADEF00D)
                            {
                                int currentWord;
                                for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                                {
                                    if (PsaFile.DataSection[animationSectionLocation + currentWord] != Constants.FADEF00D)
                                    {
                                        animationSectionLocation += currentWord;
                                        break;
                                    }
                                }
                                if (numberOfDoubleWords == currentWord)
                                {
                                    for (int i = 0; i < numberOfDoubleWords; i++)
                                    {
                                        PsaFile.DataSection[animationSectionLocation + i] = animationNameDoubleWords[i];
                                    }
                                    PsaFile.DataSection[animationLocation + 1] = animationSectionLocation * 4;
                                    break;
                                }
                            }
                            else if (PsaFile.DataSection[animationSectionLocation] == animationNameDoubleWords[0] && (PsaFile.DataSection[animationSectionLocation - 1] & 0xFF) < 15)
                            {
                                int currentWord;
                                for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                                {
                                    if (PsaFile.DataSection[animationSectionLocation + currentWord] != Constants.FADEF00D)
                                    {
                                        animationSectionLocation += currentWord;
                                        break;
                                    }
                                }
                                if (numberOfDoubleWords == currentWord)
                                {
                                    PsaFile.DataSection[animationLocation + 1] = animationSectionLocation * 4;
                                    break;
                                }
                            }
                        }

                        Console.WriteLine("ANIMATION SECTION LOCATION 4: " + animationSectionLocation);

                    }
                }
            }
            if (animationSectionLocation >= PsaFile.DataSectionSizeBytes)
            {
                animationSectionLocation = PsaFile.DataSectionSizeBytes;
                PsaFile.DataSection[animationLocation + 1] = animationSectionLocation * 4;
                for (int i = 0; i < numberOfDoubleWords; i++)
                {
                    PsaFile.DataSection.Add(animationNameDoubleWords[i]);
                }
                psaFileHelperMethods.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
            }
            else if (animationNamePointerLocation == 0)
            {
                psaFileHelperMethods.ApplyHeaderUpdatesToAccountForPsaCommandChanges();
            }
            Console.WriteLine("ANIMATION SECTION LOCATION 5: " + animationSectionLocation);

            
        }
    }
}
