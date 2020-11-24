namespace PSA2.src.Views.MovesetEditorViews
{
    partial class SubActionSelector
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.subActionsListScintilla = new PSA2.src.Views.CustomControls.ScintillaListBox();
            this.animationDetailsViewer = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.unknown7CheckBox = new System.Windows.Forms.CheckBox();
            this.transitionOutFromStartCheckBox = new System.Windows.Forms.CheckBox();
            this.unknown5CheckBox = new System.Windows.Forms.CheckBox();
            this.unknown4CheckBox = new System.Windows.Forms.CheckBox();
            this.unknown3CheckBox = new System.Windows.Forms.CheckBox();
            this.movesCharacterCheckBox = new System.Windows.Forms.CheckBox();
            this.loopCheckBox = new System.Windows.Forms.CheckBox();
            this.noOutTransitionCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.inTransitionTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.animationNameTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.animationDetailsViewer.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.searchTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.subActionsListScintilla);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.animationDetailsViewer);
            this.splitContainer1.Size = new System.Drawing.Size(200, 453);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 6;
            // 
            // searchTextBox
            // 
            this.searchTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchTextBox.Location = new System.Drawing.Point(6, 5);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(191, 20);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.Text = "🔍";
            this.searchTextBox.TextChanged += new System.EventHandler(this.searchTextBox_TextChanged);
            this.searchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyDown);
            // 
            // subActionsListScintilla
            // 
            this.subActionsListScintilla.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.subActionsListScintilla.BackgroundColor = System.Drawing.Color.White;
            this.subActionsListScintilla.CaretStyle = ScintillaNET.CaretStyle.Invisible;
            this.subActionsListScintilla.CurrentCursor = System.Windows.Forms.Cursors.Arrow;
            this.subActionsListScintilla.FontFamily = "Consolas";
            this.subActionsListScintilla.FontSize = 10F;
            this.subActionsListScintilla.FullLineSelect = false;
            this.subActionsListScintilla.ItemBackColor = System.Drawing.Color.White;
            this.subActionsListScintilla.ItemForeColor = System.Drawing.Color.Black;
            this.subActionsListScintilla.Location = new System.Drawing.Point(4, 31);
            this.subActionsListScintilla.Name = "subActionsListScintilla";
            this.subActionsListScintilla.ReadOnly = true;
            this.subActionsListScintilla.SelectedIndex = -1;
            this.subActionsListScintilla.SelectedItemBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(79)))), ((int)(((byte)(120)))));
            this.subActionsListScintilla.SelectedItemForeColor = System.Drawing.Color.White;
            this.subActionsListScintilla.ShowLineNumbers = false;
            this.subActionsListScintilla.Size = new System.Drawing.Size(193, 266);
            this.subActionsListScintilla.TabIndex = 0;
            this.subActionsListScintilla.SelectedIndexChanged += new System.EventHandler(this.subActionsListScintilla_SelectedIndexChanged);
            // 
            // animationDetailsViewer
            // 
            this.animationDetailsViewer.Controls.Add(this.groupBox1);
            this.animationDetailsViewer.Controls.Add(this.label1);
            this.animationDetailsViewer.Controls.Add(this.animationNameTextBox);
            this.animationDetailsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.animationDetailsViewer.Location = new System.Drawing.Point(0, 0);
            this.animationDetailsViewer.Name = "animationDetailsViewer";
            this.animationDetailsViewer.Size = new System.Drawing.Size(200, 149);
            this.animationDetailsViewer.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(6, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(191, 113);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Animation Flags";
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.unknown7CheckBox);
            this.panel1.Controls.Add(this.transitionOutFromStartCheckBox);
            this.panel1.Controls.Add(this.unknown5CheckBox);
            this.panel1.Controls.Add(this.unknown4CheckBox);
            this.panel1.Controls.Add(this.unknown3CheckBox);
            this.panel1.Controls.Add(this.movesCharacterCheckBox);
            this.panel1.Controls.Add(this.loopCheckBox);
            this.panel1.Controls.Add(this.noOutTransitionCheckBox);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.inTransitionTextBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(185, 94);
            this.panel1.TabIndex = 10;
            // 
            // unknown7CheckBox
            // 
            this.unknown7CheckBox.AutoSize = true;
            this.unknown7CheckBox.Location = new System.Drawing.Point(9, 190);
            this.unknown7CheckBox.Name = "unknown7CheckBox";
            this.unknown7CheckBox.Size = new System.Drawing.Size(78, 17);
            this.unknown7CheckBox.TabIndex = 9;
            this.unknown7CheckBox.Text = "Unknown7";
            this.unknown7CheckBox.UseVisualStyleBackColor = true;
            // 
            // transitionOutFromStartCheckBox
            // 
            this.transitionOutFromStartCheckBox.AutoSize = true;
            this.transitionOutFromStartCheckBox.Location = new System.Drawing.Point(9, 167);
            this.transitionOutFromStartCheckBox.Name = "transitionOutFromStartCheckBox";
            this.transitionOutFromStartCheckBox.Size = new System.Drawing.Size(143, 17);
            this.transitionOutFromStartCheckBox.TabIndex = 8;
            this.transitionOutFromStartCheckBox.Text = "Transition Out From Start";
            this.transitionOutFromStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // unknown5CheckBox
            // 
            this.unknown5CheckBox.AutoSize = true;
            this.unknown5CheckBox.Location = new System.Drawing.Point(9, 144);
            this.unknown5CheckBox.Name = "unknown5CheckBox";
            this.unknown5CheckBox.Size = new System.Drawing.Size(78, 17);
            this.unknown5CheckBox.TabIndex = 7;
            this.unknown5CheckBox.Text = "Unknown5";
            this.unknown5CheckBox.UseVisualStyleBackColor = true;
            // 
            // unknown4CheckBox
            // 
            this.unknown4CheckBox.AutoSize = true;
            this.unknown4CheckBox.Location = new System.Drawing.Point(9, 121);
            this.unknown4CheckBox.Name = "unknown4CheckBox";
            this.unknown4CheckBox.Size = new System.Drawing.Size(78, 17);
            this.unknown4CheckBox.TabIndex = 6;
            this.unknown4CheckBox.Text = "Unknown4";
            this.unknown4CheckBox.UseVisualStyleBackColor = true;
            // 
            // unknown3CheckBox
            // 
            this.unknown3CheckBox.AutoSize = true;
            this.unknown3CheckBox.Location = new System.Drawing.Point(9, 98);
            this.unknown3CheckBox.Name = "unknown3CheckBox";
            this.unknown3CheckBox.Size = new System.Drawing.Size(78, 17);
            this.unknown3CheckBox.TabIndex = 5;
            this.unknown3CheckBox.Text = "Unknown3";
            this.unknown3CheckBox.UseVisualStyleBackColor = true;
            // 
            // movesCharacterCheckBox
            // 
            this.movesCharacterCheckBox.AutoSize = true;
            this.movesCharacterCheckBox.Location = new System.Drawing.Point(9, 75);
            this.movesCharacterCheckBox.Name = "movesCharacterCheckBox";
            this.movesCharacterCheckBox.Size = new System.Drawing.Size(107, 17);
            this.movesCharacterCheckBox.TabIndex = 4;
            this.movesCharacterCheckBox.Text = "Moves Character";
            this.movesCharacterCheckBox.UseVisualStyleBackColor = true;
            // 
            // loopCheckBox
            // 
            this.loopCheckBox.AutoSize = true;
            this.loopCheckBox.Location = new System.Drawing.Point(9, 52);
            this.loopCheckBox.Name = "loopCheckBox";
            this.loopCheckBox.Size = new System.Drawing.Size(50, 17);
            this.loopCheckBox.TabIndex = 3;
            this.loopCheckBox.Text = "Loop";
            this.loopCheckBox.UseVisualStyleBackColor = true;
            // 
            // noOutTransitionCheckBox
            // 
            this.noOutTransitionCheckBox.AutoSize = true;
            this.noOutTransitionCheckBox.Location = new System.Drawing.Point(9, 29);
            this.noOutTransitionCheckBox.Name = "noOutTransitionCheckBox";
            this.noOutTransitionCheckBox.Size = new System.Drawing.Size(109, 17);
            this.noOutTransitionCheckBox.TabIndex = 2;
            this.noOutTransitionCheckBox.Text = "No Out Transition";
            this.noOutTransitionCheckBox.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "In Transition:";
            // 
            // inTransitionTextBox
            // 
            this.inTransitionTextBox.Location = new System.Drawing.Point(74, 3);
            this.inTransitionTextBox.Name = "inTransitionTextBox";
            this.inTransitionTextBox.Size = new System.Drawing.Size(38, 20);
            this.inTransitionTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Animation Name:";
            // 
            // animationNameTextBox
            // 
            this.animationNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.animationNameTextBox.Location = new System.Drawing.Point(90, 7);
            this.animationNameTextBox.Name = "animationNameTextBox";
            this.animationNameTextBox.Size = new System.Drawing.Size(104, 20);
            this.animationNameTextBox.TabIndex = 0;
            // 
            // SubActionSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "SubActionSelector";
            this.Size = new System.Drawing.Size(200, 453);
            this.Load += new System.EventHandler(this.SubActionSelector_Load);
            this.VisibleChanged += new System.EventHandler(this.SubActionSelector_VisibleChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.animationDetailsViewer.ResumeLayout(false);
            this.animationDetailsViewer.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private CustomControls.ScintillaListBox subActionsListScintilla;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Panel animationDetailsViewer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox animationNameTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox inTransitionTextBox;
        private System.Windows.Forms.CheckBox loopCheckBox;
        private System.Windows.Forms.CheckBox noOutTransitionCheckBox;
        private System.Windows.Forms.CheckBox movesCharacterCheckBox;
        private System.Windows.Forms.CheckBox unknown5CheckBox;
        private System.Windows.Forms.CheckBox unknown4CheckBox;
        private System.Windows.Forms.CheckBox unknown3CheckBox;
        private System.Windows.Forms.CheckBox unknown7CheckBox;
        private System.Windows.Forms.CheckBox transitionOutFromStartCheckBox;
        private System.Windows.Forms.Panel panel1;
    }
}
