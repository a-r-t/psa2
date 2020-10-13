using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSA2.src.FileProcessor.MovesetHandler;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class MovesetEditor: ObservableUserControl<IMovesetEditorListener>, ILocationSelectorListener
    {
        protected PsaMovesetHandler psaMovesetHandler;

        public MovesetEditor(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
            LocationSelector locationSelector = new LocationSelector(psaMovesetHandler);
            locationSelector.Dock = DockStyle.Fill;
            locationSelector.AddListener(this);
            selectorView.Controls.Add(locationSelector);
        }

        public void OnSelect(string sectionText, SectionType sectionType, int sectionIndex, int codeBlockIndex)
        {
            TabPage existingTabPage = FindExistingTabPage(sectionType, sectionIndex, codeBlockIndex);

            if (existingTabPage == null) 
            {
                TabPage codeBlockCommandsTab = new TabPage(sectionText);
                CodeBlockViewer codeBlockViewer = new CodeBlockViewer(psaMovesetHandler, sectionType, sectionIndex, codeBlockIndex);
                codeBlockViewer.Name = "codeBlockViewer";
                codeBlockViewer.Dock = DockStyle.Fill;
                codeBlockCommandsTab.Controls.Add(codeBlockViewer);
                eventsTabControl.TabPages.Insert(0, codeBlockCommandsTab);
                eventsTabControl.SelectedTab = codeBlockCommandsTab;
            }
            else
            {
                eventsTabControl.TabPages.Remove(existingTabPage);
                eventsTabControl.TabPages.Insert(0, existingTabPage);
                eventsTabControl.SelectedTab = existingTabPage;
            }
        }

        public TabPage FindExistingTabPage(SectionType sectionType, int sectionIndex, int codeBlockIndex)
        {
            foreach (TabPage tabPage in eventsTabControl.TabPages)
            {
                CodeBlockViewer codeBlockViewer = (CodeBlockViewer)tabPage.Controls["codeBlockViewer"];
                if (codeBlockViewer.SectionType == sectionType 
                    && codeBlockViewer.SectionIndex == sectionIndex
                    && codeBlockViewer.CodeBlockIndex == codeBlockIndex)
                {
                    return tabPage;
                }
            }
            return null;
        }
    }
}
