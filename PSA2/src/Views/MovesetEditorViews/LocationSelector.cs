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
    public partial class LocationSelector : ObservableUserControl<ILocationSelectorListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;

        public LocationSelector(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
        }

        private void LocationSelector_Load(object sender, EventArgs e)
        {
            int numberOfSpecialActions = psaMovesetHandler.ActionsHandler.GetNumberOfSpecialActions();
            for (int i = 0; i < numberOfSpecialActions; i++)
            {
                actionOptionsListBox.Items.Add((i + 274).ToString("X"));
            }
            actionOptionsListBox.Items[0] += " - Neutral Special";
            actionOptionsListBox.Items[1] += " - Side Special";
            actionOptionsListBox.Items[2] += " - Up Special";
            actionOptionsListBox.Items[3] += " - Down Special";

            int numberOfSubActions = psaMovesetHandler.SubActionsHandler.GetNumberOfSubActions();
            for (int i = 0; i < numberOfSubActions; i++)
            {
                string animationName = psaMovesetHandler.SubActionsHandler.GetSubActionAnimationName(i);
                subActionOptionsListBox.Items.Add(i.ToString("X") + " - " + animationName);
            }
            actionOptionsListBox.SelectedIndex = 0;

        }

        private void actionOptionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ILocationSelectorListener listener in listeners)
            {
                listener.OnSelect(LocationType.ACTION, actionOptionsListBox.SelectedIndex);
            }
        }

        private void subActionOptionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ILocationSelectorListener listener in listeners)
            {
                listener.OnSelect(LocationType.SUBACTION, subActionOptionsListBox.SelectedIndex);
            }
        }

        private void sectionsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            LocationType locationType = 0;
            int selectedIndex = -1;
            if (sectionsTabControl.SelectedIndex == 0)
            {
                locationType = LocationType.ACTION;
                if (actionOptionsListBox.SelectedIndex == -1)
                {
                    actionOptionsListBox.SelectedIndex = 0;
                }
                selectedIndex = actionOptionsListBox.SelectedIndex;
            }
            else if (sectionsTabControl.SelectedIndex == 1)
            {
                locationType = LocationType.SUBACTION;
                if (subActionOptionsListBox.SelectedIndex == -1)
                {
                    subActionOptionsListBox.SelectedIndex = 0;
                }
                selectedIndex = subActionOptionsListBox.SelectedIndex;
            }

            foreach (ILocationSelectorListener listener in listeners)
            {
                listener.OnSelect(locationType, selectedIndex);
            }
        }
    }
}
