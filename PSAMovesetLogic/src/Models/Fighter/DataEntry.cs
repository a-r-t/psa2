using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.Models.Fighter
{
    public class DataEntry
    {
        public int Location { get; set; }
        public string Name { get; set; }

        public DataEntry(int location, string name)
        {
            Location = location;
            Name = name;
        }
    }
}
