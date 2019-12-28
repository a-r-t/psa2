using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PSA2.src.utility
{
    public static class Utils
    {
        public static string IntArrayToString(int[] array)
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

        public static int ConvertWordToBase10Int(int byte1, int byte2, int byte3, int byte4)
        {
            return byte4 + (byte3 << 8) + (byte2 << 16) + (byte1 << 24);
        }

        public static float ConvertBytesToFloat(int value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            return BitConverter.ToSingle(valueBytes, 0);
        }

        public static string ConvertWordToString(int word, int startByte=0)
        {
            List<byte> letters = new List<byte>();

            if (startByte == 0)
            {
                letters.Add((byte)((word >> 24) & 0xFF));
            }
            if (startByte <= 1)
            {
                letters.Add((byte)((word >> 16) & 0xFF));
            }
            if (startByte <= 2)
            {
                letters.Add((byte)((word >> 8) & 0xFF));
            }
            if (startByte <= 3)
            {
                letters.Add((byte)(word & 0xFF));
            }


            for (int i = 0; i < letters.Count; i++)
            {
                if (letters[i] == 0)
                {
                    letters.RemoveRange(i, letters.Count - i);
                    break;
                }
            }
            return Encoding.UTF8.GetString(letters.ToArray());
        }

        public static T LoadJson<T>(string filePath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }
    }
}