namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    partial class PointerValueParameterEditorForm
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
            this.conditionNameLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
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
            // conditionNameLabel
            // 
            this.conditionNameLabel.AutoEllipsis = true;
            this.conditionNameLabel.AutoSize = true;
            this.conditionNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conditionNameLabel.Location = new System.Drawing.Point(1, 32);
            this.conditionNameLabel.Name = "conditionNameLabel";
            this.conditionNameLabel.Size = new System.Drawing.Size(117, 13);
            this.conditionNameLabel.TabIndex = 10;
            this.conditionNameLabel.Text = "Unknown Condition";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 2);
            this.label1.TabIndex = 11;
            // 
            // PointerValueParameterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.conditionNameLabel);
            this.Controls.Add(this.validationPictureBox);
            this.Controls.Add(this.parameterValueTextBox);
            this.Controls.Add(this.label2);
            this.Name = "PointerValueParameterEditorForm";
            this.Size = new System.Drawing.Size(195, 289);
            ((System.ComponentModel.ISupportInitialize)(this.validationPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox parameterValueTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox validationPictureBox;
        private System.Windows.Forms.Label conditionNameLabel;
        private System.Windows.Forms.Label label1;
    }
}
