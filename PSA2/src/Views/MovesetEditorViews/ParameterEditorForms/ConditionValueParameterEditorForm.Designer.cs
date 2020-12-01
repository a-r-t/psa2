using static PSA2.src.Configuration.ConditionsConfig;

namespace PSA2.src.Views.MovesetEditorViews.ParameterEditorForms
{
    partial class ConditionValueParameterEditorForm
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
            this.conditionsScintilla = new PSA2.src.Views.CustomControls.ScintillaListBox();
            this.conditionValueTextBox = new System.Windows.Forms.TextBox();
            this.searchTextBox = new CustomControls.ConditionsSearchTextBox();
            this.conditionNameLabel = new System.Windows.Forms.Label();
            this.applyButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.validationPictureBox = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.validationPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // conditionsScintilla
            // 
            this.conditionsScintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conditionsScintilla.BackgroundColor = System.Drawing.Color.White;
            this.conditionsScintilla.CaretStyle = ScintillaNET.CaretStyle.Invisible;
            this.conditionsScintilla.CurrentCursor = System.Windows.Forms.Cursors.Arrow;
            this.conditionsScintilla.FontFamily = "Consolas";
            this.conditionsScintilla.FontSize = 10F;
            this.conditionsScintilla.FullLineSelect = false;
            this.conditionsScintilla.ItemBackColor = System.Drawing.Color.White;
            this.conditionsScintilla.ItemForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.conditionsScintilla.Location = new System.Drawing.Point(3, 101);
            this.conditionsScintilla.Name = "conditionsScintilla";
            this.conditionsScintilla.ReadOnly = true;
            this.conditionsScintilla.SelectedIndex = -1;
            this.conditionsScintilla.SelectedItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(79)))), ((int)(((byte)(120)))));
            this.conditionsScintilla.SelectedItemForeColor = System.Drawing.Color.White;
            this.conditionsScintilla.ShowLineNumbers = false;
            this.conditionsScintilla.Size = new System.Drawing.Size(189, 202);
            this.conditionsScintilla.TabIndex = 0;
            this.conditionsScintilla.DoubleClick += new System.EventHandler<ScintillaNET.DoubleClickEventArgs>(this.conditionsScintilla_DoubleClick);
            // 
            // conditionValueTextBox
            // 
            this.conditionValueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.conditionValueTextBox.Location = new System.Drawing.Point(46, 3);
            this.conditionValueTextBox.MaxLength = 8;
            this.conditionValueTextBox.Name = "conditionValueTextBox";
            this.conditionValueTextBox.Size = new System.Drawing.Size(120, 20);
            this.conditionValueTextBox.TabIndex = 7;
            this.conditionValueTextBox.TextChanged += new System.EventHandler(this.conditionValueTextBox_TextChanged);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(3, 71);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(189, 20);
            this.searchTextBox.TabIndex = 8;
            this.searchTextBox.Text = "🔍";
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            // 
            // conditionNameLabel
            // 
            this.conditionNameLabel.AutoEllipsis = true;
            this.conditionNameLabel.AutoSize = true;
            this.conditionNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.conditionNameLabel.Location = new System.Drawing.Point(1, 32);
            this.conditionNameLabel.Name = "conditionNameLabel";
            this.conditionNameLabel.Size = new System.Drawing.Size(117, 13);
            this.conditionNameLabel.TabIndex = 9;
            this.conditionNameLabel.Text = "Unknown Condition";
            // 
            // applyButton
            // 
            this.applyButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.applyButton.Location = new System.Drawing.Point(3, 309);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(189, 23);
            this.applyButton.TabIndex = 8;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(0, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 2);
            this.label1.TabIndex = 10;
            // 
            // validationPictureBox
            // 
            this.validationPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.validationPictureBox.Location = new System.Drawing.Point(171, 3);
            this.validationPictureBox.Name = "validationPictureBox";
            this.validationPictureBox.Size = new System.Drawing.Size(20, 20);
            this.validationPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.validationPictureBox.TabIndex = 11;
            this.validationPictureBox.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Value:";
            // 
            // ConditionValueParameterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.validationPictureBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.applyButton);
            this.Controls.Add(this.conditionNameLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.conditionValueTextBox);
            this.Controls.Add(this.conditionsScintilla);
            this.Name = "ConditionValueParameterEditorForm";
            this.Size = new System.Drawing.Size(195, 335);
            ((System.ComponentModel.ISupportInitialize)(this.validationPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private CustomControls.ScintillaListBox conditionsScintilla;
        private System.Windows.Forms.TextBox conditionValueTextBox;
        private CustomControls.ConditionsSearchTextBox searchTextBox;
        private System.Windows.Forms.Label conditionNameLabel;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox validationPictureBox;
        private System.Windows.Forms.Label label2;
    }
}
