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

namespace PSA2.src.Views.CustomControls
{
    public partial class ParameterEditorForm : ObservableUserControl<IParameterEditorFormListener>
    {
        public int Value { get; set; }

        public ParameterEditorForm(int value)
        {
            Value = value;
            InitializeComponent();
            parameterValueTextBox.Text = Value.ToString();
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (parameterValueTextBox.Text.IsNumeric())
            {
                Value = (int)Double.Parse(parameterValueTextBox.Text);
                EmitParameterChange();
            }
        }

        private void EmitParameterChange()
        {
            foreach (IParameterEditorFormListener listener in listeners)
            {
                listener.OnParameterValueChange(Value);
            }
        }

        private void parameterValueTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !$"{parameterValueTextBox.Text}{e.KeyChar}".IsNumeric())
            {
                e.Handled = true;
            }
        }
    }
}
