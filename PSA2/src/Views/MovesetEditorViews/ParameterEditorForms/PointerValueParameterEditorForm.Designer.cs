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
            this.eventTypeComboBox = new System.Windows.Forms.ComboBox();
            this.codeBlockComboBox = new System.Windows.Forms.ComboBox();
            this.commandIndexComboBox = new System.Windows.Forms.ComboBox();
            this.applyButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.sectionComboBox = new System.Windows.Forms.ComboBox();
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
            this.conditionNameLabel.Size = new System.Drawing.Size(113, 13);
            this.conditionNameLabel.TabIndex = 10;
            this.conditionNameLabel.Text = "Unknown Location";
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
            // eventTypeComboBox
            // 
            this.eventTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eventTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eventTypeComboBox.FormattingEnabled = true;
            this.eventTypeComboBox.Location = new System.Drawing.Point(70, 68);
            this.eventTypeComboBox.Name = "eventTypeComboBox";
            this.eventTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.eventTypeComboBox.TabIndex = 12;
            this.eventTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.eventTypeComboBox_SelectedIndexChanged);
            // 
            // codeBlockComboBox
            // 
            this.codeBlockComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.codeBlockComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.codeBlockComboBox.FormattingEnabled = true;
            this.codeBlockComboBox.Location = new System.Drawing.Point(70, 123);
            this.codeBlockComboBox.Name = "codeBlockComboBox";
            this.codeBlockComboBox.Size = new System.Drawing.Size(121, 21);
            this.codeBlockComboBox.TabIndex = 13;
            this.codeBlockComboBox.SelectedIndexChanged += new System.EventHandler(this.codeBlockComboBox_SelectedIndexChanged);
            // 
            // commandIndexComboBox
            // 
            this.commandIndexComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.commandIndexComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.commandIndexComboBox.FormattingEnabled = true;
            this.commandIndexComboBox.Location = new System.Drawing.Point(70, 150);
            this.commandIndexComboBox.Name = "commandIndexComboBox";
            this.commandIndexComboBox.Size = new System.Drawing.Size(121, 21);
            this.commandIndexComboBox.TabIndex = 14;
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(4, 179);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(189, 23);
            this.applyButton.TabIndex = 15;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Event Type:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Code Block:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 153);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Command:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 22;
            this.label7.Text = "Section:";
            // 
            // sectionComboBox
            // 
            this.sectionComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sectionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sectionComboBox.FormattingEnabled = true;
            this.sectionComboBox.Location = new System.Drawing.Point(70, 95);
            this.sectionComboBox.Name = "sectionComboBox";
            this.sectionComboBox.Size = new System.Drawing.Size(121, 21);
            this.sectionComboBox.TabIndex = 21;
            this.sectionComboBox.SelectedIndexChanged += new System.EventHandler(this.sectionComboBox_SelectedIndexChanged);
            // 
            // PointerValueParameterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label7);
            this.Controls.Add(this.sectionComboBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.commandIndexComboBox);
            this.Controls.Add(this.codeBlockComboBox);
            this.Controls.Add(this.eventTypeComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.conditionNameLabel);
            this.Controls.Add(this.validationPictureBox);
            this.Controls.Add(this.parameterValueTextBox);
            this.Controls.Add(this.label2);
            this.Name = "PointerValueParameterEditorForm";
            this.Size = new System.Drawing.Size(195, 205);
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
        private System.Windows.Forms.ComboBox eventTypeComboBox;
        private System.Windows.Forms.ComboBox codeBlockComboBox;
        private System.Windows.Forms.ComboBox commandIndexComboBox;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox sectionComboBox;
    }
}
