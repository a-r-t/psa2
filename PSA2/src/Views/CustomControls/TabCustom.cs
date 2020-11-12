using PSA2.src.ExtentionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class TabCustom: Button
    {
        public TabCustom(string text) : base()
        {
            FlatStyle = FlatStyle.Flat;
            Font = new Font("Times New Roman", 10, FontStyle.Regular);
            Text = text;
            Width = text.Measure(Font).Width + 20;
            Height = 30;
        }

   
    }
}
