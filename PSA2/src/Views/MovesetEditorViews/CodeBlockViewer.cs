using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.Configs;
using PSA2.src.ExtentionMethods;
using ScintillaNET;
using PSA2.src.Views.MovesetEditorViews.Interfaces;

namespace PSA2.src.Views.MovesetEditorViews
{
    public partial class CodeBlockViewer : ObservableUserControl<ICodeBlockViewerListener>
    {
        protected PsaMovesetHandler psaMovesetHandler;
        public CodeBlockSelection CodeBlockSelection { get; private set; }
        protected PsaCommandsConfig psaCommandsConfig;
        private List<PsaCommand> psaCommands = new List<PsaCommand>();
        private List<string> commandTexts = new List<string>();
        private List<int> currentCommandIndexesSelected = new List<int>();

        public CodeBlockViewer(PsaMovesetHandler psaMovesetHandler, PsaCommandsConfig psaCommandsConfig, CodeBlockSelection codeBlockSelection)
        {
            this.psaMovesetHandler = psaMovesetHandler;
            this.psaCommandsConfig = psaCommandsConfig;
            CodeBlockSelection = codeBlockSelection;

            InitializeComponent();

            this.DoubleBuffered(true);
            codeBlockCommandsScintilla.DoubleBuffered(true);
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
        }

        public void LoadCodeBlockCommands()
        {
            codeBlockCommandsScintilla.ReadOnly = false;

            switch (CodeBlockSelection.SectionType)
            {
                case SectionType.ACTION:
                    psaCommands = psaMovesetHandler.ActionsHandler.GetPsaCommandsInCodeBlock(CodeBlockSelection.SectionIndex, CodeBlockSelection.CodeBlockIndex);
                    break;
                case SectionType.SUBACTION:
                    psaCommands = psaMovesetHandler.SubActionsHandler.GetPsaCommandsForSubAction(CodeBlockSelection.SectionIndex, CodeBlockSelection.CodeBlockIndex);
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

        private void codeBlockCommandsScintilla_TextChanged(object sender, EventArgs e)
        {
            // if no commands exist in code block, don't show line numbers
            codeBlockCommandsScintilla.ShowLineNumbers = psaCommands.Count > 0;
        }

        private void codeBlockCommandsScintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // when caret changes
            if ((e.Change & UpdateChange.Selection) == UpdateChange.Selection)
            {
                UpdateSelectedCommand();
                StyleDocument();
            }
            if ((e.Change & UpdateChange.HScroll) == UpdateChange.HScroll
                || (e.Change & UpdateChange.VScroll) == UpdateChange.VScroll)
            {
                StyleDocument();
            }
        }

        private void UpdateSelectedCommand()
        {
            if (psaCommands.Count > 0)
            {
                List<int> selectedCommandIndexes = codeBlockCommandsScintilla.GetSelectedLines();

                List<int> newIndexesSelected = selectedCommandIndexes.Except(this.currentCommandIndexesSelected).ToList();

                if (selectedCommandIndexes.Count != this.currentCommandIndexesSelected.Count || newIndexesSelected.Count > 0)
                {
                    List<PsaCommand> psaCommands = new List<PsaCommand>();
                    for (int i = 0; i < selectedCommandIndexes.Count; i++)
                    {
                        PsaCommand psaCommand = CodeBlockSelection.GetPsaCommandInCodeBlock(selectedCommandIndexes[i]);
                        psaCommands.Add(psaCommand);
                    }

                    foreach (ICodeBlockViewerListener listener in listeners)
                    {
                        listener.OnCommandSelected(psaCommands, CodeBlockSelection);
                    }

                    codeBlockCommandsScintilla.Focus();
                }

                this.currentCommandIndexesSelected = selectedCommandIndexes;
            }
            else
            {
                this.currentCommandIndexesSelected.Clear();

                foreach (ICodeBlockViewerListener listener in listeners)
                {
                    listener.OnCommandSelected(null, CodeBlockSelection);
                }
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

            CodeBlockSelection.InsertCommand(currentLineIndex, psaCommand);

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

            CodeBlockSelection.InsertCommand(currentLineIndex, psaCommand);

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

            CodeBlockSelection.InsertCommand(codeBlockCommandsScintilla.Lines.Count, psaCommand);

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
                    CodeBlockSelection.ModifyCommand(lineIndex, psaCommand);
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

                        CodeBlockSelection.MoveCommandUp(lineIndex);

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

                        CodeBlockSelection.MoveCommandDown(lineIndex);

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
                    CodeBlockSelection.RemoveCommand(lineIndex);
                }

                int firstVisibleLine = codeBlockCommandsScintilla.FirstVisibleLine;
                LoadCodeBlockCommands();

                codeBlockCommandsScintilla.LineScroll(firstVisibleLine, 0);

                StyleDocument();
            }
        }
    }
}
