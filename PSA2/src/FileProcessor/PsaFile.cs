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

        public int DataSectionSize
        {
            get
            {
                return FileHeader[25];
            }
        }

        /// <summary>
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
        /// Each External Data Table entry is 8 bytes
        /// </summary>
        public int NumberOfDataTableElements
        {
            get
            {
                return FileHeader[27];
            }
        }

        /// <summary>
        /// Each External Sub Routine entry is 8 bytes
        /// </summary>
        public int NumberOfExternalSubRoutines
        {
            get
            {
                return FileHeader[28];
            }
        }

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
                .AppendLine(Utils.intArrayToString(FileHeader))
                .Append("File Content:")
                .AppendLine(Utils.intArrayToString(FileContent))
                .Append("File Size:")
                .AppendLine(FileSize.ToString())
                .ToString();
        }
    }
}
