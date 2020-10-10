using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor
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
            // gets last valid value of data section
            int lastDataSectionValidValue = PsaFile.DataSection[PsaFile.DataSectionSizeBytes - 2] == Constants.FADE0D8A
                ? PsaFile.DataSectionSizeBytes - 3
                : PsaFile.DataSectionSizeBytes - 1;

            // decrease last data section value if there is free space (FADEF00Ds) at the end of the data section until it reaches a non free space value
            while (lastDataSectionValidValue >= DataSectionLocation && PsaFile.DataSection[lastDataSectionValidValue] == Constants.FADEF00D)
            {
                lastDataSectionValidValue--;
            }

            // I believe this "closes the gap" of free space that was found beforehand to shrink the overall size of the file by a bit
            int currentFileSizeBytes = lastDataSectionValidValue;
            while (lastDataSectionValidValue < PsaFile.DataSectionSizeBytes)
            {
                if (PsaFile.DataSection[lastDataSectionValidValue] != Constants.FADEF00D)
                {
                    PsaFile.DataSection[currentFileSizeBytes] = PsaFile.DataSection[lastDataSectionValidValue];
                    currentFileSizeBytes++;
                }
                lastDataSectionValidValue++;
            }

            // change size of data section to match new size, which did change since a new command was added
            PsaFile.DataSectionSize = currentFileSizeBytes * 4;

            // sort offsets in the tracker, because they need to be placed into the psa file in order
            PsaFile.OffsetSection.Sort();

            PsaFile.NumberOfOffsetEntries = PsaFile.OffsetSection.Count;

            // this calculates the new MovesetFileSize
            int movesetFileSizeLeftoverSpace = PsaFile.MovesetFileSize % 4;
            if (movesetFileSizeLeftoverSpace == 0)
            {
                movesetFileSizeLeftoverSpace = 4;
            }

            PsaFile.MovesetFileSize = ((currentFileSizeBytes + PsaFile.OffsetSection.Count + PsaFile.RemainingSections.Count) * 4) + movesetFileSizeLeftoverSpace + 28;
            
            //Console.WriteLine((PsaFile.MovesetFileSize / 4));
            
            // I guess this header location also needs to equal the MovesetFileSize :shrug:
            PsaFile.HeaderSection[17] = PsaFile.MovesetFileSize;

            // this checks if moveset is now over 544kb, a limitation of PSA-C that I will later remove
            int newMovesetFileSizeBytes = (PsaFile.MovesetFileSize + 3) / 4;
            if (newMovesetFileSizeBytes % 8 != 0)
            {
                currentFileSizeBytes = 8 - newMovesetFileSizeBytes % 8;
                newMovesetFileSizeBytes += currentFileSizeBytes;
            }
            //Console.WriteLine("NMFSB: " + newMovesetFileSizeBytes);

            newMovesetFileSizeBytes += PsaFile.ExtraSpace - 8;
            if (newMovesetFileSizeBytes > 139264)
            {
                Console.WriteLine("Current data size over 544kb");
            }
        }

        /// <summary>
        /// Searches through data section for the desired amount of free space
        /// </summary>
        /// <param name="amountOfFreeSpace">amount of free space desired (as doubleword, e.g. 4 would look for 4 doublewords)</param>
        /// <returns>starting location where the desired amount of free space has been found</returns>
        public int FindLocationWithAmountOfFreeSpace(int startLocation, int amountOfFreeSpace)
        {
            int stoppingPoint = startLocation;

            while (stoppingPoint < PsaFile.DataSectionSizeBytes)
            {
                if (PsaFile.DataSection[stoppingPoint] == Constants.FADEF00D)
                {
                    bool hasEnoughSpace = true;
                    for (int i = 0; i < amountOfFreeSpace; i++)
                    {
                        if (PsaFile.DataSection[stoppingPoint + 1 + i] != Constants.FADEF00D)
                        {
                            hasEnoughSpace = false;
                            break;
                        }
                    }
                    if (hasEnoughSpace)
                    {
                        return stoppingPoint;
                    }
                }
                stoppingPoint++;
            }
            return stoppingPoint;
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
    }
}
