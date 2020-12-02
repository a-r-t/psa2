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
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Views.Utility.SearchList;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class SubActionSelector : ObservableUserControl<ISectionSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        private SubActionSearchList subActionSearchList;

        private SubActionOption SelectedSubActionOption
        {
            get
            {
                return subActionSearchList.FilteredItems[subActionsListScintilla.SelectedIndex];
            }
        }
        private bool ignoreAnimationChanges; // set to true when code is loading in selected subaction animation details rather than user changing them in order to prevent autosave code from triggering

        public SubActionSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
            subActionsListScintilla.FontFamily = "Tahoma";
            subActionSearchList = new SubActionSearchList(searchTextBox);
        }

        private void SubActionSelector_Load(object sender, EventArgs e)
        {
            int numberOfSubActions = psaMovesetHandler.SubActionsHandler.GetNumberOfSubActions();
            List<string> subActionsNames = new List<string>();
            List<SubActionOption> subActionOptions = new List<SubActionOption>();
            for (int i = 0; i < numberOfSubActions; i++)
            {
                string animationDisplayName = i.ToString("X");
                string animationName = psaMovesetHandler.SubActionsHandler.GetAnimationName(i);
                if (animationName != "")
                {
                    animationDisplayName += $" - {animationName}";
                }
                subActionsNames.Add(animationDisplayName);
                subActionOptions.Add(new SubActionOption(animationName, i));
            }

            subActionSearchList.Items = subActionOptions;
            subActionsListScintilla.AddItems(subActionsNames);

            if (subActionsListScintilla.Items.Count > 0)
            {
                subActionsListScintilla.SelectedIndex = 0;
            }
        }

        private void subActionsListScintilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (subActionsListScintilla.Items.Count > 0)
            {
                // ignoring animation changes means it won't count changing the animation textboxes as user changes and trigger the autosave code
                ignoreAnimationChanges = true;
                UpdateSectionSelection();
                ignoreAnimationChanges = false;
            }
        }

        private void UpdateSectionSelection()
        {
            UpdateAnimationData();

            CodeBlockSelection codeBlockSelection = new CodeBlockSelection(psaMovesetHandler);
            codeBlockSelection.SectionType = SectionType.SUBACTION;
            codeBlockSelection.SectionIndex = SelectedSubActionOption.Index;

            foreach (ISectionSelectorListener listener in listeners)
            {
                listener.OnCodeBlockSelected(codeBlockSelection);
            }
        }

        private void UpdateAnimationData()
        {
            Animation animation = psaMovesetHandler.SubActionsHandler.GetSubActionAnimationData(SelectedSubActionOption.Index);
            animationNameTextBox.Text = animation.AnimationName;
            inTransitionTextBox.Text = animation.AnimationFlags.InTransition.ToString();
            noOutTransitionCheckBox.Checked = animation.AnimationFlags.NoOutTransition;
            loopCheckBox.Checked = animation.AnimationFlags.Loop;
            movesCharacterCheckBox.Checked = animation.AnimationFlags.MovesCharacter;
            unknown3CheckBox.Checked = animation.AnimationFlags.Unknown3;
            unknown4CheckBox.Checked = animation.AnimationFlags.Unknown4;
            unknown5CheckBox.Checked = animation.AnimationFlags.Unknown5;
            transitionOutFromStartCheckBox.Checked = animation.AnimationFlags.TransitionOutFromStart;
            unknown7CheckBox.Checked = animation.AnimationFlags.Unknown7;
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
            subActionsListScintilla.ClearItems();
            subActionsListScintilla.AddItems(subActionSearchList.FilteredItems.Select(option => option.ToString()).ToList());
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
            if (!ignoreAnimationChanges)
            {
                if (animationNameTextBox.Text != "")
                {
                    psaMovesetHandler.SubActionsHandler.ModifyAnimationName(SelectedSubActionOption.Index, animationNameTextBox.Text);
                }
                else
                {

                    psaMovesetHandler.SubActionsHandler.RemoveAnimationData(SelectedSubActionOption.Index);
                }
                SelectedSubActionOption.Name = animationNameTextBox.Text;
                subActionsListScintilla.ModifyItem(SelectedSubActionOption.Index, SelectedSubActionOption.ToString());
            }
        }

        /// <summary>
        /// returns animation flags based on the UI controls (textboxes/checkboxes) for the selected subaction
        /// <para>this does NOT return the actual animation flag values currently in the psa file</para>
        /// <para>this method is called to easily get what the user has set the animation flags to on the UI (which can then be used to make the file match the UI afterwards)</para>
        /// </summary>
        /// <returns></returns>
        private AnimationFlags GetSelectedSubActionAnimationFlagsFromUI()
        {
            return new AnimationFlags(
                inTransitionTextBox.Text.ToInt(),
                noOutTransitionCheckBox.Checked,
                loopCheckBox.Checked,
                movesCharacterCheckBox.Checked,
                unknown3CheckBox.Checked,
                unknown4CheckBox.Checked,
                unknown5CheckBox.Checked,
                transitionOutFromStartCheckBox.Checked,
                unknown7CheckBox.Checked
            );
        }

        private void ModifyAnimationFlags()
        {
            if (!ignoreAnimationChanges)
            {
                psaMovesetHandler.SubActionsHandler.ModifyAnimationFlags(SelectedSubActionOption.Index, GetSelectedSubActionAnimationFlagsFromUI());
            }
        }

        private void inTransitionTextBox_TextChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }

        private void noOutTransitionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }

        private void loopCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }

        private void movesCharacterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }

        private void unknown3CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }

        private void unknown4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }

        private void unknown5CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }

        private void transitionOutFromStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }

        private void unknown7CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ModifyAnimationFlags();
        }
    }
}
