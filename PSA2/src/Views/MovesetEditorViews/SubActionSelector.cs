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
using PSA2MovesetLogic.src.Models.Fighter;
using PSA2MovesetLogic.src.ExtentionMethods;
using PSA2.src.ExtentionMethods;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class SubActionSelector : ObservableUserControl<ISectionSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        protected List<SubActionOption> subActionOptions;
        protected List<SubActionOption> filteredSubActionOptions;

        public SubActionSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
            subActionsListScintilla.FontFamily = "Tahoma";
        }

        private void SubActionSelector_Load(object sender, EventArgs e)
        {
            int numberOfSubActions = psaMovesetHandler.SubActionsHandler.GetNumberOfSubActions();
            List<string> subActionsNames = new List<string>();
            subActionOptions = new List<SubActionOption>();
            for (int i = 0; i < numberOfSubActions; i++)
            {
                string animationName = psaMovesetHandler.SubActionsHandler.GetAnimationName(i);
                subActionsNames.Add(i.ToString("X") + " - " + animationName);
                subActionOptions.Add(new SubActionOption(animationName, i));
            }
            filteredSubActionOptions = subActionOptions;
            subActionsListScintilla.AddItems(subActionsNames);

            if (subActionsListScintilla.Items.Count > 0)
            {
                subActionsListScintilla.SelectedIndex = 0;
                //UpdateAnimationData();
            }
        }

        private void UpdateSectionSelection()
        {
            UpdateAnimationData();

            CodeBlockSelection codeBlockSelection = new CodeBlockSelection(psaMovesetHandler);
            codeBlockSelection.SectionType = SectionType.SUBACTION;
            codeBlockSelection.SectionIndex = filteredSubActionOptions[subActionsListScintilla.SelectedIndex].Index;

            foreach (ISectionSelectorListener listener in listeners)
            {
                listener.OnCodeBlockSelected(codeBlockSelection);
            }
        }

        private void subActionsListScintilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subActionsListScintilla.Items.Count > 0)
            {
                UpdateSectionSelection();
            }
        }

        private void UpdateAnimationData()
        {
            Animation animation = psaMovesetHandler.SubActionsHandler.GetSubActionAnimationData(subActionsListScintilla.SelectedIndex);
            animationNameTextBox.Text = animation.AnimationName;
            inTransitionTextBox.Text = animation.AnimationFlags.InTransition.ToString();
            noOutTransitionCheckBox.Checked = animation.AnimationFlags.NoOutTransition.ToBoolean();
            loopCheckBox.Checked = animation.AnimationFlags.Loop.ToBoolean();
            movesCharacterCheckBox.Checked = animation.AnimationFlags.MovesCharacter.ToBoolean();
            unknown3CheckBox.Checked = animation.AnimationFlags.Unknown3.ToBoolean();
            unknown4CheckBox.Checked = animation.AnimationFlags.Unknown4.ToBoolean();
            unknown5CheckBox.Checked = animation.AnimationFlags.Unknown5.ToBoolean();
            transitionOutFromStartCheckBox.Checked = animation.AnimationFlags.TransitionOutFromStart.ToBoolean();
            unknown7CheckBox.Checked = animation.AnimationFlags.Unknown7.ToBoolean();
        }

        private void SubActionSelector_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                UpdateSectionSelection();
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = searchTextBox.Text.Length > 2
                ? searchTextBox.Text.Substring(2)
                : "";

            if (searchText != "")
            {
                filteredSubActionOptions = subActionOptions.FindAll(option => option.Name.ContainsIgnoreCase(searchText) || option.Index.ToString("X").ContainsIgnoreCase(searchText));
            }
            else
            {
                filteredSubActionOptions = subActionOptions;
            }
            subActionsListScintilla.ClearItems();
            subActionsListScintilla.AddItems(filteredSubActionOptions.Select(option => option.Index.ToString("X") + " - " + option.Name).ToList());
        }

        protected class SubActionOption
        {
            public string Name { get; set; }
            public int Index { get; set; }

            public SubActionOption(string name, int index)
            {
                Name = name;
                Index = index;
            }
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete))
            {
                if (searchTextBox.SelectionStart < 2)
                {
                    searchTextBox.SelectionStart = 2;
                }
                if (searchTextBox.SelectionStart == 2 && searchTextBox.SelectionLength == 0) {
                    e.SuppressKeyPress = true;
                }

            }
        }

        private void animationNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void inTransitionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void noOutTransitionCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void loopCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void movesCharacterCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void unknown3CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void unknown4CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void unknown5CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void transitionOutFromStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void unknown7CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
