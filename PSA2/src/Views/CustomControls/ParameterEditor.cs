using PSA2.src.Views.MovesetEditorViews.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class ParameterEditor : ObservableUserControl<IParameterEditorListener>, IParameterEditorFormListener
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
                SuspendLayout();
                parameterEntry = value;
                Controls.Clear();
                if (parameterEntry != null)
                {
                    ParameterEditorForm parameterEditor = new ParameterEditorForm(parameterEntry);
                    parameterEditor.Dock = DockStyle.Fill;
                    parameterEditor.AddListener(this);
                    Controls.Add(parameterEditor);
                }
                ResumeLayout();
            }
        }

        public ParameterEditor()
        {
            DoubleBuffered = true;
        }

        public void OnParameterChange(int type, int value)
        {
            foreach (IParameterEditorListener listener in listeners)
            {
                listener.OnParameterChange(type, value);
            }
        }
    }
}
