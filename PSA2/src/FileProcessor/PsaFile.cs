using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.FileProcessor
{
    public class PsaFile
    {
        public int[] HeaderSection { get; private set; }
        public int[] DataSection { get; set; }
        public int FileSize { get; set; }
        public int ExtraSpace { get; private set; } // efdts
        public List<int> OffsetSection { get; private set; } // asc
        //public int FileOtherDataSize { get; set; } // rnexsize
        public List<int> RemainingSections { get; private set; } // rnext

        /// <summary>
        /// Gets total size of Moveset File (bits)
        /// To get bytes, just divide this result by 4
        /// </summary>
        public int MovesetFileSize
        {
            get
            {
                return HeaderSection[24];
            }
            set
            {
                HeaderSection[24] = value;
            }
        }

        public int GetTotalFileSize
        {
            get
            {
                return FileSize + (HeaderSection.Length * 4);
            }
        }

        /// <summary>
        /// Size of data section (doubleword)
        /// To get bytes, just divide this result by 4 (doubleword = 4 bytes = 32 bits)
        /// </summary>
        public int DataSectionSize
        {
            get
            {
                return HeaderSection[25];
            }
            set
            {
                HeaderSection[25] = value;
            }
        }

        /// <summary>
        /// Size of data section in bytes
        /// </summary>
        public int DataSectionSizeBytes
        {
            get
            {
                return HeaderSection[25] / 4;
            }
            set
            {
                HeaderSection[25] = value * 4;
            }
        }

        /// <summary>
        /// Number of offset entries in Offsets Section
        /// Each Offset Entry is one doubleword (4 bytes)
        /// </summary>
        public int NumberOfOffsetEntries
        {
            get
            {
                return HeaderSection[26];
            }
            set
            {
                HeaderSection[26] = value;
            }
        }

        /// <summary>
        /// Number of data table entries in Data Table Section 
        /// <para>Each Data Table entry is two doublewords (8 bytes)</para>
        /// <para>First doubleword is the offset, second doubleword is the name of the section (string)</para>
        /// </summary>
        public int NumberOfDataTableEntries
        {
            get
            {
                return HeaderSection[27];
            }
        }

        /// <summary>
        /// Number of external sub routine entries in External Data Section
        /// <para>Each External Sub Routine entry is two doublewords (8 bytes)</para>
        /// <para>First doubleword is the offset, second doubleword is the name of the section (string)</para>
        /// </summary>
        public int NumberOfExternalSubRoutines
        {
            get
            {
                return HeaderSection[28];
            }
        }

        /*
         * Section Locations
         * http://opensa.dantarion.com/wiki/Moveset_File_Format_(Brawl)
         */
        /// <summary>
        /// Location where Data Section starts in File Content
        /// </summary>
        public int DataSectionStartLocation
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// Location where Offsets Section starts in File Content
        /// </summary>
        public int OffsetsSectionStartLocation
        {
            get
            {
                return DataSectionStartLocation + DataSectionSizeBytes;
            }
        }

        /// <summary>
        /// Location where Data Table Section starts in File Content
        /// </summary>
        public int DataTableSectionStartLocation
        {
            get
            {
                return OffsetsSectionStartLocation + NumberOfOffsetEntries;
            }
        }

        /// <summary>
        /// Location where External Data Section starts in File Content
        /// </summary>
        public int ExternalDataSectionStartLocation
        {
            get
            {
                return DataTableSectionStartLocation + NumberOfDataTableEntries * 2;
            }
        }
        /*
         * End Section Locations
         */

        public PsaFile(int[] fileHeader, int[] fileContent, int fileSize)
        {
            HeaderSection = fileHeader;
            DataSection = fileContent;
            FileSize = fileSize;

            // if file has "extra space", account for it...not quite sure what this does exactly yet, but it's the efdts variable in PSAC, and it comes back again when saving the file
            int movesetFileSizeBytes;
            if (MovesetFileSize % 4 == 0)
            {
                movesetFileSizeBytes = MovesetFileSize / 4;
            }
            else
            {
                movesetFileSizeBytes = (MovesetFileSize + 3) / 4;
            }
            if (movesetFileSizeBytes % 8 != 0)
            {
                int leftoverBytes = 8 - movesetFileSizeBytes % 8;
                movesetFileSizeBytes += leftoverBytes;
            }
            movesetFileSizeBytes -= 8;
            int fileSizeBytes = fileSize / 4;
            ExtraSpace = fileSizeBytes - movesetFileSizeBytes;

            for (int i = DataSection.Length - 1; fileSizeBytes >= movesetFileSizeBytes; i--)
            {
                DataSection[i] = DataSection[fileSizeBytes];
                fileSizeBytes--;
            }

            // this is for the offset interlock tracker
            // which is variable "asc" in PSAC
            // not entirely sure how this all works but this is used to automatically update pointers when new commands are added
            // like updating a "goto" offset to still point to the same thing after a new command is added
            OffsetSection = new List<int>();
            for (int i = 0; i < NumberOfOffsetEntries; i++)
            {
                OffsetSection.Add(DataSection[(DataSectionSize / 4) + i]);
            }
 
            // store anything else in the moveset file (after the data section and offset section) into another array
            // since the data section can be incrased at any time, this holds on to that data for later
            int dataAndOffsetCombinedSize = DataSectionSizeBytes + NumberOfOffsetEntries; // calculate size of data section and offset section combined
            int fileOtherDataSize = (MovesetFileSize + 3) / 4 - dataAndOffsetCombinedSize - 8; // calculate size of remaining file

            // place all "other data" into this FileOtherData array to hold on to
            RemainingSections = new List<int>();
            for (int i = 0; i < fileOtherDataSize; i++)
            {
                RemainingSections.Add(DataSection[dataAndOffsetCombinedSize + i]);
            }
        }

        // TODO: Bad name, this isn't quite accurate
        public bool IsValidDataSectionLocation(int location)
        {
            return DataSection[location] >= 8096 && DataSection[location] < DataSectionSize;
        }

        public void SaveFile(string path)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            catch (Exception e) 
            {
                Console.WriteLine(e.Message);
            }

            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            for (int i = 0; i < 32; i++)
            {
                fileStream.WriteByte((byte)((HeaderSection[i] >> 24) & 0xFF));
                fileStream.WriteByte((byte)((HeaderSection[i] >> 16) & 0xFF));
                fileStream.WriteByte((byte)((HeaderSection[i] >> 8) & 0xFF));
                fileStream.WriteByte((byte)(HeaderSection[i] & 0xFF));
            }

            int fileSize;
            if (MovesetFileSize % 4 == 0)
            {
                fileSize = MovesetFileSize / 4;
            }
            else
            {
                fileSize = (MovesetFileSize + 3) / 4;
            }
            fileSize -= 8;
            for (int i = 0; i < fileSize; i++)
            {
                fileStream.WriteByte((byte)((DataSection[i] >> 24) & 0xFF));
                fileStream.WriteByte((byte)((DataSection[i] >> 16) & 0xFF));
                fileStream.WriteByte((byte)((DataSection[i] >> 8) & 0xFF));
                fileStream.WriteByte((byte)(DataSection[i] & 0xFF));
            }

            if (fileSize % 8 != 0)
            {
                int newFileSize = 8 - fileSize % 8;
                for (int i = 0; i < newFileSize; i++)
                {
                    fileStream.WriteByte(0);
                    fileStream.WriteByte(0);
                    fileStream.WriteByte(0);
                    fileStream.WriteByte(0);
                }
            }

            for (int i = DataSection.Length - 1 - ExtraSpace; i < DataSection.Length - 1; i++)
            {
                fileStream.WriteByte((byte)((DataSection[i] >> 24) & 0xFF));
                fileStream.WriteByte((byte)((DataSection[i] >> 16) & 0xFF));
                fileStream.WriteByte((byte)((DataSection[i] >> 8) & 0xFF));
                fileStream.WriteByte((byte)(DataSection[i] & 0xFF));
            }
            fileStream.Close();
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("File Header:")
                .AppendLine(Utils.IntArrayToString(HeaderSection))
                .Append("File Content:")
                .AppendLine(Utils.IntArrayToString(DataSection))
                .Append("File Size:")
                .AppendLine(FileSize.ToString())
                .ToString();
        }
    }
}
