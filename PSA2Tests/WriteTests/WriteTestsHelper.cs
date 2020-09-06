using PSA2.src.FileProcessor;
using PSA2.src.FileProcessor.MovesetParser;
using PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PSA2Tests
{
    public class WriteTestsHelper
    {
        public static PsaMovesetParser GetPsaMovesetParser(string movesetFilePath)
        {
            PsaFileParser psaFileParser = new PsaFileParser(movesetFilePath);
            return new PsaMovesetParser(psaFileParser.PsaFile);
        }

        public static bool AreFilesIdentical(string path1, string path2)
        {
            return AreFilesIdentical(new FileInfo(path1), new FileInfo(path2));
        }

        public static bool AreFilesIdentical(FileInfo first, FileInfo second)
        {
            if (first.Length != second.Length)
                return false;

            if (string.Equals(first.FullName, second.FullName, StringComparison.OrdinalIgnoreCase))
                return true;

            using (FileStream fs1 = first.OpenRead())
            using (FileStream fs2 = second.OpenRead())
            {
                for (int i = 0; i < first.Length; i++)
                {
                    if (fs1.ReadByte() != fs2.ReadByte())
                        return false;
                }
            }

            return true;
        }

    }
}
