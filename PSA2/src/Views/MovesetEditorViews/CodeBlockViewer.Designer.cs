
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.codeBlockCommandsScintilla = new ScintillaNET.Scintilla();
            this.commandParamsView = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.codeBlockCommandsScintilla);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.commandParamsView);
            this.splitContainer1.Size = new System.Drawing.Size(790, 449);
            this.splitContainer1.SplitterDistance = 349;
            this.splitContainer1.TabIndex = 8;
            // 
            // codeBlockCommandsScintilla
            // 
            this.codeBlockCommandsScintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBlockCommandsScintilla.Location = new System.Drawing.Point(0, 0);
            this.codeBlockCommandsScintilla.Name = "codeBlockCommandsScintilla";
            this.codeBlockCommandsScintilla.Size = new System.Drawing.Size(790, 349);
            this.codeBlockCommandsScintilla.TabIndex = 8;
            this.codeBlockCommandsScintilla.Text = "scintilla1";
            this.codeBlockCommandsScintilla.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.codeBlockCommandsScintilla_UpdateUI);
            this.codeBlockCommandsScintilla.TextChanged += new System.EventHandler(this.codeBlockCommandsScintilla_TextChanged);
            // 
            // commandParamsView
            // 
            this.commandParamsView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandParamsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandParamsView.ImeMode = System.Windows.Forms.ImeMode.NoControl;
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
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel commandParamsView;
        private ScintillaNET.Scintilla codeBlockCommandsScintilla;
    }
}
