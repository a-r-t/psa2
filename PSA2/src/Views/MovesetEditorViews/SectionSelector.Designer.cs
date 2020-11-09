namespace PSA2.src.Views.MovesetEditorViews
{
    partial class SectionSelector
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
            this.sectionComboBox = new System.Windows.Forms.ComboBox();
            this.sectionSelectorFormViewer = new System.Windows.Forms.Panel();
            this.openInThisTabButton = new System.Windows.Forms.Button();
            this.openInNewTabButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sectionComboBox
            // 
            this.sectionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sectionComboBox.FormattingEnabled = true;
            this.sectionComboBox.Location = new System.Drawing.Point(3, 3);
            this.sectionComboBox.Name = "sectionComboBox";
            this.sectionComboBox.Size = new System.Drawing.Size(172, 21);
            this.sectionComboBox.TabIndex = 0;
            this.sectionComboBox.SelectedIndexChanged += new System.EventHandler(this.sectionComboBox_SelectedIndexChanged);
            // 
            // sectionSelectorFormViewer
            // 
            this.sectionSelectorFormViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionSelectorFormViewer.Location = new System.Drawing.Point(0, 30);
            this.sectionSelectorFormViewer.Name = "sectionSelectorFormViewer";
            this.sectionSelectorFormViewer.Size = new System.Drawing.Size(178, 361);
            this.sectionSelectorFormViewer.TabIndex = 1;
            // 
            // openInThisTabButton
            // 
            this.openInThisTabButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openInThisTabButton.Location = new System.Drawing.Point(4, 398);
            this.openInThisTabButton.Name = "openInThisTabButton";
            this.openInThisTabButton.Size = new System.Drawing.Size(171, 23);
            this.openInThisTabButton.TabIndex = 2;
            this.openInThisTabButton.Text = "Open In Current Tab";
            this.openInThisTabButton.UseVisualStyleBackColor = true;
            // 
            // openInNewTabButton
            // 
            this.openInNewTabButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openInNewTabButton.Location = new System.Drawing.Point(3, 427);
            this.openInNewTabButton.Name = "openInNewTabButton";
            this.openInNewTabButton.Size = new System.Drawing.Size(171, 23);
            this.openInNewTabButton.TabIndex = 3;
            this.openInNewTabButton.Text = "Open In New Tab";
            this.openInNewTabButton.UseVisualStyleBackColor = true;
            // 
            // SectionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.openInNewTabButton);
            this.Controls.Add(this.openInThisTabButton);
            this.Controls.Add(this.sectionSelectorFormViewer);
            this.Controls.Add(this.sectionComboBox);
            this.Name = "SectionSelector";
            this.Size = new System.Drawing.Size(178, 457);
            this.Load += new System.EventHandler(this.LocationSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox sectionComboBox;
        private System.Windows.Forms.Panel sectionSelectorFormViewer;
        private System.Windows.Forms.Button openInThisTabButton;
        private System.Windows.Forms.Button openInNewTabButton;
    }
}
