using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.Configs;
using PSA2MovesetLogic.src.Utility;
using System.Reflection;
using PSA2.src.ExtentionMethods;
using PSA2.src.Views.MovesetEditorViews.Interfaces;
using System.Runtime.InteropServices;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class MovesetEditor: ObservableUserControl<IMovesetEditorListener>, 
        ISectionSelectorListener, 
        IEventActionsListener,
        ICommandSelectorListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        protected PsaCommandsConfig psaCommandsConfig;
        protected SectionSelector sectionSelector;
        protected ParametersEditor parametersEditor;
        protected CommandSelector commandSelector;
        protected EventActions eventActions;
        protected PsaCommandConfig currentlySelectedCommandOption;

        public CodeBlockViewer ActiveCodeBlockViewer
        {
            get
            {
                if (eventsTabControl.TabCount > 0)
                {
                    TabPage selectedTabPage = eventsTabControl.TabPages[eventsTabControl.SelectedIndex];
                    return (CodeBlockViewer)selectedTabPage.Controls["codeBlockViewer"];
                }
                else
                {
                    return null;
                }
            }
        }

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

            this.commandSelector = new CommandSelector(psaMovesetHandler, psaCommandsConfig);
            commandSelector.Dock = DockStyle.Fill;
            commandOptionsViewer.Controls.Add(commandSelector);
            commandSelector.AddListener(this);

            this.eventActions = new EventActions(psaMovesetHandler);
            eventActions.Dock = DockStyle.Fill;
            optionsTabControl.TabPages[0].Controls.Add(eventActions);
            eventActions.AddListener(this);
        }

        public void OnCodeBlockSelected(string sectionText, CodeBlockSelection codeBlockSelection)
        {
            TabPage existingTabPage = FindExistingTabPage(codeBlockSelection);

            if (existingTabPage == null) 
            {
                TabPage codeBlockCommandsTab = new TabPage(sectionText);
                CodeBlockViewer codeBlockViewer = new CodeBlockViewer(psaMovesetHandler, psaCommandsConfig, codeBlockSelection);
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

        public TabPage FindExistingTabPage(CodeBlockSelection codeBlockSelection)
        {
            foreach (TabPage tabPage in eventsTabControl.TabPages)
            {
                CodeBlockViewer codeBlockViewer = (CodeBlockViewer)tabPage.Controls["codeBlockViewer"];
                if (codeBlockViewer.CodeBlockSelection.SectionType == codeBlockSelection.SectionType
                    && codeBlockViewer.CodeBlockSelection.SectionIndex == codeBlockSelection.SectionIndex
                    && codeBlockViewer.CodeBlockSelection.CodeBlockIndex == codeBlockSelection.CodeBlockIndex)
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

        private void MovesetEditor_Load(object sender, EventArgs e)
        {
            //this.DoubleBuffered(true);
            //eventsTabControl.DoubleBuffered(true);
        }

        public void OnAppendCommand()
        {
            ActiveCodeBlockViewer?.AppendCommand(currentlySelectedCommandOption);
        }

        public void OnInsertCommandAbove()
        {
            ActiveCodeBlockViewer?.InsertCommandAbove(currentlySelectedCommandOption);
        }

        public void OnInsertCommandBelow()
        {
            ActiveCodeBlockViewer?.InsertCommandBelow(currentlySelectedCommandOption);
        }

        public void OnReplaceCommand()
        {
            ActiveCodeBlockViewer?.ReplaceCommand(currentlySelectedCommandOption);
        }

        public void OnMoveCommandUp()
        {
            ActiveCodeBlockViewer?.MoveCommandUp();
        }

        public void OnMoveCommandDown()
        {
            ActiveCodeBlockViewer?.MoveCommandDown();
        }

        public void OnRemoveCommand()
        {
            ActiveCodeBlockViewer?.RemoveCommand();
        }

        public void OnCommandSelected(PsaCommandConfig psaCommandConfig)
        {
            this.currentlySelectedCommandOption = psaCommandConfig;
        }
    }
}
