﻿namespace PSA2.src.Views.MovesetEditorViews
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
            this.parameterTypeViewer = new System.Windows.Forms.Panel();
            this.parameterTypesComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.parameterEditorFormView = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.parameterTypeViewer.SuspendLayout();
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
            this.splitContainer1.Panel2.Controls.Add(this.parameterTypeViewer);
            this.splitContainer1.Panel2.Controls.Add(this.parameterEditorFormView);
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
            // parameterTypeViewer
            // 
            this.parameterTypeViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterTypeViewer.Controls.Add(this.parameterTypesComboBox);
            this.parameterTypeViewer.Controls.Add(this.label1);
            this.parameterTypeViewer.Controls.Add(this.categoryLabel);
            this.parameterTypeViewer.Location = new System.Drawing.Point(3, 3);
            this.parameterTypeViewer.Name = "parameterTypeViewer";
            this.parameterTypeViewer.Size = new System.Drawing.Size(399, 47);
            this.parameterTypeViewer.TabIndex = 10;
            // 
            // parameterTypesComboBox
            // 
            this.parameterTypesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterTypesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTypesComboBox.FormattingEnabled = true;
            this.parameterTypesComboBox.Location = new System.Drawing.Point(46, 18);
            this.parameterTypesComboBox.Name = "parameterTypesComboBox";
            this.parameterTypesComboBox.Size = new System.Drawing.Size(343, 21);
            this.parameterTypesComboBox.TabIndex = 8;
            this.parameterTypesComboBox.SelectedIndexChanged += new System.EventHandler(this.parameterTypesComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Type:";
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.categoryLabel.Location = new System.Drawing.Point(3, 0);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(57, 13);
            this.categoryLabel.TabIndex = 6;
            this.categoryLabel.Text = "Category";
            // 
            // parameterEditorFormView
            // 
            this.parameterEditorFormView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterEditorFormView.Location = new System.Drawing.Point(3, 48);
            this.parameterEditorFormView.Name = "parameterEditorFormView";
            this.parameterEditorFormView.Size = new System.Drawing.Size(399, 126);
            this.parameterEditorFormView.TabIndex = 9;
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
            this.parameterTypeViewer.ResumeLayout(false);
            this.parameterTypeViewer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox parameterNamesListBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox parameterTypesComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.Panel parameterEditorFormView;
        private System.Windows.Forms.Panel parameterTypeViewer;
    }
}
