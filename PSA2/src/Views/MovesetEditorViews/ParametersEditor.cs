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
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PropertyGridEx;
using PSA2.src.Views.MovesetEditorViews.Interfaces;
using PSA2.src.Views.CustomControls;
using System.Net;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class ParametersEditor : ObservableUserControl<IParametersEditorListener>, 
        ICodeBlockViewerListener,
        IMovesetEditorListener,
        IParameterEditorListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        public PsaCommandConfig PsaCommandConfig { get; private set; }
        public PsaCommand PsaCommand { get; private set; }
        public CodeBlockSelection CodeBlockSelection { get; private set; }
        public int CommandIndex { get; private set; }
        private int previousParameterSelected = -1;
        protected PsaCommandsConfig psaCommandsConfig;

        public ParametersEditor(PsaMovesetHandler psaMovesetHandler, PsaCommandsConfig psaCommandsConfig)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = psaCommandsConfig;
            InitializeComponent();
            parameterEditor.AddListener(this);
        }

        public void OnCommandSelected(List<PsaCommand> psaCommands, List<int> commandIndexes, CodeBlockSelection codeBlockSelection)
        {
            parameterEditor.ParameterEntry = null;
            parameterNamesListBox.Items.Clear();

            if (psaCommands != null && psaCommands.Count == 1)
            {
                PsaCommand psaCommand = psaCommands[0];

                PsaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);

                CodeBlockSelection = codeBlockSelection;
                CommandIndex = commandIndexes[0];
                PsaCommand = psaCommand;
                previousParameterSelected = -1;
                PopulateParameterNames(psaCommand);
            }
        }

        private void PopulateParameterNames(PsaCommand psaCommand)
        {
            parameterNamesListBox.Items.Clear();
            if (PsaCommandConfig != null && PsaCommandConfig.CommandParams != null)
            {
                for (int i = 0; i < PsaCommandConfig.CommandParams.Count; i++)
                {
                    PsaCommandParamConfig psaCommandParamConfig = PsaCommandConfig.CommandParams[i];
                    parameterNamesListBox.Items.Add(psaCommandParamConfig.ParamName);
                }
            }
            else
            {
                for (int i = 0; i < psaCommand.Parameters.Count; i++)
                {
                    parameterNamesListBox.Items.Add($"arg{i}");
                }
            }

            if (parameterNamesListBox.Items.Count > 0)
            {
                parameterNamesListBox.SelectedIndex = 0;
            }
        }

        private void parameterNamesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedParameterIndex = parameterNamesListBox.SelectedIndex;
            if (selectedParameterIndex != previousParameterSelected)
            {
                int type = PsaCommand.Parameters[selectedParameterIndex].Type;
                int value = PsaCommand.Parameters[selectedParameterIndex].Value;
                ParameterEntry parameterEntry = new ParameterEntry(parameterNamesListBox.SelectedItem.ToString(), type, value);
                parameterEditor.ParameterEntry = parameterEntry;
            }
            previousParameterSelected = selectedParameterIndex;
        }

        public void OnParameterChange(int type, int value)
        {
            int selectedParameterIndex = parameterNamesListBox.SelectedIndex;
            PsaCommand.Parameters[selectedParameterIndex].Type = type;
            PsaCommand.Parameters[selectedParameterIndex].Value = value;

            foreach (IParametersEditorListener listener in listeners)
            {
                listener.OnParameterChange(CommandIndex, PsaCommand);
            }
        }
    }
}
