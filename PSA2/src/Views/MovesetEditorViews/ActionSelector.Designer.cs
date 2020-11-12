namespace PSA2.src.Views.MovesetEditorViews
{
    partial class ActionSelector
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
            this.actionsListScintilla = new PSA2.src.Views.CustomControls.ScintillaListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.descriptionScintilla = new PSA2.src.Views.CustomControls.ScintillaExt();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // actionsListScintilla
            // 
            this.actionsListScintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.actionsListScintilla.BackgroundColor = System.Drawing.Color.White;
            this.actionsListScintilla.CaretStyle = ScintillaNET.CaretStyle.Invisible;
            this.actionsListScintilla.CurrentCursor = System.Windows.Forms.Cursors.Arrow;
            this.actionsListScintilla.FullLineSelect = false;
            this.actionsListScintilla.ItemBackColor = System.Drawing.Color.White;
            this.actionsListScintilla.ItemForeColor = System.Drawing.Color.Black;
            this.actionsListScintilla.Location = new System.Drawing.Point(4, 5);
            this.actionsListScintilla.Name = "actionsListScintilla";
            this.actionsListScintilla.ReadOnly = true;
            this.actionsListScintilla.SelectedItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(79)))), ((int)(((byte)(120)))));
            this.actionsListScintilla.SelectedItemForeColor = System.Drawing.Color.White;
            this.actionsListScintilla.ShowLineNumbers = false;
            this.actionsListScintilla.Size = new System.Drawing.Size(193, 215);
            this.actionsListScintilla.TabIndex = 0;
            this.actionsListScintilla.SelectedIndexChanged += new System.EventHandler(this.actionsListScintilla_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Name:";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(47, 6);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(150, 20);
            this.nameTextBox.TabIndex = 3;
            // 
            // descriptionScintilla
            // 
            this.descriptionScintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.descriptionScintilla.CurrentCursor = null;
            this.descriptionScintilla.FullLineSelect = false;
            this.descriptionScintilla.Location = new System.Drawing.Point(3, 32);
            this.descriptionScintilla.Name = "descriptionScintilla";
            this.descriptionScintilla.ShowLineNumbers = false;
            this.descriptionScintilla.Size = new System.Drawing.Size(194, 146);
            this.descriptionScintilla.TabIndex = 5;
            this.descriptionScintilla.Text = "scintillaExt1";
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
            this.splitContainer1.Panel1.Controls.Add(this.actionsListScintilla);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.descriptionScintilla);
            this.splitContainer1.Panel2.Controls.Add(this.nameTextBox);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new System.Drawing.Size(200, 408);
            this.splitContainer1.SplitterDistance = 223;
            this.splitContainer1.TabIndex = 6;
            // 
            // ActionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ActionSelector";
            this.Size = new System.Drawing.Size(200, 408);
            this.Load += new System.EventHandler(this.ActionSelector_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControls.ScintillaListBox actionsListScintilla;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nameTextBox;
        private CustomControls.ScintillaExt descriptionScintilla;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
