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
    public partial class HexValueParameterEditorForm : ObservableUserControl<IParameterEditorFormListener>
    {
        public int Value { get; set; }

        public HexValueParameterEditorForm(int value): base()
        {
            Value = value;
            InitializeComponent();
            parameterValueTextBox.Text = Value.ToString("X8");
            validationPictureBox.ImageLocation = "./images/green_check_mark.png";
        }

        private void EmitParameterChange()
        {
            foreach (IParameterEditorFormListener listener in listeners)
            {
                listener.OnParameterValueChange(Value);
            }
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Value = Convert.ToInt32(parameterValueTextBox.Text, 16);
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
