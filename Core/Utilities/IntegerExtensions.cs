using System;

namespace MicroServer.Core.Extensions
{
    /// <summary>
    /// Extension methods for Integers
    /// </summary>
    public static class IntegerExtensions
    {
        private static string hexValues = "0123456789ABCDEF";

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this Int16 value, string prefix)
        {
            char[] charValues = new char[4];
            charValues[0] = hexValues[(byte)((value >> 12) & 0x0F)];
            charValues[1] = hexValues[(byte)((value >> 8) & 0x0F)];
            charValues[2] = hexValues[(byte)((value >> 4) & 0x0F)];
            charValues[3] = hexValues[(byte)((value >> 0) & 0x0F)];
            return prefix + new string(charValues);
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this Int32 value, string prefix)
        {
            char[] charValues = new char[8];
            charValues[0] = hexValues[(byte)((value >> 28) & 0x0F)];
            charValues[1] = hexValues[(byte)((value >> 24) & 0x0F)];
            charValues[2] = hexValues[(byte)((value >> 20) & 0x0F)];
            charValues[3] = hexValues[(byte)((value >> 16) & 0x0F)];
            charValues[4] = hexValues[(byte)((value >> 12) & 0x0F)];
            charValues[5] = hexValues[(byte)((value >> 8) & 0x0F)];
            charValues[6] = hexValues[(byte)((value >> 4) & 0x0F)];
            charValues[7] = hexValues[(byte)((value >> 0) & 0x0F)];
            return prefix + new string(charValues);
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this UInt16 value, string prefix)
        {
            char[] charValues = new char[4];
            charValues[0] = hexValues[(byte)((value >> 12) & 0x0F)];
            charValues[1] = hexValues[(byte)((value >> 8) & 0x0F)];
            charValues[2] = hexValues[(byte)((value >> 4) & 0x0F)];
            charValues[3] = hexValues[(byte)((value >> 0) & 0x0F)];
            return prefix + new string(charValues);
        }

        /// <summary>
        /// Converts a Integer to Hex string
        /// </summary>
        /// <param name="value">Integer to convert</param>
        /// <param name="prefix">String'0x' to include before the Hex value.</param>
        /// <returns>Integer value in Hex</returns>
        public static string ToHexString(this UInt32 value, string prefix)
        {
            char[] charValues = new char[8];
            charValues[0] = hexValues[(byte)((value >> 28) & 0x0F)];
            charValues[1] = hexValues[(byte)((value >> 24) & 0x0F)];
            charValues[2] = hexValues[(byte)((value >> 20) & 0x0F)];
            charValues[3] = hexValues[(byte)((value >> 16) & 0x0F)];
            charValues[4] = hexValues[(byte)((value >> 12) & 0x0F)];
            charValues[5] = hexValues[(byte)((value >> 8) & 0x0F)];
            charValues[6] = hexValues[(byte)((value >> 4) & 0x0F)];
            charValues[7] = hexValues[(byte)((value >> 0) & 0x0F)];
            return prefix + new string(charValues);
        }

        //private static UInt16 ReverseBytes(UInt16 value)
        //{
        //    return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        //}

        //private static UInt32 ReverseBytes(UInt32 value)
        //{
        //    return (UInt32)(value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
        //           (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        //}

    }
}
