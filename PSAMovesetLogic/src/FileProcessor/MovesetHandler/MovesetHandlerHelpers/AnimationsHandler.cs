using PSA2MovesetLogic.src.ExtentionMethods;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class AnimationsHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }
        private int numberOfSpecialActions;
        private int numberOfSubActions;
        private PsaFileHelperMethods psaFileHelperMethods;

        public AnimationsHandler(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation, int numberOfSpecialActions, int numberOfSubActions, PsaFileHelperMethods psaFileHelperMethods)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            this.numberOfSpecialActions = numberOfSpecialActions;
            this.numberOfSubActions = numberOfSubActions;
            this.psaFileHelperMethods = psaFileHelperMethods;
        }

        // snstr
        public int GetAnimationSectionLocation()
        {
            return PsaFile.DataSection[DataSectionLocation + 11] / 4 + 274 + numberOfSpecialActions;
        }

        public string GetAnimationName(int animationNamePointerLocation)
        {
            if (PsaFile.DataSection[animationNamePointerLocation] == 0)
            {
                return "NONE";
            }
            else
            {
                int animationNameLocation = PsaFile.DataSection[animationNamePointerLocation] / 4; // j

                // TODO: Double check this, I think it should be PsaFile.DataSection.Count
                if (animationNameLocation < PsaFile.DataSectionSize) // and animationNameLocation >= stf whatever that means
                {
                    StringBuilder animationName = new StringBuilder();
                    int nameEndByteIndex = 0;
                    while (true) // originally i < 47 -- 48 char limit (which is weird because PSA-C has a 31 char limit on animation names...)
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

        public AnimationFlags GetAnimationFlags(int animationFlagsLocation)
        {
            // will need to look at this later to figure out why it works
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

        public void ModifyAnimationName(int animationLocation, string newAnimationName)
        {
            // k is animationLocation

            int animationSectionLocation = GetAnimationSectionLocation();

            // apparently there's a 31 char limit according to psa-c, not sure if there's actual reason for that or not yet
            if (newAnimationName.Length > 31)
            {
                newAnimationName = newAnimationName.Substring(0, 31);
            }

            int animationSectionEndLocation = PsaFile.DataSection[DataSectionLocation] / 4; // an2 --- total guess

            // SubaRename
            int animationNamePointerLocation = PsaFile.DataSection[animationLocation + 1]; // n
            int animationNameLocation = animationNamePointerLocation / 4; // i

            Console.WriteLine("K: " + animationLocation);
            Console.WriteLine("N: " + animationNamePointerLocation);
            Console.WriteLine("I: " + animationNameLocation);
            Console.WriteLine("AN2: " + animationSectionEndLocation);

            int[] animationNameDoubleWords = Utils.ConvertStringToDoubleWords(newAnimationName);
            int numberOfDoubleWords = animationNameDoubleWords.Length;

            Console.WriteLine("AN1: " + numberOfDoubleWords);

            // This part looks through the strings section of the file to see if there's a string that already exists of the same name
            // if so, it can just point to that existing string instead of having to add it in again
            int newAnimationNameLocation; // g
            for (newAnimationNameLocation = animationSectionLocation; newAnimationNameLocation < animationSectionEndLocation; newAnimationNameLocation++)
            {
                // if matching first doubleword is found and is a valid string
                if (PsaFile.DataSection[newAnimationNameLocation] == animationNameDoubleWords[0] && (PsaFile.DataSection[newAnimationNameLocation - 1] & 0xFF) < 15)
                {
                    // check if character sequence is identical to new animation name
                    if (PsaFile.DataSection.Slice(newAnimationNameLocation, numberOfDoubleWords).SequenceEqual(animationNameDoubleWords))
                    {
                        // if identical, set the pointer for the animation name to the existing string
                        PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
                        break;
                    }
                }
            }
            Console.WriteLine("ANIMATION SECTION LOCATION 1: " + newAnimationNameLocation);

            // if there previously was no animation name offset, add one here to the offset section
            if (animationNamePointerLocation == 0)
            {
                PsaFile.OffsetSection.Add(animationLocation * 4 + 4);
            }

            // else if there was previously animation data
            else
            {
                // if the animation name doesn't already exist (it would exist if another animation is using the same name)
                if (newAnimationNameLocation >= animationSectionEndLocation)
                {
                    PsaFile.DataSection[animationLocation + 1] = 0;
                }


                // check in file if there's another animation that uses the name of the name being replaced
                bool animationNameStillInUse = false;
                for (int i = CodeBlockDataStartLocation; i < PsaFile.DataSectionSizeBytes; i++)
                {
                    if (PsaFile.DataSection[i] == animationNamePointerLocation)
                    {
                        animationNameStillInUse = true;
                        break;
                    }
                }

                // if no other animation is using the replaced animation name, replace the string itself with freespace
                if (!animationNameStillInUse)
                {
                    // if animation name is in the animation section, replace with 0s to represent free space
                    if (animationNameLocation >= animationSectionLocation && animationNameLocation < animationSectionEndLocation)
                    {
                        while ((PsaFile.DataSection[animationNameLocation] & 0xFF) > 13)
                        {
                            PsaFile.DataSection[animationNameLocation] = 0;
                            animationNameLocation++;
                        }
                        PsaFile.DataSection[animationNameLocation] = 0;
                    }
                    // not sure how or why the animation name would be outside the animation section, but in this circumstance the freespace is represented with FADEF00Ds
                    else
                    {
                        while ((PsaFile.DataSection[animationNameLocation] & 0xFF) > 15)
                        {
                            PsaFile.DataSection[animationNameLocation] = Constants.FADEF00D;
                            animationNameLocation++;
                        }
                        PsaFile.DataSection[animationNameLocation] = Constants.FADEF00D;
                    }
                }
            }

            // if the animation name doesn't already exist (it would exist if another animation is using the same name)
            if (newAnimationNameLocation >= animationSectionEndLocation)
            {
                // check if there's enough free space in the animation name section for the new animation to be added to
                for (newAnimationNameLocation = animationSectionLocation; newAnimationNameLocation < animationSectionEndLocation; newAnimationNameLocation++)
                {
                    // if location starts with a free space indicator
                    if ((PsaFile.DataSection[newAnimationNameLocation] == 0 || PsaFile.DataSection[newAnimationNameLocation] == Constants.FADEF00D) && (PsaFile.DataSection[newAnimationNameLocation - 1] & 0xFF) < 15)
                    {
                        int currentWord;
                        for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                        {
                            if (PsaFile.DataSection[newAnimationNameLocation + currentWord] != 0 && PsaFile.DataSection[newAnimationNameLocation + currentWord] != Constants.FADEF00D)
                            {
                                newAnimationNameLocation += currentWord;
                                break;
                            }
                        }
                        // if free space in this location is enough to hold entire animation name, place the animation name in this location
                        if (currentWord == numberOfDoubleWords && currentWord < animationSectionEndLocation)
                        {
                            for (int i = 0; i < numberOfDoubleWords; i++)
                            {
                                PsaFile.DataSection[newAnimationNameLocation + i] = animationNameDoubleWords[i];
                            }
                            PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
                            break;
                        }
                    }
                }

                Console.WriteLine("ANIMATION SECTION LOCATION 2: " + newAnimationNameLocation);

                // if there is not enough existing free space in the animation name section to place the new animation name
                if (newAnimationNameLocation >= animationSectionEndLocation)
                {
                    // look in the entire code block data section to see if there's enough free space for the animation name to be placed
                    for (newAnimationNameLocation = CodeBlockDataStartLocation; newAnimationNameLocation < animationSectionLocation; newAnimationNameLocation++)
                    {
                        if (PsaFile.DataSection[newAnimationNameLocation] == Constants.FADEF00D)
                        {
                            int currentWord;
                            for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                            {
                                if (PsaFile.DataSection[newAnimationNameLocation + currentWord] != Constants.FADEF00D)
                                {
                                    newAnimationNameLocation += currentWord;
                                    break;
                                }
                            }
                            if (numberOfDoubleWords == currentWord)
                            {
                                for (int i = 0; i < numberOfDoubleWords; i++)
                                {
                                    PsaFile.DataSection[newAnimationNameLocation + i] = animationNameDoubleWords[i];
                                }
                                PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
                                break;
                            }
                        }
                    }

                    Console.WriteLine("ANIMATION SECTION LOCATION 3: " + newAnimationNameLocation);

                    // if there is not enough existing free space in the entire code block data section to place the new animation name
                    if (newAnimationNameLocation >= animationSectionLocation)
                    {
                        // this checks from after the animation section to the end of the data section for FADEF00Ds
                        for (newAnimationNameLocation = PsaFile.DataSection[DataSectionLocation] / 4 + numberOfSubActions * 2; newAnimationNameLocation < PsaFile.DataSectionSizeBytes; newAnimationNameLocation++)
                        {
                            if (PsaFile.DataSection[newAnimationNameLocation] == Constants.FADEF00D)
                            {
                                int currentWord;
                                for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                                {
                                    if (PsaFile.DataSection[newAnimationNameLocation + currentWord] != Constants.FADEF00D)
                                    {
                                        newAnimationNameLocation += currentWord;
                                        break;
                                    }
                                }
                                if (numberOfDoubleWords == currentWord)
                                {
                                    for (int i = 0; i < numberOfDoubleWords; i++)
                                    {
                                        PsaFile.DataSection[newAnimationNameLocation + i] = animationNameDoubleWords[i];
                                    }
                                    PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
                                    break;
                                }
                            }
                            else if (PsaFile.DataSection[newAnimationNameLocation] == animationNameDoubleWords[0] && (PsaFile.DataSection[newAnimationNameLocation - 1] & 0xFF) < 15)
                            {
                                int currentWord;
                                for (currentWord = 1; currentWord < numberOfDoubleWords; currentWord++)
                                {
                                    if (PsaFile.DataSection[newAnimationNameLocation + currentWord] != Constants.FADEF00D)
                                    {
                                        newAnimationNameLocation += currentWord;
                                        break;
                                    }
                                }
                                if (numberOfDoubleWords == currentWord)
                                {
                                    PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
                                    break;
                                }
                            }
                        }

                        Console.WriteLine("ANIMATION SECTION LOCATION 4: " + newAnimationNameLocation);

                    }
                }
            }

            // if new animation name location is beyond data section (meaning space wasn't found for it in all the above checks), append the animation name to the end of the data section
            if (newAnimationNameLocation >= PsaFile.DataSectionSizeBytes)
            {
                newAnimationNameLocation = PsaFile.DataSectionSizeBytes;
                PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
                for (int i = 0; i < numberOfDoubleWords; i++)
                {
                    PsaFile.DataSection.Add(animationNameDoubleWords[i]);
                }
                psaFileHelperMethods.UpdateMovesetHeaders();
            }

            // if there was previously no animation name offset for this animation (it didn't have animation data before)
            else if (animationNamePointerLocation == 0)
            {
                psaFileHelperMethods.UpdateMovesetHeaders();
            }

            Console.WriteLine("ANIMATION SECTION LOCATION 5: " + newAnimationNameLocation);
        }


    }
}
