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

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    public partial class ParameterEditorFormBase : ObservableUserControl<IParameterEditorFormListener>
    {
        protected int Value { get; set; }

        // this constructor is unused, only here because winforms design viewer throws a tantrum without it
        public ParameterEditorFormBase()
        {
            InitializeComponent();
        }

        public ParameterEditorFormBase(int value)
        {
            Value = value;
            InitializeComponent();
        }

        protected void EmitParameterChange()
        {
            foreach (IParameterEditorFormListener listener in listeners)
            {
                listener.OnParameterValueChange(Value);
            }
        }
    }
}
