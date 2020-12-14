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
using PSA2.src.ExtentionMethods;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;

// There are two SectionType classes across the solutions for frontend and backend, so this specifies to use the one that is defined in the CommandLocationTracker
using SectionTypeCLT = PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.SectionType;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers;

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    public partial class PointerValueParameterEditorForm : ParameterEditorFormBase
    {
        private bool ignoreTextChanged;
        bool isPointerValid;
        bool isPointerSelectorValid;
        private string[] eventTypeOptions = new string[] { "Actions", "Sub Actions", "Subroutines" };
        private string[] actionCodeBlockOptions = new string[] { "Entry", "Exit" };
        private string[] subActionCodeBlockOptions = new string[] { "Main", "GFX", "SFX", "Other" };

        public PointerValueParameterEditorForm(PsaMovesetHandler psaMovesetHandler, int value): base(psaMovesetHandler)
        {
            InitializeComponent();
            isPointerValid = true;
            ignoreTextChanged = true;
            parameterValueTextBox.Text = value.ToString("X8");
            ignoreTextChanged = false;
            validationPictureBox.ImageLocation = "./images/green_check_mark.png";
            eventTypeComboBox.Items.AddRange(eventTypeOptions);
            eventTypeComboBox.SelectedIndex = 0;
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!ignoreTextChanged)
            {
                try
                {
                    int convertedIntValue = Convert.ToInt32(parameterValueTextBox.Text, 16);
                    EmitParameterChange(convertedIntValue);
                    validationPictureBox.ImageLocation = "./images/green_check_mark.png";
                    isPointerValid = true;
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                {
                    validationPictureBox.ImageLocation = "./images/red_x.png";
                    isPointerValid = false;
                }
            }
            SetCurrentPointerName();
        }

        // sets label for which area in the moveset file the pointer value is pointing to (e.g. action, sub action, etc)
        // if no variable in config, sets label to "Unknown Location"
        private void SetCurrentPointerName()
        {
            if (!isPointerValid)
            {
                sectionNameLabel.Text = "Unknown Location";
                codeBlockNameLabel.Visible = false;
                commandIndexLabel.Visible = false;
            }
            else
            {
                int pointerValue = Convert.ToInt32(parameterValueTextBox.Text, 16);
                if (psaMovesetHandler.CommandLocationTracker.CommandLocations.ContainsKeyForward(pointerValue))
                {
                    (SectionTypeCLT, int, int, int) sectionInfo = psaMovesetHandler.CommandLocationTracker.CommandLocations.GetForward(pointerValue);
                    string sectionType = sectionInfo.Item1.AsString();

                    int sectionId;
                    switch (sectionInfo.Item1) 
                    {
                        case SectionTypeCLT.ACTION:
                            sectionId = sectionInfo.Item2 + 274;
                            break;
                        default:
                            sectionId = sectionInfo.Item2;
                            break;
                    }
                    sectionNameLabel.Text = $"{sectionType} - {sectionId.ToString("X")}";
                    
                    switch (sectionInfo.Item1)
                    {
                        case SectionTypeCLT.ACTION:
                            codeBlockNameLabel.Text = actionCodeBlockOptions[sectionInfo.Item3];
                            break;
                        case SectionTypeCLT.SUBACTION:
                            codeBlockNameLabel.Text = subActionCodeBlockOptions[sectionInfo.Item3];
                            break;
                    }

                    codeBlockNameLabel.Visible = true;

                    commandIndexLabel.Text = $"Command Index: {(sectionInfo.Item4 + 1)}";
                    commandIndexLabel.Visible = true;
                }
                else if (psaMovesetHandler.CommandLocationTracker.CodeBlockLocations.ContainsKeyForward(pointerValue))
                {
                    (SectionTypeCLT, int, int) sectionInfo = psaMovesetHandler.CommandLocationTracker.CodeBlockLocations.GetForward(pointerValue);
                    string sectionType = sectionInfo.Item1.AsString();

                    int sectionId;
                    switch (sectionInfo.Item1)
                    {
                        case SectionTypeCLT.ACTION:
                            sectionId = sectionInfo.Item2 + 274;
                            break;
                        default:
                            sectionId = sectionInfo.Item2;
                            break;
                    }
                    sectionNameLabel.Text = $"{sectionType} - {sectionId.ToString("X")}";

                    switch (sectionInfo.Item1)
                    {
                        case SectionTypeCLT.ACTION:
                            codeBlockNameLabel.Text = actionCodeBlockOptions[sectionInfo.Item3];
                            break;
                        case SectionTypeCLT.SUBACTION:
                            codeBlockNameLabel.Text = subActionCodeBlockOptions[sectionInfo.Item3];
                            break;
                    }

                    codeBlockNameLabel.Visible = true;

                    commandIndexLabel.Visible = false;
                }
                else
                {
                    sectionNameLabel.Text = "Unknown Location";
                    codeBlockNameLabel.Visible = false;
                    commandIndexLabel.Visible = false;
                }
            }
        }

        private void eventTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (eventTypeComboBox.SelectedIndex)
            {
                case 0: // actions

                    // load section index options
                    sectionComboBox.Items.Clear();
                    int numberOfSpecialActions = psaMovesetHandler.ActionsHandler.GetNumberOfSpecialActions();
                    for (int i = 0; i < numberOfSpecialActions; i++)
                    {
                        // Add 274 so special action 0 shows up as 112, etc
                        sectionComboBox.Items.Add((i + 274).ToString("X"));
                    }
                    sectionComboBox.SelectedIndex = -1;
                    sectionComboBox.SelectedIndex = 0;

                    // load code block options
                    codeBlockComboBox.Items.Clear();
                    codeBlockComboBox.Items.AddRange(actionCodeBlockOptions);
                    codeBlockComboBox.SelectedIndex = -1;
                    codeBlockComboBox.SelectedIndex = 0;

                    break;

                case 1: // subactions

                    // load section index options
                    sectionComboBox.Items.Clear();
                    int numberOfSubActions = psaMovesetHandler.SubActionsHandler.GetNumberOfSubActions();
                    for (int i = 0; i < numberOfSubActions; i++)
                    {
                        sectionComboBox.Items.Add(i.ToString("X"));
                    }
                    sectionComboBox.SelectedIndex = -1;
                    sectionComboBox.SelectedIndex = 0;

                    // load code block options
                    codeBlockComboBox.Items.Clear();
                    codeBlockComboBox.Items.AddRange(subActionCodeBlockOptions);
                    codeBlockComboBox.SelectedIndex = -1;
                    codeBlockComboBox.SelectedIndex = 0;

                    break;
                case 2: // subroutines
                    break;
            }
        }

        private void sectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sectionComboBox.SelectedIndex >= 0 && codeBlockComboBox.SelectedIndex >= 0)
            {
                LoadCommandIndexes();
            }
        }

        private void codeBlockComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sectionComboBox.SelectedIndex >= 0 && codeBlockComboBox.SelectedIndex >= 0)
            {
                LoadCommandIndexes();
            }
        }

        private void LoadCommandIndexes()
        {
            int codeBlockId = codeBlockComboBox.SelectedIndex;
            int numberOfPsaCommands = 0;
            bool isCodeBlockActive = true;
            switch (eventTypeComboBox.SelectedIndex)
            {
                case 0: // actions
                    commandIndexComboBox.Items.Clear();
                    int actionId = sectionComboBox.SelectedIndex;
                    numberOfPsaCommands = psaMovesetHandler.ActionsHandler.GetNumberOfPsaCommandsInCodeBlock(actionId, codeBlockId);
                    isCodeBlockActive = psaMovesetHandler.ActionsHandler.GetCodeBlockCommandsPointerLocation(actionId, codeBlockId) != 0;
                    break;
                case 1: // sub actions
                    commandIndexComboBox.Items.Clear();
                    int subActionId = sectionComboBox.SelectedIndex;
                    numberOfPsaCommands = psaMovesetHandler.SubActionsHandler.GetNumberOfPsaCommandsInCodeBlock(subActionId, codeBlockId);
                    isCodeBlockActive = psaMovesetHandler.SubActionsHandler.GetCodeBlockCommandsPointerLocation(subActionId, codeBlockId) != 0;
                    break;
                case 2: // subroutines
                    break;
            }

            if (isCodeBlockActive)
            {
                if (numberOfPsaCommands == 0)
                {
                    commandIndexComboBox.Enabled = false;
                }
                else
                {
                    for (int i = 0; i < numberOfPsaCommands; i++)
                    {
                        commandIndexComboBox.Items.Add((i + 1));
                    }
                    commandIndexComboBox.Enabled = true;
                    commandIndexComboBox.SelectedIndex = -1;
                    commandIndexComboBox.SelectedIndex = 0;
                }
                applyButton.Enabled = true;
                isPointerSelectorValid = true;
            }
            else
            {
                commandIndexComboBox.Enabled = false;
                applyButton.Enabled = false;
                isPointerSelectorValid = false;
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            if (isPointerSelectorValid)
            {
                int sectionId = sectionComboBox.SelectedIndex;
                int codeBlockId = codeBlockComboBox.SelectedIndex;
                bool hasCommands = false;

                SectionTypeCLT sectionType = SectionTypeCLT.ACTION;
                switch(eventTypeComboBox.SelectedIndex)
                {
                    case 0: // actions
                        sectionType = SectionTypeCLT.ACTION;
                        hasCommands = psaMovesetHandler.ActionsHandler.GetNumberOfPsaCommandsInCodeBlock(sectionId, codeBlockId) > 0;
                        break;
                    case 1: // sub actions
                        sectionType = SectionTypeCLT.SUBACTION;
                        hasCommands = psaMovesetHandler.SubActionsHandler.GetNumberOfPsaCommandsInCodeBlock(sectionId, codeBlockId) > 0;
                        break;
                    case 2: // subroutines
                        sectionType = SectionTypeCLT.SUBROUTINE;
                        break;
                }

                int location;
                if (hasCommands)
                {
                    int commandIndex = commandIndexComboBox.SelectedIndex;
                    (SectionTypeCLT, int, int, int) sectionInfo = (sectionType, sectionId, codeBlockId, commandIndex);
                    location = psaMovesetHandler.CommandLocationTracker.CommandLocations.GetBackward(sectionInfo);
                }
                else
                {
                    (SectionTypeCLT, int, int) sectionInfo = (sectionType, sectionId, codeBlockId);
                    location = psaMovesetHandler.CommandLocationTracker.CodeBlockLocations.GetBackward(sectionInfo);
                }
                
                parameterValueTextBox.Text = location.ToString("X8");
            }
        }
    }
}
