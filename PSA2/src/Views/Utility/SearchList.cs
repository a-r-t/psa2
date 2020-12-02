using PSA2.src.Views.CustomControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.Views.Utility
{
    public class SearchList<T>
    {
        protected string SearchText { get; set; } = "";
        protected bool AlwaysRunFilter { get; set; }

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

        public SearchList(SearchTextBox searchTextBox) {
            searchTextBox.SearchTextChanged += SearchTextChanged;
        }

        private void SearchTextChanged(object sender, SearchTextBox.SearchTextChangedEventArgs e)
        {
            SearchText = e.SearchText;
            FilterItems();
        }

        protected void FilterItems()
        {
            if (Items != null && (SearchText != "" || (SearchText == "" && AlwaysRunFilter)))
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
