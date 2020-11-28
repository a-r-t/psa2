using System;
using System.Collections.Generic;
using System.Text;

namespace PSA2.src.ExtentionMethods
{
    public static class IntExtensions
    {
        /// <summary>
        /// Converts an int value to a boolean
        /// </summary>
        /// <param name="i">The int value</param>
        /// <returns>boolean -- false (if i is 0), true otherwise</returns>
        public static bool ToBoolean(this int i)
        {
            return i != 0;
        }
    }
}
