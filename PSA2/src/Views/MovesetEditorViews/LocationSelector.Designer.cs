namespace PSA2.src.Views.MovesetEditorViews
{
    partial class LocationSelector
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
            this.sectionsTabControl = new System.Windows.Forms.TabControl();
            this.actionsTab = new System.Windows.Forms.TabPage();
            this.actionOptionsListBox = new System.Windows.Forms.ListBox();
            this.subActionsTab = new System.Windows.Forms.TabPage();
            this.subActionOptionsListBox = new System.Windows.Forms.ListBox();
            this.sectionsTabControl.SuspendLayout();
            this.actionsTab.SuspendLayout();
            this.subActionsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // sectionsTabControl
            // 
            this.sectionsTabControl.Controls.Add(this.actionsTab);
            this.sectionsTabControl.Controls.Add(this.subActionsTab);
            this.sectionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionsTabControl.Location = new System.Drawing.Point(0, 0);
            this.sectionsTabControl.Name = "sectionsTabControl";
            this.sectionsTabControl.SelectedIndex = 0;
            this.sectionsTabControl.Size = new System.Drawing.Size(178, 461);
            this.sectionsTabControl.TabIndex = 0;
            this.sectionsTabControl.SelectedIndexChanged += new System.EventHandler(this.sectionsTabControl_SelectedIndexChanged);
            // 
            // actionsTab
            // 
            this.actionsTab.Controls.Add(this.actionOptionsListBox);
            this.actionsTab.Location = new System.Drawing.Point(4, 22);
            this.actionsTab.Name = "actionsTab";
            this.actionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.actionsTab.Size = new System.Drawing.Size(170, 435);
            this.actionsTab.TabIndex = 0;
            this.actionsTab.Text = "Actions";
            this.actionsTab.UseVisualStyleBackColor = true;
            // 
            // actionOptionsListBox
            // 
            this.actionOptionsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.actionOptionsListBox.FormattingEnabled = true;
            this.actionOptionsListBox.Location = new System.Drawing.Point(3, 3);
            this.actionOptionsListBox.Name = "actionOptionsListBox";
            this.actionOptionsListBox.Size = new System.Drawing.Size(164, 429);
            this.actionOptionsListBox.TabIndex = 0;
            this.actionOptionsListBox.SelectedIndexChanged += new System.EventHandler(this.actionOptionsListBox_SelectedIndexChanged);
            // 
            // subActionsTab
            // 
            this.subActionsTab.Controls.Add(this.subActionOptionsListBox);
            this.subActionsTab.Location = new System.Drawing.Point(4, 22);
            this.subActionsTab.Name = "subActionsTab";
            this.subActionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.subActionsTab.Size = new System.Drawing.Size(170, 435);
            this.subActionsTab.TabIndex = 1;
            this.subActionsTab.Text = "SubActions";
            this.subActionsTab.UseVisualStyleBackColor = true;
            // 
            // subActionOptionsListBox
            // 
            this.subActionOptionsListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.subActionOptionsListBox.FormattingEnabled = true;
            this.subActionOptionsListBox.Location = new System.Drawing.Point(3, 3);
            this.subActionOptionsListBox.Name = "subActionOptionsListBox";
            this.subActionOptionsListBox.Size = new System.Drawing.Size(164, 429);
            this.subActionOptionsListBox.TabIndex = 0;
            this.subActionOptionsListBox.SelectedIndexChanged += new System.EventHandler(this.subActionOptionsListBox_SelectedIndexChanged);
            // 
            // LocationSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionsTabControl);
            this.Name = "LocationSelector";
            this.Size = new System.Drawing.Size(178, 461);
            this.Load += new System.EventHandler(this.LocationSelector_Load);
            this.sectionsTabControl.ResumeLayout(false);
            this.actionsTab.ResumeLayout(false);
            this.subActionsTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl sectionsTabControl;
        private System.Windows.Forms.TabPage actionsTab;
        private System.Windows.Forms.TabPage subActionsTab;
        private System.Windows.Forms.ListBox actionOptionsListBox;
        private System.Windows.Forms.ListBox subActionOptionsListBox;
    }
}
