using PSA2.src.ExtentionMethods;
using PSA2.src.Views.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PSA2.src.Configuration.ConditionsConfig;

namespace PSA2.src.Views.Utility
{
    public class ConditionSearchList : SearchList<Condition>
    {
        public ConditionSearchList(SearchTextBox searchTextBox): base(searchTextBox)
        {
            FilterExpression = option => option.Name.ContainsIgnoreCase(SearchText);
        }
    }
}
