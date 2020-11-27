﻿using PSA2MovesetLogic.src.ExtentionMethods;
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
            int animationSectionLocation = GetAnimationSectionLocation();

            // apparently there's a 31 char limit according to psa-c, not sure if there's actual reason for that or not yet
            if (newAnimationName.Length > 31)
            {
                newAnimationName = newAnimationName.Substring(0, 31);
            }

            int animationSectionEndLocation = PsaFile.DataSection[DataSectionLocation] / 4;
            int animationNamePointerLocation = PsaFile.DataSection[animationLocation + 1];
            int animationNameLocation = animationNamePointerLocation / 4;

            int[] animationNameDoubleWords = Utils.ConvertStringToDoubleWords(newAnimationName);

            int newAnimationNameLocation; // g

            newAnimationNameLocation = FindExistingAnimationNameInAnimationSection(animationNameDoubleWords);
            if (newAnimationNameLocation != -1)
            {
                // set animation name pointer location to the location of the already existing animation
                PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
            }

            // if there previously was no animation name offset, add one here to the offset section
            if (animationNamePointerLocation == 0)
            {
                PsaFile.OffsetSection.Add(animationLocation * 4 + 4);
            }

            // else if there was previously animation data
            else
            {
                // if the animation name doesn't already exist (it would exist if another animation is using the same name)
                if (newAnimationNameLocation == -1)
                {
                    PsaFile.DataSection[animationLocation + 1] = 0;
                }

                // check in file if there's another animation that uses the old animation name (the one being replaced)
                bool animationNameStillInUse = IsAnimationNameInUse(animationNamePointerLocation);

                // if no other animation is using the replaced animation name, replace the string itself with freespace
                if (!animationNameStillInUse)
                {
                    // if animation name is in the animation section, replace with 0s to represent free space
                    if (animationNameLocation >= animationSectionLocation && animationNameLocation < animationSectionEndLocation)
                    {
                        ReplaceAnimationNameWithFreeSpace(animationNameLocation, 0, 13);
                    }
                    // in the circumstance where animation name was located outside animation section, free space is represented with FADEF00Ds instead of 0s
                    else
                    {
                        ReplaceAnimationNameWithFreeSpace(animationNameLocation, Constants.FADEF00D, 15);
                    }
                }
            }

            // if the animation name doesn't already exist (it would exist if another animation is using the same name)
            if (newAnimationNameLocation == -1)
            {
                InsertAnimationNameIntoFreeSpace(animationLocation, animationNameDoubleWords);
            }

            psaFileHelperMethods.UpdateMovesetHeaders();
        }

        private int FindExistingAnimationNameInAnimationSection(int[] animationNameDoubleWords)
        {
            // This part looks through the strings section of the file to see if there's a string that already exists of the same name
            // if so, it can just point to that existing string instead of having to add it in again
            int animationSectionLocation = GetAnimationSectionLocation();
            int animationSectionEndLocation = PsaFile.DataSection[DataSectionLocation] / 4;
            for (int i = animationSectionLocation; i < animationSectionEndLocation; i++)
            {
                // if matching first doubleword is found and is a valid string
                if (PsaFile.DataSection[i] == animationNameDoubleWords[0] && (PsaFile.DataSection[i - 1] & 0xFF) < 15)
                {
                    // check if character sequence is identical to new animation name
                    if (PsaFile.DataSection.Slice(i, animationNameDoubleWords.Length).SequenceEqual(animationNameDoubleWords))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private bool IsAnimationNameInUse(int animationNamePointerLocation)
        {
            for (int i = CodeBlockDataStartLocation; i < PsaFile.DataSection.Count; i++)
            {
                if (PsaFile.DataSection[i] == animationNamePointerLocation)
                {
                    return true;
                }
            }
            return false;
        }

        private void ReplaceAnimationNameWithFreeSpace(int animationNameLocation, int freeSpaceIndicator, int byteLimit)
        {
            while ((PsaFile.DataSection[animationNameLocation] & 0xFF) > byteLimit)
            {
                PsaFile.DataSection[animationNameLocation] = freeSpaceIndicator;
                animationNameLocation++;
            }
            PsaFile.DataSection[animationNameLocation] = freeSpaceIndicator;
        }

        private void InsertAnimationNameIntoFreeSpace(int animationLocation, int[] animationNameDoubleWords)
        {
            int newAnimationNameLocation;

            // check if there is enough free space in the animation section to place the new animation name
            newAnimationNameLocation = FindFreeSpaceInAnimationSection(animationNameDoubleWords.Length);

            if (newAnimationNameLocation != -1)
            {
                ReplaceDataWithAnimationName(animationLocation, newAnimationNameLocation, animationNameDoubleWords);
            }

            // if there is not enough existing free space in the animation name section to place the new animation name
            else
            {
                // look in the entire code block data section to see if there's enough free space for the animation name to be placed
                newAnimationNameLocation = FindFreeSpaceInCodeBlockDataSection(animationNameDoubleWords.Length);

                if (newAnimationNameLocation != -1)
                {
                    ReplaceDataWithAnimationName(animationLocation, newAnimationNameLocation, animationNameDoubleWords);
                }

                // if there is not enough existing free space in the entire code block data section to place the new animation name
                else
                {
                    // this checks from after the animation section to the end of the data section for FADEF00Ds
                    newAnimationNameLocation = FindExistingAnimationNameInDataSection(animationNameDoubleWords);

                    if (newAnimationNameLocation != -1)
                    {
                        PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
                    }

                    if (newAnimationNameLocation == -1)
                    {
                        newAnimationNameLocation = FindFreeSpaceInDataSection(animationNameDoubleWords.Length);

                        if (newAnimationNameLocation != -1)
                        {
                            ReplaceDataWithAnimationName(animationLocation, newAnimationNameLocation, animationNameDoubleWords);
                        }
                    }
                }
            }

            // if free space for animation name STILL cannot be found after all of that checking,
            // just append the animation name to the end of the data section and call it a day
            if (newAnimationNameLocation == -1)
            {
                newAnimationNameLocation = PsaFile.DataSection.Count;
                PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
                for (int i = 0; i < animationNameDoubleWords.Length; i++)
                {
                    PsaFile.DataSection.Add(animationNameDoubleWords[i]);
                }
            }
        }

        private int FindFreeSpaceInAnimationSection(int freeSpaceNeeded)
        {
            int animationSectionLocation = GetAnimationSectionLocation();
            int animationSectionEndLocation = PsaFile.DataSection[DataSectionLocation] / 4;

            // check if there's enough free space in the animation name section for the new animation to be added to
            for (int i = animationSectionLocation; i < animationSectionEndLocation - freeSpaceNeeded; i++)
            {
                // if location starts with a free space indicator
                if ((PsaFile.DataSection[i] == 0 || PsaFile.DataSection[i] == Constants.FADEF00D) && (PsaFile.DataSection[i - 1] & 0xFF) < 15)
                {
                    // if there is enough freespace for the entire animation name
                    List<int> sequence = PsaFile.DataSection.Slice(i, freeSpaceNeeded);
                    // if there are any items in sequence that aren't a 0 or FADEF00D, the sequence is not only free space
                    bool isSequenceOnlyFreespace = !sequence.Where(s => s != 0 && s != Constants.FADEF00D).Any();
                    if (isSequenceOnlyFreespace)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public int FindFreeSpaceInCodeBlockDataSection(int freeSpaceNeeded)
        {
            int animationSectionLocation = GetAnimationSectionLocation();
            // look in the entire code block data section to see if there's enough free space for the animation name to be placed
            for (int i = CodeBlockDataStartLocation; i < animationSectionLocation; i++)
            {
                if (PsaFile.DataSection[i] == Constants.FADEF00D)
                {
                    // if there is enough freespace for the entire animation name
                    List<int> sequence = PsaFile.DataSection.Slice(i, freeSpaceNeeded);
                    // if there are any items in sequence that aren't a FADEF00D, the sequence is not only free space
                    bool isSequenceOnlyFreespace = !sequence.Where(s => s != Constants.FADEF00D).Any();
                    if (isSequenceOnlyFreespace)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public int FindExistingAnimationNameInDataSection(int[] animationNameDoubleWords)
        {
            for (int i = PsaFile.DataSection[DataSectionLocation] / 4 + numberOfSubActions * 2; i < PsaFile.DataSectionSizeBytes; i++)
            {
                if (PsaFile.DataSection[i] == animationNameDoubleWords[0] && (PsaFile.DataSection[i - 1] & 0xFF) < 15)
                {
                    // check if character sequence is identical to new animation name
                    if (PsaFile.DataSection.Slice(i, animationNameDoubleWords.Length).SequenceEqual(animationNameDoubleWords))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public int FindFreeSpaceInDataSection(int freeSpaceNeeded)
        {
            for (int i = PsaFile.DataSection[DataSectionLocation] / 4 + numberOfSubActions * 2; i < PsaFile.DataSectionSizeBytes; i++)
            {
                if (PsaFile.DataSection[i] == Constants.FADEF00D)
                {
                    // if there is enough freespace for the entire animation name
                    List<int> sequence = PsaFile.DataSection.Slice(i, freeSpaceNeeded);
                    // if there are any items in sequence that aren't a FADEF00D, the sequence is not only free space
                    bool isSequenceOnlyFreespace = !sequence.Where(s => s != Constants.FADEF00D).Any();
                    if (isSequenceOnlyFreespace)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private void ReplaceDataWithAnimationName(int animationLocation, int newAnimationNameLocation, int[] animationNameDoubleWords)
        {
            for (int i = 0; i < animationNameDoubleWords.Length; i++)
            {
                PsaFile.DataSection[newAnimationNameLocation + i] = animationNameDoubleWords[i];
            }
            PsaFile.DataSection[animationLocation + 1] = newAnimationNameLocation * 4;
        }
    }
}
