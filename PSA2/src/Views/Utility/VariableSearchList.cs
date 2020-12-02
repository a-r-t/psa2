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
        public string MemoryType { get; set; }
        public string DataType { get; set; }

        public VariableSearchList(SearchTextBox searchTextBox): base(searchTextBox)
        {
            FilterExpression = variable =>
            {
                if (MemoryType != "" && variable.MemoryType != MemoryType)
                {
                    return false;
                }
                if (DataType != "" && variable.DataType != DataType)
                {
                    return false;
                }
                return variable.Name.ContainsIgnoreCase(SearchText);
            };
        }
    }
}
