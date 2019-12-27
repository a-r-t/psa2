using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class IntAttribute : Attribute
    {
        public int Value { get; set; }
        public int SseValue { get; set; }

        public IntAttribute(string name, string description, string location, int value, int sseValue)
            : base(name, description, location)
        {
            Value = value;
            SseValue = sseValue;
        }
    }
}
