using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;
using PSA2.src.ExtentionMethods;

namespace PSA2.src.Views.CustomControls
{
    public partial class TabControlCustom : Panel
    {
        public ObservableCollection<TabPageCustom> TabPages { get; }
        private Panel tabsHolder;
        private Panel tabViewer;
        private TabList tabList;
        
        private List<TabCustom> tabs;
        private int currentTabIndex = -1;
        public int CurrentTabIndex
        {
            get
            {
                return currentTabIndex;
            }
            set
            {
                if (value != currentTabIndex && value < TabPages.Count)
                {
                    SelectTab(value);
                    currentTabIndex = value;
                }
            }
        }

        public TabControlCustom(): base()
        {
            TabPages = new ObservableCollection<TabPageCustom>();
            TabPages.CollectionChanged += new NotifyCollectionChangedEventHandler(OnTabPageChange);
            tabs = new List<TabCustom>();
            SetUpControls();
            tabViewer.DoubleBuffered(true);
        }

        private void OnTabPageChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex == TabPages.Count - 1)
                {
                    TabPageCustom newTabPage = (TabPageCustom)e.NewItems[0];
                    AppendTab(newTabPage, e.NewStartingIndex);
                }
            }
        }

        private void AppendTab(TabPageCustom tabPage, int tabIndex)
        {
            TabCustom tab = CreateTab(tabPage, tabIndex);
            tabs.Add(tab);
            tabsHolder.Controls.Add(tab);
            if (tabs.Count == 1)
            {
                CurrentTabIndex = 0;
            }
        }

        private TabCustom CreateTab(TabPageCustom tabPage, int tabIndex)
        {
            TabCustom tab = new TabCustom(tabPage.TabText);
            int xLocation = 0;
            if (tabs.Count > 0)
            {
                TabCustom lastItem = tabs[tabs.Count - 1];
                xLocation = lastItem.Location.X + lastItem.Width + 1;
            }
            tab.Location = new Point(xLocation, 0);
            tab.Height = tabsHolder.Height;
            tab.Index = tabIndex;
            tab.Width += 20;
            tab.MouseDown += (sender, EventArgs) => { OnTabClicked(sender, EventArgs); };
            tab.MouseMove += (sender, MouseEventArgs) => { OnTabDragged(sender, MouseEventArgs); };
            return tab;
        }

        private void OnTabClicked(object sender, EventArgs e)
        {
            int tabIndex = (sender as TabCustom).Index;
            if (CurrentTabIndex != tabIndex)
            {
                CurrentTabIndex = tabIndex;
            }
        }

        private void OnTabDragged(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int tabIndex = (sender as TabCustom).Index;

                TabCustom currentTab = tabs[tabIndex];
                int mouseLocationX = e.Location.X + currentTab.Location.X;
                if (tabIndex > 0)
                {
                    TabCustom leftTab = tabs[tabIndex - 1];
                    int offset = leftTab.Width > currentTab.Width
                        ? leftTab.Width - currentTab.Width
                        : 0;
                    if (mouseLocationX < leftTab.Location.X + leftTab.Size.Width - offset)
                    {
                        currentTab.Location = leftTab.Location;
                        leftTab.Location = new Point(currentTab.Location.X + currentTab.Width + 1, currentTab.Location.Y);
                        currentTab.Index--;
                        leftTab.Index++;

                        TabCustom tempTab = tabs[tabIndex - 1];
                        tabs[tabIndex - 1] = tabs[tabIndex];
                        tabs[tabIndex] = tempTab;

                        TabPageCustom tempTabPage = TabPages[tabIndex - 1];
                        TabPages[tabIndex - 1] = TabPages[tabIndex];
                        TabPages[tabIndex] = tempTabPage;

                        CurrentTabIndex = currentTab.Index;
                        return;
                    }
                }
                if (tabIndex < tabs.Count - 1)
                {
                    TabCustom rightTab = tabs[tabIndex + 1];
                    int offset = rightTab.Width > currentTab.Width
                        ? rightTab.Width - currentTab.Width
                        : 0;
                    if (mouseLocationX > rightTab.Location.X + offset)
                    {
                        Point temp = currentTab.Location;
                        rightTab.Location = currentTab.Location;
                        currentTab.Location = new Point(temp.X + rightTab.Width + 1, temp.Y);
                        currentTab.Index++;
                        rightTab.Index--;

                        TabCustom tempTab = tabs[tabIndex + 1];
                        tabs[tabIndex + 1] = tabs[tabIndex];
                        tabs[tabIndex] = tempTab;

                        TabPageCustom tempTabPage = TabPages[tabIndex + 1];
                        TabPages[tabIndex + 1] = TabPages[tabIndex];
                        TabPages[tabIndex] = tempTabPage;

                        CurrentTabIndex = currentTab.Index;
                        return;
                    }
                }
            }
        }

        private void SelectTab(int tabIndex)
        {
            tabViewer.SuspendLayout();
            tabViewer.Controls.Clear();
            tabViewer.Controls.Add(TabPages[tabIndex]);
            tabViewer.ResumeLayout();

            for (int i = 0; i < tabs.Count; i++)
            {
                tabs[i].IsSelected = i == tabIndex;
            }
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

            tabList = new TabList();
            tabList.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tabList.Location = new Point(tabsHolder.Right - tabList.Width, 0);
            tabList.Height = tabsHolder.Height;
            tabsHolder.Controls.Add(tabList);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.DrawString("HELLO", new Font(new FontFamily("Times New Roman"), 12, FontStyle.Regular), new SolidBrush(Color.DarkBlue), new Point(0, 0));
            base.OnPaint(e);
        }
    }
}
