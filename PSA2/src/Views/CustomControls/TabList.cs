using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.Views.CustomControls
{
    public partial class TabList : PanelButton
    {
        private Panel list;

        public TabList(): base()
        {
            SetupControls();
        }

        private void SetupControls()
        {
            Text = "⯆";
            Size = new Size(50, 20);
            Font = new Font(Font.FontFamily, 18, Font.Style);
        }

        protected override void OnSelectionStatusChanged()
        {
            base.OnSelectionStatusChanged();
        }


    }


}
