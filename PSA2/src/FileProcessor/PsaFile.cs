using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils = utility.UtilityMethods.Utility;

namespace PSA2.src.FileProcessor
{
    public class PsaFile
    {
        public int[] FileHeader { get; private set; }
        public int[] FileContent { get; private set; }
        public int FileSize { get; private set; }

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
