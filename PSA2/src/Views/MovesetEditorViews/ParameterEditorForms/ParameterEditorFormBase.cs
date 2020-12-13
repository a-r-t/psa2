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
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    public partial class ParameterEditorFormBase : ObservableUserControl<IParameterEditorFormListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;

        // this constructor is for the designer so it stops throwing a hissy fit
        public ParameterEditorFormBase()
        {
            InitializeComponent();
        }

        public ParameterEditorFormBase(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
        }

        protected void EmitParameterChange(int value)
        {
            foreach (IParameterEditorFormListener listener in listeners)
            {
                listener.OnParameterValueChange(value);
            }
        }
    }
}
