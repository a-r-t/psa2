using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.fighter
{
    public class CommandParam
    {
        public string ParamName { get; set; }
        public string Description { get; set; }
        public List<string> DataTypes { get; set; }
        public bool CanUseVariable { get; set; }
        public string DefaultValue { get; set; }
        public string Value { get; set; }

        public CommandParam(string paramName, string description, List<string> dataTypes, bool canUseVariable, string defaultValue=null)
        {
            ParamName = paramName;
            Description = description;
            DataTypes = dataTypes;
            CanUseVariable = canUseVariable;

            if (defaultValue != null) {
                DefaultValue = defaultValue;
            }
            else
            {
                switch (DataTypes[0].ToLower()) {
                    case "scalar":
                        DefaultValue = "0";
                        break;
                    case "boolean":
                        DefaultValue = "false";
                        break;
                    case "variable":
                        DefaultValue = "IC-Basic[0]";
                        break;
                    case "value":
                    case "pointer":
                    case "(4)":
                    case "Any":
                    default:
                        DefaultValue = "00000000";
                        break;
                }
            }
        }
    }
}
