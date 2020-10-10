using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2.src.ExtentionMethods
{
    public static class ListExtensions
    {
        /// <summary>
        /// Gets an item from list at specified index
        /// <para>Supports negative indexes (the reason why this extension was made) which start from the end of the list</para>
        /// <para>Calling myList.GetAt(-1) is the same as myList[list.Count -1]</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T GetAt<T>(this List<T> list, int index)
        {
            return index >= 0
                ? list[index]
                : list[list.Count + index];
        }
    }
}
