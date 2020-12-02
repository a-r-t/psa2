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
using System.Net;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Views.CustomControls;
using PSA2.src.Views.MovesetEditorViews.SectionSelectors;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class MovesetEditor: ObservableUserControl<IMovesetEditorListener>, 
        ISectionSelectorListener, 
        IEventActionsListener,
        ICommandSelectorListener,
        IParametersEditorListener
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
                    TabPageCustom selectedTabPage = eventsTabControl.TabPages[eventsTabControl.SelectedIndex];
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

            this.parametersEditor = new ParametersEditor(psaMovesetHandler, psaCommandsConfig);
            parametersEditor.Dock = DockStyle.Fill;
            parametersEditorViewer.Controls.Add(parametersEditor);
            parametersEditor.AddListener(this);

            this.commandSelector = new CommandSelector(psaMovesetHandler, psaCommandsConfig);
            commandSelector.Dock = DockStyle.Fill;
            commandOptionsViewer.Controls.Add(commandSelector);
            commandSelector.AddListener(this);

            this.eventActions = new EventActions(psaMovesetHandler);
            eventActions.Dock = DockStyle.Fill;
            optionsTabControl.TabPages[0].Controls.Add(eventActions);
            eventActions.AddListener(this);
        }

        public void OnCodeBlockSelected(CodeBlockSelection codeBlockSelection)
        {
            TabPageCustom existingTabPage = FindExistingTabPage(codeBlockSelection);
            if (existingTabPage == null)
            {
                TabPageCustom currentTabPage = eventsTabControl.TabPages[eventsTabControl.CurrentTabIndex];
                currentTabPage.TabText = codeBlockSelection.ToSectionString();
                currentTabPage.Controls.Clear();
                CodeBlockViewer codeBlockViewer = new CodeBlockViewer(psaMovesetHandler, psaCommandsConfig, codeBlockSelection);
                codeBlockViewer.Name = "codeBlockViewer";
                codeBlockViewer.Dock = DockStyle.Fill;
                codeBlockViewer.AddListener(parametersEditor);
                currentTabPage.Controls.Add(codeBlockViewer);
            }
            else
            {
                eventsTabControl.CurrentTabIndex = existingTabPage.TabIndex;
            }
            parametersEditor.Enabled = true;
        }

        public TabPageCustom FindExistingTabPage(CodeBlockSelection codeBlockSelection)
        {
            foreach (TabPageCustom tabPage in eventsTabControl.TabPages)
            {
                CodeBlockViewer codeBlockViewer = (CodeBlockViewer)tabPage.Controls["codeBlockViewer"];
                if (codeBlockViewer != null
                    && codeBlockViewer.CodeBlockSelection.SectionType == codeBlockSelection.SectionType
                    && codeBlockViewer.CodeBlockSelection.SectionIndex == codeBlockSelection.SectionIndex
                    && codeBlockViewer.CodeBlockSelection.CodeBlockIndex == codeBlockSelection.CodeBlockIndex)
                {
                    return tabPage;
                }
            }
            return null;
        }

        private void eventsTabControl_SelectedTabIndexChanged(object sender, EventArgs e)
        {
            CodeBlockViewer activeCodeBlockViewer = ActiveCodeBlockViewer;
            if (activeCodeBlockViewer != null)
            {
                activeCodeBlockViewer.EmitCommandSelected();
                parametersEditor.Enabled = true;
            }
            else
            {
                parametersEditor.Enabled = false;
            }
        }

        private void eventsTabControl_Click(object sender, EventArgs e)
        {
            this.ActiveControl = commandOptionsViewer;
        }

        private void MovesetEditor_Load(object sender, EventArgs e)
        {
            //this.DoubleBuffered(true);
            //eventsTabControl.DoubleBuffered(true);
            TabPageCustom tabPage = new TabPageCustom();
            tabPage.TabText = "New Tab";
            eventsTabControl.TabPages.Add(tabPage);
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

        public void OnParameterChange(int commandIndex, PsaCommand psaCommand)
        {
            ActiveCodeBlockViewer?.ModifyCommand(commandIndex, psaCommand);
        }
    }
}
