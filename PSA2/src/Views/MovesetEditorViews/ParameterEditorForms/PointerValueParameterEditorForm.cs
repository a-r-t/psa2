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
    public partial class PointerValueParameterEditorForm : ParameterEditorFormBase
    {
        private bool ignoreTextChanged;

        public PointerValueParameterEditorForm(PsaMovesetHandler psaMovesetHandler, int value): base(psaMovesetHandler)
        {
            InitializeComponent();
        }

        private void parameterValueTextBox_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
