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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ParametersEditor));
            this.parametersPropertyGrid = new PropertyGridEx.PropertyGridEx();
            this.SuspendLayout();
            // 
            // parametersPropertyGrid
            // 
            // 
            // 
            // 
            this.parametersPropertyGrid.DocCommentDescription.AutoEllipsis = true;
            this.parametersPropertyGrid.DocCommentDescription.Cursor = System.Windows.Forms.Cursors.Default;
            this.parametersPropertyGrid.DocCommentDescription.Location = new System.Drawing.Point(3, 18);
            this.parametersPropertyGrid.DocCommentDescription.Name = "";
            this.parametersPropertyGrid.DocCommentDescription.Size = new System.Drawing.Size(399, 37);
            this.parametersPropertyGrid.DocCommentDescription.TabIndex = 1;
            this.parametersPropertyGrid.DocCommentImage = null;
            // 
            // 
            // 
            this.parametersPropertyGrid.DocCommentTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.parametersPropertyGrid.DocCommentTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.parametersPropertyGrid.DocCommentTitle.Location = new System.Drawing.Point(3, 3);
            this.parametersPropertyGrid.DocCommentTitle.Name = "";
            this.parametersPropertyGrid.DocCommentTitle.Size = new System.Drawing.Size(399, 15);
            this.parametersPropertyGrid.DocCommentTitle.TabIndex = 0;
            this.parametersPropertyGrid.DocCommentTitle.UseMnemonic = false;
            this.parametersPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.parametersPropertyGrid.Name = "parametersPropertyGrid";
            this.parametersPropertyGrid.SelectedObject = ((object)(resources.GetObject("parametersPropertyGrid.SelectedObject")));
            this.parametersPropertyGrid.ShowCustomProperties = true;
            this.parametersPropertyGrid.Size = new System.Drawing.Size(405, 314);
            this.parametersPropertyGrid.TabIndex = 0;
            // 
            // 
            // 
            this.parametersPropertyGrid.ToolStrip.AccessibleName = "ToolBar";
            this.parametersPropertyGrid.ToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ToolBar;
            this.parametersPropertyGrid.ToolStrip.AllowMerge = false;
            this.parametersPropertyGrid.ToolStrip.AutoSize = false;
            this.parametersPropertyGrid.ToolStrip.CanOverflow = false;
            this.parametersPropertyGrid.ToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.parametersPropertyGrid.ToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.parametersPropertyGrid.ToolStrip.Location = new System.Drawing.Point(0, 1);
            this.parametersPropertyGrid.ToolStrip.Name = "";
            this.parametersPropertyGrid.ToolStrip.Padding = new System.Windows.Forms.Padding(2, 0, 1, 0);
            this.parametersPropertyGrid.ToolStrip.Size = new System.Drawing.Size(405, 25);
            this.parametersPropertyGrid.ToolStrip.TabIndex = 1;
            this.parametersPropertyGrid.ToolStrip.TabStop = true;
            this.parametersPropertyGrid.ToolStrip.Text = "PropertyGridToolBar";
            // 
            // ParametersEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.parametersPropertyGrid);
            this.Name = "ParametersEditor";
            this.Size = new System.Drawing.Size(405, 314);
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGridEx.PropertyGridEx parametersPropertyGrid;
    }
}
