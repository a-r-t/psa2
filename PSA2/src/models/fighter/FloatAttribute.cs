using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class FloatAttribute : Attribute
    {
        public float Value { get; set; }
        public float SseValue { get; set; }

        public FloatAttribute(string name, string description, string location, float value, float sseValue) 
            : base(name, description, location)
        {
            Value = value;
            SseValue = sseValue;
        }
    }
}
