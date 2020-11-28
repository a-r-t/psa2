using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSA2MovesetLogic.src.ExtentionMethods
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

        /// <summary>
        /// Returns a piece of a list
        /// <para>For example, a list [1, 2, 3, 4, 5] called with Slice(1, 3) would return [2, 3, 4]</para>
        /// </summary>
        /// <param name="startIndex">start index of slice</param>
        /// <param name="endIndex">end index of slice</param>
        /// <returns>Sliced list</returns>
        public static List<T> Slice<T>(this List<T> list, int startIndex, int endIndex)
        {
            return list.Skip(startIndex).Take(endIndex).ToList();
        }
    }
}
