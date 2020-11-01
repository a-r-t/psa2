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

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class ParametersEditor : ObservableUserControl<IParametersEditorListener>, ICodeBlockViewerListener
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
            parametersPropertyGrid.PropertySort = PropertySort.CategorizedAlphabetical;
        }

        public void OnCommandSelected(List<PsaCommand> psaCommands, CodeBlockSelection codeBlockSelection)
        {
            if (psaCommands != null && psaCommands.Count == 1)
            {
                PsaCommand psaCommand = psaCommands[0];

                PsaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);

                this.codeBlockSelection = codeBlockSelection;
                PsaCommand = psaCommand;

                parametersPropertyGrid.MoveSplitterTo((int)(parametersPropertyGrid.Width * .5));

                // This code allows you to put a rich text box inside the property grid - holding on to this for later
                //foreach (Control control in parametersPropertyGrid.DocComment.Controls)
                //{
                //    Console.WriteLine(control.ToString());
                //}
                //parametersPropertyGrid.DocComment.Controls.ToString();
                //parametersPropertyGrid.DocComment.Controls.Clear();
                //parametersPropertyGrid.DocComment.Controls.Add(new RichTextBox() { Dock = DockStyle.Fill, Text = "HI" });

                parametersPropertyGrid.Item.Clear();

                if (PsaCommandConfig != null && PsaCommandConfig.CommandParams != null)
                {
                    for (int i = 0; i < PsaCommandConfig.CommandParams.Count; i++)
                    {
                        PsaCommandParamConfig psaCommandParamConfig = PsaCommandConfig.CommandParams[i];
                        parametersPropertyGrid.Item.Add(
                            psaCommandParamConfig.ParamName,
                            psaCommand.Parameters[i].Value,
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
                            psaCommand.Parameters[i].Value,
                            false,
                            "Parameter",
                            "N/A",
                            true
                        );
                    }
                }

                /*
                 * How to add a drop down to the property grid

                parametersPropertyGrid.Item.Add("Language", "", false, "Misc", "", true);
                string[] choices = new string[80];
                for (int i = 0; i < 80; i++)
                {
                    choices[i] = $"{i}";
                }
                parametersPropertyGrid.Item[parametersPropertyGrid.Item.Count - 1].Choices = new CustomChoices(choices);
                */

                parametersPropertyGrid.Refresh();
                parametersPropertyGrid.Enabled = false;
                parametersPropertyGrid.Enabled = true;
            }
            else
            {
                parametersPropertyGrid.Item.Clear();
                parametersPropertyGrid.Refresh();
                parametersPropertyGrid.Enabled = false;
                parametersPropertyGrid.Enabled = true;
            }
        }

        private void parametersPropertyGrid_Resize(object sender, EventArgs e)
        {
            parametersPropertyGrid.MoveSplitterTo((int)(parametersPropertyGrid.Width * .5));
        }
    }
}
