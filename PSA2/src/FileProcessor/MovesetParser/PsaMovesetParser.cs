using Newtonsoft.Json;
using PSA2.src.models.fighter;
using Attribute = PSA2.src.models.fighter.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PSA2.src.utility;

namespace PSA2.src.FileProcessor.MovesetParser
{
    public class PsaMovesetParser
    {
        public PsaFile PsaFile { get; set; }
        public Fighter Fighter { get; set; }

        public PsaMovesetParser(PsaFile psaFile)
        {
            PsaFile = psaFile;
            Fighter = new Fighter();
            ParseFighter();
        }

        public void ParseFighter()
        {
            Prelim();
            //LoadAttributes();


        }

        private void Prelim()
        {
            string movesetName = GetMovesetName();
            (int dataSectionLocation, List<string> dataTableEntryNames, List<string> externalSubRoutineEntryNames) = GetDataTableAndExternalSubRoutineEntryNames();
        }

        private string GetMovesetName()
        {
            StringBuilder movesetName = new StringBuilder();
            int nameEndByteIndex = 4;
            while (true)
            {
                string nextStringData = Utils.ConvertWordToString(PsaFile.FileHeader[nameEndByteIndex]);
                movesetName.Append(nextStringData);
                if (nextStringData.Length == 4)
                {
                    nameEndByteIndex++;
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine(movesetName.ToString());
            return movesetName.ToString();
        }

        private (int, List<string>, List<string>) GetDataTableAndExternalSubRoutineEntryNames()
        {
            int numberOfBytesInDataSection = PsaFile.DataSectionSize / 4;
            int dataTableStartLocation = numberOfBytesInDataSection + PsaFile.NumberOfOffsetEntries - 2;
            int totalNumberOfDataElements = PsaFile.NumberOfDataTableElements + PsaFile.NumberOfExternalSubRoutines;
            int dataElementNameEntriesStartLocation = numberOfBytesInDataSection + PsaFile.NumberOfOffsetEntries + totalNumberOfDataElements * 2;
            List<string> dataTable = new List<string>();
            List<string> externalSubRoutines = new List<string>();
            int dataSectionLocation = 0;
            for (int i = 0; i < totalNumberOfDataElements; i++)
            {
                StringBuilder dataElementName = new StringBuilder();
                int currentDataTableEntryIndex = i + 1;
                int nextDataTableEntryLocation = dataTableStartLocation + (currentDataTableEntryIndex * 2);
                int nextDataElementNameLocation = PsaFile.FileContent[nextDataTableEntryLocation + 1];
                if (nextDataElementNameLocation >= 0)
                {
                    while (true)
                    {
                        int startByte = nextDataElementNameLocation % 4;
                        string nextStringData = Utils.ConvertWordToString(PsaFile.FileContent[dataElementNameEntriesStartLocation + nextDataElementNameLocation / 4], startByte: startByte);
                        if (nextStringData.Length != 0)
                        {
                            dataElementName.Append(nextStringData);
                            nextDataElementNameLocation += nextStringData.Length;
                        }
                        else
                        {
                            break;
                        }
                    }
                    
                    if (dataSectionLocation == 0 && dataElementName.ToString() == "data")
                    {
                        dataSectionLocation = PsaFile.FileContent[nextDataTableEntryLocation] / 4;
                    }
                    
                    if (i < PsaFile.NumberOfDataTableElements)
                    {
                        dataTable.Add(dataElementName.ToString());
                    }
                    else
                    {
                        externalSubRoutines.Add(dataElementName.ToString());
                    }
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine(dataSectionLocation);
            foreach (string dt in dataTable)
            {
                //Console.WriteLine(dt);
            }

            foreach (string es in externalSubRoutines)
            {
                //Console.WriteLine(es);
            }
            return (dataSectionLocation, dataTable, externalSubRoutines);
        }

        private void LoadAttributes()
        {
            const int TOTAL_NUMBER_OF_ATTRIBUTES = 185;
            AttributeConfig attributesConfig = JsonConvert.DeserializeObject<AttributeConfig>(File.ReadAllText("data/attribute_data.json"));
            List<Attribute> attributes = new List<Attribute>();

            for (int i = 0; i < TOTAL_NUMBER_OF_ATTRIBUTES; i++)
            {
                AttributeConfig.AttributeData attributeData = attributesConfig.Attributes[i];
                if (attributesConfig.Attributes[i].Type == "int")
                {
                    attributes.Add(new IntAttribute(attributeData.Name, attributeData.Description, attributeData.Location, PsaFile.FileContent[i], PsaFile.FileContent[i + TOTAL_NUMBER_OF_ATTRIBUTES]));
                }
                else
                {
                    attributes.Add(
                        new FloatAttribute(
                            attributeData.Name,
                            attributeData.Description,
                            attributeData.Location,
                            Utils.ConvertBytesToFloat(PsaFile.FileContent[i]),
                            Utils.ConvertBytesToFloat(PsaFile.FileContent[i + TOTAL_NUMBER_OF_ATTRIBUTES])));
                }
            }

            Fighter.Attributes = attributes;
            PrintFighterAttributes();
        }

        public void PrintFighterAttributes()
        {
            foreach (Attribute attribute in Fighter.Attributes)
            {
                if (attribute is IntAttribute)
                {
                    IntAttribute intAttribute = (IntAttribute)attribute;
                    Console.WriteLine(String.Format("Name: {0}, Value: {1}, SSE Value: {2}", intAttribute.Name, intAttribute.Value, intAttribute.SseValue));
                }
                else
                {
                    FloatAttribute floatAttribute = (FloatAttribute)attribute;
                    Console.WriteLine(String.Format("Name: {0}, Value: {1}, SSE Value: {2}", floatAttribute.Name, floatAttribute.Value, floatAttribute.SseValue));
                }
            }
        }

    }
}
