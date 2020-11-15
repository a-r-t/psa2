﻿using System;
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
        private TabListButton tabListButton;
        private ScintillaListSelect tabListScintilla;
        private Size currentControlSize;
        private int currentLastTabIndex;
        
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
                        SwapTabs(tabIndex, tabIndex - 1);
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
                        SwapTabs(tabIndex, tabIndex + 1);
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
            tabsHolder.Resize += TabsHolder_Resize;
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

            tabListButton = new TabListButton();
            tabListButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tabListButton.Location = new Point(tabsHolder.Right - tabListButton.Width, 0);
            tabListButton.Height = tabsHolder.Height;
            tabListButton.Visible = false;
            tabsHolder.Controls.Add(tabListButton);

            tabListScintilla = new ScintillaListSelect();
            tabListScintilla.Size = new Size(100, 100);
            tabListScintilla.Visible = false;
            tabListScintilla.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tabListScintilla.Location = new Point(tabListButton.Location.X + tabListButton.Width - tabListScintilla.Width, tabListButton.Location.Y + tabListButton.Size.Height);
            tabListScintilla.BorderStyle = BorderStyle.FixedSingle;
            tabListScintilla.MouseDown += TabListItemClicked;
            Controls.Add(tabListScintilla);
            tabListScintilla.BringToFront();
            tabListButton.SelectedStatusChanged += (sender, EventArgs) => { TabListButtonSelectedStatusChanged();  };
            currentLastTabIndex = tabs.Count - 1;
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
                for (int i = 0; i < tabs.Count; i++)
                {
                    if (tabs[i].Text == selectedItem)
                    {
                        tabIndex = i;
                        break;
                    }
                }

                SwapTabs(tabIndex, currentLastTabIndex);
                tabListScintilla.ModifyItem(tabListScintilla.CurrentHoveredIndex, tabs[tabIndex].Text);
                ChangeTabsDisplayed();
                tabListButton.IsSelected = false;
                tabListScintilla.Visible = false;

                // TODO: Find a way to make the CurrentTabIndex set method handle this
                currentTabIndex = currentLastTabIndex;
                SelectTab(currentTabIndex);
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void ChangeTabsDisplayed()
        {
            int totalTabWidth = getTotalTabWidth();
            int lastTabIndex = tabs.Count - 1;
            while (totalTabWidth > tabsHolder.Width - tabListButton.Width)
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
            currentLastTabIndex = lastTabIndex;
        }

        private int getTotalTabWidth()
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
