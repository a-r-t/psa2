using PSA2.src.Models.Fighter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSA2.src.utility;
using PSA2.src.FileProcessor.MovesetHandler;

namespace PSA2.src.FileProcessor
{
    public class PsaFileParser
    {
        public string FilePath { get; set; }
        public PsaFile PsaFile { get; private set; }

        public PsaFileParser(string filePath)
        {
            FilePath = filePath;
            (int[] fileHeader, int[] fileContent, int fileSize) = ReadFile(FilePath);
            PsaFile = new PsaFile(fileHeader, fileContent, fileSize);
        }

        /// <summary>
        /// Gets file header, file content, and file size from moveset file
        /// </summary>
        /// <param name="FileName">the moveset file to parse</param>
        /// <returns>Tuple: file header (int array 32), file content (int array 148449), file size (int)</returns>        
        private (int[], int[], int) ReadFile(string fileName)
        {
            try
            {
                FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                // measured in bytes (divide by 10 to get kb)
                int minFileSize = 50000;
                int maxFileSize = 593920;
                //int maxFileSize = 7000000;
                if (fileStream.Length >= minFileSize && fileStream.Length <= maxFileSize)
                {
                    int fileId = Utils.ConvertDoubleWordToBase10Int(
                        fileStream.ReadByte(),
                        fileStream.ReadByte(),
                        fileStream.ReadByte(),
                        fileStream.ReadByte()
                    );
                    if (fileId != 1095910144)
                    {
                        throw new NotSupportedException("File is not a valid PAC file (moveset, fighter.pac, etc)");
                    }
                    else
                    {
                        int[] fileHeader = GetFileHeader(fileStream, fileId);
                        (int[] fileContent, int fileSize) = GetFileContent(fileStream);
                        fileStream.Close();
                        return (fileHeader, fileContent, fileSize);
                    }
                }
                else
                {
                    throw new NotSupportedException("File is not within allowed size range of 50kb to 593.92kb");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("Unable to parse PAC file: %s", e.Message));
                Console.WriteLine(e.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Gets file header from moveset file
        /// file header = first 32 words in moveset file converted to base 10 int
        /// </summary>
        /// <param name="fileStream">the file stream object</param>
        /// <param name="fileId">first word in moveset file converted to base 10 int</param>
        /// <returns>file header (array of size 32) </returns>
        private int[] GetFileHeader(FileStream fileStream, int fileId)
        {
            int[] header = new int[32];
            header[0] = fileId;
            for (int i = 1; i < 32; i++)
            {
                header[i] = Utils.ConvertDoubleWordToBase10Int(
                    fileStream.ReadByte(),
                    fileStream.ReadByte(),
                    fileStream.ReadByte(),
                    fileStream.ReadByte()
                );
            }
            return header;
        }

        /// <summary>
        /// Gets file content from moveset file
        /// file content = all words after initial 32 words in moveset file converted to base 10 int
        /// </summary>
        /// <param name="fileStream">the file stream object</param>
        /// <returns>tuple: file content (array of size 148449), file size in kb (basically number of indexes used by the moveset file in the file content array)</returns>
        private (int[], int) GetFileContent(FileStream fileStream)
        {
            int[] fileContent = new int[148449];
            int currentIndex = 0;
            int byte1;
            while ((byte1 = fileStream.ReadByte()) != -1)
            {
                int byte2 = fileStream.ReadByte();
                int byte3 = fileStream.ReadByte();
                int byte4 = fileStream.ReadByte();
                if (byte2 != -1 && byte3 != -1 && byte4 != -1)
                {
                    fileContent[currentIndex] = Utils.ConvertDoubleWordToBase10Int(byte1, byte2, byte3, byte4);
                    currentIndex++;
                }
            }
            fileStream.Close();
            int fileSize = currentIndex * 4;
            return (fileContent, fileSize);
        }

        public PsaMovesetHandler ParseMovesetFile()
        {
            return new PsaMovesetHandler(PsaFile);
        }
    }
}
