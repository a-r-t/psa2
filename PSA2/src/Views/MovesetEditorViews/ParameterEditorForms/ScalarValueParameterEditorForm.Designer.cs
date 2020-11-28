namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    partial class ScalarValueParameterEditorForm
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
            this.parameterValueTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.validationPictureBox = new System.Windows.Forms.PictureBox();
            this.maxLabel = new System.Windows.Forms.Label();
            this.minLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.validationPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // parameterValueTextBox
            // 
            this.parameterValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterValueTextBox.Location = new System.Drawing.Point(46, 3);
            this.parameterValueTextBox.Name = "parameterValueTextBox";
            this.parameterValueTextBox.Size = new System.Drawing.Size(120, 20);
            this.parameterValueTextBox.TabIndex = 6;
            this.parameterValueTextBox.TextChanged += new System.EventHandler(this.parameterValueTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Value:";
            // 
            // validationPictureBox
            // 
            this.validationPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.validationPictureBox.Location = new System.Drawing.Point(171, 3);
            this.validationPictureBox.Name = "validationPictureBox";
            this.validationPictureBox.Size = new System.Drawing.Size(20, 20);
            this.validationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.validationPictureBox.TabIndex = 7;
            this.validationPictureBox.TabStop = false;
            // 
            // maxLabel
            // 
            this.maxLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.maxLabel.AutoSize = true;
            this.maxLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.maxLabel.Location = new System.Drawing.Point(2, 36);
            this.maxLabel.Name = "maxLabel";
            this.maxLabel.Size = new System.Drawing.Size(35, 13);
            this.maxLabel.TabIndex = 8;
            this.maxLabel.Text = "label1";
            // 
            // minLabel
            // 
            this.minLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.minLabel.AutoSize = true;
            this.minLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minLabel.Location = new System.Drawing.Point(2, 55);
            this.minLabel.Name = "minLabel";
            this.minLabel.Size = new System.Drawing.Size(35, 13);
            this.minLabel.TabIndex = 9;
            this.minLabel.Text = "label1";
            // 
            // ScalarValueParameterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.minLabel);
            this.Controls.Add(this.maxLabel);
            this.Controls.Add(this.validationPictureBox);
            this.Controls.Add(this.parameterValueTextBox);
            this.Controls.Add(this.label2);
            this.Name = "ScalarValueParameterEditorForm";
            this.Size = new System.Drawing.Size(195, 70);
            ((System.ComponentModel.ISupportInitialize)(this.validationPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox parameterValueTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox validationPictureBox;
        private System.Windows.Forms.Label maxLabel;
        private System.Windows.Forms.Label minLabel;
    }
}
