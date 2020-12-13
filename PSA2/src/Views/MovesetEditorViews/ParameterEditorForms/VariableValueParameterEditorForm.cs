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
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.Views.Utility.SearchList;
using PSA2.src.Configuration;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    // TODO: These values need to be known to the user in someway through the UI
    // Max Id: 16777215
    // Min Id: -8388607
    public partial class VariableValueParameterEditorForm : ParameterEditorFormBase
    {
        private string[] memoryTypeOptions = new string[] { "IC", "LA", "RA" };
        private string[] dataTypeOptions = new string[] { "Basic", "Float", "Bit" };
        private bool ignoreChanges;
        private PsaVariable psaVariable;
        private VariableSearchList variableSearchList;
        private bool isIdValid;

        public VariableValueParameterEditorForm(PsaMovesetHandler psaMovesetHandler, int value): base(psaMovesetHandler)
        {
            InitializeComponent();
            variableSearchList = new VariableSearchList(searchTextBox);
            memoryTypeComboBox.Items.AddRange(memoryTypeOptions);
            dataTypeComboBox.Items.AddRange(dataTypeOptions);

            ignoreChanges = true;
            psaVariable = ConvertParamValueToVariable(value);
            memoryTypeComboBox.SelectedIndex = psaVariable.MemoryType;
            dataTypeComboBox.SelectedIndex = psaVariable.DataType;
            idTextBox.Text = psaVariable.Id.ToString();
            ignoreChanges = false;

            validationPictureBox.ImageLocation = "./images/green_check_mark.png";
            isIdValid = true;

            variableSearchList.Items = Config.VariablesConfig.GetAllVariables()
                .OrderBy(v => v.MemoryType)
                .ThenBy(v => v.DataType)
                .ThenBy(v => v.Id)
                .ToList();

            dataTypeFilterComboBox.Items.AddRange(dataTypeOptions);
            dataTypeFilterComboBox.Items.Insert(0, "All");
            dataTypeFilterComboBox.SelectedIndex = 0;
        }

        private PsaVariable ConvertParamValueToVariable(int paramValue)
        {
            int memoryType = paramValue >> 28 & 0xF;
            int datatype = paramValue >> 24 & 0xF;
            int id = paramValue & 0xFFFFFF;

            // if variable is negative value, convert it to an int properly
            // it is negative if first digit in id (as hex) is an 8
            if ((paramValue & 0xFFFFFF).ToString("X")[0] == '8')
            {
                // why am I unable to make this a one liner, I'm a fraud
                id -= 8388608;
                id = -id;
            }
            return new PsaVariable(memoryType, datatype, id);
        }

        private void memoryTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                if (psaVariable.MemoryType != memoryTypeComboBox.SelectedIndex)
                {
                    psaVariable.MemoryType = memoryTypeComboBox.SelectedIndex;
                    EmitParameterChange(psaVariable.ToIntValue());
                }
            }
            SetCurrentSelectedVariableName();
        }

        private void dataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                if (psaVariable.DataType != dataTypeComboBox.SelectedIndex)
                {
                    psaVariable.DataType = dataTypeComboBox.SelectedIndex;
                    EmitParameterChange(psaVariable.ToIntValue());
                }
            }
            SetCurrentSelectedVariableName();
        }

        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                try
                {
                    if (idTextBox.Text.Contains(".") || idTextBox.Text.Contains(","))
                    {
                        throw new FormatException("Must be whole number");
                    }
                    int convertedInt = Convert.ToInt32(idTextBox.Text);
                    if (convertedInt < -8388607 || convertedInt > 16777215)
                    {
                        throw new FormatException("Must be between -8388607 and 16777215");
                    }

                    // if variable is negative, convert it properly so it can be read properly in the file
                    if (convertedInt < 0)
                    {
                        convertedInt = Math.Abs(convertedInt) + 8388608;
                    }
                    psaVariable.Id = convertedInt;
                    EmitParameterChange(psaVariable.ToIntValue());
                    validationPictureBox.ImageLocation = "./images/green_check_mark.png";
                    isIdValid = true;
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                {
                    validationPictureBox.ImageLocation = "./images/red_x.png";
                    isIdValid = false;
                }
            }
            SetCurrentSelectedVariableName();
        }


        // sets name of variable that is currently selected as the parma value from config
        // if no variable in config, sets label to "Unknown Variable"
        private void SetCurrentSelectedVariableName()
        {
            if (!isIdValid)
            {
                variableNameLabel.Text = "Variable: Unknown";
            }
            else
            {
                Variable selectedVariable = Config.VariablesConfig.GetVariable(psaVariable.MemoryType, psaVariable.DataType, psaVariable.Id);
                if (selectedVariable != null)
                {
                    variableNameLabel.Text = selectedVariable.Name;
                }
                else
                {
                    variableNameLabel.Text = "Variable: Unknown";
                }
            }
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            variablesScintilla.ClearItems();
            variablesScintilla.AddItems(variableSearchList.FilteredItems.Select(c => c.Name).ToList());
        }

        private void dataTypeFilterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            variablesScintilla.ClearItems();
            string selectedItem = dataTypeFilterComboBox.SelectedItem.ToString();
            if (selectedItem == "All")
            {
                variableSearchList.DataType = null;
            }
            else
            {
                variableSearchList.DataType = selectedItem;
            }
            variablesScintilla.AddItems(variableSearchList.FilteredItems.Select(c => c.Name).ToList());
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            ApplyVariableSelection();
        }

        private void variablesScintilla_DoubleClick(object sender, ScintillaNET.DoubleClickEventArgs e)
        {
            ApplyVariableSelection();
        }

        private void ApplyVariableSelection()
        {
            Variable selectedVariable = variableSearchList.FilteredItems[variablesScintilla.SelectedIndex];
            memoryTypeComboBox.SelectedItem = selectedVariable.MemoryType;
            dataTypeComboBox.SelectedItem = selectedVariable.DataType;
            idTextBox.Text = selectedVariable.Id.ToString();
        }
    }
}
