using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.Configs;
using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Attribute = PSA2MovesetLogic.src.Models.Fighter.Attribute;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers
{
    public class AttributesParser
    {
        public PsaFile PsaFile { get; private set; }
        const int TOTAL_NUMBER_OF_ATTRIBUTES = 185;
        public AttributeConfig AttributeConfig { get; private set; }

        public AttributesParser(PsaFile psaFile)
        {
            PsaFile = psaFile;
            AttributeConfig = Utils.LoadJson<AttributeConfig>("data/attribute_data.json");
        }

        public List<Attribute> GetAttributes()
        {
            List<Attribute> attributes = new List<Attribute>();

            for (int i = 0; i < TOTAL_NUMBER_OF_ATTRIBUTES; i++)
            {
                AttributeData attributeData = AttributeConfig.Attributes[i];
                attributes.Add(
                    new Attribute(
                        attributeData.Name,
                        attributeData.Description,
                        attributeData.Location,
                        attributeData.Type,
                        PsaFile.DataSection[i],
                        PsaFile.DataSection[i + TOTAL_NUMBER_OF_ATTRIBUTES]
                    )
                );
            }

            return attributes;
        }

        public Attribute GetAttributeByIndex(int attributeIndex)
        {
            if (attributeIndex >= 0 && attributeIndex < TOTAL_NUMBER_OF_ATTRIBUTES - 1)
            {
                AttributeConfig attributesConfig = Utils.LoadJson<AttributeConfig>("data/attribute_data.json");
                AttributeData attributeData = attributesConfig.Attributes[attributeIndex];
                return new Attribute(
                    attributeData.Name,
                    attributeData.Description,
                    attributeData.Location,
                    attributeData.Type,
                    PsaFile.DataSection[attributeIndex],
                    PsaFile.DataSection[attributeIndex + TOTAL_NUMBER_OF_ATTRIBUTES]
                );
            }
            else
            {
                throw new IndexOutOfRangeException($"Attribute Index {attributeIndex} does not exist -- must be between 0 and {TOTAL_NUMBER_OF_ATTRIBUTES - 1}");
            }
        }

        public void PrintAttributes()
        {
            List<Attribute> attributes = GetAttributes();
            foreach (Attribute attribute in attributes)
            {
                if (attribute.Type == "int")
                {
                    Console.WriteLine(string.Format("Name: {0}, Value: {1}, SSE Value: {2}", attribute.Name, attribute.Value, attribute.SseValue));
                }
                else
                {
                    Console.WriteLine(string.Format("Name: {0}, Value: {1}, SSE Value: {2}", attribute.Name, Utils.ConvertBytesToFloat(attribute.Value), Utils.ConvertBytesToFloat(attribute.SseValue)));
                }
            }
        }

        public void SetAttribute(int attributeIndex, int value, bool sse = false)
        {
            if (attributeIndex < TOTAL_NUMBER_OF_ATTRIBUTES)
            {
                int formatValue = AttributeConfig.Attributes[attributeIndex].Type == "float" ? Utils.ConvertIntToIeeFloatingPoint(value) : value;
                PsaFile.DataSection[attributeIndex + (sse ? 185 : 0)] = formatValue;
            }
            else
            {
                throw new IndexOutOfRangeException($"Invalid attribute index -- must be between 0 and {TOTAL_NUMBER_OF_ATTRIBUTES - 1}");
            }
        }
    }
}
