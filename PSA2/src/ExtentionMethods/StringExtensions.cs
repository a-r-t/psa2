﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSA2.src.ExtentionMethods
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this string s)
        {
            double retNum;
            bool isNum = Double.TryParse(Convert.ToString(s), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static string ToTitleCase(this string s)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s.ToLower());
        }

        public static Size Measure(this string s, Font f)
        {
            return TextRenderer.MeasureText(s, f);
        }
    }
}
