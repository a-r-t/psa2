using PSA2MovesetLogic.src.FileProcessor;
using PSA2MovesetLogic.src.FileProcessor.MovesetHandler;
using PSA2.src.Views.MovesetEditorViews;
using PSA2.src.Views.MovesetEditorViews.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views
{
    public partial class MainForm : Form, IMovesetEditorListener
    {
        public MainForm()
        {
            InitializeComponent();
            DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //PsaFileParser psaFileParser = new PsaFileParser("data/FitRidley.pac");
            //PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitRidley-DataSection.pac");
            //PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitRidleyOversized.pac");
            //PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitRidley-CodeModified.pac");
            //PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitSnake.pac");
            //PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitMetaKnight.pac");
            //PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitWario.pac");
            PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Brawl File Partition/Clean Fighters/mario/FitMario.pac");
            PsaMovesetHandler psaMovesetHandler = psaFileParser.ParseMovesetFile();
            MovesetEditor movesetEditor = new MovesetEditor(psaMovesetHandler);
            movesetEditor.AddListener(this);
            viewPanel.Controls.Add(movesetEditor);
            movesetEditor.Dock = DockStyle.Fill;
        }
    }
}
