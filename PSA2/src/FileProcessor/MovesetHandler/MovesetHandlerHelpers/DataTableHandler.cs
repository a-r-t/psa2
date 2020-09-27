using PSA2.src.Models.Fighter;
using PSA2.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class DataTableHandler
    {
        public PsaFile PsaFile { get; private set; }

        public DataTableHandler(PsaFile psaFile)
        {
            PsaFile = psaFile;
        }

        public List<DataEntry> GetDataTableEntries()
        {
            List<DataEntry> dataTableEntries = new List<DataEntry>();
            int dataElementNameEntriesStartLocation = PsaFile.ExternalDataSectionStartLocation + PsaFile.NumberOfExternalSubRoutines * 2;
            for (int i = 0; i < PsaFile.NumberOfDataTableEntries; i++)
            {
                int dataOffset = PsaFile.FileContent[PsaFile.DataTableSectionStartLocation + i * 2] / 4;
                int nameStringOffset = PsaFile.FileContent[PsaFile.DataTableSectionStartLocation + 1 + i * 2];
                int startBit = nameStringOffset;
                StringBuilder dataEntryName = new StringBuilder();
                while (true)
                {
                    string nextStringData = Utils.ConvertDoubleWordToString(PsaFile.FileContent[dataElementNameEntriesStartLocation + startBit / 4], startByte: startBit % 4);

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
                dataTableEntries.Add(new DataEntry(dataOffset, dataEntryName.ToString()));
            }

            return dataTableEntries;
        }

        public DataEntry GetDataTableEntryByName(string dataTableEntryName)
        {
            int dataElementNameEntriesStartLocation = PsaFile.ExternalDataSectionStartLocation + PsaFile.NumberOfExternalSubRoutines * 2;
            for (int i = 0; i < PsaFile.NumberOfDataTableEntries; i++)
            {
                int dataLocation = PsaFile.FileContent[PsaFile.DataTableSectionStartLocation + i * 2] / 4;
                int nameStringOffset = PsaFile.FileContent[PsaFile.DataTableSectionStartLocation + 1 + i * 2];
                int startBit = nameStringOffset;
                StringBuilder dataEntryName = new StringBuilder();
                while (true)
                {
                    string nextStringData = Utils.ConvertDoubleWordToString(PsaFile.FileContent[dataElementNameEntriesStartLocation + startBit / 4], startByte: startBit % 4);

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
                if (dataEntryName.ToString() == dataTableEntryName)
                {
                    return new DataEntry(dataLocation, dataEntryName.ToString());
                }
            }
            throw new KeyNotFoundException(string.Format("Unable to find data entry in Data Table with name {0}", dataTableEntryName));
        }

        public void PrintDataTableEntries()
        {
            List<DataEntry> dataTableEntries = GetDataTableEntries();
            foreach (DataEntry dte in dataTableEntries)
            {
                Console.WriteLine(string.Format("Name: {0}, Location: {1}", dte.Name, dte.Location));
            }
        }
    }
}
