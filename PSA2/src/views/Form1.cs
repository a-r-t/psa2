using PSA2.src.FileProcessor;
using PSA2.src.FileProcessor.MovesetParser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.views
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //PsaFileParser psaFileParser = new PsaFileParser("data/FitRidley.pac");
            // PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitRidley-DataSection.pac");
            //PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitRidleyOversized.pac");
            PsaFileParser psaFileParser = new PsaFileParser("E:/Hobby Stuff/Super Smash Bros Brawl Hacking/Workbench/decompile psac/testmoveset/FitRidley-CodeModified.pac");

            PsaMovesetParser psaMovesetParser = psaFileParser.ParseMovesetFile();
        }
    }
}
