namespace PSA2.src.Views.MovesetEditorViews
{
    partial class CommandSelector
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.commandsListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // commandsListBox
            // 
            this.commandsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandsListBox.FormattingEnabled = true;
            this.commandsListBox.HorizontalScrollbar = true;
            this.commandsListBox.Location = new System.Drawing.Point(3, 3);
            this.commandsListBox.Name = "commandsListBox";
            this.commandsListBox.Size = new System.Drawing.Size(147, 355);
            this.commandsListBox.TabIndex = 0;
            this.commandsListBox.SelectedIndexChanged += new System.EventHandler(this.commandsListBox_SelectedIndexChanged);
            // 
            // CommandSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.commandsListBox);
            this.Name = "CommandSelector";
            this.Size = new System.Drawing.Size(153, 363);
            this.Load += new System.EventHandler(this.CommandSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox commandsListBox;
    }
}
