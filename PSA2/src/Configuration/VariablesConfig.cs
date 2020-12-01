using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Configuration
{
    public class VariablesConfig
    {
        public List<MemoryType> MemoryTypes { get; set; }

        public VariablesConfig()
        {
            MemoryTypes = new List<MemoryType>();
        }

        public class MemoryType
        {
            public string Name { get; set; }
            public List<DataType> DataTypes { get; set; }

            public MemoryType(string name)
            {
                Name = name;
                DataTypes = new List<DataType>();
            }
        }

        public class DataType
        {
            public string Name { get; set; }
            public List<DataType> DataTypes { get; set; }

            public DataType(string name)
            {
                Name = name;
                DataTypes = new List<DataType>();
            }
        }

        public class Id
        {
            public string Name { get; set; }
            public int Index { get; set; }
            public string Description { get; set; }

            public Id(string name, int index, string description)
            {
                Name = name;
                Index = index;
                Description = description;
            }
        }
    }


}
