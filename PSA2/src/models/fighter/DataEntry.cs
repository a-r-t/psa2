using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public class DataEntry
    {
        public int Offset { get; set; }
        public string Name { get; set; }

        public DataEntry(int offset, string name)
        {
            Offset = offset;
            Name = name;
        }
    }
}
