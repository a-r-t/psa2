using PSA2MovesetLogic.src.ExtentionMethods;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    /// <summary>
    /// Class is for reading/writing animation data (name and flags)
    /// <para>Intended be used for subaction animations</para>
    /// </summary>
    public class AnimationsHandler
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public int CodeBlockDataStartLocation { get; private set; }
        private int numberOfSpecialActions;
        private int numberOfSubActions;

        public AnimationsHandler(PsaFile psaFile, int dataSectionLocation, int codeBlockDataStartLocation, int numberOfSpecialActions, int numberOfSubActions)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            CodeBlockDataStartLocation = codeBlockDataStartLocation;
            this.numberOfSpecialActions = numberOfSpecialActions;
            this.numberOfSubActions = numberOfSubActions;
        }

        /// <summary>
        /// The location where the animation section starts
        /// <para>snstr in PSA-C</para>
        /// </summary>
        /// <returns>location where the animation section starts</returns>
        public int GetAnimationSectionLocation()
        {
            return PsaFile.DataSection[DataSectionLocation + 11] / 4 + 274 + numberOfSpecialActions;
        }

        /// <summary>
        /// Reads in animation name at a specified location and converts it to a string
        /// </summary>
        /// <param name="animationNamePointerLocation">location of the pointer to an animation name</param>
        /// <returns>animation name as string</returns>
        public string GetAnimationName(int animationNamePointerLocation)
        {
            // if animation name pointer location doesn't point to anything, no animation data exists for animation
            if (PsaFile.DataSection[animationNamePointerLocation] == 0)
            {
                return "";
            }
            else
            {
                int animationNameLocation = PsaFile.DataSection[animationNamePointerLocation] / 4;

                // TODO: Double check this, I think it should be PsaFile.DataSection.Count
                if (animationNameLocation > CodeBlockDataStartLocation && animationNameLocation < PsaFile.DataSectionSize)
                {
                    StringBuilder animationName = new StringBuilder();
                    int nameEndByteIndex = 0;

                    // originally i < 47 -- 48 char limit (which is weird because PSA-C has a 31 char limit on animation names...)
                    // for now it'll read in the entire animation name regardless of length until I look further into why there's an inconsistency and if there really is a length limit or not
                    while (true) 
                    {
                        // read in string chars one double word at a time (1 double word = 4 chars max)
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

                    return animationName.ToString();
                }
                else
                {
                    throw new ArgumentException("Invalid animation name pointer location");
                }
            }
        }

        /// <summary>
        /// Gets animation flags for a specified animation
        /// </summary>
        /// <param name="animationLocation">location of animation to get flags of</param>
        /// <returns>animation flags of animation</returns>
        public AnimationFlags GetAnimationFlags(int animationLocation)
        {
            int animationFlagsValue = PsaFile.DataSection[animationLocation];
            int animationFlagsOptions = (animationFlagsValue >> 16) & 0xFF;
            int inTransition = (animationFlagsValue >> 24) & 0xFF;
            bool noOutTransition = (animationFlagsOptions & 0x1).ToBoolean();
            bool loop = (animationFlagsOptions & 0x2).ToBoolean();
            bool movesCharacter = (animationFlagsOptions & 0x4).ToBoolean();
            bool unknown3 = (animationFlagsOptions & 0x8).ToBoolean();
            bool unknown4 = (animationFlagsOptions & 0x10).ToBoolean();
            bool unknown5 = (animationFlagsOptions & 0x20).ToBoolean();
            bool transitionOutFromStart = (animationFlagsOptions & 0x40).ToBoolean();
            bool unknown7 = (animationFlagsOptions & 0x80).ToBoolean();
            return new AnimationFlags(inTransition, noOutTransition, loop, movesCharacter, unknown3, unknown4, unknown5, transitionOutFromStart, unknown7);
        }

        /// <summary>
        /// Modifies the animation name
        /// <para>if no animation data at the given location exists, it will be created</para>
        /// </summary>
        /// <param name="animationLocation">location of animation to modify</param>
        /// <param name="newAnimationName">name to change animation to -- if no animation data exists, it will be added. animation name cannot be an empty string</param>
        public void ModifyAnimationName(int animationLocation, string newAnimationName)
        {
            if (newAnimationName.Length == 0)
            {
                throw new ArgumentException("Cannot change animation name to an empty string");
            }

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

            int newAnimationNameLocation;

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

            PsaFile.HelperMethods.UpdateMovesetHeaders(DataSectionLocation);
        }

        /// <summary>
        /// Removes animation data at a given location
        /// <para>this is generally used to save room in the file, e.g. if a subaction is not in use, there's no reason for it to have animation data</para>
        /// </summary>
        /// <param name="animationLocation">location of animation to remove</param>
        public void RemoveAnimationData(int animationLocation)
        {
            int animationSectionLocation = GetAnimationSectionLocation();
            int animationSectionEndLocation = PsaFile.DataSection[DataSectionLocation] / 4; // an2
            int animationNamePointerLocation = PsaFile.DataSection[animationLocation + 1]; // n
            int animationNameLocation = animationNamePointerLocation / 4; // i

            // if animationNamePointerLocation, animation name data already does not exist
            if (animationNamePointerLocation > 0)
            {
                PsaFile.DataSection[animationLocation] = 0;
                PsaFile.DataSection[animationLocation + 1] = 0;
                PsaFile.HelperMethods.RemoveOffsetFromOffsetInterlockTracker(animationLocation * 4 + 4);

                bool isStillInUse = IsAnimationNameInUse(animationNamePointerLocation);

                // if animationNamePointerLocation was not found in data section (nothing else is pointing to the same animation name)
                // replace name with free space
                if (!isStillInUse)
                {
                    // replace animation name with free space
                    if (animationNameLocation >= animationSectionLocation && animationNameLocation < animationSectionEndLocation)
                    {
                        ReplaceAnimationNameWithFreeSpace(animationNameLocation, 0, 13);
                    }
                    else
                    {
                        ReplaceAnimationNameWithFreeSpace(animationNameLocation, Constants.FADEF00D, 15);
                    }
                }
            }

            PsaFile.HelperMethods.UpdateMovesetHeaders(DataSectionLocation);
        }

        /// <summary>
        /// Modify animation flags for an animation
        /// <para>animation flags include "loop", "in transition", etc.</para>
        /// </summary>
        /// <param name="animationLocation">location of animation to modify the flags of</param>
        /// <param name="animationFlags">data to change the animation flags to</param>
        public void ModifyAnimationFlags(int animationLocation, AnimationFlags animationFlags)
        {
            // in transition
            int newAnimationFlagsValue = animationFlags.InTransition * 16777216;

            // no out transition
            if (animationFlags.NoOutTransition)
            {
                newAnimationFlagsValue += 65536;
            }
            // loop
            if (animationFlags.Loop)
            {
                newAnimationFlagsValue += 131072;
            }
            // moves character
            if (animationFlags.MovesCharacter)
            {
                newAnimationFlagsValue += 262144;
            }
            // unknown 3
            if (animationFlags.Unknown3)
            {
                newAnimationFlagsValue += 524288;
            }
            // unknown 4
            if (animationFlags.Unknown4)
            {
                newAnimationFlagsValue += 1048576;
            }
            // unknown 5
            if (animationFlags.Unknown5)
            {
                newAnimationFlagsValue += 2097152;
            }
            // transition out from start
            if (animationFlags.TransitionOutFromStart)
            {
                newAnimationFlagsValue += 4194304;
            }
            // unknown 7
            if (animationFlags.Unknown7)
            {
                newAnimationFlagsValue += 8388608;
            }
            PsaFile.DataSection[animationLocation] = newAnimationFlagsValue;
        }

        /// <summary>
        /// Searches through animation section to see if a given animation name already exists
        /// </summary>
        /// <param name="animationNameDoubleWords">animation name to search for (in double words format)</param>
        /// <returns>the location where the animation name was found, or -1 if not found</returns>
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

        /// <summary>
        /// Searches through file to see if a specified animation name is being used somewhere
        /// <para>strings are shared all over the place so you never know where an animation name could be used</para>
        /// </summary>
        /// <param name="animationNamePointerLocation">pointer to the animation name to check for</param>
        /// <returns>whether animation name is being used or not</returns>
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

        /// <summary>
        /// Given an animation name location, replace it with freespace
        /// <para>This method is used when removing an animation name</para>
        /// <para>I am still not 100% sure what is going on with the 0xFF mask but :shrug: I don't change things from PSA-C unless I am sure they are incorrect/unnecessary, so for now it stays</para>
        /// </summary>
        /// <param name="animationNameLocation">location of animation name</param>
        /// <param name="freeSpaceIndicator">which symbol to use as a free space marker (generally either 0 or FADEF00D depending on where in file animation name is located)</param>
        /// <param name="byteLimit">has to do with the weird 0xFF mask, not really sure if this is necessary or not but leaving it in for now to stay consistent with PSA-C -- this number is generally 13 if animation is in animation section and 15 if outside animation section</param>
        private void ReplaceAnimationNameWithFreeSpace(int animationNameLocation, int freeSpaceIndicator, int byteLimit)
        {
            while ((PsaFile.DataSection[animationNameLocation] & 0xFF) > byteLimit)
            {
                PsaFile.DataSection[animationNameLocation] = freeSpaceIndicator;
                animationNameLocation++;
            }
            PsaFile.DataSection[animationNameLocation] = freeSpaceIndicator;
        }

        /// <summary>
        /// Searches through file several times to find free space to add animation name to
        /// <para>It looks through the animation section, code block data section, and then the entire data section for free space</para>
        /// <para>It also does a check to see if animation name exists in data section already...after first looking through other free space...not quite sure why this is but I kept it from PSA-C to keep consistency until I am sure it is unnecessary/out of order like I think it is</para>
        /// <para>If there is no free space available after doing all of this checking, the animation name will just be appended on to the end of the data section</para>
        /// </summary>
        /// <param name="animationLocation">location of animation which will have its name inserted somewhere in the moveset file</param>
        /// <param name="animationNameDoubleWords">animation name (in double words format)</param>
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

                    else
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

        /// <summary>
        /// Searches animation section for a specific amount of free space
        /// <para>free space can be either 0 or FADEF00D</para>
        /// </summary>
        /// <param name="freeSpaceNeeded">amount of free space needed</param>
        /// <returns>location where enough free space was found, or -1 if not found</returns>
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

        /// <summary>
        /// Searches code block data section for a specific amount of free space
        /// <para>free space is FADEF00D</para>
        /// </summary>
        /// <param name="freeSpaceNeeded">amount of free space needed</param>
        /// <returns>location where enough free space was found, or -1 if not found</returns>
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

        /// <summary>
        /// Searches through data section to see if a given animation name already exists
        /// </summary>
        /// <param name="animationNameDoubleWords">animation name to search for (in double words format)</param>
        /// <returns>the location where the animation name was found, or -1 if not found</returns>
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

        /// <summary>
        /// Searches data section for a specific amount of free space
        /// <para>free space is FADEF00D</para>
        /// </summary>
        /// <param name="freeSpaceNeeded">amount of free space needed</param>
        /// <returns>location where enough free space was found, or -1 if not found</returns>
        public int FindFreeSpaceInDataSection(int freeSpaceNeeded)
        {
            for (int i = PsaFile.DataSection[DataSectionLocation] / 4 + numberOfSubActions * 2; i < PsaFile.DataSection.Count - freeSpaceNeeded; i++)
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

        /// <summary>
        /// Places animation name in the file at the specified location
        /// <para>also sets animation name pointer to the animation name</para>
        /// </summary>
        /// <param name="animationLocation">location of animation which will point to the animation name</param>
        /// <param name="newAnimationNameLocation">location to place the animation name in the file</param>
        /// <param name="animationNameDoubleWords">animation name (in double words format)</param>
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
