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
using PSA2.src.Configuration;
using PSA2MovesetLogic.src.Models.Fighter;

namespace PSA2.src.Views.MovesetEditorViews.SectionSelectors
{
    public partial class SubroutineSelector : ObservableUserControl<ISectionSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        private List<int> subroutines;

        public SubroutineSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
            subroutinesListScintilla.FontFamily = "Tahoma";
        }

        private void SubroutineSelector_Load(object sender, EventArgs e)
        {
            subroutines = psaMovesetHandler.SubRoutinesHandler.GetAllSubroutineLocations()
                .OrderBy(x => x.ToString("X8")).ToList();
            subroutinesListScintilla.AddItems(subroutines.Select(subroutine => subroutine.ToString("X")).ToList());
        }

        private void subroutinesListScintilla_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSectionSelection();
        }

        private void UpdateSectionSelection()
        {
            CodeBlockSelection codeBlockSelection = new CodeBlockSelection(psaMovesetHandler);
            codeBlockSelection.SectionType = SectionType.SUBROUTINE;
            codeBlockSelection.SectionIndex = subroutines[subroutinesListScintilla.SelectedIndex];

            foreach (ISectionSelectorListener listener in listeners)
            {
                listener.OnCodeBlockSelected(codeBlockSelection);
            }
        }

        private void SubroutineSelector_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                UpdateSectionSelection();
            }
        }
    }
}
