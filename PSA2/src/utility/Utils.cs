using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSA2.src.utility
{
    public static class Utils
    {
        public static string intArrayToString(int[] array)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i]);
                if (i + 1 != array.Length)
                {
                    stringBuilder.Append(", ");
                }
            }
            stringBuilder.Append("]");
            return stringBuilder.ToString();
        }

        public static int convertWordToBase10Int(int byte1, int byte2, int byte3, int byte4)
        {
            return byte4 + (byte3 << 8) + (byte2 << 16) + (byte1 << 24);
        }

        public static float ConvertBytesToFloat(int value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            return BitConverter.ToSingle(valueBytes, 0);
        }

        public static string ConvertWordToString(int word)
        {
            List<byte> letters = new List<byte>
            {
                (byte)((word >> 24) & 0xFF),
                (byte)((word >> 16) & 0xFF),
                (byte)((word >> 8) & 0xFF),
                (byte)(word & 0xFF)
            };
            List<byte> lettersNoNulls = letters.Where(letter => letter != 0).ToList();
            return Encoding.UTF8.GetString(lettersNoNulls.ToArray());
        }
    }
}