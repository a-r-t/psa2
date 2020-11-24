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

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class SubActionSelector : ObservableUserControl<ISectionSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;

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
            for (int i = 0; i < numberOfSubActions; i++)
            {
                string animationName = psaMovesetHandler.SubActionsHandler.GetSubActionAnimationName(i);
                subActionsNames.Add(i.ToString("X") + " - " + animationName);
            }
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
            codeBlockSelection.SectionIndex = subActionsListScintilla.SelectedIndex;

            foreach (ISectionSelectorListener listener in listeners)
            {
                listener.OnCodeBlockSelected(animationNameTextBox.Text, codeBlockSelection);
            }
        }

        private void subActionsListScintilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSectionSelection();
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
    }
}
