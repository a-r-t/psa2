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
using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.Utility;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class MovesetEditor: ObservableUserControl<IMovesetEditorListener>, ISectionSelectorListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        protected PsaCommandsConfig psaCommandsConfig;
        protected SectionSelector sectionSelector;
        protected ParametersEditor parametersEditor;

        public MovesetEditor(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = Utils.LoadJson<PsaCommandsConfig>("data/psa_command_data.json");

            InitializeComponent();

            this.sectionSelector = new SectionSelector(psaMovesetHandler);
            sectionSelector.Dock = DockStyle.Fill;
            sectionSelector.AddListener(this);
            selectorView.Controls.Add(sectionSelector);

            this.parametersEditor = new ParametersEditor(psaMovesetHandler);
            parametersEditor.Dock = DockStyle.Fill;
            parametersEditorViewer.Controls.Add(parametersEditor);
        }

        public void OnCodeBlockSelected(string sectionText, SectionSelectionInfo sectionSelectionInfo)
        {
            TabPage existingTabPage = FindExistingTabPage(sectionSelectionInfo);

            if (existingTabPage == null) 
            {
                TabPage codeBlockCommandsTab = new TabPage(sectionText);
                CodeBlockViewer codeBlockViewer = new CodeBlockViewer(psaMovesetHandler, psaCommandsConfig, sectionSelectionInfo);
                codeBlockViewer.Name = "codeBlockViewer";
                codeBlockViewer.Dock = DockStyle.Fill;
                codeBlockViewer.AddListener(parametersEditor);
                codeBlockCommandsTab.Controls.Add(codeBlockViewer);
                eventsTabControl.TabPages.Insert(0, codeBlockCommandsTab);
                eventsTabControl.SelectedTab = codeBlockCommandsTab;
            }
            else
            {
                if (eventsTabControl.SelectedTab != existingTabPage)
                {
                    eventsTabControl.TabPages.Remove(existingTabPage);
                    eventsTabControl.TabPages.Insert(0, existingTabPage);
                    eventsTabControl.SelectedTab = existingTabPage;
                }
            }
        }

        public TabPage FindExistingTabPage(SectionSelectionInfo sectionSelectionInfo)
        {
            foreach (TabPage tabPage in eventsTabControl.TabPages)
            {
                CodeBlockViewer codeBlockViewer = (CodeBlockViewer)tabPage.Controls["codeBlockViewer"];
                if (codeBlockViewer.SectionSelectionInfo.SectionType == sectionSelectionInfo.SectionType
                    && codeBlockViewer.SectionSelectionInfo.SectionIndex == sectionSelectionInfo.SectionIndex
                    && codeBlockViewer.SectionSelectionInfo.CodeBlockIndex == sectionSelectionInfo.CodeBlockIndex)
                {
                    return tabPage;
                }
            }
            return null;
        }

        private void eventsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void eventsTabControl_Click(object sender, EventArgs e)
        {
            this.ActiveControl = commandOptionsViewer;
        }
    }
}
