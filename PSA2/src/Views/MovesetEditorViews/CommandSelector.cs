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

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class CommandSelector : ObservableUserControl<ICommandSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        protected PsaCommandsConfig psaCommandsConfig;
        private BindingList<string> commands = new BindingList<string>();

        public CommandSelector(PsaMovesetHandler psaMovesetHandler, PsaCommandsConfig psaCommandsConfig)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = psaCommandsConfig;
            InitializeComponent();
        }

        private void CommandSelector_Load(object sender, EventArgs e)
        {
            foreach (PsaCommandConfig psaCommandConfig in psaCommandsConfig.PsaCommands)
            {
                commands.Add(psaCommandConfig.CommandName);
            }

            commandsListBox.DataSource = commands;
        }
    }
}