using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Configuration
{
    public class ActionAliasesConfig
    {
        public List<ActionAlias> ActionAliases { get; set; }

        public ActionAliasesConfig()
        {
            ActionAliases = new List<ActionAlias>();
        }

        public string GetActionAlias(int index)
        {
            ActionAlias actionAlias = ActionAliases.Find(aa => aa.Index == index);
            if (actionAlias != null)
            {
                return actionAlias.Alias;
            }
            else
            {
                return "";
            }
        }
    }

    public class ActionAlias
    {
        public int Index { get; set; }
        public string Alias { get; set; }

        public ActionAlias(int index, string alias)
        {
            Index = index;
            Alias = alias;
        }
    }
}
