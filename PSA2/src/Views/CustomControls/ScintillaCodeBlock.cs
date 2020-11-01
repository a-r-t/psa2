using PSA2MovesetLogic.src.FileProcessor.MovesetHandler.MovesetHandlerHelpers.CommandHandlerHelpers;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public class ScintillaCodeBlock : ScintillaExt
    {
        public List<PsaCommand> PsaCommands { get; set; }

        public ScintillaCodeBlock()
        {
            Initialize();
        }

        private void Initialize()
        {
            SetSelectionBackColor(true, Color.FromArgb(38, 79, 120));
            Styles[Style.Default].BackColor = Color.White;
            CaretForeColor = Color.Black;
            Styles[Style.Default].Font = "Consolas";
            Styles[Style.Default].SizeF = 12;
            StyleClearAll();

            Styles[1].ForeColor = Color.FromArgb(68, 156, 214);
            Styles[2].ForeColor = Color.Black;

            Styles[3].ForeColor = Color.Green;

            Styles[4].FillLine = true;
            Styles[4].ForeColor = Color.FromArgb(68, 156, 214);
            Styles[4].BackColor = Color.FromArgb(38, 79, 120);

            Styles[5].FillLine = true;
            Styles[5].ForeColor = Color.White;
            Styles[5].BackColor = Color.FromArgb(38, 79, 120);

            Styles[6].FillLine = true;
            Styles[6].ForeColor = Color.Green;
            Styles[6].BackColor = Color.FromArgb(38, 79, 120);

            // sets margin colors
            Styles[Style.LineNumber].BackColor = Color.FromArgb(240, 240, 240);
            Styles[Style.LineNumber].ForeColor = Color.Black;
            MultipleSelection = true;
            CaretStyle = CaretStyle.Line;

            // Turns wrap mode on
            //codeBlockCommandsScintilla.WrapStartIndent = 4;
            //codeBlockCommandsScintilla.WrapMode = WrapMode.Whitespace;
            //codeBlockCommandsScintilla.WrapIndentMode = WrapIndentMode.Indent;

            ReadOnly = true;
            CurrentCursor = Cursors.Arrow;

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

            Lexer = Lexer.Container;

        }

        public void StyleDocument()
        {
            List<int> selectedLines = GetSelectedLines();
            StartStyling(0);
            for (int i = 0; i < Lines.Count; i++)
            {
                StyleLineIndex(i, selectedLines.Contains(i));
            }
        }

        private void StyleLineIndex(int lineIndex, bool isSelected)
        {
            int commandNameStyle = !isSelected ? 1 : 4;
            int paramNameStyle = !isSelected ? 2 : 5;
            int valueStyle = !isSelected ? 3 : 6;

            string text = Lines[lineIndex].Text;

            if (!string.IsNullOrEmpty(text))
            {
                int lastIndexOfColon = text.LastIndexOf(':');
                if (lastIndexOfColon >= 0)
                {
                    // style everything up to the last colon
                    SetStyling(lastIndexOfColon, commandNameStyle);

                    SetStyling(1, paramNameStyle);

                    int stoppingPoint = lastIndexOfColon;
                    for (int i = 0; i < PsaCommands[lineIndex].NumberOfParams; i++)
                    {
                        for (int j = stoppingPoint + 2; j < text.Length; j++)
                        {
                            if (text[j] != '=')
                            {
                                SetStyling(1, paramNameStyle);
                            }
                            else
                            {
                                SetStyling(2, paramNameStyle);
                                stoppingPoint = j;
                                break;
                            }
                        }
                        for (int j = stoppingPoint + 1; j < text.Length; j++)
                        {
                            if (text[j] != ',')
                            {
                                SetStyling(1, valueStyle);
                            }
                            else
                            {
                                SetStyling(3, paramNameStyle);
                                stoppingPoint = j + 2;
                                break;
                            }
                        }
                    }

                }
                else
                {
                    SetStyling(text.Length, commandNameStyle);
                }
            }
        }
    }
}
