using Newtonsoft.Json;
using PSA2.src.models.fighter;
using Attribute = PSA2.src.models.fighter.Attribute;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PSA2.src.FileProcessor.MovesetParser.AttributeConfig;

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
            LoadAttributes();


        }

        private void LoadAttributes()
        {
            const int TOTAL_NUMBER_OF_ATTRIBUTES = 185;
            AttributeConfig attributesConfig = JsonConvert.DeserializeObject<AttributeConfig>(File.ReadAllText("data/attribute_data.json"));
            List<Attribute> attributes = new List<Attribute>();

            for (int i = 0; i < TOTAL_NUMBER_OF_ATTRIBUTES; i++)
            {
                AttributeData attributeData = attributesConfig.Attributes[i];
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
                            convertBytesToFloat(PsaFile.FileContent[i]), 
                            convertBytesToFloat(PsaFile.FileContent[i + TOTAL_NUMBER_OF_ATTRIBUTES])));
                }
            }

            Fighter.Attributes = attributes;

/*            foreach (Attribute attribute in Fighter.Attributes)
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
            }*/
        }

        public float convertBytesToFloat(int value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            return BitConverter.ToSingle(valueBytes, 0);
        }

    }
}
