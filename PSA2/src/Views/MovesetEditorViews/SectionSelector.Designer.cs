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
            this.sectionsTreeView = new PSA2.src.Views.SectionsTreeView();
            this.SuspendLayout();
            // 
            // sectionsTreeView
            // 
            this.sectionsTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sectionsTreeView.Location = new System.Drawing.Point(0, 0);
            this.sectionsTreeView.Name = "sectionsTreeView";
            this.sectionsTreeView.Size = new System.Drawing.Size(178, 461);
            this.sectionsTreeView.TabIndex = 0;
            this.sectionsTreeView.SelectedNodeChanged += new System.Windows.Forms.TreeViewEventHandler(this.sectionsTreeView_SelectedNodeChanged);
            this.sectionsTreeView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.sectionsTreeView_MouseDoubleClick);
            // 
            // LocationSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.sectionsTreeView);
            this.Name = "LocationSelector";
            this.Size = new System.Drawing.Size(178, 461);
            this.Load += new System.EventHandler(this.LocationSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private SectionsTreeView sectionsTreeView;
    }
}
