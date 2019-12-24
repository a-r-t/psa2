using System;

namespace utility.UtilityMethods
{
    public static class Utility
    {
        public static void printIntArray(int[] array)
        {
            Console.Write("\n[");
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]);
                if (i + 1 != array.Length)
                {
                    Console.Write(", ");
                }
            }
            Console.Write("]\n");
        }

        public static int convertWordToBase10Int(int byte1, int byte2, int byte3, int byte4)
        {
            return byte4 + (byte3 << 8) + (byte2 << 16) + (byte1 << 24);
        }
    }
}