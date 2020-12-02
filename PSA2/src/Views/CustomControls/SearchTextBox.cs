using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class SearchTextBox : TextBox
    {
        public class SearchTextChangedEventArgs
        {
            public string SearchText { get; }

            public SearchTextChangedEventArgs(string searchText)
            {
                SearchText = searchText;
            }
        }
        public event EventHandler<SearchTextChangedEventArgs> SearchTextChanged;

        protected string SearchText
        {
            get
            {
                return Text.Length > 2
                ? Text.Substring(2)
                : "";
            }
        }

        public SearchTextBox(): base()
        {
            Text = "🔍";
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete))
            {
                if (SelectionStart < 2)
                {
                    SelectionStart = 2;
                }

                // if user attempts to backspace the magnifying glass emoji, suppress the key press
                if (SelectionStart == 2 && SelectionLength == 0)
                {
                    e.SuppressKeyPress = true;
                    return;
                }
            }
            base.OnKeyDown(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            SearchTextChanged?.Invoke(this, new SearchTextChangedEventArgs(SearchText));
            base.OnTextChanged(e);
        }
    }
}
