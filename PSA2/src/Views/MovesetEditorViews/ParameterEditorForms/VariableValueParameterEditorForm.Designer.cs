namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    partial class VariableValueParameterEditorForm
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
            this.validationPictureBox = new System.Windows.Forms.PictureBox();
            this.idTextBox = new System.Windows.Forms.TextBox();
            this.memoryTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dataTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.variableNameLabel = new System.Windows.Forms.Label();
            this.searchTextBox = new PSA2.src.Views.CustomControls.SearchTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dataTypeFilterComboBox = new System.Windows.Forms.ComboBox();
            this.variablesScintilla = new PSA2.src.Views.CustomControls.ScintillaListBox();
            this.applyButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.validationPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // validationPictureBox
            // 
            this.validationPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.validationPictureBox.Location = new System.Drawing.Point(175, 57);
            this.validationPictureBox.Name = "validationPictureBox";
            this.validationPictureBox.Size = new System.Drawing.Size(20, 20);
            this.validationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.validationPictureBox.TabIndex = 7;
            this.validationPictureBox.TabStop = false;
            // 
            // idTextBox
            // 
            this.idTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.idTextBox.Location = new System.Drawing.Point(83, 57);
            this.idTextBox.Name = "idTextBox";
            this.idTextBox.Size = new System.Drawing.Size(86, 20);
            this.idTextBox.TabIndex = 13;
            this.idTextBox.TextChanged += new System.EventHandler(this.idTextBox_TextChanged);
            // 
            // memoryTypeComboBox
            // 
            this.memoryTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.memoryTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.memoryTypeComboBox.FormattingEnabled = true;
            this.memoryTypeComboBox.Location = new System.Drawing.Point(83, 3);
            this.memoryTypeComboBox.Name = "memoryTypeComboBox";
            this.memoryTypeComboBox.Size = new System.Drawing.Size(112, 21);
            this.memoryTypeComboBox.TabIndex = 20;
            this.memoryTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.memoryTypeComboBox_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 19;
            this.label5.Text = "Memory Type:";
            // 
            // dataTypeComboBox
            // 
            this.dataTypeComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTypeComboBox.FormattingEnabled = true;
            this.dataTypeComboBox.Location = new System.Drawing.Point(83, 30);
            this.dataTypeComboBox.Name = "dataTypeComboBox";
            this.dataTypeComboBox.Size = new System.Drawing.Size(112, 21);
            this.dataTypeComboBox.TabIndex = 18;
            this.dataTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.dataTypeComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Data Type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "ID:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(2, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(195, 2);
            this.label6.TabIndex = 21;
            // 
            // variableNameLabel
            // 
            this.variableNameLabel.AutoEllipsis = true;
            this.variableNameLabel.AutoSize = true;
            this.variableNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.variableNameLabel.Location = new System.Drawing.Point(3, 85);
            this.variableNameLabel.Name = "variableNameLabel";
            this.variableNameLabel.Size = new System.Drawing.Size(110, 13);
            this.variableNameLabel.TabIndex = 22;
            this.variableNameLabel.Text = "Unknown Variable";
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(3, 123);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(192, 20);
            this.searchTextBox.TabIndex = 23;
            this.searchTextBox.Text = "🔍";
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 13);
            this.label8.TabIndex = 25;
            this.label8.Text = "Data Type:";
            // 
            // dataTypeFilterComboBox
            // 
            this.dataTypeFilterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataTypeFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataTypeFilterComboBox.FormattingEnabled = true;
            this.dataTypeFilterComboBox.Location = new System.Drawing.Point(69, 149);
            this.dataTypeFilterComboBox.Name = "dataTypeFilterComboBox";
            this.dataTypeFilterComboBox.Size = new System.Drawing.Size(125, 21);
            this.dataTypeFilterComboBox.TabIndex = 27;
            this.dataTypeFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.dataTypeFilterComboBox_SelectedIndexChanged);
            // 
            // variablesScintilla
            // 
            this.variablesScintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.variablesScintilla.BackgroundColor = System.Drawing.Color.White;
            this.variablesScintilla.CaretStyle = ScintillaNET.CaretStyle.Invisible;
            this.variablesScintilla.CurrentCursor = System.Windows.Forms.Cursors.Arrow;
            this.variablesScintilla.FontFamily = "Consolas";
            this.variablesScintilla.FontSize = 10F;
            this.variablesScintilla.FullLineSelect = false;
            this.variablesScintilla.ItemBackColor = System.Drawing.Color.White;
            this.variablesScintilla.ItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.variablesScintilla.Location = new System.Drawing.Point(3, 175);
            this.variablesScintilla.Name = "variablesScintilla";
            this.variablesScintilla.ReadOnly = true;
            this.variablesScintilla.SelectedIndex = -1;
            this.variablesScintilla.SelectedItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(79)))), ((int)(((byte)(120)))));
            this.variablesScintilla.SelectedItemForeColor = System.Drawing.Color.White;
            this.variablesScintilla.ShowLineNumbers = false;
            this.variablesScintilla.Size = new System.Drawing.Size(191, 205);
            this.variablesScintilla.TabIndex = 28;
            this.variablesScintilla.DoubleClick += new System.EventHandler<ScintillaNET.DoubleClickEventArgs>(this.variablesScintilla_DoubleClick);
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(5, 386);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(189, 23);
            this.applyButton.TabIndex = 29;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // VariableValueParameterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.variablesScintilla);
            this.Controls.Add(this.dataTypeFilterComboBox);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.variableNameLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.validationPictureBox);
            this.Controls.Add(this.idTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataTypeComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.memoryTypeComboBox);
            this.Name = "VariableValueParameterEditorForm";
            this.Size = new System.Drawing.Size(198, 412);
            ((System.ComponentModel.ISupportInitialize)(this.validationPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox validationPictureBox;
        private System.Windows.Forms.TextBox idTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox memoryTypeComboBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox dataTypeComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label variableNameLabel;
        private CustomControls.SearchTextBox searchTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox dataTypeFilterComboBox;
        private CustomControls.ScintillaListBox variablesScintilla;
        private System.Windows.Forms.Button applyButton;
    }
}
