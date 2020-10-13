using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views
{
    public partial class SectionsTreeView : TreeView
    {
        public event TreeViewEventHandler SelectedNodeChanged;

        public SectionsTreeView() : base()
        {
            this.AfterSelect += new TreeViewEventHandler(SelectNodeChangedEvent);
            this.MouseUp += new MouseEventHandler(MouseUpEvent);
        }

        private void SelectNodeChangedEvent(object sender, TreeViewEventArgs e)
        {
            SelectedNodeChangedTrigger(sender, e);
        }
        private void MouseUpEvent(object sender, MouseEventArgs e)
        {
            if (this.SelectedNode == null) {
                SelectedNodeChangedTrigger(sender, new TreeViewEventArgs(null));
            }
        }
        private void SelectedNodeChangedTrigger(object sender, TreeViewEventArgs e)
        {
            SelectedNodeChanged?.Invoke(sender, e);
        }
    }
}
