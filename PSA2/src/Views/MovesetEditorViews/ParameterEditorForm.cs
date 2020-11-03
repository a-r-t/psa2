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
        public ParameterEntry ParameterEntry { get; set; }
        
        private string[] parameterTypes = new string[] { "Hex", "Scalar", "Pointer", "Boolean", "(4)", "Variable", "Requirement" };

        public ParameterEditorForm(ParameterEntry parameterEntry)
        {
            InitializeComponent();
            parameterTypesComboBox.Items.AddRange(parameterTypes);
            ParameterEntry = parameterEntry;
            categoryLabel.Text = ParameterEntry.Category;
            parameterTypesComboBox.SelectedIndex = ParameterEntry.Type;
            parameterValueTextBox.Text = ParameterEntry.Value.ToString();
        }

        private void parameterTypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ParameterEntry.Type = parameterTypesComboBox.SelectedIndex;
            EmitParameterChange();
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (parameterValueTextBox.Text.IsNumeric())
            {
                ParameterEntry.Value = (int)Double.Parse(parameterValueTextBox.Text);
                EmitParameterChange();
            }
        }

        private void EmitParameterChange()
        {
            foreach (IParameterEditorFormListener listener in listeners)
            {
                listener.OnParameterChange(ParameterEntry.Type, ParameterEntry.Value);
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
