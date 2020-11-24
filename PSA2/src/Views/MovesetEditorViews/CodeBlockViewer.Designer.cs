
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
            this.codeBlockCommandsScintilla = new PSA2.src.Views.CustomControls.ScintillaCodeBlock();
            this.label1 = new System.Windows.Forms.Label();
            this.codeBlockComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // codeBlockCommandsScintilla
            // 
            this.codeBlockCommandsScintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codeBlockCommandsScintilla.CurrentCursor = System.Windows.Forms.Cursors.Arrow;
            this.codeBlockCommandsScintilla.FullLineSelect = true;
            this.codeBlockCommandsScintilla.Location = new System.Drawing.Point(0, 31);
            this.codeBlockCommandsScintilla.MultipleSelection = true;
            this.codeBlockCommandsScintilla.Name = "codeBlockCommandsScintilla";
            this.codeBlockCommandsScintilla.PsaCommands = null;
            this.codeBlockCommandsScintilla.ReadOnly = true;
            this.codeBlockCommandsScintilla.ShowLineNumbers = false;
            this.codeBlockCommandsScintilla.Size = new System.Drawing.Size(790, 418);
            this.codeBlockCommandsScintilla.TabIndex = 8;
            this.codeBlockCommandsScintilla.WrapStartIndent = 4;
            this.codeBlockCommandsScintilla.UpdateUI += new System.EventHandler<ScintillaNET.UpdateUIEventArgs>(this.codeBlockCommandsScintilla_UpdateUI);
            this.codeBlockCommandsScintilla.TextChanged += new System.EventHandler(this.codeBlockCommandsScintilla_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Code Block:";
            // 
            // codeBlockComboBox
            // 
            this.codeBlockComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codeBlockComboBox.FormattingEnabled = true;
            this.codeBlockComboBox.Location = new System.Drawing.Point(74, 4);
            this.codeBlockComboBox.Name = "codeBlockComboBox";
            this.codeBlockComboBox.Size = new System.Drawing.Size(121, 21);
            this.codeBlockComboBox.TabIndex = 10;
            this.codeBlockComboBox.SelectedIndexChanged += new System.EventHandler(this.codeBlockComboBox_SelectedIndexChanged);
            // 
            // CodeBlockViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.codeBlockComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.codeBlockCommandsScintilla);
            this.Name = "CodeBlockViewer";
            this.Size = new System.Drawing.Size(790, 449);
            this.Load += new System.EventHandler(this.CodeBlockViewer_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ScintillaCodeBlock codeBlockCommandsScintilla;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox codeBlockComboBox;
    }
}
