using PSA2.utility;
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
            MovesetFile movesetFile = new MovesetFile("data/FitRidley.pac");
            ParseMovesetFile parseMovesetFile = new ParseMovesetFile();
            parseMovesetFile.getAttributes(movesetFile.FileContent);
        }
    }
}
