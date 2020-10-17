using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSA2.src.FileProcessor.MovesetHandler;
using PSA2.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.Utility;
using PSA2.src.ExtentionMethods;
using ScintillaNET;
using PSA2.src.Views.MovesetEditorViews.Interfaces;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class CodeBlockViewer : ObservableUserControl<ICodeBlockViewerListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        public SectionSelectionInfo SectionSelectionInfo { get; private set; }
        protected PsaCommandsConfig psaCommandsConfig;
        private List<PsaCommand> psaCommands = new List<PsaCommand>();
        private List<string> commandTexts = new List<string>();

        public CodeBlockViewer(PsaMovesetHandler psaMovesetHandler, PsaCommandsConfig psaCommandsConfig, SectionSelectionInfo sectionSelectionInfo)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = psaCommandsConfig;
            SectionSelectionInfo = sectionSelectionInfo;
            InitializeComponent();
        }

        public void LoadCodeBlockCommands()
        {
            
            codeBlockCommandsScintilla.ClearAll();

            switch (SectionSelectionInfo.SectionType)
            {
                case SectionType.ACTION:
                    psaCommands = psaMovesetHandler.ActionsHandler.GetPsaCommandsInCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex);
                    break;
                case SectionType.SUBACTION:
                    psaCommands = psaMovesetHandler.SubActionsHandler.GetPsaCommandsForSubAction(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex);
                    break;
            }

            commandTexts.Clear();
            foreach (PsaCommand psaCommand in psaCommands) {
                string commandText = GetCommandText(psaCommand);
                //codeBlockCommandsListBox.Items.Add(commandText);
                commandTexts.Add(commandText);
            }

            codeBlockCommandsScintilla.Text = string.Join("\n", commandTexts);

            //codeBlockCommandsListBox.DataSource = commandTexts;


        }

        public string GetCommandText(PsaCommand psaCommand)
        {
            PsaCommandConfig psaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);

            StringBuilder commandTextBuilder = new StringBuilder();

            if (psaCommandConfig != null)
            {
                commandTextBuilder
                    .Append(psaCommandConfig.CommandName);

                if (psaCommand.Parameters.Count > 0)
                {
                    commandTextBuilder.Append(": ");
                }

                for (int i = 0; i < psaCommand.Parameters.Count; i++)
                {
                    commandTextBuilder
                        .Append(psaCommandConfig.CommandParams[i].ParamName)
                        .Append("=")
                        .Append(psaCommand.Parameters[i].Value);

                    if (i < psaCommand.Parameters.Count - 1)
                    {
                        commandTextBuilder.Append(", ");
                    }
                }
            }
            else
            {
                commandTextBuilder
                    .Append(psaCommand.Instruction);

                if (psaCommand.Parameters.Count > 0)
                {
                    commandTextBuilder.Append(": ");
                }

                for (int i = 0; i < psaCommand.Parameters.Count; i++)
                {
                    commandTextBuilder
                        .Append($"arg{i}=")
                        .Append(psaCommand.Parameters[i].Value);

                    if (i < psaCommand.Parameters.Count - 1)
                    {
                        commandTextBuilder.Append(", ");
                    }

                }
            }
            return commandTextBuilder.ToString();
        }

        private void CodeBlockViewer_Load(object sender, EventArgs e)
        {
            
            this.DoubleBuffered(true);

            codeBlockCommandsScintilla.SetSelectionBackColor(true, Color.FromArgb(38, 79, 120));
            codeBlockCommandsScintilla.Styles[Style.Default].BackColor = Color.White;
            codeBlockCommandsScintilla.CaretForeColor = Color.Black;
            codeBlockCommandsScintilla.Styles[Style.Default].Font = "Consolas";
            codeBlockCommandsScintilla.Styles[Style.Default].SizeF = 12;
            codeBlockCommandsScintilla.StyleClearAll();

            codeBlockCommandsScintilla.Styles[1].ForeColor = Color.FromArgb(68, 156, 214);
            codeBlockCommandsScintilla.Styles[2].ForeColor = Color.Black;
            codeBlockCommandsScintilla.Styles[3].ForeColor = Color.Green;
            codeBlockCommandsScintilla.Styles[4].ForeColor = Color.White;

            /*
             For dark mode:
                codeBlockCommandsScintilla.Styles[Style.Default].BackColor = Color.FromArgb(30, 30, 30);
                codeBlockCommandsScintilla.CaretForeColor = Color.White;
                codeBlockCommandsScintilla.Styles[1].ForeColor = Color.FromArgb(68, 156, 214);
                codeBlockCommandsScintilla.Styles[2].ForeColor = Color.White;
                codeBlockCommandsScintilla.Styles[3].ForeColor = Color.FromArgb(220, 210, 127);
                codeBlockCommandsScintilla.Styles[4].ForeColor = Color.Black;

             */
            //codeBlockCommandsListBox.DoubleBuffered(true);

            LoadCodeBlockCommands();
            codeBlockCommandsScintilla.Lexer = Lexer.Container;
            //codeBlockCommandsScintilla.Refresh();
            /*
            if (codeBlockCommandsListBox.Items.Count > 0)
            {
                codeBlockCommandsListBox.SelectedIndex = 0;
            }
            */

        }

        private void codeBlockCommandsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*
            if (codeBlockCommandsListBox.SelectedIndex != SectionSelectionInfo.CommandIndex)
            {
                PsaCommand psaCommand;
                switch (SectionSelectionInfo.SectionType)
                {
                    case SectionType.ACTION:
                        psaCommand = psaMovesetHandler.ActionsHandler.GetPsaCommandInCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, codeBlockCommandsListBox.SelectedIndex);
                        break;
                    case SectionType.SUBACTION:
                        psaCommand = psaMovesetHandler.SubActionsHandler.GetPsaCommandForSubActionCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, codeBlockCommandsListBox.SelectedIndex);
                        break;
                    default:
                        throw new ArgumentException("Invalid section type");
                }

                PsaCommandConfig psaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);
                SectionSelectionInfo.CommandIndex = codeBlockCommandsListBox.SelectedIndex;
                foreach (ICodeBlockViewerListener listener in listeners)
                {
                    listener.OnCommandSelected(psaCommandConfig, psaCommand, SectionSelectionInfo);
                }
            }
            */
        }

        private void codeBlockCommandsListBox_Leave(object sender, EventArgs e)
        {
            //codeBlockCommandsListBox.Update();
        }

        private void codeBlockCommandsListBox_Enter(object sender, EventArgs e)
        {

        }

        private void codeBlockCommandsListBox_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private int maxLineNumberCharLength;
        private void codeBlockCommandsScintilla_TextChanged(object sender, EventArgs e)
        {
            //Console.WriteLine("HI");
            //StyleVisibleLines();

            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...
            var maxLineNumberCharLength = codeBlockCommandsScintilla.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                return;

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            codeBlockCommandsScintilla.Margins[0].Width = codeBlockCommandsScintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            this.maxLineNumberCharLength = maxLineNumberCharLength;
        }

        private void codeBlockCommandsScintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // when caret changes
            if (e.Change == UpdateChange.Selection)
            {
                if (!codeBlockCommandsScintilla.SelectedText.Contains("\n")) {
                    int selectedCommandIndex = codeBlockCommandsScintilla.CurrentLine;
                    if (selectedCommandIndex != SectionSelectionInfo.CommandIndex)
                    {
                        PsaCommand psaCommand;
                        switch (SectionSelectionInfo.SectionType)
                        {
                            case SectionType.ACTION:
                                psaCommand = psaMovesetHandler.ActionsHandler.GetPsaCommandInCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, selectedCommandIndex);
                                break;
                            case SectionType.SUBACTION:
                                psaCommand = psaMovesetHandler.SubActionsHandler.GetPsaCommandForSubActionCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, selectedCommandIndex);
                                break;
                            default:
                                throw new ArgumentException("Invalid section type");
                        }

                        PsaCommandConfig psaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);
                        SectionSelectionInfo.CommandIndex = selectedCommandIndex;
                        foreach (ICodeBlockViewerListener listener in listeners)
                        {
                            listener.OnCommandSelected(psaCommandConfig, psaCommand, SectionSelectionInfo);
                        }
                        codeBlockCommandsScintilla.Focus();
                    }
                }
                else
                {
                    // multiple commands selected
                    //codeBlockCommandsScintilla.SelectionStart;
                    //codeBlockCommandsScintilla.SelectionEnd;
                }
            } 
        }

        private void codeBlockCommandsScintilla_StyleNeeded(object sender, StyleNeededEventArgs e)
        {
            
            var startPos = codeBlockCommandsScintilla.GetEndStyled();
            var endPos = e.Position;

            int startLineIndex = codeBlockCommandsScintilla.LineFromPosition(startPos);
            int endLineIndex = codeBlockCommandsScintilla.LineFromPosition(endPos);

            if (endLineIndex >= codeBlockCommandsScintilla.Lines.Count)
            {
                endLineIndex = codeBlockCommandsScintilla.Lines.Count - 1;
            }

            string text = codeBlockCommandsScintilla.Text;
            while (startPos != 0 && text[startPos - 1] != '\n')
            {
                startPos--;
            }

            codeBlockCommandsScintilla.StartStyling(startPos);

            for (int i = startLineIndex; i <= endLineIndex; i++)
            {
                StyleLineIndex(i);
            }
            
        }

/*        private void StyleVisibleLines()
        {
            int startLineIndex = codeBlockCommandsScintilla.FirstVisibleLine;
            int endLineIndex = startLineIndex + codeBlockCommandsScintilla.LinesOnScreen;

            if (endLineIndex >= codeBlockCommandsScintilla.Lines.Count)
            {
                endLineIndex = codeBlockCommandsScintilla.Lines.Count - 1;
            }

            codeBlockCommandsScintilla.StartStyling(codeBlockCommandsScintilla.GetEndStyled());
            Console.WriteLine("START: " + startLineIndex + ", END: " + endLineIndex);
            for (int i = startLineIndex; i < endLineIndex; i++)
            {
                StyleLineIndex(i);
            }
        }*/

        private void StyleLineIndex(int lineIndex)
        {
            string text = codeBlockCommandsScintilla.Lines[lineIndex].Text;

            if (!string.IsNullOrEmpty(text))
            {
                int lastIndexOfColon = text.LastIndexOf(':');
                if (lastIndexOfColon >= 0)
                {
                    // style everything up to the last colon
                    codeBlockCommandsScintilla.SetStyling(lastIndexOfColon, 1);

                    codeBlockCommandsScintilla.SetStyling(1, 2);

                    int stoppingPoint = lastIndexOfColon;
                    for (int i = 0; i < psaCommands[lineIndex].NumberOfParams; i++) {
                        for (int j = stoppingPoint + 2; j < text.Length; j++)
                        {
                            if (text[j] != '=')
                            {
                                codeBlockCommandsScintilla.SetStyling(1, 2);
                            }
                            else
                            {
                                codeBlockCommandsScintilla.SetStyling(2, 2);
                                stoppingPoint = j;
                                break;
                            }
                        }
                        for (int j = stoppingPoint + 1; j < text.Length; j++)
                        {
                            if (text[j] != ',')
                            {
                                codeBlockCommandsScintilla.SetStyling(1, 3);
                            }
                            else
                            {
                                codeBlockCommandsScintilla.SetStyling(3, 2);
                                stoppingPoint = j + 2;
                                break;
                            }
                        }
                    }

                }
                else
                {
                    codeBlockCommandsScintilla.SetStyling(text.Length, 1);
                }
            }
        }

        public void AddCommandAbove(PsaCommandConfig psaCommandConfig)
        {
            int currentLineIndex = codeBlockCommandsScintilla.CurrentLine;
            PsaCommand psaCommand = psaCommandConfig.ToPsaCommand();
            switch (SectionSelectionInfo.SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.InsertCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, currentLineIndex, psaCommand);
                    break;
                case SectionType.SUBACTION:
                    psaCommands = psaMovesetHandler.SubActionsHandler.GetPsaCommandsForSubAction(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex);
                    break;
            }
            string commandText = GetCommandText(psaCommand);
            //commandTexts.Insert(currentLineIndex, commandText);
            LoadCodeBlockCommands();
        }

        public void AddCommandBelow(PsaCommandConfig psaCommandConfig)
        {
            throw new NotImplementedException();
        }

        public void ReplaceCommand(PsaCommandConfig psaCommandConfig)
        {
            throw new NotImplementedException();
        }

        public void MoveCommandUp()
        {
            throw new NotImplementedException();
        }

        public void MoveCommandDown()
        {
            throw new NotImplementedException();
        }

        public void RemoveCommand()
        {
            throw new NotImplementedException();
        }
    }
}
