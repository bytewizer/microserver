using System;
using System.Text;

using MicroServer.Utilities;

namespace MicroServer.Extensions
{
    /// <summary>
    /// Extension methods for Strings
    /// </summary>
    public static class StringExtensions
    {
        public static char[] CopyTo(this string source, int sourceIndex, char[] destination, int destinationIndex, int count)
        {
            return source.ToCharArray();
        }

        public static string ToCamelCase(this string value)
        {
            if (StringUtility.IsNullOrEmpty(value)) return value;
            return null; //value.First().ToString().ToLower() + (value.Length > 1 ? value.Substring(1) : "");
        }

        /// <summary>
        /// Determines if the string 'source' starts with string 'value'.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns>True if string starts with value</returns>
        public static bool StartsWith(this string source, string value)
        {
            return source.ToLower().IndexOf(value.ToLower()) == 0;
        }

        /// <summary>
        /// Determines if the string 'source' ends with string 'value'.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns>True if string ends with value</returns>
        public static bool EndsWith(this string source, string value)
        {
            return source.ToLower().IndexOf(value.ToLower()) == source.Length - value.Length;
        }

        /// <summary>
        /// Determines if the string 'source' contains the string 'value'.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns>True if string ends with value</returns>
        public static bool Contains(this string source, string value)
        {
            return source.ToLower().IndexOf(value.ToLower()) >= 0;
        }
        
        /// <summary>
        /// Replace all occurrences of the 'find' string with the 'replace' string.
        /// </summary>
        /// <param name="source">Original string</param>
        /// <param name="find">String to find within the original string</param>
        /// <param name="replace">String to be used in place of the find string</param>
        /// <returns>Final string after all instances have been replaced.</returns>
        public static string Replace(this string source, string find, string replace)
        {
            return StringUtility.Replace(source, find, replace);
        }
	}
}
