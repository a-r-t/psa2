namespace PSA2.src.Views
{
    partial class TabControlTest
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControlCustom1 = new PSA2.src.Views.CustomControls.TabControlCustom();
            this.SuspendLayout();
            // 
            // tabControlCustom1
            // 
            this.tabControlCustom1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCustom1.Location = new System.Drawing.Point(0, 0);
            this.tabControlCustom1.Name = "tabControlCustom1";
            this.tabControlCustom1.Size = new System.Drawing.Size(800, 450);
            this.tabControlCustom1.TabIndex = 0;
            // 
            // TabControlTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControlCustom1);
            this.Name = "TabControlTest";
            this.Text = "TabControlTest";
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControls.TabControlCustom tabControlCustom1;
    }
}