using PSA2.src.utility;
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
        public int[] FileHeader { get; private set; }
        public int[] FileContent { get; set; }
        public int FileSize { get; set; }
        public int ExtraSpace { get; private set; } // efdts
        public int[] OffsetInterlockTracker { get; private set; } // asc
        public int CompressedSize { get; set; } // rnexsize
        public int[] CompressionTracker { get; private set; } = new int[2000]; // rnext

        /// <summary>
        /// Gets total size of Moveset File (bits)
        /// To get bytes, just divide this result by 4
        /// </summary>
        public int MovesetFileSize
        {
            get
            {
                return FileHeader[24];
            }
            set
            {
                FileHeader[24] = value;
            }
        }

        public int GetTotalFileSize
        {
            get
            {
                return FileSize + (FileHeader.Length * 4);
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
                return FileHeader[25];
            }
            set
            {
                FileHeader[25] = value;
            }
        }

        /// <summary>
        /// Size of data section in bytes
        /// </summary>
        public int DataSectionSizeBytes
        {
            get
            {
                return FileHeader[25] / 4;
            }
            set
            {
                FileHeader[25] = value * 4;
            }
        }



        /// <summary>
        /// Number of offset entries in Offsets Section
        /// Each Offset Entry is 4 bytes
        /// </summary>
        public int NumberOfOffsetEntries
        {
            get
            {
                return FileHeader[26];
            }
            set
            {
                FileHeader[26] = value;
            }
        }

        /// <summary>
        /// Number of data table entries in Data Table Section 
        /// Each Data Table entry is 8 bytes
        /// </summary>
        public int NumberOfDataTableEntries
        {
            get
            {
                return FileHeader[27];
            }
        }

        /// <summary>
        /// Number of external sub routine entries in External Data Section
        /// Each External Sub Routine entry is 8 bytes
        /// </summary>
        public int NumberOfExternalSubRoutines
        {
            get
            {
                return FileHeader[28];
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
                return DataSectionStartLocation + DataSectionSize / 4;
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
            FileHeader = fileHeader;
            FileContent = fileContent;
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

            for (int i = FileContent.Length - 1; fileSizeBytes >= movesetFileSizeBytes; i--)
            {
                FileContent[i] = FileContent[fileSizeBytes];
                fileSizeBytes--;
            }

            // this is for the pointer interlock tracker
            // which is variable "asc" in PSAC
            // not entirely sure how this all works but this is used to automatically update pointers when new commands are added
            // like updating a "goto" offset to still point to the same thing after a new command is added
            OffsetInterlockTracker = new int[37000];
            for (int i = 0; i < NumberOfOffsetEntries; i++)
            {
                OffsetInterlockTracker[i] = FileContent[(DataSectionSize / 4) + i];
            }
            for (int i = NumberOfOffsetEntries; i < OffsetInterlockTracker.Length; i++)
            {
                OffsetInterlockTracker[i] = 16777216; // hex is 100 0000, not sure the significance
            }

            // the rnexsize and rnext stuff I'm PRETTY sure has to do with compression, but I'm not positive
            // this code is found when reading in movesets right before the attribute reading starts
            // not sure if this code is right or not...
            int totalSize = ((DataSectionSize / 4) + NumberOfOffsetEntries);
            CompressedSize = (MovesetFileSize + 3) / 4 - totalSize - 8;
            for (int i = 0; i < CompressedSize; i++)
            {
                CompressionTracker[i] = FileContent[totalSize];
                totalSize++;
            }
            for (int i = CompressedSize; i < CompressionTracker.Length; i++)
            {
                CompressionTracker[i] = 0;
            }
        }

        // TODO: Bad name, this isn't quite accurate
        public bool IsValidDataSectionLocation(int location)
        {
            return FileContent[location] >= 8096 && FileContent[location] < DataSectionSize;
        }

        public void SaveFile(string path)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            catch (Exception e) { }
            FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write);
            for (int i = 0; i < 32; i++)
            {
                fileStream.WriteByte((byte)((FileHeader[i] >> 24) & 0xFF));
                fileStream.WriteByte((byte)((FileHeader[i] >> 16) & 0xFF));
                fileStream.WriteByte((byte)((FileHeader[i] >> 8) & 0xFF));
                fileStream.WriteByte((byte)(FileHeader[i] & 0xFF));
            }

            int fileSize = 0;
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
                fileStream.WriteByte((byte)((FileContent[i] >> 24) & 0xFF));
                fileStream.WriteByte((byte)((FileContent[i] >> 16) & 0xFF));
                fileStream.WriteByte((byte)((FileContent[i] >> 8) & 0xFF));
                fileStream.WriteByte((byte)(FileContent[i] & 0xFF));
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

            for (int i = FileContent.Length - 1 - ExtraSpace; i < FileContent.Length - 1; i++)
            {
                fileStream.WriteByte((byte)((FileContent[i] >> 24) & 0xFF));
                fileStream.WriteByte((byte)((FileContent[i] >> 16) & 0xFF));
                fileStream.WriteByte((byte)((FileContent[i] >> 8) & 0xFF));
                fileStream.WriteByte((byte)(FileContent[i] & 0xFF));
            }
            fileStream.Close();
        }

        public override string ToString()
        {
            return new StringBuilder()
                .Append("File Header:")
                .AppendLine(Utils.IntArrayToString(FileHeader))
                .Append("File Content:")
                .AppendLine(Utils.IntArrayToString(FileContent))
                .Append("File Size:")
                .AppendLine(FileSize.ToString())
                .ToString();
        }
    }
}
