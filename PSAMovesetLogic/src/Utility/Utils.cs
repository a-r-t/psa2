using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PSA2MovesetLogic.src.Utility
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

        public static int ConvertDoubleWordToBase10Int(int byte1, int byte2, int byte3, int byte4)
        {
            return byte4 + (byte3 << 8) + (byte2 << 16) + (byte1 << 24);
        }

        public static float ConvertBytesToFloat(int value)
        {
            byte[] valueBytes = BitConverter.GetBytes(value);
            return BitConverter.ToSingle(valueBytes, 0);
        }

        public static int ConvertIntToIeeFloatingPoint(int value)
        {
            byte[] valueBytes = BitConverter.GetBytes((float)value);
            if (BitConverter.IsLittleEndian)
            {
                return valueBytes[0] + valueBytes[1] * 256 + valueBytes[2] * 65536 + valueBytes[3] * 16777216;
            }
            else
            {
                return valueBytes[3] + valueBytes[2] * 256 + valueBytes[1] * 65536 + valueBytes[0] * 16777216;
            }
        }

        public static string ConvertDoubleWordToString(int word, int startByte=0)
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


        public static int[] ConvertStringToDoubleWords(string s)
        {
            // length of array is number of bytes required to hold all characters in animation name
            // each double word (4 bytes) can hold 4 characters max (1 char per byte)
            // there always has to be at least "null" byte to end a string, so a 1 is added to the length to ensure the "null" byte will always exist
            // if there's overflow (like the name has 5 letters), the added extra double word will have 1 letter and then 3 "null" bytes...so it always works
            int[] doubleWords = new int[(s.Length / 4) + 1];
            byte[] bytes = Encoding.UTF8.GetBytes(s);
            for (int i = 0; i < s.Length; i++)
            {
                if (i % 4 == 0)
                {
                    doubleWords[i / 4] = bytes[i] * 16777216;
                }
                else if (i % 4 == 1)
                {
                    doubleWords[i / 4] += bytes[i] * 65536;
                }
                else if (i % 4 == 2)
                {
                    doubleWords[i / 4] += bytes[i] * 256;
                }
                else
                {
                    doubleWords[i / 4] += bytes[i];
                }
            }
            return doubleWords;
        }

        public static T LoadJson<T>(string filePath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        public static string ConvertIntToOffset(int offset)
        {
            return String.Format("0x{0}", offset.ToString("X"));
        }
    }
}