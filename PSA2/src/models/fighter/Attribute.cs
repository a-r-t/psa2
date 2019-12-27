using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.models.fighter
{
    public abstract class Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public Attribute(string name, string description, string location)
        {
            Name = name;
            Description = description;
            Location = location;
        }
    }
}
