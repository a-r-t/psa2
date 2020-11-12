using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace PSA2.src.Views.CustomControls
{
    public partial class TabControlCustom : Panel
    {
        public ObservableCollection<TabPageCustom> TabPages { get; }
        private Panel tabsHolder;
        private Panel tabViewer;
        private List<TabCustom> tabs;

        public TabControlCustom(): base()
        {
            TabPages = new ObservableCollection<TabPageCustom>();
            TabPages.CollectionChanged += new NotifyCollectionChangedEventHandler(OnTabPageChange);
            tabs = new List<TabCustom>();
            SetUpControls();
        }

        private void OnTabPageChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                TabPageCustom newItem = (TabPageCustom)e.NewItems[0];
                TabCustom tab = new TabCustom(newItem.TabText);
                int xLocation = 0;
                if (tabs.Count > 0)
                {
                    TabCustom lastItem = tabs[tabs.Count - 1];
                    xLocation = lastItem.Location.X + lastItem.Width + 1;
                }
                tab.Location = new Point(xLocation, 0);
                tab.BackColor = Color.Orange;
                tabs.Add(tab);
                int index = tabs.Count - 1;
                tab.Click += (s, EventArgs) => { OnTabClicked(sender, EventArgs, index); };
                tabsHolder.Controls.Add(tab);
            }
        }

        private void OnTabClicked(object sender, EventArgs e, int tabIndex)
        {
            tabViewer.Controls.Clear();
            tabViewer.Controls.Add(TabPages[tabIndex]);
        }

        public void SetUpControls()
        {
            tabsHolder = new Panel();
            tabsHolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tabsHolder.Location = new Point(0, 0);
            tabsHolder.Height = 40;
            tabsHolder.BackColor = Color.Blue;
            Controls.Add(tabsHolder);

            tabViewer = new Panel();
            tabViewer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            tabViewer.Location = new Point(0, 40);
            Controls.Add(tabViewer);

            TabPageCustom tabPage1 = new TabPageCustom();
            tabPage1.BackColor = Color.Red;
            tabPage1.TabText = "Hello There";
            TabPages.Add(tabPage1);

            TabPageCustom tabPage2 = new TabPageCustom();
            tabPage2.BackColor = Color.Green;
            tabPage2.TabText = "The Angel From";
            TabPages.Add(tabPage2);

            TabPageCustom tabPage3 = new TabPageCustom();
            tabPage3.BackColor = Color.Purple;
            tabPage3.TabText = "My Nightmare, I don't know";
            TabPages.Add(tabPage3);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawString("HELLO", new Font(new FontFamily("Times New Roman"), 12, FontStyle.Regular), new SolidBrush(Color.DarkBlue), new Point(0, 0));
            base.OnPaint(e);
        }
    }
}
