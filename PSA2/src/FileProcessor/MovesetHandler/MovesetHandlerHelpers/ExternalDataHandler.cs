using PSA2.src.Models.Fighter;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class ExternalDataHandler
    {
        public PsaFile PsaFile { get; private set; }

        public ExternalDataHandler(PsaFile psaFile)
        {
            PsaFile = psaFile;
        }

        public List<DataEntry> GetExternalDataEntries()
        {
            List<DataEntry> externalDataEntries = new List<DataEntry>();
            int dataElementNameEntriesStartLocation = PsaFile.ExternalDataSectionStartLocation + PsaFile.NumberOfExternalSubRoutines * 2;
            for (int i = 0; i < PsaFile.NumberOfExternalSubRoutines; i++)
            {
                int dataOffset = PsaFile.DataSection[PsaFile.ExternalDataSectionStartLocation + i * 2] / 4;
                int nameStringOffset = PsaFile.DataSection[PsaFile.ExternalDataSectionStartLocation + 1 + i * 2];
                int startBit = nameStringOffset;
                StringBuilder dataEntryName = new StringBuilder();
                while (true)
                {
                    string nextStringData = Utils.ConvertDoubleWordToString(PsaFile.DataSection[dataElementNameEntriesStartLocation + startBit / 4], startByte: startBit % 4);

                    if (nextStringData.Length != 0)
                    {
                        dataEntryName.Append(nextStringData);
                        startBit += nextStringData.Length;
                    }
                    else
                    {
                        break;
                    }
                }
                externalDataEntries.Add(new DataEntry(dataOffset, dataEntryName.ToString()));
            }

            return externalDataEntries;
        }

        public DataEntry GetExternalDataEntryByName(string externalDataEntryName)
        {
            int dataElementNameEntriesStartLocation = PsaFile.ExternalDataSectionStartLocation + PsaFile.NumberOfExternalSubRoutines * 2;
            for (int i = 0; i < PsaFile.NumberOfExternalSubRoutines; i++)
            {
                int dataLocation = PsaFile.DataSection[PsaFile.ExternalDataSectionStartLocation + i * 2] / 4;
                int nameStringOffset = PsaFile.DataSection[PsaFile.ExternalDataSectionStartLocation + 1 + i * 2];
                int startBit = nameStringOffset;
                StringBuilder dataEntryName = new StringBuilder();
                while (true)
                {
                    string nextStringData = Utils.ConvertDoubleWordToString(PsaFile.DataSection[dataElementNameEntriesStartLocation + startBit / 4], startByte: startBit % 4);

                    if (nextStringData.Length != 0)
                    {
                        dataEntryName.Append(nextStringData);
                        startBit += nextStringData.Length;
                    }
                    else
                    {
                        break;
                    }
                }
                if (dataEntryName.ToString() == externalDataEntryName)
                {
                    return new DataEntry(dataLocation, dataEntryName.ToString());
                }
            }

            throw new KeyNotFoundException(string.Format("Unable to find data entry in External Data with name {0}", externalDataEntryName));
        }

        public void PrintExternalDataEntries()
        {
            List<DataEntry> externalDataEntries = GetExternalDataEntries();
            foreach (DataEntry dte in externalDataEntries)
            {
                Console.WriteLine(string.Format("Name: {0}, Offset: {1}", dte.Name, dte.Location));
            }
        }
    }
}
