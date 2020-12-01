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
using PSA2.src.Views.MovesetEditorViews.Interfaces;
using PSA2.src.Views.CustomControls;
using System.Net;
using PSA2.src.ExtentionMethods;
using ScintillaNET;
using PSA2.src.Views.MovesetEditorViews.ParameterEditorForms;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class ParametersEditor : ObservableUserControl<IParametersEditorListener>, 
        ICodeBlockViewerListener,
        IMovesetEditorListener,
        IParameterEditorFormListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        public PsaCommandConfig PsaCommandConfig { get; private set; }
        public PsaCommand PsaCommand { get; private set; }
        public CodeBlockSelection CodeBlockSelection { get; private set; }
        public int CommandIndex { get; private set; }
        private int previousParameterSelected = -1;
        protected PsaCommandsConfig psaCommandsConfig;
        private const int Hex = 0;
        private const int Scalar = 1;
        private const int Pointer = 2;
        private const int Boolean = 3;
        private const int Unknown4 = 4;
        private const int Variable = 5;
        private const int Condition = 6;
        private string[] parameterTypes = new string[] { "Hex", "Scalar", "Pointer", "Boolean", "(4)", "Variable", "Condition" };


        public ParametersEditor(PsaMovesetHandler psaMovesetHandler, PsaCommandsConfig psaCommandsConfig)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = psaCommandsConfig;
            InitializeComponent();
            parameterTypesComboBox.Items.AddRange(parameterTypes);
            parameterEditorFormView.DoubleBuffered(true);
            this.DoubleBuffered(true);
            parameterTypeViewer.Visible = false;
            parameterNamesScintilla.Visible = false;
        }

        public void OnCommandSelected(List<PsaCommand> psaCommands, List<int> commandIndexes, CodeBlockSelection codeBlockSelection)
        {
            // TODO: Find out why this activates twice because it really shouldn't
            parameterNamesScintilla.ClearItems();

            if (psaCommands != null && psaCommands.Count == 1 && psaCommands[0].Parameters.Count > 0)
            {
                PsaCommand psaCommand = psaCommands[0];

                PsaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);

                CodeBlockSelection = codeBlockSelection;
                CommandIndex = commandIndexes[0];
                PsaCommand = psaCommand;
                previousParameterSelected = -1;
                PopulateParameterNames(psaCommand);
                parameterTypeViewer.Visible = true;
                parameterNamesScintilla.Visible = true;
            }
            else
            {
                parameterEditorFormView.Controls.Clear();
                parameterTypeViewer.Visible = false;
                parameterNamesScintilla.Visible = false;
            }

            parameterNamesScintilla.StyleDocument();
            
        }

        private void PopulateParameterNames(PsaCommand psaCommand)
        {
            if (PsaCommandConfig != null && PsaCommandConfig.CommandParams != null)
            {
                for (int i = 0; i < PsaCommandConfig.CommandParams.Count; i++)
                {
                    PsaCommandParamConfig psaCommandParamConfig = PsaCommandConfig.CommandParams[i];
                    parameterNamesScintilla.AddItem(psaCommandParamConfig.ParamName);
                }
            }
            else
            {
                for (int i = 0; i < psaCommand.Parameters.Count; i++)
                {
                    parameterNamesScintilla.AddItem($"arg{i}");
                }
            }
        }

        private void parameterNamesScintilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedParameterIndex = parameterNamesScintilla.SelectedIndex;
            if (selectedParameterIndex >= 0)
            {
                if (selectedParameterIndex != previousParameterSelected)
                {
                    parameterTypesComboBox.SelectedIndex = -1;
                    parameterTypesComboBox.SelectedIndex = PsaCommand.Parameters[selectedParameterIndex].Type;
                }
                previousParameterSelected = selectedParameterIndex;
            }
        }

        private void parameterTypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (parameterTypesComboBox.SelectedIndex != -1)
            {
                int selectedParameterIndex = parameterNamesScintilla.SelectedIndex;
                int value = PsaCommand.Parameters[selectedParameterIndex].Value;
                parameterEditorFormView.SuspendLayout();
                parameterEditorFormView.Controls.Clear();
                ParameterEditorFormBase parameterEditorForm = GetParameterForm(parameterTypesComboBox.SelectedIndex, value);
                parameterEditorForm.Name = "parameterEditorForm";
                parameterEditorForm.Dock = DockStyle.Fill;
                parameterEditorForm.AddListener(this);
                parameterEditorFormView.Controls.Add(parameterEditorForm);
                parameterEditorFormView.ResumeLayout();

                if (PsaCommand.Parameters[selectedParameterIndex].Type != parameterTypesComboBox.SelectedIndex)
                {
                    PsaCommand.Parameters[selectedParameterIndex].Type = parameterTypesComboBox.SelectedIndex;
                    foreach (IParametersEditorListener listener in listeners)
                    {
                        listener.OnParameterChange(CommandIndex, PsaCommand);
                    }
                }
            }
        }

        private ParameterEditorFormBase GetParameterForm(int parameterType, int value)
        {
            switch (parameterType)
            {
                case Hex:
                    return new HexValueParameterEditorForm(value);
                case Scalar:
                    return new ScalarValueParameterEditorForm(value);
                case Pointer:
                    return new HexValueParameterEditorForm(value);
                case Boolean:
                    return new BooleanValueParameterEditorForm(value);
                case Unknown4:
                    return new HexValueParameterEditorForm(value);
                case Variable:
                    return new VariableValueParameterEditorForm(value);
                case Condition:
                    return new ConditionValueParameterEditorForm(value);
                default:
                    throw new ArgumentException("Unrecognized parameter type");
            }
        }

        public void OnParameterValueChange(int value)
        {
            int selectedParameterIndex = parameterNamesScintilla.SelectedIndex;
            PsaCommand.Parameters[selectedParameterIndex].Value = value;

            foreach (IParametersEditorListener listener in listeners)
            {
                listener.OnParameterChange(CommandIndex, PsaCommand);
            }
        }

        private void ParametersEditor_EnabledChanged(object sender, EventArgs e)
        {
            mainContainer.Visible = Enabled;
        }
    }
}
