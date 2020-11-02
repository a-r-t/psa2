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

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class ParametersEditor : ObservableUserControl<IParametersEditorListener>, 
        ICodeBlockViewerListener,
        IMovesetEditorListener,
        IParameterEntryUserControlListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        public PsaCommandConfig PsaCommandConfig { get; private set; }
        public PsaCommand PsaCommand { get; private set; }
        public CodeBlockSelection codeBlockSelection { get; private set; }
        protected PsaCommandsConfig psaCommandsConfig;

        public ParametersEditor(PsaMovesetHandler psaMovesetHandler, PsaCommandsConfig psaCommandsConfig)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = psaCommandsConfig;
            InitializeComponent();
            parametersPanel.AddOnChangeListener(this);
        }

        public void OnCommandSelected(List<PsaCommand> psaCommands, CodeBlockSelection codeBlockSelection)
        {
            parametersPanel.ClearParameterEntries();

            if (psaCommands != null && psaCommands.Count == 1)
            {
                PsaCommand psaCommand = psaCommands[0];

                PsaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);

                this.codeBlockSelection = codeBlockSelection;
                PsaCommand = psaCommand;

                parametersPanel.ClearParameterEntries();
                PopulatePropertyGrid(psaCommand);
                parametersPanel.Reload();
            }

            parametersPanel.Reload();

        }

        private void PopulatePropertyGrid(PsaCommand psaCommand)
        {
            if (PsaCommandConfig != null && PsaCommandConfig.CommandParams != null)
            {
                HashSet<string> usedCategoryNames = new HashSet<string>();

                for (int i = 0; i < PsaCommandConfig.CommandParams.Count; i++)
                {
                    PsaCommandParamConfig psaCommandParamConfig = PsaCommandConfig.CommandParams[i];

                    string categoryName = psaCommandParamConfig.ParamName;
                    int index = 1;
                    while (usedCategoryNames.Contains(categoryName))
                    {
                        categoryName = $"psaCommandParamConfig.ParamName {index}";
                    }
                    usedCategoryNames.Add(categoryName);

                    ParameterEntry parameterEntry = new ParameterEntry(categoryName, psaCommand.Parameters[i].Type, psaCommand.Parameters[i].Value);
                    parametersPanel.AddParameterEntry(parameterEntry);
                }
            }
            else
            {
                for (int i = 0; i < psaCommand.Parameters.Count; i++)
                {
                    ParameterEntry parameterEntry = new ParameterEntry($"arg{i}", psaCommand.Parameters[i].Type, psaCommand.Parameters[i].Value);
                    parametersPanel.AddParameterEntry(parameterEntry);
                }
            }
        }

        private void parametersPanel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
        }

        public void OnParameterChange(bool isDirty)
        {
            applyButton.Enabled = isDirty;
        }
    }
}
