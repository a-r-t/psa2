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
using PSA2.src.Views.MovesetEditorViews.Interfaces;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class SectionSelector : ObservableUserControl<ISectionSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        private string[] sections = new string[] { "Actions", "Sub Actions" };
        private ActionSelector actionSelector;
        private SubActionSelector subActionSelector;

        public SectionSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();

            actionSelector = new ActionSelector(psaMovesetHandler);
            actionSelector.Dock = DockStyle.Fill;
            actionSelector.Visible = false;
            actionSelector.Name = "actionSelector";

            subActionSelector = new SubActionSelector(psaMovesetHandler);
            subActionSelector.Dock = DockStyle.Fill;
            subActionSelector.Visible = false;
            subActionSelector.Name = "subActionSelector";

            sectionSelectorFormViewer.Controls.Add(actionSelector);
            sectionSelectorFormViewer.Controls.Add(subActionSelector);

        }

        private void LocationSelector_Load(object sender, EventArgs e)
        {
            sectionComboBox.Items.AddRange(sections);
            sectionComboBox.SelectedIndex = 0;
            /*
            sectionsTreeView.Nodes.Add("Actions");
            int numberOfSpecialActions = psaMovesetHandler.ActionsHandler.GetNumberOfSpecialActions();
            for (int i = 0; i < numberOfSpecialActions; i++)
            {
                sectionsTreeView.Nodes[0].Nodes.Add((i + 274).ToString("X"));

                sectionsTreeView.Nodes[0].Nodes[i].Nodes.Add("Entry");
                sectionsTreeView.Nodes[0].Nodes[i].Nodes.Add("Exit");
            }
            sectionsTreeView.Nodes[0].Nodes[0].Text += " - Neutral Special";
            sectionsTreeView.Nodes[0].Nodes[1].Text += " - Side Special";
            sectionsTreeView.Nodes[0].Nodes[2].Text += " - Up Special";
            sectionsTreeView.Nodes[0].Nodes[3].Text += " - Down Special";

            sectionsTreeView.Nodes.Add("Sub Actions");
            int numberOfSubActions = psaMovesetHandler.SubActionsHandler.GetNumberOfSubActions();
            for (int i = 0; i < numberOfSubActions; i++)
            {
                string animationName = psaMovesetHandler.SubActionsHandler.GetSubActionAnimationName(i);
                sectionsTreeView.Nodes[1].Nodes.Add(i.ToString("X") + " - " + animationName);

                sectionsTreeView.Nodes[1].Nodes[i].Nodes.Add("Main");
                sectionsTreeView.Nodes[1].Nodes[i].Nodes.Add("GFX");
                sectionsTreeView.Nodes[1].Nodes[i].Nodes.Add("SFX");
                sectionsTreeView.Nodes[1].Nodes[i].Nodes.Add("Other");
            }
            sectionsTreeView.SelectedNode = sectionsTreeView.Nodes[0];
            */
        }

        /*
        private void sectionsTreeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (sectionsTreeView.SelectedNode.Nodes.Count == 0)
            {
                TreeNode node = sectionsTreeView.SelectedNode;
                List<string> sectionsText = new List<string>();
                while (node.Parent != null)
                {
                    sectionsText.Insert(0, node.Text);
                    node = node.Parent;
                }

                sectionsText.Insert(0, node.Text);

                SectionType sectionType;
                switch (node.Index) {
                    case 0:
                        sectionType = SectionType.ACTION;
                        break;
                    case 1:
                        sectionType = SectionType.SUBACTION;
                        break;
                    default:
                        throw new ArgumentException("Invalid section type");
                }

                string sectionText = string.Join(" - ", sectionsText);

                CodeBlockSelection codeBlockSelection = new CodeBlockSelection(psaMovesetHandler);
                codeBlockSelection.SectionType = sectionType;
                codeBlockSelection.SectionIndex = sectionsTreeView.SelectedNode.Parent.Index;
                codeBlockSelection.CodeBlockIndex = sectionsTreeView.SelectedNode.Index;

                foreach (ISectionSelectorListener listener in listeners)
                {
                    listener.OnCodeBlockSelected(
                        sectionText,
                        codeBlockSelection
                    );
                }
            }
        }
        */

        private void sectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            sectionSelectorFormViewer.Controls.Clear();
            switch (sectionComboBox.SelectedItem)
            {
                case "Actions":
                    ActionSelector actionSelector = new ActionSelector(psaMovesetHandler);
                    actionSelector.Dock = DockStyle.Fill;
                    sectionSelectorFormViewer.Controls.Add(actionSelector);
                    break;
                case "Sub Actions":
                    SubActionSelector subActionSelector = new SubActionSelector(psaMovesetHandler);
                    subActionSelector.Dock = DockStyle.Fill;
                    sectionSelectorFormViewer.Controls.Add(subActionSelector);
                    break;
            }
            */

            switch (sectionComboBox.SelectedItem)
            {
                case "Actions":
                    sectionSelectorFormViewer.Controls["actionSelector"].Visible = true;
                    sectionSelectorFormViewer.Controls["subActionSelector"].Visible = false;
                    break;
                case "Sub Actions":
                    sectionSelectorFormViewer.Controls["actionSelector"].Visible = false;
                    sectionSelectorFormViewer.Controls["subActionSelector"].Visible = true;
                    break;
            }
        }
    }
}
