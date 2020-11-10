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

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class SubActionSelector : ObservableUserControl<ISectionSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;

        public SubActionSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
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
                UpdateAnimationData();
            }
        }

        private void subActionsListScintilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateAnimationData();
        }

        private void UpdateAnimationData()
        {
            Animation animation = psaMovesetHandler.SubActionsHandler.GetSubActionAnimationData(subActionsListScintilla.SelectedIndex);
            animationNameTextBox.Text = animation.AnimationName;
            inTransitionTextBox.Text = animation.AnimationFlags.InTransition.ToString();
            noOutTransitionCheckBox.Checked = animation.AnimationFlags.NoOutTransition != 0;
            loopCheckBox.Checked = animation.AnimationFlags.Loop != 0;
            movesCharacterCheckBox.Checked = animation.AnimationFlags.MovesCharacter != 0;
            unknown3CheckBox.Checked = animation.AnimationFlags.Unknown3 != 0;
            unknown4CheckBox.Checked = animation.AnimationFlags.Unknown4 != 0;
            unknown5CheckBox.Checked = animation.AnimationFlags.Unknown5 != 0;
            transitionOutFromStartCheckBox.Checked = animation.AnimationFlags.TransitionOutFromStart != 0;
            unknown7CheckBox.Checked = animation.AnimationFlags.Unknown7 != 0;
        }
    }
}
