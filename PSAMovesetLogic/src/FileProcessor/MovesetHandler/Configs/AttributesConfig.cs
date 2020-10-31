using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.FileProcessor.MovesetHandler.Configs
{
    public class AttributeConfig
    {
        public List<AttributeData> Attributes { get; set; }

        public AttributeConfig()
        {
            Attributes = new List<AttributeData>();
        }
    }

    public class AttributeData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }

        public AttributeData(string name, string description, string location, string type)
        {
            Name = name;
            Description = description;
            Location = location;
            Type = type;
        }
    }
}
