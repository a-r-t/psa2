using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.CustomControls
{
    public class ParameterEntry
    {
        public string Category { get; set; }
        public int Type { get; set; }
        public int Value { get; set; }

        public ParameterEntry(string category, int type, int value)
        {
            Category = category;
            Type = type;
            Value = value;
        }
    }
}
