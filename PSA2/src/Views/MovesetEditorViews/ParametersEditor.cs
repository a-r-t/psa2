﻿using System;
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
        public PsaCommand PsaCommand { get; private set; }
        public SectionSelectionInfo SectionSelectionInfo { get; private set; }

        public ParametersEditor(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
            parametersPropertyGrid.PropertySort = PropertySort.CategorizedAlphabetical;
        }

        public void OnCommandSelected(PsaCommandConfig psaCommandConfig, PsaCommand psaCommand, SectionSelectionInfo sectionSelectionInfo)
        {
            PsaCommandConfig = psaCommandConfig;
            SectionSelectionInfo = sectionSelectionInfo;
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
            parametersPropertyGrid.Enabled = false;
            parametersPropertyGrid.Enabled = true;


        }

        private void parametersPropertyGrid_Resize(object sender, EventArgs e)
        {
            parametersPropertyGrid.MoveSplitterTo((int)(parametersPropertyGrid.Width * .5));
        }
    }
}