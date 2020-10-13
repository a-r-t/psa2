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
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PropertyGridEx;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class ParametersEditor : ObservableUserControl<IParametersEditorListener>, ICodeBlockViewerListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        public PsaCommandConfig PsaCommandConfig { get; private set; }
        public SectionType SectionType { get; private set; }
        public int SectionIndex { get; private set; }
        public int CodeBlockIndex { get; private set;  }
        public int CommandIndex { get; private set; }

        public ParametersEditor(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
        }

        public void OnCommandSelected(PsaCommandConfig psaCommandConfig, PsaCommand psaCommand, SectionType sectionType, int sectionIndex, int codeBlockIndex, int commandIndex)
        {
            PsaCommandConfig = psaCommandConfig;
            SectionType = sectionType;
            SectionIndex = sectionIndex;
            CodeBlockIndex = codeBlockIndex;
            CommandIndex = commandIndex;

            parametersPropertyGrid.Item.Clear();

            if (psaCommandConfig != null && psaCommandConfig.CommandParams != null)
            {
                for (int i = 0; i < psaCommandConfig.CommandParams.Count; i++)
                {
                    PsaCommandParamConfig psaCommandParamConfig = psaCommandConfig.CommandParams[i];
                    parametersPropertyGrid.Item.Add(
                        psaCommandParamConfig.ParamName, 
                        psaCommand.Parameters[i].Type,
                        false,
                        "Parameter",
                        psaCommandParamConfig.Description,
                        true
                    );
                }
            }
            else
            {
                for (int i = 0; i < psaCommand.Parameters.Count; i++)
                {
                    parametersPropertyGrid.Item.Add(
                        $"arg{i}",
                        psaCommand.Parameters[i].Type,
                        false,
                        "Parameter",
                        "N/A",
                        true
                    );
                }
            }
            parametersPropertyGrid.Refresh();
        }
    }
}
