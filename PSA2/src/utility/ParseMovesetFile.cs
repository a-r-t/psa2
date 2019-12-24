using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PSA2.utility
{
    public class ParseMovesetFile
    {
        public void getAttributes(int[] fileContent)
        {
            MovesetAttributes movesetAttributes = JsonConvert.DeserializeObject<MovesetAttributes>(File.ReadAllText("data/attribute_data.json"));
            //Console.WriteLine(attributes.Attributes[0].Name);

            List<FighterAttribute> fighterAttributes = new List<FighterAttribute>();


            for (int i = 0; i < 185; i++)
            {
                if (movesetAttributes.Attributes[i].Type == "int")
                {
                    fighterAttributes.Add(new FighterIntAttribute(movesetAttributes.Attributes[i], fileContent[i], fileContent[i + 185]));
                }
                else
                {
                    byte[] attributeValueBytes = BitConverter.GetBytes(fileContent[i]);
                    float attributeValue = BitConverter.ToSingle(attributeValueBytes, 0);
                    byte[] attributeSSEValueBytes = BitConverter.GetBytes(fileContent[i + 185]);
                    float attributeSSEValue = BitConverter.ToSingle(attributeSSEValueBytes, 0);
                    fighterAttributes.Add(new FighterFloatAttribute(movesetAttributes.Attributes[i], attributeValue, attributeSSEValue));
                }
            }

            foreach (FighterAttribute fa in fighterAttributes)
            {
                if (fa is FighterIntAttribute)
                {

                    Console.WriteLine(String.Format("Name: {0}, Value: {1}, SSE Value: {2}", fa.Attribute.Name, ((FighterIntAttribute)fa).Value, ((FighterIntAttribute)fa).SSEValue));
                }
                else
                {
                    Console.WriteLine(String.Format("Name: {0}, Value: {1}, SSE Value: {2}", fa.Attribute.Name, ((FighterFloatAttribute)fa).Value, ((FighterFloatAttribute)fa).SSEValue));
                }
            }

        }

        class Attribute
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
            public string Type { get; set; }

            public Attribute(string name, string description, string location, string type)
            {
                Name = name;
                Description = description;
                Location = location;
                Type = type;
            }
        }

        class MovesetAttributes
        {
            public List<Attribute> Attributes { get; set; }

            public MovesetAttributes()
            {
                Attributes = new List<Attribute>();
            }
        }

        class FighterAttribute
        {
            public Attribute Attribute { get; set; }

            public FighterAttribute(Attribute attribute)
            {
                Attribute = attribute;
            }
        }

        class FighterIntAttribute : FighterAttribute
        {
            public int Value { get; set; }
            public int SSEValue { get; set; }

            public FighterIntAttribute(Attribute attribute, int value, int sseValue) : base(attribute)
            {
                Value = value;
                SSEValue = sseValue;
            }
        }

        class FighterFloatAttribute : FighterAttribute
        {
            public float Value { get; set; }
            public float SSEValue { get; set; }

            public FighterFloatAttribute(Attribute attribute, float value, float sseValue) : base(attribute)
            {
                Value = value;
                SSEValue = sseValue;
            }
        }
    }


}
