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
        private string tabText;
        public string TabText 
        { 
            get
            {
                return tabText;
            }
            set
            {
                tabText = value;
                OnTabTextChanged(this, new EventArgs());
            }
        }
        public Font TabFont { get; set; }

        public event EventHandler TabTextChanged;

        public TabPageCustom(): base()
        {
            Dock = DockStyle.Fill;
            TabFont = new Font("Times New Roman", 12, FontStyle.Regular);
        }

        private void OnTabTextChanged(object sender, EventArgs e)
        {
            TabTextChanged?.Invoke(sender, e);
        }
    }
}
