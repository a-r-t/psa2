using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Models.Fighter.Misc
{
    public class MiscSection1Param
    {
        public string Name { get; set; }
        public int Value { get; set; }

        public MiscSection1Param(string name, int value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{{{nameof(Name)}={Name}, {nameof(Value)}={Value.ToString("X")}}}";
        }
    }
}
