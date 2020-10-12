namespace PSA2.src.Views.MovesetEditorViews
{
    partial class CodeBlockViewer
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
            this.codeBlockOptionsListBox = new System.Windows.Forms.ListBox();
            this.codeBlockCommandsListBox = new System.Windows.Forms.ListBox();
            this.commandOptionsListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // codeBlockOptionsListBox
            // 
            this.codeBlockOptionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.codeBlockOptionsListBox.FormattingEnabled = true;
            this.codeBlockOptionsListBox.Location = new System.Drawing.Point(4, 4);
            this.codeBlockOptionsListBox.Name = "codeBlockOptionsListBox";
            this.codeBlockOptionsListBox.Size = new System.Drawing.Size(120, 433);
            this.codeBlockOptionsListBox.TabIndex = 6;
            this.codeBlockOptionsListBox.SelectedIndexChanged += new System.EventHandler(this.codeBlockOptionsListBox_SelectedIndexChanged);
            // 
            // codeBlockCommandsListBox
            // 
            this.codeBlockCommandsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codeBlockCommandsListBox.FormattingEnabled = true;
            this.codeBlockCommandsListBox.HorizontalScrollbar = true;
            this.codeBlockCommandsListBox.Location = new System.Drawing.Point(131, 4);
            this.codeBlockCommandsListBox.Name = "codeBlockCommandsListBox";
            this.codeBlockCommandsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.codeBlockCommandsListBox.Size = new System.Drawing.Size(422, 433);
            this.codeBlockCommandsListBox.TabIndex = 7;
            // 
            // commandOptionsListBox
            // 
            this.commandOptionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandOptionsListBox.FormattingEnabled = true;
            this.commandOptionsListBox.HorizontalScrollbar = true;
            this.commandOptionsListBox.Location = new System.Drawing.Point(559, 4);
            this.commandOptionsListBox.Name = "commandOptionsListBox";
            this.commandOptionsListBox.Size = new System.Drawing.Size(228, 433);
            this.commandOptionsListBox.TabIndex = 8;
            // 
            // CodeBlockViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.commandOptionsListBox);
            this.Controls.Add(this.codeBlockCommandsListBox);
            this.Controls.Add(this.codeBlockOptionsListBox);
            this.Name = "CodeBlockViewer";
            this.Size = new System.Drawing.Size(790, 441);
            this.Load += new System.EventHandler(this.CodeBlockViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox codeBlockOptionsListBox;
        private System.Windows.Forms.ListBox codeBlockCommandsListBox;
        private System.Windows.Forms.ListBox commandOptionsListBox;
    }
}
