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
            int md = PsaFile.FileHeader[25] / 4;
            int par = PsaFile.FileHeader[26];
            int k = PsaFile.FileHeader[27] + PsaFile.FileHeader[28];
            int dat = 0;
            int i = md + par + k * 2;
            string movesetName = GetMovesetName();

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
