
using PSA2.src.Views.CustomControls;

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
            this.codeBlockCommandsScintilla = new PSA2.src.Views.CustomControls.ScintillaExt();
            this.SuspendLayout();
            // 
            // codeBlockCommandsScintilla
            // 
            this.codeBlockCommandsScintilla.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBlockCommandsScintilla.FullLineSelect = true;
            this.codeBlockCommandsScintilla.Location = new System.Drawing.Point(0, 0);
            this.codeBlockCommandsScintilla.Name = "codeBlockCommandsScintilla";
            this.codeBlockCommandsScintilla.Size = new System.Drawing.Size(790, 449);
            this.codeBlockCommandsScintilla.TabIndex = 8;
            this.codeBlockCommandsScintilla.Text = "scintilla1";
            this.codeBlockCommandsScintilla.WrapStartIndent = 4;
            this.codeBlockCommandsScintilla.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.codeBlockCommandsScintilla_UpdateUI);
            this.codeBlockCommandsScintilla.TextChanged += new System.EventHandler(this.codeBlockCommandsScintilla_TextChanged);
            this.codeBlockCommandsScintilla.MouseCaptureChanged += new System.EventHandler(this.codeBlockCommandsScintilla_MouseCaptureChanged);
            this.codeBlockCommandsScintilla.MouseMove += new System.Windows.Forms.MouseEventHandler(this.codeBlockCommandsScintilla_MouseMove);
            // 
            // CodeBlockViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.codeBlockCommandsScintilla);
            this.Name = "CodeBlockViewer";
            this.Size = new System.Drawing.Size(790, 449);
            this.Load += new System.EventHandler(this.CodeBlockViewer_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private ScintillaExt codeBlockCommandsScintilla;
    }
}
