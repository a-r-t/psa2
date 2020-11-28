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

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    public partial class VariableValueParameterEditorForm : ParameterEditorFormBase
    {
        private string[] memoryTypeOptions = new string[] { "IC", "LA", "RA" };
        private string[] dataTypeOptions = new string[] { "Basic", "Float", "Bit" };
        private bool ignoreChanges;
        private PsaVariable psaVariable;

        public VariableValueParameterEditorForm(int value): base(value)
        {
            InitializeComponent();
            memoryTypeComboBox.Items.AddRange(memoryTypeOptions);
            dataTypeComboBox.Items.AddRange(dataTypeOptions);
            ignoreChanges = true;
            psaVariable = ConvertParamValueToVariable(Value);
            memoryTypeComboBox.SelectedIndex = psaVariable.MemoryType;
            dataTypeComboBox.SelectedIndex = psaVariable.DataType;
            idTextBox.Text = psaVariable.Id.ToString();
            ignoreChanges = false;
            validationPictureBox.ImageLocation = "./images/green_check_mark.png";
        }

        private PsaVariable ConvertParamValueToVariable(int paramValue)
        {
            int memoryType = paramValue >> 28 & 0xF;
            int datatype = paramValue >> 24 & 0xF;
            int id = paramValue & 0xFFFFFF;

            // if variable is negative value, convert it to an int properly
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
                    Value = psaVariable.ToIntValue();
                    EmitParameterChange();
                }
            }
        }

        private void dataTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreChanges)
            {
                if (psaVariable.DataType != dataTypeComboBox.SelectedIndex)
                {
                    psaVariable.DataType = dataTypeComboBox.SelectedIndex;
                    Value = psaVariable.ToIntValue();
                    EmitParameterChange();
                }
            }
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
                    Value = psaVariable.ToIntValue();
                    EmitParameterChange();
                    validationPictureBox.ImageLocation = "./images/green_check_mark.png";
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                {
                    validationPictureBox.ImageLocation = "./images/red_x.png";
                }
            }
        }
    }
}
