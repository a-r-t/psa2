using PSA2MovesetLogic.src.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Configuration
{
    public class Config
    {
        public static ActionAliasesConfig ActionAliasesConfig = Utils.LoadJson<ActionAliasesConfig>("./aliases/Actions.json");
        public static ConditionsConfig ConditionsConfig = Utils.LoadJson<ConditionsConfig>("./config/Conditions.json");
        public static VariablesConfig VariablesConfig = Utils.LoadJson<VariablesConfig>("./config/Variables.json");
    }
}
