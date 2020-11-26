using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace PSA2MovesetLogic.src.ExtentionMethods
{
    public static class ArrayExtensions
    {
        public static T[] Slice<T>(this T[] array, int startIndex, int endIndex)
        {
            return (T[])array.Skip(startIndex).Take(endIndex);
        }
    }
}
