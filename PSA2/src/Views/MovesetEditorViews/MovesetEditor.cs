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

        public void OnCodeBlockSelected(string sectionText, SectionType sectionType, int sectionIndex, int codeBlockIndex)
        {
            TabPage existingTabPage = FindExistingTabPage(sectionType, sectionIndex, codeBlockIndex);

            if (existingTabPage == null) 
            {
                TabPage codeBlockCommandsTab = new TabPage(sectionText);
                CodeBlockViewer codeBlockViewer = new CodeBlockViewer(psaMovesetHandler, psaCommandsConfig, sectionType, sectionIndex, codeBlockIndex);
                codeBlockViewer.Name = "codeBlockViewer";
                codeBlockViewer.Dock = DockStyle.Fill;
                codeBlockViewer.AddListener(parametersEditor);
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

        private void eventsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void eventsTabControl_Click(object sender, EventArgs e)
        {
            this.ActiveControl = commandOptionsViewer;
        }
    }
}
