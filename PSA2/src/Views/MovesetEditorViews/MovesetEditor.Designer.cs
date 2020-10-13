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
            this.controlStripView = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.codeBlockView = new System.Windows.Forms.Panel();
            this.eventsTabControl = new System.Windows.Forms.TabControl();
            this.selectorView = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.commandsListView = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.codeBlockView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // controlStripView
            // 
            this.controlStripView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlStripView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controlStripView.Location = new System.Drawing.Point(3, 296);
            this.controlStripView.Name = "controlStripView";
            this.controlStripView.Size = new System.Drawing.Size(516, 94);
            this.controlStripView.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.codeBlockView);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.selectorView);
            this.splitContainer2.Size = new System.Drawing.Size(403, 287);
            this.splitContainer2.SplitterDistance = 277;
            this.splitContainer2.TabIndex = 0;
            // 
            // codeBlockView
            // 
            this.codeBlockView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.codeBlockView.Controls.Add(this.eventsTabControl);
            this.codeBlockView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBlockView.Location = new System.Drawing.Point(0, 0);
            this.codeBlockView.Name = "codeBlockView";
            this.codeBlockView.Size = new System.Drawing.Size(277, 287);
            this.codeBlockView.TabIndex = 1;
            // 
            // eventsTabControl
            // 
            this.eventsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsTabControl.HotTrack = true;
            this.eventsTabControl.Location = new System.Drawing.Point(0, 0);
            this.eventsTabControl.Name = "eventsTabControl";
            this.eventsTabControl.SelectedIndex = 0;
            this.eventsTabControl.Size = new System.Drawing.Size(275, 285);
            this.eventsTabControl.TabIndex = 0;
            // 
            // selectorView
            // 
            this.selectorView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectorView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectorView.Location = new System.Drawing.Point(0, 0);
            this.selectorView.Name = "selectorView";
            this.selectorView.Size = new System.Drawing.Size(122, 287);
            this.selectorView.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.commandsListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(516, 287);
            this.splitContainer1.SplitterDistance = 109;
            this.splitContainer1.TabIndex = 0;
            // 
            // commandsListView
            // 
            this.commandsListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandsListView.Location = new System.Drawing.Point(0, 0);
            this.commandsListView.Name = "commandsListView";
            this.commandsListView.Size = new System.Drawing.Size(109, 287);
            this.commandsListView.TabIndex = 0;
            // 
            // MovesetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.controlStripView);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MovesetEditor";
            this.Size = new System.Drawing.Size(522, 393);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.codeBlockView.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel controlStripView;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel codeBlockView;
        private System.Windows.Forms.Panel selectorView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel commandsListView;
        private System.Windows.Forms.TabControl eventsTabControl;
    }
}
