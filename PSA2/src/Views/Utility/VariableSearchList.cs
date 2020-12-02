using PSA2.src.Configuration;
using PSA2.src.ExtentionMethods;
using PSA2.src.Views.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.Utility
{
    public class VariableSearchList : SearchList<Variable>
    {
        private string dataType;
        public string DataType { 
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
                FilterItems();
            }
        }

        public VariableSearchList(SearchTextBox searchTextBox): base(searchTextBox)
        {
            AlwaysRunFilter = true;
            FilterExpression = variable =>
            {
                if (DataType != null && variable.DataType != DataType)
                {
                    return false;
                }
                return variable.Name.ContainsIgnoreCase(SearchText);
            };
        }
    }
}
