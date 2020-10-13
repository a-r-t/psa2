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
            this.codeBlockCommandsListBox = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.commandParamsView = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // codeBlockCommandsListBox
            // 
            this.codeBlockCommandsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBlockCommandsListBox.FormattingEnabled = true;
            this.codeBlockCommandsListBox.HorizontalScrollbar = true;
            this.codeBlockCommandsListBox.Location = new System.Drawing.Point(0, 0);
            this.codeBlockCommandsListBox.Name = "codeBlockCommandsListBox";
            this.codeBlockCommandsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.codeBlockCommandsListBox.Size = new System.Drawing.Size(790, 349);
            this.codeBlockCommandsListBox.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.codeBlockCommandsListBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.commandParamsView);
            this.splitContainer1.Size = new System.Drawing.Size(790, 449);
            this.splitContainer1.SplitterDistance = 349;
            this.splitContainer1.TabIndex = 8;
            // 
            // commandParamsView
            // 
            this.commandParamsView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandParamsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandParamsView.Location = new System.Drawing.Point(0, 0);
            this.commandParamsView.Name = "commandParamsView";
            this.commandParamsView.Size = new System.Drawing.Size(790, 96);
            this.commandParamsView.TabIndex = 0;
            // 
            // CodeBlockViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "CodeBlockViewer";
            this.Size = new System.Drawing.Size(790, 449);
            this.Load += new System.EventHandler(this.CodeBlockViewer_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListBox codeBlockCommandsListBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel commandParamsView;
    }
}
