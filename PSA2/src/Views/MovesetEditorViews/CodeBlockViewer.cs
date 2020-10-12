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
    public partial class CodeBlockViewer : ObservableUserControl<ICodeBlockViewer>, ILocationSelectorListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        protected LocationType? currentLocationType;
        protected int currentLocationIndex = -1;
        protected int currentCodeBlockIndex = -1;
        protected PsaCommandsConfig psaCommandsConfig;

        public CodeBlockViewer(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
        }

        private void PopulateCodeBlockOptions()
        {
            if (currentLocationType == LocationType.ACTION)
            {
                codeBlockOptionsListBox.Items.Add("Entry");
                codeBlockOptionsListBox.Items.Add("Exit");
            }
            else if (currentLocationType == LocationType.SUBACTION)
            {
                codeBlockOptionsListBox.Items.Add("Main");
                codeBlockOptionsListBox.Items.Add("Gfx");
                codeBlockOptionsListBox.Items.Add("Sfx");
                codeBlockOptionsListBox.Items.Add("Other");
            }
        }

        public void OnSelect(LocationType locationType, int locationIndex)
        {
            if (currentLocationType != locationType)
            {
                currentLocationType = locationType;
                codeBlockOptionsListBox.Items.Clear();
                PopulateCodeBlockOptions();
                codeBlockOptionsListBox.SelectedIndex = 0;
            }
            currentLocationIndex = locationIndex;

            currentCodeBlockIndex = codeBlockOptionsListBox.SelectedIndex;
            LoadCodeBlockCommands();
        }

        public void LoadCodeBlockCommands()
        {
            codeBlockCommandsListBox.Items.Clear();

            List<PsaCommand> psaCommands = null;
            switch (currentLocationType)
            {
                case LocationType.ACTION:
                    psaCommands = psaMovesetHandler.ActionsHandler.GetPsaCommandsInCodeBlock(currentLocationIndex, currentCodeBlockIndex);
                    break;
                case LocationType.SUBACTION:
                    psaCommands = psaMovesetHandler.SubActionsHandler.GetPsaCommandsForSubAction(currentLocationIndex, currentCodeBlockIndex);
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
            this.psaCommandsConfig = Utils.LoadJson<PsaCommandsConfig>("data/psa_command_data.json");

            foreach (PsaCommandConfig psaCommandConfig in psaCommandsConfig.PsaCommands)
            {
                commandOptionsListBox.Items.Add(psaCommandConfig.CommandName);
            }
        }

        private void codeBlockOptionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (codeBlockOptionsListBox.SelectedIndex != currentCodeBlockIndex)
            {
                currentCodeBlockIndex = codeBlockOptionsListBox.SelectedIndex;
                LoadCodeBlockCommands();
            }
        }
    }
}
