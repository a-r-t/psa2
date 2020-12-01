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

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    public partial class HexValueParameterEditorForm : ParameterEditorFormBase
    {
        private bool ignoreTextChanged;

        public HexValueParameterEditorForm(int value): base()
        {
            InitializeComponent();
            ignoreTextChanged = true;
            parameterValueTextBox.Text = value.ToString("X8");
            ignoreTextChanged = false;
            validationPictureBox.ImageLocation = "./images/green_check_mark.png";
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
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                {
                    validationPictureBox.ImageLocation = "./images/red_x.png";
                }
            }
        }
    }
}
