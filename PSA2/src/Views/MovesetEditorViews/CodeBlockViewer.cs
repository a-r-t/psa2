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
        public List<PsaCommand> PsaCommands { get; set; }  = new List<PsaCommand>();
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
            LoadCodeBlockCommands();

            UpdateSelectedCommand();

            codeBlockCommandsScintilla.StyleDocument();
        }

        public void LoadCodeBlockCommands()
        {
            codeBlockCommandsScintilla.ReadOnly = false;

            PsaCommands = CodeBlockSelection.GetPsaCommandsInCodeBlock();

            codeBlockCommandsScintilla.PsaCommands = PsaCommands;

            codeBlockCommandsScintilla.ClearAll();
            commandTexts.Clear();

            if (PsaCommands.Count > 0)
            {
                foreach (PsaCommand psaCommand in PsaCommands)
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
                codeBlockCommandsScintilla.ShowLineNumbers = false;
            }

        }

        public string GetCommandText(PsaCommand psaCommand)
        {
            PsaCommandConfig psaCommandConfig = psaCommandsConfig.GetPsaCommandConfigByInstruction(psaCommand.Instruction);

            StringBuilder commandTextBuilder = new StringBuilder();

            if (psaCommandConfig != null)
            {
                commandTextBuilder.Append(psaCommandConfig.CommandName.Replace(' ', '\u2800')); // replace space with braille space to ignore scintilla word wrap

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
                commandTextBuilder.Append(psaCommand.Instruction);

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
            codeBlockCommandsScintilla.ShowLineNumbers = PsaCommands.Count > 0;
        }

        private void codeBlockCommandsScintilla_UpdateUI(object sender, UpdateUIEventArgs e)
        {
            // when caret changes
            if ((e.Change & UpdateChange.Selection) == UpdateChange.Selection)
            {
                UpdateSelectedCommand();
            }

            if ((e.Change & UpdateChange.Selection) == UpdateChange.Selection
                || (e.Change & UpdateChange.HScroll) == UpdateChange.HScroll
                || (e.Change & UpdateChange.VScroll) == UpdateChange.VScroll)
            {
                codeBlockCommandsScintilla.StyleDocument();
            }
        }

        public void UpdateSelectedCommand()
        {
            if (PsaCommands.Count > 0)
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
                        listener.OnCommandSelected(psaCommands, selectedCommandIndexes, CodeBlockSelection);
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
                    listener.OnCommandSelected(null, null, CodeBlockSelection);
                }
            }
        }

        public void EmitCommandSelected()
        {
            List<int> selectedCommandIndexes = codeBlockCommandsScintilla.GetSelectedLines();

            List<PsaCommand> psaCommands = new List<PsaCommand>();
            for (int i = 0; i < selectedCommandIndexes.Count; i++)
            {
                PsaCommand psaCommand = CodeBlockSelection.GetPsaCommandInCodeBlock(selectedCommandIndexes[i]);
                if (psaCommand != null)
                {
                    psaCommands.Add(psaCommand);
                }
            }
            if (psaCommands.Count == 0)
            {
                psaCommands = null;
            }

            foreach (ICodeBlockViewerListener listener in listeners)
            {
                listener.OnCommandSelected(psaCommands, selectedCommandIndexes, CodeBlockSelection);
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

            codeBlockCommandsScintilla.StyleDocument();
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

            codeBlockCommandsScintilla.StyleDocument();
        }

        public void AppendCommand(PsaCommandConfig psaCommandConfig)
        {
            PsaCommand psaCommand = psaCommandConfig.ToPsaCommand();

            CodeBlockSelection.InsertCommand(codeBlockCommandsScintilla.Lines.Count, psaCommand);

            List<int> currentSelectedLines = codeBlockCommandsScintilla.GetSelectedLines();

            LoadCodeBlockCommands();
            codeBlockCommandsScintilla.LineScroll(codeBlockCommandsScintilla.Lines.Count, 0);
            codeBlockCommandsScintilla.SelectLines(currentSelectedLines);

            codeBlockCommandsScintilla.StyleDocument();
        }

        public void ReplaceCommand(PsaCommandConfig psaCommandConfig)
        {
            if (PsaCommands.Count > 0)
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

                codeBlockCommandsScintilla.StyleDocument();
            }
        }

        public void MoveCommandUp()
        {
            if (PsaCommands.Count > 0)
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

                codeBlockCommandsScintilla.StyleDocument();
            }
        }

        public void MoveCommandDown()
        {
            if (PsaCommands.Count > 0)
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

                codeBlockCommandsScintilla.StyleDocument();
            }
        }

        public void RemoveCommand()
        {
            if (PsaCommands.Count > 0)
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

                codeBlockCommandsScintilla.StyleDocument();
            }
        }
    }
}
