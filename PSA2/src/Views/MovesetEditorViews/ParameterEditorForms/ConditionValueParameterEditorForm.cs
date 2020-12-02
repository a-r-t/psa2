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
using PSA2.src.Configuration;
using static PSA2.src.Configuration.ConditionsConfig;
using PSA2.src.Views.Utility.SearchList;

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    public partial class ConditionValueParameterEditorForm : ParameterEditorFormBase
    {
        private bool ignoreTextChanged;
        private ConditionSearchList conditionSearchList;

        public ConditionValueParameterEditorForm(int value): base()
        {
            InitializeComponent();
            conditionSearchList = new ConditionSearchList(searchTextBox);
            conditionSearchList.Items = Config.ConditionsConfig.Conditions;
            ignoreTextChanged = true;
            conditionsScintilla.AddItems(conditionSearchList.Items.Select(c => c.Name).ToList());
            conditionValueTextBox.Text = value.ToString("X8");
            conditionsScintilla.SelectedIndex = 0;
            ignoreTextChanged = false;

            Condition selectedCondition = Config.ConditionsConfig.GetConditionByIndex(value);
            if (selectedCondition != null)
            {
                conditionNameLabel.Text = selectedCondition.Name;
            }

            validationPictureBox.ImageLocation = "./images/green_check_mark.png";
        }

        private void conditionValueTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!ignoreTextChanged)
            {
                try
                {
                    int convertedIntValue = Convert.ToInt32(conditionValueTextBox.Text, 16);

                    Condition selectedCondition = Config.ConditionsConfig.GetConditionByIndex(convertedIntValue);
                    if (selectedCondition != null)
                    {
                        conditionNameLabel.Text = selectedCondition.Name;
                    }
                    else
                    {
                        conditionNameLabel.Text = "Condition: Unknown";
                    }

                    EmitParameterChange(convertedIntValue);
                    validationPictureBox.ImageLocation = "./images/green_check_mark.png";
                }
                catch (Exception ex) when (ex is FormatException || ex is ArgumentOutOfRangeException)
                {
                    validationPictureBox.ImageLocation = "./images/red_x.png";
                }
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            ApplyConditionSelection();
        }

        private void conditionsScintilla_DoubleClick(object sender, ScintillaNET.DoubleClickEventArgs e)
        {
            ApplyConditionSelection();
        }

        private void ApplyConditionSelection()
        {
            Condition selectedCondition = conditionSearchList.FilteredItems[conditionsScintilla.SelectedIndex];
            conditionValueTextBox.Text = selectedCondition.Index.ToString("X8");
        }

        private void searchTextBox_TextChanged(object sender, EventArgs e)
        {
            conditionsScintilla.ClearItems();
            conditionsScintilla.AddItems(conditionSearchList.FilteredItems.Select(c => c.Name).ToList());
        }

        private void ConditionValueParameterEditorForm_Load(object sender, EventArgs e)
        {

        }
    }
}
