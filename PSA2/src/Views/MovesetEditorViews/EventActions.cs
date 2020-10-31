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

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class EventActions : ObservableUserControl<IEventActionsListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;

        public EventActions(PsaMovesetHandler psaMovesetHandler)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            InitializeComponent();
        }

        private void insertCommandAboveBtn_Click(object sender, EventArgs e)
        {
            foreach (IEventActionsListener listener in listeners)
            {
                listener.OnInsertCommandAbove();
            }
        }

        private void insertCommandBelowBtn_Click(object sender, EventArgs e)
        {
            foreach (IEventActionsListener listener in listeners)
            {
                listener.OnInsertCommandBelow();
            }
        }


        private void appendCommandBtn_Click(object sender, EventArgs e)
        {
            foreach (IEventActionsListener listener in listeners)
            {
                listener.OnAppendCommand();
            }
        }

        private void replaceCommandBtn_Click(object sender, EventArgs e)
        {
            foreach (IEventActionsListener listener in listeners)
            {
                listener.OnReplaceCommand();
            }
        }

        private void moveCommandUpBtn_Click(object sender, EventArgs e)
        {
            foreach (IEventActionsListener listener in listeners)
            {
                listener.OnMoveCommandUp();
            }
        }

        private void moveCommandDownBtn_Click(object sender, EventArgs e)
        {
            foreach (IEventActionsListener listener in listeners)
            {
                listener.OnMoveCommandDown();
            }
        }

        private void removeCommandBtn_Click(object sender, EventArgs e)
        {
            foreach (IEventActionsListener listener in listeners)
            {
                listener.OnRemoveCommand();
            }
        }

        private void copyBtn_Click(object sender, EventArgs e)
        {

        }

        private void pasteBtn_Click(object sender, EventArgs e)
        {

        }

        private void cutBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
