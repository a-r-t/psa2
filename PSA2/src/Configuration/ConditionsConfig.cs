using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Configuration
{
    public class ConditionsConfig
    {
        public List<Condition> Conditions { get; set; }

        public ConditionsConfig()
        {
            Conditions = new List<Condition>();
        }

        public Condition GetConditionByIndex(int index)
        {
            return Conditions.Find(c => c.Index == index);
        }

        public class Condition
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public List<Param> Params { get; set; }

            public Condition(int index, string name, string description)
            {
                Index = index;
                Name = name;
                Description = description;
                Params = new List<Param>();
            }

            /// <summary>
            /// Returns description for condition as well as for condition's params
            /// </summary>
            public string GetFullDescription()
            {
                StringBuilder fullDescription = new StringBuilder()
                    .Append(Description);

                if (Params.Count > 0)
                {
                    fullDescription
                        .Append("\n\n")
                        .Append($"Params ({Params.Count}):");

                    for (int i = 0; i < Params.Count; i++)
                    {
                        Param param = Params[i];
                        fullDescription
                            .Append("\n\n")
                            .Append("• ")
                            .Append(param.Name)
                            .Append("\n\n")
                            .Append(param.Description);
                    }
                }
                return fullDescription.ToString();
            }
        }

        public class Param
        {
            public int Index { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string DataType { get; set; }

            public Param(int index, string name, string description, string dataType)
            {
                Index = index;
                Name = name;
                Description = description;
                DataType = dataType;
            }
        }
    }
}
