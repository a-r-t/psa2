using PSA2.src.ExtentionMethods;
using PSA2.src.Views.MovesetEditorViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.CustomControls
{
    public partial class SubActionsSearchTextBox : SearchTextBox<SubActionOption>
    {
        public SubActionsSearchTextBox(): base()
        {
            FilterExpression = option => option.Name.ContainsIgnoreCase(SearchText) || option.Index.ToString("X").ContainsIgnoreCase(SearchText);
        }
    }
}
