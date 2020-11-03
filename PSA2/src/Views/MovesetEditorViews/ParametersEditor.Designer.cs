namespace PSA2.src.Views.MovesetEditorViews
{
    partial class ParametersEditor
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
            this.parameterNamesListBox = new System.Windows.Forms.ListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.parameterEditor = new PSA2.src.Views.CustomControls.ParameterEditor();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // parameterNamesListBox
            // 
            this.parameterNamesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterNamesListBox.FormattingEnabled = true;
            this.parameterNamesListBox.Location = new System.Drawing.Point(3, 3);
            this.parameterNamesListBox.Name = "parameterNamesListBox";
            this.parameterNamesListBox.Size = new System.Drawing.Size(399, 121);
            this.parameterNamesListBox.TabIndex = 2;
            this.parameterNamesListBox.SelectedIndexChanged += new System.EventHandler(this.parameterNamesListBox_SelectedIndexChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.parameterEditor);
            this.splitContainer1.Size = new System.Drawing.Size(405, 314);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.parameterNamesListBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 133);
            this.panel1.TabIndex = 3;
            // 
            // parameterEditor
            // 
            this.parameterEditor.AutoScroll = true;
            this.parameterEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parameterEditor.Location = new System.Drawing.Point(0, 0);
            this.parameterEditor.Name = "parameterEditor";
            this.parameterEditor.ParameterEntry = null;
            this.parameterEditor.Size = new System.Drawing.Size(405, 177);
            this.parameterEditor.TabIndex = 0;
            // 
            // ParametersEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ParametersEditor";
            this.Size = new System.Drawing.Size(405, 314);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControls.ParameterEditor parameterEditor;
        private System.Windows.Forms.ListBox parameterNamesListBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
    }
}
