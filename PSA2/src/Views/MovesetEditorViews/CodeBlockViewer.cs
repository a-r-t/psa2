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

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class CodeBlockViewer : ObservableUserControl<ICodeBlockViewer>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        public SectionType SectionType { get; private set; }
        public int SectionIndex { get; private set; }
        public int CodeBlockIndex { get; private set; }
        protected PsaCommandsConfig psaCommandsConfig;

        public CodeBlockViewer(PsaMovesetHandler psaMovesetHandler, SectionType sectionType, int sectionIndex, int codeBlockIndex)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = Utils.LoadJson<PsaCommandsConfig>("data/psa_command_data.json");
            SectionType = sectionType;
            SectionIndex = sectionIndex;
            CodeBlockIndex = codeBlockIndex;
            InitializeComponent();
        }

        public void LoadCodeBlockCommands()
        {
            codeBlockCommandsListBox.Items.Clear();

            List<PsaCommand> psaCommands = null;
            switch (SectionType)
            {
                case SectionType.ACTION:
                    psaCommands = psaMovesetHandler.ActionsHandler.GetPsaCommandsInCodeBlock(SectionIndex, CodeBlockIndex);
                    break;
                case SectionType.SUBACTION:
                    psaCommands = psaMovesetHandler.SubActionsHandler.GetPsaCommandsForSubAction(SectionIndex, CodeBlockIndex);
                    break;
            }
            foreach (PsaCommand psaCommand in psaCommands) {
                string commandText = GetCommandText(psaCommand);
                codeBlockCommandsListBox.Items.Add(commandText);
            }
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
            LoadCodeBlockCommands();
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
    }
}
