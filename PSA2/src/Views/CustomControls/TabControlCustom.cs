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
    /// <summary>
    /// I will comment up this class soon as well as all the other classes involved with this custom tab control
    /// </summary>
    public partial class TabControlCustom : Panel
    {
        public ObservableCollection<TabPageCustom> TabPages { get; }
        private Panel tabsHolder;
        private Panel tabViewer;
        private TabListButton tabListButton;
        private ScintillaListSelect tabListScintilla;
        private Size currentControlSize;
        private int currentLastTabIndex;
        private AddTabButton addTabButton;
        public int TabCount
        {
            get
            {
                return TabPages.Count;
            }
        }
        public int SelectedIndex
        {
            get
            {
                return CurrentTabIndex;
            }
        }
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
                if (value >= 0 && value < TabPages.Count)
                {
                    SelectTab(value);
                }
                else
                {
                    for (int i = 0; i < tabs.Count; i++)
                    {
                        tabs[i].IsSelected = false;
                    }
                    tabViewer.Controls.Clear();
                }
                currentTabIndex = value;
                SelectedTabIndexChanged(this, new EventArgs());
            }
        }

        public event EventHandler SelectedTabIndexChanged;

        public TabControlCustom(): base()
        {
            TabPages = new ObservableCollection<TabPageCustom>();
            TabPages.CollectionChanged += new NotifyCollectionChangedEventHandler(OnTabPageChange);
            tabs = new List<TabCustom>();
            SetUpControls();
            tabViewer.DoubleBuffered(true);
        }

        // TODO: Set this up to be called when current tab index is changed
        private void SelectTabIndexChangedEvent(object sender, EventArgs e)
        {
            SelectedTabIndexChanged?.Invoke(sender, e);
        }

        private void OnTabPageChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (e.NewStartingIndex == TabPages.Count - 1)
                {
                    TabPageCustom newTabPage = (TabPageCustom)e.NewItems[0];
                    newTabPage.TabTextChanged += (s, EventArgs) => { OnTabPageTextChanged(s, EventArgs); };
                    newTabPage.TabIndex = TabPages.Count - 1;
                    AppendTab(newTabPage, newTabPage.TabIndex);
                }
            }
        }

        private void OnTabPageTextChanged(object sender, EventArgs e)
        {
            TabPageCustom tabPage = (TabPageCustom)sender;
            tabs[tabPage.TabIndex].Text = tabPage.TabText;
        }

        private void OnTabResized(object sender, EventArgs e)
        {
            TabCustom resizedTab = (TabCustom)sender;
            for (int i = resizedTab.Index + 1; i < tabs.Count; i++)
            {
                TabCustom tab = tabs[i];
                TabCustom tabBefore = tabs[i - 1];
                tabs[i].Location = new Point(tabBefore.Location.X + tabBefore.Width + 1, tab.Location.Y);
            }
            ChangeTabsDisplayed();
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
            //tab.Width += 20;
            tab.MouseDown += (sender, EventArgs) => { OnTabMouseDown(sender, EventArgs); };
            tab.MouseMove += (sender, MouseEventArgs) => { OnTabDragged(sender, MouseEventArgs); };
            tab.MouseUp += (sender, MouseEventArgs) => { OnTabMouseUp(sender, MouseEventArgs); };
            tab.Resize += (sender, EventArgs) => { OnTabResized(sender, EventArgs); };
            tab.Name = tab.Text;
            return tab;
        }

        private void OnTabMouseDown(object sender, EventArgs e)
        {
            TabCustom clickedTab = (TabCustom)sender;
            if (clickedTab.IsMouseOverXButton())
            {
                clickedTab.XButtonMouseDown = true;
            }
            else
            {
                int tabIndex = clickedTab.Index;
                if (CurrentTabIndex != tabIndex)
                {
                    CurrentTabIndex = tabIndex;
                }
            }
        }

        private void OnTabMouseUp(object sender, MouseEventArgs e)
        {
            TabCustom clickedTab = (TabCustom)sender;
            if (clickedTab.IsMouseOverXButton() && clickedTab.XButtonMouseDown)
            {
                tabs.RemoveAt(clickedTab.Index);
                TabPages.RemoveAt(clickedTab.Index);
                tabsHolder.Controls.Remove(clickedTab);
                for (int i = clickedTab.Index; i < tabs.Count; i++)
                {
                    TabCustom tab = tabs[i];
                    tab.Index--;
                    tab.Location = new Point(tab.Location.X - clickedTab.Width - 1, tab.Location.Y);
                }

                if (CurrentTabIndex - 1 >= 0 && tabs.Count > 0 && CurrentTabIndex == clickedTab.Index)
                {
                    CurrentTabIndex--;
                }
                else if (CurrentTabIndex - 1 >= 0 && tabs.Count > 0 && CurrentTabIndex > clickedTab.Index)
                {
                    CurrentTabIndex--;
                    //SelectTab(CurrentTabIndex);
                }
                else if (CurrentTabIndex - 1 >= 0 && tabs.Count > 0 && CurrentTabIndex < clickedTab.Index)
                {
                    SelectTab(CurrentTabIndex);
                }
                else if (CurrentTabIndex - 1 == -1 && tabs.Count > 0)
                {
                    CurrentTabIndex = 0;
                    //SelectTab(CurrentTabIndex);
                }
                else
                {
                    CurrentTabIndex = -1;
                }

                ChangeTabsDisplayed();
            }
            else
            {
                clickedTab.XButtonMouseDown = false;
            }
        }

        private void OnTabDragged(object sender, MouseEventArgs e)
        {                
            TabCustom tab = (TabCustom)sender;
            if (e.Button == MouseButtons.Left && !tab.IsMouseOverXButton() && !tab.XButtonMouseDown)
            {
                tab.XButtonDisabled = true;
                int tabIndex = tab.Index;
                if (tabIndex < tabs.Count)
                {
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
                            SwapTabs(tabIndex, tabIndex - 1);
                            CurrentTabIndex = currentTab.Index;
                            return;
                        }
                    }
                    if (tabIndex < currentLastTabIndex)
                    {
                        TabCustom rightTab = tabs[tabIndex + 1];
                        int offset = rightTab.Width > currentTab.Width
                            ? rightTab.Width - currentTab.Width
                            : 0;
                        if (mouseLocationX > rightTab.Location.X + offset)
                        {
                            SwapTabs(tabIndex, tabIndex + 1);
                            CurrentTabIndex = currentTab.Index;
                            return;
                        }
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
            tabsHolder.Resize += TabsHolder_Resize;
            Controls.Add(tabsHolder);

            tabViewer = new Panel();
            tabViewer.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            tabViewer.Height = Height - tabsHolder.Height;
            tabViewer.Location = new Point(0, 40);
            Controls.Add(tabViewer);

            /*
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
            */

            tabListButton = new TabListButton();
            tabListButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tabListButton.Location = new Point(tabsHolder.Right - tabListButton.Width, 0);
            tabListButton.Height = tabsHolder.Height;
            tabListButton.Visible = false;
            tabsHolder.Controls.Add(tabListButton);

            tabListScintilla = new ScintillaListSelect();
            tabListScintilla.Size = new Size(200, 100);
            tabListScintilla.Visible = false;
            tabListScintilla.Anchor = AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            tabListScintilla.Location = new Point(tabListButton.Location.X + tabListButton.Width - tabListScintilla.Width, tabListButton.Location.Y + tabListButton.Size.Height);
            tabListScintilla.BorderStyle = BorderStyle.FixedSingle;
            tabListScintilla.MouseDown += TabListItemClicked;
            Controls.Add(tabListScintilla);
            tabListScintilla.BringToFront();
            tabListButton.SelectedStatusChanged += (sender, EventArgs) => { TabListButtonSelectedStatusChanged();  };
            currentLastTabIndex = tabs.Count - 1;

            addTabButton = new AddTabButton();
            addTabButton.Width = 40;
            addTabButton.PlusSignThickness = 2;
            addTabButton.PlusSignPadding = 6;
            addTabButton.MouseDown += (sender, MouseEventArgs) => { AddTabButtonMouseDown(sender, MouseEventArgs); };
            addTabButton.MouseUp += (sender, MouseEventArgs) => { AddTabButtonMouseUp(sender, MouseEventArgs); };
            tabsHolder.Controls.Add(addTabButton);
            addTabButton.BringToFront();
            SetAddTabButtonLocation();
        }

        private void AddTabButtonMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                addTabButton.IsMouseDown = true;
                addTabButton.IsSelected = true;
            }
        }

        private void AddTabButtonMouseUp(object sender, MouseEventArgs e)
        {
            if (addTabButton.IsMouseDown && addTabButton.IsPointOverButton(e.Location))
            {
                TabPageCustom newTabPage = new TabPageCustom();
                newTabPage.TabText = "New Tab";
                TabPages.Add(newTabPage);
                TabCustom newTab = tabs[tabs.Count - 1];
                CurrentTabIndex = newTab.Index;
                ChangeTabsDisplayed();
            }
            addTabButton.IsMouseDown = false;
            addTabButton.IsSelected = false;
        }

        private void SetAddTabButtonLocation()
        {
            int xLocation = 0;
            if (tabs.Count > 0 && currentLastTabIndex > -1)
            {
                TabCustom lastItem = tabs[currentLastTabIndex];
                xLocation = lastItem.Location.X + lastItem.Width + 1;
            }
            int yLocation = (tabsHolder.Height / 2) - (addTabButton.Width / 2);
            addTabButton.Location = new Point(xLocation, yLocation);
        }

        private void TabListButtonSelectedStatusChanged()
        {
            tabListScintilla.Visible = tabListButton.IsSelected;
            if (tabListScintilla.Visible)
            {
                tabListScintilla.CurrentHoveredIndex = -1;
            }
        }

        private void TabListItemClicked(object sender, MouseEventArgs e)
        {
            if (tabListScintilla.CurrentHoveredIndex != -1 && currentLastTabIndex >= 0)
            {
                string selectedItem = tabListScintilla.Items[tabListScintilla.CurrentHoveredIndex];
                int tabIndex = 0;
                for (int i = currentLastTabIndex + 1; i < tabs.Count; i++)
                {
                    if (tabs[i].Text == selectedItem)
                    {
                        tabIndex = i;
                        break;
                    }
                }

                SwapTabs(tabIndex, currentLastTabIndex);
                tabListScintilla.ModifyItem(tabListScintilla.CurrentHoveredIndex, tabs[tabIndex].Text);
                tabListButton.IsSelected = false;
                tabListScintilla.Visible = false;

                CurrentTabIndex = currentLastTabIndex;
                //SelectTab(currentTabIndex);
                ChangeTabsDisplayed();
            }
        }

        private void SwapTabs(int firstTabIndex, int secondTabIndex)
        {
            int frontTabIndex = Math.Min(firstTabIndex, secondTabIndex);
            int backTabIndex = Math.Max(firstTabIndex, secondTabIndex);
            TabCustom frontTab = tabs[frontTabIndex];
            TabCustom backTab = tabs[backTabIndex];

            int frontTabWidth = frontTab.Width;
            int backTabWidth = backTab.Width;

            Point temp = frontTab.Location;
            frontTab.Location = backTab.Location;
            backTab.Location = temp;

            int tempIndex = frontTab.Index;
            frontTab.Index = backTab.Index;
            backTab.Index = tempIndex;

            tabs[frontTabIndex] = backTab;
            tabs[backTabIndex] = frontTab;

            TabPageCustom tempTabPage = TabPages[frontTabIndex];
            TabPages[frontTabIndex] = TabPages[backTabIndex];
            TabPages[backTabIndex] = tempTabPage;

            if (frontTabWidth != backTabWidth)
            {
                int difference = frontTabWidth - backTabWidth;
                for (int i = frontTabIndex + 1; i < tabs.Count; i++)
                {
                    tabs[i].Location = new Point(tabs[i].Location.X - difference, tabs[i].Location.Y);
                    if (i > backTabIndex)
                    {
                        tabs[i].Location = new Point(tabs[i].Location.X + difference, tabs[i].Location.Y);
                    }
                }
            }
        }

        private void TabsHolder_Resize(object sender, EventArgs e)
        {
            Size newSize = Size;
            if (newSize.Width != currentControlSize.Width)
            {
                ChangeTabsDisplayed();
            }
            currentControlSize = newSize;
        }

        private void ChangeTabsDisplayed()
        {
            if (tabs.Count > 0)
            {
                int totalTabWidth = GetTotalTabWidth();
                int lastTabIndex = tabs.Count - 1;

                // if current selected tab is off screen, swap it with first tab index

                if (CurrentTabIndex >= 0)
                {
                    TabCustom currentTab = tabs[CurrentTabIndex];
                    if (currentTab.Location.X + currentTab.Width > tabsHolder.Width - tabListButton.Width - (addTabButton.Width + 3))
                    {
                        SwapTabs(CurrentTabIndex, 0);
                        CurrentTabIndex = 0;
                    }
                }

                while (totalTabWidth > tabsHolder.Width - tabListButton.Width - (addTabButton.Width + 3) && totalTabWidth > 0)
                {
                    TabCustom tab = tabs[lastTabIndex];
                    totalTabWidth -= tab.Width;
                    tab.Visible = false;
                    lastTabIndex--;
                }

                for (int i = 0; i <= lastTabIndex; i++)
                {
                    tabs[i].Visible = true;
                }

                if (lastTabIndex != tabs.Count - 1 && lastTabIndex > -1)
                {
                    if (lastTabIndex != currentLastTabIndex)
                    {
                        tabListScintilla.ClearItems();
                        tabListButton.Visible = true;
                        int tabDifference = (tabs.Count - 1) - lastTabIndex;
                        if (tabDifference > tabListScintilla.Items.Count)
                        {
                            for (int i = lastTabIndex + 1; i < tabs.Count; i++)
                            {
                                tabListScintilla.AddItem(tabs[i].Text);
                            }
                        }
                        else if (tabDifference < 0)
                        {
                            for (int i = 0; i < tabDifference; i++)
                            {
                                tabListScintilla.RemoveItem(tabListScintilla.Items.Count - 1);
                            }
                        }
                    }
                }
                else
                {
                    tabListButton.Visible = false;
                    tabListButton.IsSelected = false;
                    tabListScintilla.ClearItems();
                    tabListScintilla.Visible = false;
                    if (tabs.Count > 0)
                    {
                        tabs[0].Visible = true;
                    }
                }

                addTabButton.Visible = lastTabIndex > -1;

                currentLastTabIndex = lastTabIndex;

                SetAddTabButtonLocation();
            }
        }

        private int GetTotalTabWidth()
        {
            int totalWidth = 0;
            foreach (TabCustom tab in tabs)
            {
                totalWidth += tab.Width;
            }
            return totalWidth;
        }
    }
}
