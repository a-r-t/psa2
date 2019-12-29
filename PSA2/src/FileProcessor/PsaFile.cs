using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor
{
    public class PsaFile
    {
        public int[] FileHeader { get; private set; }
        public int[] FileContent { get; private set; }
        public int FileSize { get; private set; }

        /// <summary>
        /// Size of data section (bits)
        /// To get bytes, just divide this result by 4
        /// </summary>
        public int DataSectionSize
        {
            get
            {
                return FileHeader[25];
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
