using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class TabPageCustom : Panel
    {
        public string TabText { get; set; }
        public Font TabFont { get; set; }

        public TabPageCustom(): base()
        {
            Dock = DockStyle.Fill;
            TabFont = new Font("Times New Roman", 12, FontStyle.Regular);
        }

    }
}
