using PSA2.src.FileProcessor.MovesetParser.Configs;
using PSA2.src.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = PSA2.src.models.fighter.Attribute;

namespace PSA2.src.FileProcessor.MovesetParser.MovesetParserHelpers
{
    public class AttributesParser
    {
        public PsaFile PsaFile { get; private set; }
        const int TOTAL_NUMBER_OF_ATTRIBUTES = 185;

        public AttributesParser(PsaFile psaFile)
        {
            PsaFile = psaFile;
        }

        public List<Attribute> GetAttributes()
        {
            AttributeConfig attributesConfig = Utils.LoadJson<AttributeConfig>("data/attribute_data.json");
            List<Attribute> attributes = new List<Attribute>();

            for (int i = 0; i < TOTAL_NUMBER_OF_ATTRIBUTES; i++)
            {
                AttributeData attributeData = attributesConfig.Attributes[i];
                attributes.Add(
                    new Attribute(
                        attributeData.Name, 
                        attributeData.Description,
                        attributeData.Location, 
                        attributeData.Type, 
                        PsaFile.FileContent[i], 
                        PsaFile.FileContent[i + TOTAL_NUMBER_OF_ATTRIBUTES]
                    )
                );
            }

            return attributes;
        }

        public Attribute GetAttributeByIndex(int attributeIndex)
        {
            if (attributeIndex > 0 && attributeIndex < TOTAL_NUMBER_OF_ATTRIBUTES - 1)
            {
                AttributeConfig attributesConfig = Utils.LoadJson<AttributeConfig>("data/attribute_data.json");
                AttributeData attributeData = attributesConfig.Attributes[attributeIndex];
                return new Attribute(
                    attributeData.Name,
                    attributeData.Description,
                    attributeData.Location,
                    attributeData.Type,
                    PsaFile.FileContent[attributeIndex],
                    PsaFile.FileContent[attributeIndex + TOTAL_NUMBER_OF_ATTRIBUTES]
                );
            }
            else
            {
                throw new IndexOutOfRangeException(String.Format("Attribute Index {0} does not exist -- must be between 0 and {1}", attributeIndex, TOTAL_NUMBER_OF_ATTRIBUTES));
            }
        }



        public void PrintAttributes()
        {
            List<Attribute> attributes = GetAttributes();
            foreach (Attribute attribute in attributes)
            {
                if (attribute.Type == "int")
                {
                    Console.WriteLine(String.Format("Name: {0}, Value: {1}, SSE Value: {2}", attribute.Name, attribute.Value, attribute.SseValue));
                }
                else
                {
                    Console.WriteLine(String.Format("Name: {0}, Value: {1}, SSE Value: {2}", attribute.Name, Utils.ConvertBytesToFloat(attribute.Value), Utils.ConvertBytesToFloat(attribute.SseValue)));
                }
            }
        }
    }
}
