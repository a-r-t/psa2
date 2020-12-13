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

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    public partial class ScalarValueParameterEditorForm : ParameterEditorFormBase
    {
        private bool ignoreTextChanged;

        public ScalarValueParameterEditorForm(PsaMovesetHandler psaMovesetHandler, int value): base(psaMovesetHandler)
        {
            InitializeComponent();
            ignoreTextChanged = true;
            parameterValueTextBox.Text = $"{(decimal)value / 60000m:0.#######}";
            ignoreTextChanged = false;
            maxLabel.Text = $"Max Value: {int.MaxValue / 60000m:0.##}";
            minLabel.Text = $"Min Value: {int.MinValue / 60000m:0.##}";
            validationPictureBox.ImageLocation = "./images/green_check_mark.png";
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!ignoreTextChanged)
            {
                try
                {
                    int convertedIntValue = (int)(Convert.ToDecimal(parameterValueTextBox.Text) * 60000m);
                    EmitParameterChange(convertedIntValue);
                    validationPictureBox.ImageLocation = "./images/green_check_mark.png";
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException || ex is OverflowException)
                {
                    validationPictureBox.ImageLocation = "./images/red_x.png";
                }
            }
        }
    }
}
