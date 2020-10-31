using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter
{
    public class Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public int Value { get; set; }
        public int SseValue { get; set; }

        public Attribute(string name, string description, string location, string type, int value, int sseValue)
        {
            Name = name;
            Description = description;
            Location = location;
            Type = type;
            Value = value;
            SseValue = sseValue;
        }
    }
}
