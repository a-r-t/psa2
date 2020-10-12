using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSA2.src.FileProcessor.MovesetHandler;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class MovesetEditor: ObservableUserControl<IMovesetEditorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;

        public MovesetEditor(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
            LocationSelector locationSelector = new LocationSelector(psaMovesetHandler);
            locationSelector.Dock = DockStyle.Fill;
            CodeBlockViewer codeBlockViewer = new CodeBlockViewer(psaMovesetHandler);
            codeBlockViewer.Dock = DockStyle.Fill;
            locationSelector.AddListener(codeBlockViewer);
            selectorView.Controls.Add(locationSelector);
            codeBlockView.Controls.Add(codeBlockViewer);
        }

    }
}
