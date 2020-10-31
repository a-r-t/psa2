using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.Configs;
using PSA2MovesetLogic.src.Utility;
using PSA2.src.ExtentionMethods;
using ScintillaNET;
using PSA2.src.Views.MovesetEditorViews.Interfaces;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using System.Runtime.InteropServices;

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

            this.DoubleBuffered(true);
            codeBlockCommandsScintilla.DoubleBuffered(true);
        }

        public void LoadCodeBlockCommands()
        {
            codeBlockCommandsScintilla.ReadOnly = false;

            switch (SectionSelectionInfo.SectionType)
            {
                case SectionType.ACTION:
                    psaCommands = psaMovesetHandler.ActionsHandler.GetPsaCommandsInCodeBlock(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex);
                    break;
                case SectionType.SUBACTION:
                    psaCommands = psaMovesetHandler.SubActionsHandler.GetPsaCommandsForSubAction(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex);
                    break;
            }

            codeBlockCommandsScintilla.ClearAll();
            commandTexts.Clear();

            if (psaCommands.Count > 0)
            {
                foreach (PsaCommand psaCommand in psaCommands)
                {
                    string commandText = GetCommandText(psaCommand);
                    commandTexts.Add(commandText);
                }

                codeBlockCommandsScintilla.Text = string.Join("\n", commandTexts);
                codeBlockCommandsScintilla.Enabled = true;
                codeBlockCommandsScintilla.ReadOnly = true;
            }
            else
            {
                codeBlockCommandsScintilla.ClearSelections();
                codeBlockCommandsScintilla.Enabled = false;
            }

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
                        .Append(psaCommandConfig.CommandParams[i].ParamName.Replace(' ', '\u2800')) // replace space with braille space to ignore scintilla word wrap
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
            codeBlockCommandsScintilla.SetSelectionBackColor(true, Color.FromArgb(38, 79, 120));
            codeBlockCommandsScintilla.Styles[Style.Default].BackColor = Color.White;
            codeBlockCommandsScintilla.CaretForeColor = Color.Black;
            codeBlockCommandsScintilla.Styles[Style.Default].Font = "Consolas";
            codeBlockCommandsScintilla.Styles[Style.Default].SizeF = 12;
            codeBlockCommandsScintilla.StyleClearAll();

            codeBlockCommandsScintilla.Styles[1].ForeColor = Color.FromArgb(68, 156, 214);
            codeBlockCommandsScintilla.Styles[2].ForeColor = Color.Black;

            codeBlockCommandsScintilla.Styles[3].ForeColor = Color.Green;

            codeBlockCommandsScintilla.Styles[4].FillLine = true;
            codeBlockCommandsScintilla.Styles[4].ForeColor = Color.FromArgb(68, 156, 214);
            codeBlockCommandsScintilla.Styles[4].BackColor = Color.FromArgb(38, 79, 120);

            codeBlockCommandsScintilla.Styles[5].FillLine = true;
            codeBlockCommandsScintilla.Styles[5].ForeColor = Color.White;
            codeBlockCommandsScintilla.Styles[5].BackColor = Color.FromArgb(38, 79, 120);

            codeBlockCommandsScintilla.Styles[6].FillLine = true;
            codeBlockCommandsScintilla.Styles[6].ForeColor = Color.Green;
            codeBlockCommandsScintilla.Styles[6].BackColor = Color.FromArgb(38, 79, 120);

            // sets margin colors
            codeBlockCommandsScintilla.Styles[Style.LineNumber].BackColor = Color.FromArgb(240, 240, 240);
            codeBlockCommandsScintilla.Styles[Style.LineNumber].ForeColor = Color.Black;
            codeBlockCommandsScintilla.MultipleSelection = true;
            codeBlockCommandsScintilla.CaretStyle = CaretStyle.Line;

            // Turns wrap mode on
            //codeBlockCommandsScintilla.WrapStartIndent = 4;
            //codeBlockCommandsScintilla.WrapMode = WrapMode.Whitespace;
            //codeBlockCommandsScintilla.WrapIndentMode = WrapIndentMode.Indent;

            codeBlockCommandsScintilla.ReadOnly = true;

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

            UpdateSelectedCommand();

            codeBlockCommandsScintilla.Lexer = Lexer.Container;
            StyleDocument();

            //codeBlockCommandsScintilla.Margins[0].Width = 0;
            //codeBlockCommandsScintilla.Margins[1].Width = 0;

            //codeBlockCommandsScintilla.Refresh();
            /*
            if (codeBlockCommandsListBox.Items.Count > 0)
            {
                codeBlockCommandsListBox.SelectedIndex = 0;
            }
            */

        }

        private int maxLineNumberCharLength;
        private void codeBlockCommandsScintilla_TextChanged(object sender, EventArgs e)
        {
            if (psaCommands.Count > 0)
            {
                codeBlockCommandsScintilla.Margins[0].Type = MarginType.Number;

                // Did the number of characters in the line number display change?
                // i.e. nnn VS nn, or nnnn VS nn, etc...
                var maxLineNumberCharLength = codeBlockCommandsScintilla.Lines.Count.ToString().Length;
                if (maxLineNumberCharLength == this.maxLineNumberCharLength)
                {
                    return;
                }

                // Calculate the width required to display the last line number
                // and include some padding for good measure.
                const int padding = 2;
                codeBlockCommandsScintilla.Margins[0].Width = codeBlockCommandsScintilla.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
                this.maxLineNumberCharLength = maxLineNumberCharLength;

                codeBlockCommandsScintilla.Margins[1].Width = codeBlockCommandsScintilla.Margins[0].Width - (8 * (maxLineNumberCharLength + 1));
            }
            else
            {
                codeBlockCommandsScintilla.Margins[0].Width = 0;
                codeBlockCommandsScintilla.Margins[0].Type = MarginType.Text;
                codeBlockCommandsScintilla.Margins[1].Width = 0;
            }
        }

        private void codeBlockCommandsScintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // when caret changes
            if (e.Change == UpdateChange.Selection)
            {
                UpdateSelectedCommand();

                //HighlightSelectedLines();
                StyleDocument();
            }

            // if selection changes
            if ((e.Change & UpdateChange.Selection) > 0)
            {
            }

            if ((e.Change & UpdateChange.VScroll) > 0 || (e.Change & UpdateChange.HScroll) > 0)
            {
                //HighlightSelectedLines();
                StyleDocument();
            }

        }

        private void UpdateSelectedCommand()
        {
            int selectedCommandIndex = codeBlockCommandsScintilla.CurrentLine;
            if (selectedCommandIndex != SectionSelectionInfo.CommandIndex && commandTexts.Count > 0)
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

        private void StyleDocument()
        {
            List<int> selectedLines = codeBlockCommandsScintilla.GetSelectedLines();
            codeBlockCommandsScintilla.StartStyling(0);
            for (int i = 0; i < codeBlockCommandsScintilla.Lines.Count; i++)
            {
                StyleLineIndex(i, selectedLines.Contains(i));
            }

        }

        private void StyleLineIndex(int lineIndex, bool isSelected)
        {
            int commandNameStyle = !isSelected ? 1 : 4;
            int paramNameStyle = !isSelected ? 2 : 5;
            int valueStyle = !isSelected ? 3 : 6;

            string text = codeBlockCommandsScintilla.Lines[lineIndex].Text;

            if (!string.IsNullOrEmpty(text))
            {
                int lastIndexOfColon = text.LastIndexOf(':');
                if (lastIndexOfColon >= 0)
                {
                    // style everything up to the last colon
                    codeBlockCommandsScintilla.SetStyling(lastIndexOfColon, commandNameStyle);

                    codeBlockCommandsScintilla.SetStyling(1, paramNameStyle);

                    int stoppingPoint = lastIndexOfColon;
                    for (int i = 0; i < psaCommands[lineIndex].NumberOfParams; i++) {
                        for (int j = stoppingPoint + 2; j < text.Length; j++)
                        {
                            if (text[j] != '=')
                            {
                                codeBlockCommandsScintilla.SetStyling(1, paramNameStyle);
                            }
                            else
                            {
                                codeBlockCommandsScintilla.SetStyling(2, paramNameStyle);
                                stoppingPoint = j;
                                break;
                            }
                        }
                        for (int j = stoppingPoint + 1; j < text.Length; j++)
                        {
                            if (text[j] != ',')
                            {
                                codeBlockCommandsScintilla.SetStyling(1, valueStyle);
                            }
                            else
                            {
                                codeBlockCommandsScintilla.SetStyling(3, paramNameStyle);
                                stoppingPoint = j + 2;
                                break;
                            }
                        }
                    }

                }
                else
                {
                    codeBlockCommandsScintilla.SetStyling(text.Length, commandNameStyle);
                }
            }
        }

        public void InsertCommandAbove(PsaCommandConfig psaCommandConfig)
        {
            List<int> currentSelectedLines = codeBlockCommandsScintilla.GetSelectedLines();

            int currentLineIndex = currentSelectedLines.Min();
            PsaCommand psaCommand = psaCommandConfig.ToPsaCommand();
            switch (SectionSelectionInfo.SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.InsertCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, currentLineIndex, psaCommand);
                    break;
                case SectionType.SUBACTION:
                    psaMovesetHandler.SubActionsHandler.InsertCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, currentLineIndex, psaCommand);
                    break;
            }

            int firstVisibleLine = codeBlockCommandsScintilla.FirstVisibleLine;
            LoadCodeBlockCommands();

            for (int i = 0; i < currentSelectedLines.Count; i++)
            {
                if (currentSelectedLines[i] >= currentLineIndex)
                {
                    currentSelectedLines[i]++;
                }
            }

            codeBlockCommandsScintilla.LineScroll(firstVisibleLine, 0);
            codeBlockCommandsScintilla.SelectLines(currentSelectedLines);
         
            StyleDocument();
        }

        public void InsertCommandBelow(PsaCommandConfig psaCommandConfig)
        {
            List<int> currentSelectedLines = codeBlockCommandsScintilla.GetSelectedLines();

            int currentLineIndex = currentSelectedLines.Max();
            PsaCommand psaCommand = psaCommandConfig.ToPsaCommand();
            switch (SectionSelectionInfo.SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.InsertCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, currentLineIndex + 1, psaCommand);
                    break;
                case SectionType.SUBACTION:
                    psaMovesetHandler.SubActionsHandler.InsertCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, currentLineIndex + 1, psaCommand);
                    break;
            }

            int firstVisibleLine = codeBlockCommandsScintilla.FirstVisibleLine;
            LoadCodeBlockCommands();

            for (int i = 0; i < currentSelectedLines.Count; i++)
            {
                if (currentSelectedLines[i] > currentLineIndex)
                {
                    currentSelectedLines[i]++;
                }
            }
            codeBlockCommandsScintilla.LineScroll(firstVisibleLine, 0);
            codeBlockCommandsScintilla.SelectLines(currentSelectedLines);

            StyleDocument();
        }

        public void AppendCommand(PsaCommandConfig psaCommandConfig)
        {
            PsaCommand psaCommand = psaCommandConfig.ToPsaCommand();
            switch (SectionSelectionInfo.SectionType)
            {
                case SectionType.ACTION:
                    psaMovesetHandler.ActionsHandler.InsertCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, codeBlockCommandsScintilla.Lines.Count, psaCommand);
                    break;
                case SectionType.SUBACTION:
                    psaMovesetHandler.SubActionsHandler.InsertCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, codeBlockCommandsScintilla.Lines.Count, psaCommand);
                    break;
            }

            List<int> currentSelectedLines = codeBlockCommandsScintilla.GetSelectedLines();

            LoadCodeBlockCommands();
            codeBlockCommandsScintilla.LineScroll(codeBlockCommandsScintilla.Lines.Count, 0);
            codeBlockCommandsScintilla.SelectLines(currentSelectedLines);

            StyleDocument();
        }

        public void ReplaceCommand(PsaCommandConfig psaCommandConfig)
        {
            if (psaCommands.Count > 0)
            {
                List<int> currentSelectedLines = codeBlockCommandsScintilla.GetSelectedLines();

                PsaCommand psaCommand = psaCommandConfig.ToPsaCommand();

                foreach (int lineIndex in currentSelectedLines)
                {
                    switch (SectionSelectionInfo.SectionType)
                    {
                        case SectionType.ACTION:
                            psaMovesetHandler.ActionsHandler.ModifyCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, lineIndex, psaCommand);
                            break;
                        case SectionType.SUBACTION:
                            psaMovesetHandler.SubActionsHandler.ModifyCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, lineIndex, psaCommand);
                            break;
                    }
                }

                int firstVisibleLine = codeBlockCommandsScintilla.FirstVisibleLine;
                LoadCodeBlockCommands();

                codeBlockCommandsScintilla.LineScroll(firstVisibleLine, 0);
                codeBlockCommandsScintilla.SelectLines(currentSelectedLines);

                StyleDocument();
            }
        }

        public void MoveCommandUp()
        {
            if (psaCommands.Count > 0)
            {
                List<int> currentSelectedLines = codeBlockCommandsScintilla.GetSelectedLines();
                currentSelectedLines.Sort();

                for (int i = 0; i < currentSelectedLines.Count; i++)
                {
                    if (currentSelectedLines[i] - i > 0)
                    {
                        int lineIndex = currentSelectedLines[i];
                        switch (SectionSelectionInfo.SectionType)
                        {
                            case SectionType.ACTION:
                                psaMovesetHandler.ActionsHandler.MoveCommandUp(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, lineIndex);
                                break;
                            case SectionType.SUBACTION:
                                psaMovesetHandler.SubActionsHandler.MoveCommandUp(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, lineIndex);
                                break;
                        }
                        currentSelectedLines[i]--;
                    }
                }

                int firstVisibleLine = codeBlockCommandsScintilla.FirstVisibleLine;
                LoadCodeBlockCommands();

                codeBlockCommandsScintilla.LineScroll(firstVisibleLine, 0);

                codeBlockCommandsScintilla.SelectLines(currentSelectedLines);

                StyleDocument();
            }
        }

        public void MoveCommandDown()
        {
            if (psaCommands.Count > 0)
            {
                List<int> currentSelectedLines = codeBlockCommandsScintilla.GetSelectedLines();
                currentSelectedLines.Sort();

                int offset = 0;
                for (int i = currentSelectedLines.Count - 1; i >= 0; i--)
                {
                    if (currentSelectedLines[i] + offset < codeBlockCommandsScintilla.Lines.Count - 1)
                    {
                        int lineIndex = currentSelectedLines[i];
                        switch (SectionSelectionInfo.SectionType)
                        {
                            case SectionType.ACTION:
                                psaMovesetHandler.ActionsHandler.MoveCommandDown(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, lineIndex);
                                break;
                            case SectionType.SUBACTION:
                                psaMovesetHandler.SubActionsHandler.MoveCommandDown(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, lineIndex);
                                break;
                        }
                        currentSelectedLines[i]++;
                    }
                    offset++;
                }

                int firstVisibleLine = codeBlockCommandsScintilla.FirstVisibleLine;
                LoadCodeBlockCommands();

                codeBlockCommandsScintilla.LineScroll(firstVisibleLine, 0);
                codeBlockCommandsScintilla.SelectLines(currentSelectedLines);

                StyleDocument();
            }
        }

        public void RemoveCommand()
        {
            if (psaCommands.Count > 0)
            {
                List<int> currentSelectedLines = codeBlockCommandsScintilla.GetSelectedLines();

                currentSelectedLines.Sort();

                for (int i = currentSelectedLines.Count - 1; i >= 0; i--)
                {
                    int lineIndex = currentSelectedLines[i];
                    switch (SectionSelectionInfo.SectionType)
                    {
                        case SectionType.ACTION:
                            psaMovesetHandler.ActionsHandler.RemoveCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, lineIndex);
                            break;
                        case SectionType.SUBACTION:
                            psaMovesetHandler.SubActionsHandler.RemoveCommand(SectionSelectionInfo.SectionIndex, SectionSelectionInfo.CodeBlockIndex, lineIndex);
                            break;
                    }
                }

                int firstVisibleLine = codeBlockCommandsScintilla.FirstVisibleLine;
                LoadCodeBlockCommands();

                codeBlockCommandsScintilla.LineScroll(firstVisibleLine, 0);

                StyleDocument();
            }
        }

        private void codeBlockCommandsScintilla_MouseMove(object sender, MouseEventArgs e)
        {
            //codeBlockCommandsScintilla.UpdateCursor();
        }

        private void codeBlockCommandsScintilla_MouseCaptureChanged(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.Arrow;

        }

        private void codeBlockCommandsScintilla_MouseDown(object sender, MouseEventArgs e)
        {
            //codeBlockCommandsScintilla.Enabled = true;

        }

        private void codeBlockCommandsScintilla_MouseUp(object sender, MouseEventArgs e)
        {
            //codeBlockCommandsScintilla.Enabled = false;

        }

        private void CodeBlockViewer_MouseDown(object sender, MouseEventArgs e)
        {
            //codeBlockCommandsScintilla.Enabled = true;

        }

    }
}
