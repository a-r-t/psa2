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
    public partial class BooleanValueParameterEditorForm : ParameterEditorFormBase
    {
        private bool ignoreComboBoxChanged;
        private string[] booleanOptions = new string[] { "True", "False" };

        public BooleanValueParameterEditorForm(PsaMovesetHandler psaMovesetHandler, int value): base(psaMovesetHandler)
        {
            InitializeComponent();
            ignoreComboBoxChanged = true;
            valueComboBox.Items.AddRange(booleanOptions);
            ignoreComboBoxChanged = false;

            // if value is greater than 0, it is True, so selected index is 0 for "True" in the combobox
            // a bit backwards I know
            valueComboBox.SelectedIndex = value > 0 ? 0 : 1; 
        }

        private void valueComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignoreComboBoxChanged)
            {
                if (valueComboBox.SelectedIndex == 0)
                {
                    EmitParameterChange(1);
                }
                else
                {
                    EmitParameterChange(0);
                }
            }
        }
    }
}
