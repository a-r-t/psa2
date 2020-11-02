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
    public partial class ParameterEditor : ObservableUserControl<IParameterEditorListener>
    {
        private ParameterEntry parameterEntry;
        public ParameterEntry ParameterEntry
        { 
            get
            {
                return parameterEntry;
            }
            set
            {
                parameterEntry = value;
                originalType = parameterEntry.Type;
                originalValue = parameterEntry.Value;
                categoryLabel.Text = parameterEntry.Category;
                parameterTypesComboBox.SelectedIndex = parameterEntry.Type;
                parameterValueTextBox.Text = parameterEntry.Value.ToString();
            }
        }
        private string[] parameterTypes = new string[] { "Hex", "Scalar", "Pointer", "Boolean", "(4)", "Variable", "Requirement" };
        private int originalType;
        private int originalValue;

        public ParameterEditor()
        {
            InitializeComponent();
            parameterTypesComboBox.Items.AddRange(parameterTypes);
        }

        private void parameterTypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnChange();
            parameterEntry.Type = parameterTypesComboBox.SelectedIndex;
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            OnChange();
            if (parameterValueTextBox.Text.IsNumeric())
            {
                parameterEntry.Value = Int32.Parse(parameterValueTextBox.Text);
            }
        }

        private void OnChange()
        {
            bool isDirty = parameterTypesComboBox.SelectedIndex != originalType
                || (parameterValueTextBox.Text.IsNumeric() && Int32.Parse(parameterValueTextBox.Text) != originalValue);

            foreach (IParameterEditorListener listener in listeners)
            {
                listener.OnParameterChange(isDirty);
            }
        }

        private void parameterValueTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !($"{parameterValueTextBox.Text}{e.KeyChar}".IsNumeric());
        }
    }
}
