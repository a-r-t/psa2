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

        public MemoryType GetMemoryType(string memoryType)
        {
            return MemoryTypes.Find(mt => mt.Name == memoryType);
        }

        public List<Variable> GetAllVariables()
        {
            List<Variable> variables = new List<Variable>();
            foreach (MemoryType memoryType in MemoryTypes)
            {
                foreach (DataType dataType in memoryType.DataTypes)
                {
                    foreach (Id id in dataType.Ids)
                    {
                        variables.Add(new Variable(memoryType.Name, dataType.Name, id.Index, id.Name, id.Description));
                    }
                }
            }
            return variables;
        }

        public List<Variable> GetVariablesByMemoryType(string memoryType)
        {
            List<Variable> variables = new List<Variable>();
            MemoryType selectedMemoryType = GetMemoryType(memoryType);
            if (selectedMemoryType != null)
            {
                foreach (DataType dataType in selectedMemoryType.DataTypes)
                {
                    foreach (Id id in dataType.Ids)
                    {
                        variables.Add(new Variable(selectedMemoryType.Name, dataType.Name, id.Index, id.Name, id.Description));
                    }
                }
            }
            return variables;
        }

        public List<Variable> GetVariablesByMemoryTypeAndDataType(string memoryType, string dataType)
        {
            List<Variable> variables = new List<Variable>();
            MemoryType selectedMemoryType = GetMemoryType(memoryType);
            if (selectedMemoryType != null)
            {
                DataType selectedDataType = selectedMemoryType.GetDataType(dataType);
                if (selectedDataType != null)
                {
                    foreach (Id id in selectedDataType.Ids)
                    {
                        variables.Add(new Variable(selectedMemoryType.Name, selectedDataType.Name, id.Index, id.Name, id.Description));
                    }
                }

            }
            return variables;
        }

        public List<Variable> GetVariablesByDataType(string dataType)
        {
            List<Variable> variables = new List<Variable>();
            foreach (MemoryType memoryType in MemoryTypes)
            {
                DataType selectedDataType = memoryType.GetDataType(dataType);
                if (selectedDataType != null)
                {
                    foreach (Id id in selectedDataType.Ids)
                    { 
                        variables.Add(new Variable(memoryType.Name, selectedDataType.Name, id.Index, id.Name, id.Description));
                    }
                }
            }
            return variables;
        }

        public Variable GetVariable(string memoryType, string dataType, int index)
        {
            MemoryType selectedMemoryType = GetMemoryType(memoryType);
            if (selectedMemoryType != null)
            {
                DataType selectedDataType = selectedMemoryType.GetDataType(dataType);
                if (selectedDataType != null)
                {
                    Id selectedId = selectedDataType.GetId(index);
                    if (selectedId != null)
                    {
                        return new Variable(selectedMemoryType.Name, selectedDataType.Name, selectedId.Index, selectedId.Name, selectedId.Description);
                    }
                }
            }
            return null;
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

            public DataType GetDataType(string dataType)
            {
                return DataTypes.Find(dt => dt.Name == dataType);
            }
        }

        public class DataType
        {
            public string Name { get; set; }
            public List<Id> Ids { get; set; }

            public DataType(string name)
            {
                Name = name;
                Ids = new List<Id>();
            }

            public Id GetId(int index)
            {
                return Ids.Find(id => id.Index == index);
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

    public class Variable
    {
        public string MemoryType { get; set; }
        public string DataType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Variable(string memoryType, string dataType, int id, string name, string description)
        {
            MemoryType = memoryType;
            DataType = dataType;
            Id = id;
            Name = name;
            Description = description;
        }
    }

}
