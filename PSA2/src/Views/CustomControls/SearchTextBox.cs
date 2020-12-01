using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class SearchTextBox<T> : TextBox
    {
        private List<T> items;
        public List<T> Items 
        { 
            get
            {
                return items;
            }
            set
            {
                items = value;
                FilteredItems = items;
            }
        }

        public List<T> FilteredItems { get; private set; }

        private Predicate<T> filterExpression;
        public Predicate<T> FilterExpression 
        { 
            get
            {
                return filterExpression;
            }
            set
            {
                filterExpression = value;
                FilterItems();
            }
        }

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
            FilterItems();
            base.OnTextChanged(e);
        }

        private void FilterItems()
        {
            if (SearchText != "")
            {
                FilteredItems = Items.FindAll(FilterExpression);
            }
            else
            {
                FilteredItems = Items;
            }
        }
    }
}
