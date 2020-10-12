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

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class CodeBlockViewer : ObservableUserControl<ICodeBlockViewer>, ILocationSelectorListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        protected LocationType? currentLocationType;
        protected int currentLocationIndex = -1;
        protected int currentCodeBlockIndex = -1;

        public CodeBlockViewer(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
        }

        private void PopulateCodeBlockOptions()
        {
            if (currentLocationType == LocationType.ACTION)
            {
                codeBlockOptionsListBox.Items.Add("ENTRY");
                codeBlockOptionsListBox.Items.Add("EXIT");
            }
            else if (currentLocationType == LocationType.SUBACTION)
            {
                codeBlockOptionsListBox.Items.Add("MAIN");
                codeBlockOptionsListBox.Items.Add("GFX");
                codeBlockOptionsListBox.Items.Add("SFX");
                codeBlockOptionsListBox.Items.Add("OTHER");
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
            


            Console.WriteLine(locationType.ToString() + ", " + locationIndex);
        }

        public void LoadCodeBlockCommands()
        {
            codeBlockCommandsListBox.Items.Clear();
            Console.WriteLine(currentLocationIndex + ", " + currentCodeBlockIndex);
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
            foreach (PsaCommand psaCommand in psaCommands)
            {
                codeBlockCommandsListBox.Items.Add(psaCommand.ToString());
            }
        }

        private void CodeBlockViewer_Load(object sender, EventArgs e)
        {

        }

        private void codeBlockOptionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Console.WriteLine("Code Block Options Changed");
            if (codeBlockOptionsListBox.SelectedIndex != currentCodeBlockIndex)
            {
                currentCodeBlockIndex = codeBlockOptionsListBox.SelectedIndex;
                LoadCodeBlockCommands();
            }
        }
    }
}
