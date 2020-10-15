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
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.Utility;
using PSA2.src.ExtentionMethods;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class CodeBlockViewer : ObservableUserControl<ICodeBlockViewerListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        public SectionSelectionInfo SectionSelectionInfo { get; private set; }
        protected PsaCommandsConfig psaCommandsConfig;
        private List<string> commandTexts = new List<string>();

        public CodeBlockViewer(PsaMovesetHandler psaMovesetHandler, PsaCommandsConfig psaCommandsConfig, SectionSelectionInfo sectionSelectionInfo)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = psaCommandsConfig;
            SectionSelectionInfo = sectionSelectionInfo;
            InitializeComponent();
        }

        public void LoadCodeBlockCommands()
        {
            codeBlockCommandsListBox.Items.Clear();

            List<PsaCommand> psaCommands = null;
            switch (SectionSelectionInfo.SectionType)
            {
                case SectionType.ACTION:
                    psaCommands = psaMovesetHandler.ActionsHandler.GetPsaCommandsInCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex);
                    break;
                case SectionType.SUBACTION:
                    psaCommands = psaMovesetHandler.SubActionsHandler.GetPsaCommandsForSubAction(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex);
                    break;
            }
            foreach (PsaCommand psaCommand in psaCommands) {
                string commandText = GetCommandText(psaCommand);
                //codeBlockCommandsListBox.Items.Add(commandText);
                commandTexts.Add(commandText);
            }
            codeBlockCommandsListBox.DataSource = commandTexts;

        }

        public string GetCommandText(PsaCommand psaCommand)
        {
            PsaCommandConfig psaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);

            StringBuilder commandTextBuilder = new StringBuilder();

            if (psaCommandConfig != null)
            {
                commandTextBuilder
                    .Append(psaCommandConfig.CommandName);

                if (psaCommand.Parameters.Count > 0)
                {
                    commandTextBuilder.Append(": ");
                }

                for (int i = 0; i < psaCommand.Parameters.Count; i++)
                {
                    commandTextBuilder
                        .Append(psaCommandConfig.CommandParams[i].ParamName)
                        .Append("=")
                        .Append(psaCommand.Parameters[i].Value);

                    if (i < psaCommand.Parameters.Count - 1)
                    {
                        commandTextBuilder.Append(", ");
                    }
                }
            }
            else
            {
                commandTextBuilder
                    .Append(psaCommand.Instruction);

                if (psaCommand.Parameters.Count > 0)
                {
                    commandTextBuilder.Append(": ");
                }

                for (int i = 0; i < psaCommand.Parameters.Count; i++)
                {
                    commandTextBuilder
                        .Append($"arg{i}=")
                        .Append(psaCommand.Parameters[i].Value);

                    if (i < psaCommand.Parameters.Count - 1)
                    {
                        commandTextBuilder.Append(", ");
                    }

                }
            }
            return commandTextBuilder.ToString();
        }

        private void CodeBlockViewer_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered(true);
            splitContainer1.DoubleBuffered(true);
            splitContainer1.Panel1.DoubleBuffered(true);
            codeBlockCommandsListBox.DoubleBuffered(true);

            LoadCodeBlockCommands();
            if (codeBlockCommandsListBox.Items.Count > 0)
            {
                codeBlockCommandsListBox.SelectedIndex = 0;
            }
/*            foreach (PsaCommandConfig psaCommandConfig in psaCommandsConfig.PsaCommands)
            {
                commandOptionsListBox.Items.Add(psaCommandConfig.CommandName);
            }*/
        }

        private void codeBlockOptionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
/*            if (codeBlockOptionsListBox.SelectedIndex != currentCodeBlockIndex)
            {
                currentCodeBlockIndex = codeBlockOptionsListBox.SelectedIndex;
                LoadCodeBlockCommands();
            }*/
        }

        private void codeBlockCommandsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (codeBlockCommandsListBox.SelectedIndex != SectionSelectionInfo.CommandIndex)
            {
                PsaCommand psaCommand;
                switch (SectionSelectionInfo.SectionType)
                {
                    case SectionType.ACTION:
                        psaCommand = psaMovesetHandler.ActionsHandler.GetPsaCommandInCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, codeBlockCommandsListBox.SelectedIndex);
                        break;
                    case SectionType.SUBACTION:
                        psaCommand = psaMovesetHandler.SubActionsHandler.GetPsaCommandForSubActionCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, codeBlockCommandsListBox.SelectedIndex);
                        break;
                    default:
                        throw new ArgumentException("Invalid section type");
                }

                PsaCommandConfig psaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);
                SectionSelectionInfo.CommandIndex = codeBlockCommandsListBox.SelectedIndex;
                foreach (ICodeBlockViewerListener listener in listeners)
                {
                    listener.OnCommandSelected(psaCommandConfig, psaCommand, SectionSelectionInfo);
                }
            }
        }

        private void codeBlockCommandsListBox_Leave(object sender, EventArgs e)
        {
            //codeBlockCommandsListBox.Update();
        }

        private void codeBlockCommandsListBox_Enter(object sender, EventArgs e)
        {

        }

        private void codeBlockCommandsListBox_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
