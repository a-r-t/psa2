using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSA2.src.Views.MovesetEditorViews.Interfaces;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class ActionSelector : ObservableUserControl<ISectionSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;

        public ActionSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
            actionsListScintilla.FontFamily = "Tahoma";
        }

        private void ActionSelector_Load(object sender, EventArgs e)
        {
            int numberOfSpecialActions = psaMovesetHandler.ActionsHandler.GetNumberOfSpecialActions();
            List<string> actionNames = new List<string>();
            for (int i = 0; i < numberOfSpecialActions; i++)
            {
                actionNames.Add((i + 274).ToString("X"));
            }
            actionNames[0] += " - Neutral Special";
            actionNames[1] += " - Side Special";
            actionNames[2] += " - Up Special";
            actionNames[3] += " - Down Special";

            actionsListScintilla.AddItems(actionNames);
            //UpdateSectionSelection();
        }

        private void actionsListScintilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSectionSelection();
        }

        private void UpdateSectionSelection()
        {
            CodeBlockSelection codeBlockSelection = new CodeBlockSelection(psaMovesetHandler);
            codeBlockSelection.SectionType = SectionType.ACTION;
            codeBlockSelection.SectionIndex = actionsListScintilla.SelectedIndex;
            string item = actionsListScintilla.Items[actionsListScintilla.SelectedIndex];
            int nameStartIndex = item.IndexOf(" - ");
     
            string name = nameStartIndex != -1
                ? item.Substring(nameStartIndex + 3)
                : "";

            foreach (ISectionSelectorListener listener in listeners)
            {
                listener.OnCodeBlockSelected(name, codeBlockSelection);
            }
        }

        private void ActionSelector_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                UpdateSectionSelection();
            }
        }
    }
}
