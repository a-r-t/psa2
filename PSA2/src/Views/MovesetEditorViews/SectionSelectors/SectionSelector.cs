﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2.src.Views.MovesetEditorViews.Interfaces;

namespace PSA2.src.Views.MovesetEditorViews.SectionSelectors
{
    public partial class SectionSelector : ObservableUserControl<ISectionSelectorListener>,
        ISectionSelectorListener
    {
        protected PsaMovesetHandler psaMovesetHandler;
        private string[] sections = new string[] { "Actions", "Sub Actions", "Subroutines" };
        private ActionSelector actionSelector;
        private SubActionSelector subActionSelector;
        private SubroutineSelector subroutineSelector;
        private CodeBlockSelection codeBlockSelection;

        public SectionSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();

            actionSelector = new ActionSelector(psaMovesetHandler);
            actionSelector.Dock = DockStyle.Fill;
            actionSelector.Visible = false;
            actionSelector.Name = "actionSelector";
            actionSelector.AddListener(this);

            subActionSelector = new SubActionSelector(psaMovesetHandler);
            subActionSelector.Dock = DockStyle.Fill;
            subActionSelector.Visible = false;
            subActionSelector.Name = "subActionSelector";
            subActionSelector.AddListener(this);

            subroutineSelector = new SubroutineSelector(psaMovesetHandler);
            subroutineSelector.Dock = DockStyle.Fill;
            subroutineSelector.Visible = false;
            subroutineSelector.Name = "subroutineSelector";
            subroutineSelector.AddListener(this);

            sectionSelectorFormViewer.Controls.Add(actionSelector);
            sectionSelectorFormViewer.Controls.Add(subActionSelector);
            sectionSelectorFormViewer.Controls.Add(subroutineSelector);

        }

        private void LocationSelector_Load(object sender, EventArgs e)
        {
            sectionComboBox.Items.AddRange(sections);
            sectionComboBox.SelectedIndex = 0;
        }

        private void sectionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (sectionComboBox.SelectedItem)
            {
                case "Actions":
                    ShowSectionSelectorForm("actionSelector");
                    break;
                case "Sub Actions":
                    ShowSectionSelectorForm("subActionSelector");
                    break;
                case "Subroutines":
                    ShowSectionSelectorForm("subroutineSelector");
                    break;
            }
        }

        public void ShowSectionSelectorForm(string selectorName)
        {
            for (int i = 0; i < sectionSelectorFormViewer.Controls.Count; i++)
            {
                Control control = sectionSelectorFormViewer.Controls[i];
                control.Visible = control.Name == selectorName;
            }
        }

        public void OnCodeBlockSelected(CodeBlockSelection codeBlockSelection)
        {
            this.codeBlockSelection = codeBlockSelection;
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            foreach (ISectionSelectorListener listener in listeners)
            {
                listener.OnCodeBlockSelected(codeBlockSelection);
            }
        }
    }
}