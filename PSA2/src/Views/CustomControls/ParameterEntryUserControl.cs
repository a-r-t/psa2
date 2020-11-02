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
    public partial class ParameterEntryUserControl : ObservableUserControl<IParameterEntryUserControlListener>
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
                categoryLabel.Text = parameterEntry.Category;
                parameterTypesComboBox.SelectedIndex = parameterEntry.Type;
                parameterValueTextBox.Text = parameterEntry.Value.ToString();
            }
        }
        private string[] parameterTypes = new string[] { "Hex", "Scalar", "Pointer", "Boolean", "(4)", "Variable", "Requirement" };

        public ParameterEntryUserControl()
        {
            InitializeComponent();
            parameterTypesComboBox.Items.AddRange(parameterTypes);
        }

        private void parameterTypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnChange();
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            OnChange();
        }

        private void OnChange()
        {
            bool isDirty = parameterTypesComboBox.SelectedIndex != parameterEntry.Type 
                || (parameterValueTextBox.Text.IsNumeric() && Int32.Parse(parameterValueTextBox.Text) != parameterEntry.Value);

            foreach (IParameterEntryUserControlListener listener in listeners)
            {
                listener.OnParameterChange(isDirty);
            }
        }
    }
}
