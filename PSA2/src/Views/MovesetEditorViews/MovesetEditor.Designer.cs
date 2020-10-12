namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class MovesetEditor : ObservableUserControl<IMovesetEditorListener>
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
            this.selectorView = new System.Windows.Forms.Panel();
            this.codeBlockView = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // selectorView
            // 
            this.selectorView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.selectorView.Location = new System.Drawing.Point(3, 3);
            this.selectorView.Name = "selectorView";
            this.selectorView.Size = new System.Drawing.Size(200, 387);
            this.selectorView.TabIndex = 0;
            // 
            // codeBlockView
            // 
            this.codeBlockView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codeBlockView.Location = new System.Drawing.Point(209, 3);
            this.codeBlockView.Name = "codeBlockView";
            this.codeBlockView.Size = new System.Drawing.Size(310, 387);
            this.codeBlockView.TabIndex = 1;
            // 
            // MovesetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.codeBlockView);
            this.Controls.Add(this.selectorView);
            this.Name = "MovesetEditor";
            this.Size = new System.Drawing.Size(522, 393);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel selectorView;
        private System.Windows.Forms.Panel codeBlockView;
    }
}
