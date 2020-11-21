using PSA2.src.Views.CustomControls;
using PSA2.src.Views.MovesetEditorViews.Interfaces;

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
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.codeBlockView = new System.Windows.Forms.Panel();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.eventsTabControl = new TabControlCustom();
            this.optionsTabControl = new System.Windows.Forms.TabControl();
            this.actionTabPage = new System.Windows.Forms.TabPage();
            this.commandDescriptionTabPage = new System.Windows.Forms.TabPage();
            this.variablesTabPage = new System.Windows.Forms.TabPage();
            this.errorListTabPage = new System.Windows.Forms.TabPage();
            this.hexCalculatorTabPage = new System.Windows.Forms.TabPage();
            this.selectorView = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.commandOptionsViewer = new System.Windows.Forms.Panel();
            this.parametersEditorViewer = new System.Windows.Forms.Panel();
            this.commandOptionsTabControl = new System.Windows.Forms.TabControl();
            this.commandsTabPage = new System.Windows.Forms.TabPage();
            this.parametersTabPage = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.codeBlockView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.optionsTabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.commandOptionsTabControl.SuspendLayout();
            this.commandsTabPage.SuspendLayout();
            this.parametersTabPage.SuspendLayout();
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
            this.splitContainer2.Size = new System.Drawing.Size(358, 408);
            this.splitContainer2.SplitterDistance = 167;
            this.splitContainer2.TabIndex = 0;
            // 
            // codeBlockView
            // 
            this.codeBlockView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.codeBlockView.Controls.Add(this.splitContainer4);
            this.codeBlockView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeBlockView.Location = new System.Drawing.Point(0, 0);
            this.codeBlockView.Name = "codeBlockView";
            this.codeBlockView.Size = new System.Drawing.Size(167, 408);
            this.codeBlockView.TabIndex = 1;
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            this.splitContainer4.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.eventsTabControl);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.optionsTabControl);
            this.splitContainer4.Size = new System.Drawing.Size(165, 406);
            this.splitContainer4.SplitterDistance = 260;
            this.splitContainer4.TabIndex = 1;
            // 
            // eventsTabControl
            // 
            this.eventsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsTabControl.Location = new System.Drawing.Point(0, 0);
            this.eventsTabControl.Name = "eventsTabControl";
            this.eventsTabControl.Size = new System.Drawing.Size(165, 260);
            this.eventsTabControl.TabIndex = 0;
            this.eventsTabControl.SelectedIndexChanged += new System.EventHandler(this.eventsTabControl_SelectedIndexChanged);
            this.eventsTabControl.Click += new System.EventHandler(this.eventsTabControl_Click);
            // 
            // optionsTabControl
            // 
            this.optionsTabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.optionsTabControl.Controls.Add(this.actionTabPage);
            this.optionsTabControl.Controls.Add(this.commandDescriptionTabPage);
            this.optionsTabControl.Controls.Add(this.variablesTabPage);
            this.optionsTabControl.Controls.Add(this.errorListTabPage);
            this.optionsTabControl.Controls.Add(this.hexCalculatorTabPage);
            this.optionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.optionsTabControl.Location = new System.Drawing.Point(0, 0);
            this.optionsTabControl.Name = "optionsTabControl";
            this.optionsTabControl.SelectedIndex = 0;
            this.optionsTabControl.Size = new System.Drawing.Size(165, 142);
            this.optionsTabControl.TabIndex = 1;
            // 
            // actionTabPage
            // 
            this.actionTabPage.Location = new System.Drawing.Point(4, 4);
            this.actionTabPage.Name = "actionTabPage";
            this.actionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.actionTabPage.Size = new System.Drawing.Size(157, 116);
            this.actionTabPage.TabIndex = 0;
            this.actionTabPage.Text = "Actions";
            this.actionTabPage.UseVisualStyleBackColor = true;
            // 
            // commandDescriptionTabPage
            // 
            this.commandDescriptionTabPage.Location = new System.Drawing.Point(4, 4);
            this.commandDescriptionTabPage.Name = "commandDescriptionTabPage";
            this.commandDescriptionTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.commandDescriptionTabPage.Size = new System.Drawing.Size(157, 116);
            this.commandDescriptionTabPage.TabIndex = 1;
            this.commandDescriptionTabPage.Text = "Descriptions";
            this.commandDescriptionTabPage.UseVisualStyleBackColor = true;
            // 
            // variablesTabPage
            // 
            this.variablesTabPage.Location = new System.Drawing.Point(4, 4);
            this.variablesTabPage.Name = "variablesTabPage";
            this.variablesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.variablesTabPage.Size = new System.Drawing.Size(157, 116);
            this.variablesTabPage.TabIndex = 4;
            this.variablesTabPage.Text = "Variables";
            this.variablesTabPage.UseVisualStyleBackColor = true;
            // 
            // errorListTabPage
            // 
            this.errorListTabPage.Location = new System.Drawing.Point(4, 4);
            this.errorListTabPage.Name = "errorListTabPage";
            this.errorListTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.errorListTabPage.Size = new System.Drawing.Size(157, 116);
            this.errorListTabPage.TabIndex = 2;
            this.errorListTabPage.Text = "Errors";
            this.errorListTabPage.UseVisualStyleBackColor = true;
            // 
            // hexCalculatorTabPage
            // 
            this.hexCalculatorTabPage.Location = new System.Drawing.Point(4, 4);
            this.hexCalculatorTabPage.Name = "hexCalculatorTabPage";
            this.hexCalculatorTabPage.Size = new System.Drawing.Size(157, 116);
            this.hexCalculatorTabPage.TabIndex = 3;
            this.hexCalculatorTabPage.Text = "Hex Calculator";
            this.hexCalculatorTabPage.UseVisualStyleBackColor = true;
            // 
            // selectorView
            // 
            this.selectorView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.selectorView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectorView.Location = new System.Drawing.Point(0, 0);
            this.selectorView.Name = "selectorView";
            this.selectorView.Size = new System.Drawing.Size(187, 408);
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
            this.splitContainer1.Panel1.Controls.Add(this.commandOptionsTabControl);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(548, 408);
            this.splitContainer1.SplitterDistance = 186;
            this.splitContainer1.TabIndex = 0;
            // 
            // commandOptionsViewer
            // 
            this.commandOptionsViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.commandOptionsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandOptionsViewer.Location = new System.Drawing.Point(3, 3);
            this.commandOptionsViewer.Name = "commandOptionsViewer";
            this.commandOptionsViewer.Size = new System.Drawing.Size(186, 68);
            this.commandOptionsViewer.TabIndex = 0;
            // 
            // parametersEditorViewer
            // 
            this.parametersEditorViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.parametersEditorViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersEditorViewer.Location = new System.Drawing.Point(3, 3);
            this.parametersEditorViewer.Name = "parametersEditorViewer";
            this.parametersEditorViewer.Size = new System.Drawing.Size(172, 376);
            this.parametersEditorViewer.TabIndex = 0;
            // 
            // commandOptionsTabControl
            // 
            this.commandOptionsTabControl.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.commandOptionsTabControl.Controls.Add(this.commandsTabPage);
            this.commandOptionsTabControl.Controls.Add(this.parametersTabPage);
            this.commandOptionsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.commandOptionsTabControl.Location = new System.Drawing.Point(0, 0);
            this.commandOptionsTabControl.Name = "commandOptionsTabControl";
            this.commandOptionsTabControl.SelectedIndex = 0;
            this.commandOptionsTabControl.Size = new System.Drawing.Size(186, 408);
            this.commandOptionsTabControl.TabIndex = 0;
            // 
            // commandsTabPage
            // 
            this.commandsTabPage.Controls.Add(this.commandOptionsViewer);
            this.commandsTabPage.Location = new System.Drawing.Point(4, 4);
            this.commandsTabPage.Name = "commandsTabPage";
            this.commandsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.commandsTabPage.Size = new System.Drawing.Size(192, 74);
            this.commandsTabPage.TabIndex = 0;
            this.commandsTabPage.Text = "Commands";
            this.commandsTabPage.UseVisualStyleBackColor = true;
            // 
            // parametersTabPage
            // 
            this.parametersTabPage.Controls.Add(this.parametersEditorViewer);
            this.parametersTabPage.Location = new System.Drawing.Point(4, 4);
            this.parametersTabPage.Name = "parametersTabPage";
            this.parametersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.parametersTabPage.Size = new System.Drawing.Size(178, 382);
            this.parametersTabPage.TabIndex = 1;
            this.parametersTabPage.Text = "Parameters";
            this.parametersTabPage.UseVisualStyleBackColor = true;
            // 
            // MovesetEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "MovesetEditor";
            this.Size = new System.Drawing.Size(548, 408);
            this.Load += new System.EventHandler(this.MovesetEditor_Load);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.codeBlockView.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.optionsTabControl.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.commandOptionsTabControl.ResumeLayout(false);
            this.commandsTabPage.ResumeLayout(false);
            this.parametersTabPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel codeBlockView;
        private System.Windows.Forms.Panel selectorView;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private TabControlCustom eventsTabControl;
        private System.Windows.Forms.Panel commandOptionsViewer;
        private System.Windows.Forms.Panel parametersEditorViewer;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.TabControl optionsTabControl;
        private System.Windows.Forms.TabPage actionTabPage;
        private System.Windows.Forms.TabPage commandDescriptionTabPage;
        private System.Windows.Forms.TabPage errorListTabPage;
        private System.Windows.Forms.TabPage hexCalculatorTabPage;
        private System.Windows.Forms.TabPage variablesTabPage;
        private System.Windows.Forms.TabControl commandOptionsTabControl;
        private System.Windows.Forms.TabPage commandsTabPage;
        private System.Windows.Forms.TabPage parametersTabPage;
    }
}
