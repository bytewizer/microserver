using System;

namespace MicroServer.Core.Extensions
{
    /// <summary>
    /// Extension methods for Char
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Check if the provided char is white space
        /// </summary>
        /// <param name="source">Char to validate</param>
        /// <returns>True if the Char is white space</returns>
        public static bool IsWhiteSpace(char source)
        {
            return (source == ' ' || source == '\t' || source == '\n' || source == '\r');
        }

        /// <summary>
        /// Check if the provided char is white space
        /// </summary>
        /// <param name="source">Char to validate</param>
        /// <returns>True if the Char is white space</returns>
        public static bool IsDigit(char ch)
        {
            return ch >= '0' && ch <= '9';
        }

        /// <summary>
        /// Check if the provided char is Uri safe
        /// </summary>
        /// <param name="source">Char to validate</param>
        /// <returns>True if the Char is Uri safe</returns>
        static bool IsSafeUriChar(this char c)
        {
            return c == '.' || c == '-' || c == '_' || c == '~'
                || c >= 'a' && c <= 'z'
                || c >= 'A' && c <= 'Z'
                || c >= '0' && c <= '9';
        }
    }
}
