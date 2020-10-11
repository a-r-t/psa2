using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class MovesetEditor: ObservableUserControl<IMovesetEditorListener>
    {
        public MovesetEditor()
        {
            InitializeComponent();
        }

    }
}
