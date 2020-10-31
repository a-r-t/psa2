using PSA2MovesetLogic.src.ExtentionMethods;
using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor
{
    /// <summary>
    /// Common methods used throughout the program that use/maniuplate the PsaFile object are placed here
    /// <para>Psa command manipulation (adding, removing, modifying, etc) use methods in this class a lot</para>
    /// </summary>
    public class PsaFileHelperMethods
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }

        public PsaFileHelperMethods(PsaFile psaFile, int dataSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
        }

        /// <summary>
        /// This basically rewrites the PSA File to account for data section size changes, offset interlock changes, and external subroutine changes after PSA commands are manipulated in a code block
        /// <para>Some header values are updated as well to reflect the current size of the data section, etc</para>
        /// <para>Fixam method in PSA-C</para>
        /// </summary>
        public void ApplyHeaderUpdatesToAccountForPsaCommandChanges()
        {
            // remove any free space (FADEF00D) trailing at the end of the data section to save some space
            for (int i = PsaFile.DataSection.Count - 1; i >= DataSectionLocation; i--)
            {
                if (PsaFile.DataSection[i] == Constants.FADEF00D)
                {
                    PsaFile.DataSection.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }

            // if FADE0D8A is in moveset file, it was used to mark the end of the data section
            // it's not required, but just to keep things consistent with PSA-C and the old PSA, this checks the moveset to see if FADE0D8A is present
            // if it is present, it removes it from its location and places it at the end of the data section
            int dataSectionEndNotatorIndex = PsaFile.DataSection.FindIndex(data => data == Constants.FADE0D8A);
            if (dataSectionEndNotatorIndex >= 0)
            {
                PsaFile.DataSection.RemoveAt(dataSectionEndNotatorIndex);
                PsaFile.DataSection.Add(Constants.FADE0D8A);
            }

            // change size of data section to match new size, which could have changed such as if a new command was added
            PsaFile.DataSectionSize = PsaFile.DataSection.Count * 4;

            // sort offsets in the tracker, because they need to be placed into the psa file in order
            PsaFile.OffsetSection.Sort();

            PsaFile.NumberOfOffsetEntries = PsaFile.OffsetSection.Count;

            // this calculates the new MovesetFileSize
            int movesetFileSizeLeftoverSpace = PsaFile.MovesetFileSize % 4;
            if (movesetFileSizeLeftoverSpace == 0)
            {
                movesetFileSizeLeftoverSpace = 4;
            }

            // update header MovesetFileSize
            int movesetFileSizeBytes = PsaFile.DataSection.Count + PsaFile.OffsetSection.Count + PsaFile.DataTableSections.Count;
            PsaFile.MovesetFileSize = (movesetFileSizeBytes * 4) + movesetFileSizeLeftoverSpace + 28;
            
            // I guess this header location also needs to equal the MovesetFileSize :shrug:
            PsaFile.HeaderSection[17] = PsaFile.MovesetFileSize;
        }

        /// <summary>
        /// Searches through data section for the desired amount of free space (FADEF00D)
        /// </summary>
        /// <param name="amountOfFreeSpace">amount of free space desired (as doubleword, e.g. 4 would look for 4 doublewords)</param>
        /// <returns>starting location where the desired amount of free space has been found</returns>
        public int FindLocationWithAmountOfFreeSpace(int startLocation, int amountOfFreeSpace)
        {
            int freeSpaceFound = 0;
            for (int i = startLocation; i < PsaFile.DataSection.Count; i++)
            {
                if (PsaFile.DataSection[i] == Constants.FADEF00D)
                {
                    freeSpaceFound++;
                }
                else
                {
                    freeSpaceFound = 0;
                }
                
                if (freeSpaceFound == amountOfFreeSpace)
                {
                    return i + 1 - amountOfFreeSpace;
                }
            }
            return PsaFile.DataSection.Count;
        }

        /// <summary>
        /// This will remove an offset location from the tracker that is no longer being pointed to by a command
        /// <para>Delasc method in PSA-C</para>
        /// </summary>
        /// <param name="locationToRemove">The offset to remove from the tracker</param>
        /// <returns>Whether the offset was found and removed or not</returns>
        public bool RemoveOffsetFromOffsetInterlockTracker(int locationToRemove)
        {
            for (int i = 0; i < PsaFile.OffsetSection.Count; i++)
            {
                if (PsaFile.OffsetSection[i] == locationToRemove)
                {
                    PsaFile.OffsetSection.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets a location in the Data Section to a specified value
        /// <para>If the specified location is larger than the amount of space already in the Data Section, the value is appended on to the end of the Data Section instead</para>
        /// </summary>
        /// <param name="location">Location (index) in Data Section to set</param>
        /// <param name="value">Value to set the Data Section to at the specified location</param>
        public void SetDataSectionValue(int location, int value)
        {
            if (location < PsaFile.DataSection.Count)
            {
                PsaFile.DataSection[location] = value;
            }
            else
            {
                PsaFile.DataSection.Add(value);
            }
        }
    }
}
