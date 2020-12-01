using PSA2.src.ExtentionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PSA2.src.Configuration.ConditionsConfig;

namespace PSA2.src.Views.CustomControls
{
    public partial class ConditionsSearchTextBox : SearchTextBox<Condition>
    {
        public ConditionsSearchTextBox(): base()
        {
            FilterExpression = option => option.Name.ContainsIgnoreCase(SearchText);
        }
    }
}
