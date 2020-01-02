using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class ActionsParser
    {
        public PsaFile PsaFile { get; private set; }
        public int DataSectionLocation { get; private set; }
        public PsaCommandsConfig PsaCommandsConfig { get; private set; }

        public ActionsParser(PsaFile psaFile, int dataSectionLocation)
        {
            PsaFile = psaFile;
            DataSectionLocation = dataSectionLocation;
            PsaCommandsConfig = Utils.LoadJson<PsaCommandsConfig>("data/psa_command_data.json");
        }

        public int GetNumberOfSpecialActions()
        {
            Console.WriteLine(String.Format("Number of Special Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 10] - PsaFile.FileContent[DataSectionLocation + 9]) / 4;
        }

        public int GetNumberOfSubActions()
        {
            Console.WriteLine(String.Format("Number of Sub Actions: {0}", (PsaFile.FileContent[DataSectionLocation + 13] - PsaFile.FileContent[DataSectionLocation + 12]) / 4));
            return (PsaFile.FileContent[DataSectionLocation + 13] - PsaFile.FileContent[DataSectionLocation + 12]) / 4;
        }

        
    }
}
