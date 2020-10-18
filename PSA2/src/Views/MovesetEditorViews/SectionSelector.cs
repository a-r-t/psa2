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
using PSA2.src.Views.MovesetEditorViews.Interfaces;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class SectionSelector : ObservableUserControl<ISectionSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;

        public SectionSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
        }

        private void LocationSelector_Load(object sender, EventArgs e)
        {
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

        }

        private void sectionsTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Console.WriteLine("OLD EVENT");
            /*
            foreach (ILocationSelectorListener listener in listeners)
            {
                listener.OnSelect(LocationType.ACTION, sectionsTreeView.SelectedNode.Index);
            }
            */
        }

        private void sectionsTreeView_SelectedNodeChanged(object sender, TreeViewEventArgs e)
        {

        }

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

                SectionSelectionInfo sectionSelectionInfo = new SectionSelectionInfo(psaMovesetHandler);
                sectionSelectionInfo.SectionType = sectionType;
                sectionSelectionInfo.SectionIndex = sectionsTreeView.SelectedNode.Parent.Index;
                sectionSelectionInfo.CodeBlockIndex = sectionsTreeView.SelectedNode.Index;

                foreach (ISectionSelectorListener listener in listeners)
                {
                    listener.OnCodeBlockSelected(
                        sectionText,
                        sectionSelectionInfo
                    );
                }
            }
        }

        /*
        private void sectionsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationType locationType = 0;
            int selectedIndex = -1;
            if (sectionsTabControl.SelectedIndex == 0)
            {
                locationType = LocationType.ACTION;
                if (actionOptionsListBox.SelectedIndex == -1)
                {
                    actionOptionsListBox.SelectedIndex = 0;
                }
                selectedIndex = actionOptionsListBox.SelectedIndex;
            }
            else if (sectionsTabControl.SelectedIndex == 1)
            {
                locationType = LocationType.SUBACTION;
                if (subActionOptionsListBox.SelectedIndex == -1)
                {
                    subActionOptionsListBox.SelectedIndex = 0;
                }
                selectedIndex = subActionOptionsListBox.SelectedIndex;
            }

            foreach (ILocationSelectorListener listener in listeners)
            {
                listener.OnSelect(locationType, selectedIndex);
            }
        }*/
    }
}
