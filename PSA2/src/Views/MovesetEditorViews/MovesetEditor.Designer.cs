﻿namespace PSA2.src.Views.MovesetEditorViews
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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.codeBlockView = new System.Windows.Forms.Panel();
            this.eventsTabControl = new System.Windows.Forms.TabControl();
            this.selectorView = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.commandOptionsViewer = new System.Windows.Forms.Panel();
            this.parametersEditorViewer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.codeBlockView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
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
            this.splitContainer2.Size = new System.Drawing.Size(409, 393);
            this.splitContainer2.SplitterDistance = 283;
            this.splitContainer2.TabIndex = 0;
            // 
            // codeBlockView
            // 
            this.codeBlockView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.codeBlockView.Controls.Add(this.eventsTabControl);
            this.codeBlockView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBlockView.Location = new System.Drawing.Point(0, 0);
            this.codeBlockView.Name = "codeBlockView";
            this.codeBlockView.Size = new System.Drawing.Size(283, 393);
            this.codeBlockView.TabIndex = 1;
            // 
            // eventsTabControl
            // 
            this.eventsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsTabControl.HotTrack = true;
            this.eventsTabControl.Location = new System.Drawing.Point(0, 0);
            this.eventsTabControl.Name = "eventsTabControl";
            this.eventsTabControl.SelectedIndex = 0;
            this.eventsTabControl.Size = new System.Drawing.Size(281, 391);
            this.eventsTabControl.TabIndex = 0;
            this.eventsTabControl.SelectedIndexChanged += new System.EventHandler(this.eventsTabControl_SelectedIndexChanged);
            this.eventsTabControl.Click += new System.EventHandler(this.eventsTabControl_Click);
            // 
            // selectorView
            // 
            this.selectorView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectorView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectorView.Location = new System.Drawing.Point(0, 0);
            this.selectorView.Name = "selectorView";
            this.selectorView.Size = new System.Drawing.Size(122, 393);
            this.selectorView.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(522, 393);
            this.splitContainer1.SplitterDistance = 109;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.commandOptionsViewer);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.parametersEditorViewer);
            this.splitContainer3.Size = new System.Drawing.Size(109, 393);
            this.splitContainer3.SplitterDistance = 194;
            this.splitContainer3.TabIndex = 0;
            // 
            // commandOptionsViewer
            // 
            this.commandOptionsViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandOptionsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandOptionsViewer.Location = new System.Drawing.Point(0, 0);
            this.commandOptionsViewer.Name = "commandOptionsViewer";
            this.commandOptionsViewer.Size = new System.Drawing.Size(109, 194);
            this.commandOptionsViewer.TabIndex = 0;
            // 
            // parametersEditorViewer
            // 
            this.parametersEditorViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.parametersEditorViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersEditorViewer.Location = new System.Drawing.Point(0, 0);
            this.parametersEditorViewer.Name = "parametersEditorViewer";
            this.parametersEditorViewer.Size = new System.Drawing.Size(109, 195);
            this.parametersEditorViewer.TabIndex = 0;
            // 
            // MovesetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel codeBlockView;
        private System.Windows.Forms.Panel selectorView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl eventsTabControl;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.Panel commandOptionsViewer;
        private System.Windows.Forms.Panel parametersEditorViewer;
    }
}
