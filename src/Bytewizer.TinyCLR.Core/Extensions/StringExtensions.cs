namespace Bytewizer.TinyCLR
{
    /// <summary>
    /// Contains extension methods for <see cref="string"/> object.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="value">The string to compare.</param>
        /// <returns><c>true</c> if value matches the beginning of this string; otherwise, <c>false.</c></returns>
        public static bool StartsWith(this string source, string value)
        {
            return source.ToLower().IndexOf(value.ToLower()) == 0;
        }

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="value">The string to compare.</param>
        /// <returns><c>true</c> if value matches the beginning of this string; otherwise, <c>false.</c></returns>
        public static bool EndsWith(this string source, string value)
        {
            return source.ToLower().IndexOf(value.ToLower()) == source.Length - value.Length;
        }

        /// <summary>
        /// Returns a value indicating whether a specified substring occurs within this string.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <param name="value">The string to seek.</param>
        /// <returns><c>true</c> if the value parameter occurs within this string, or if value is the empty string (""); otherwise, <c>false.</c></returns>
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
            int i;
            int iStart = 0;

            if (source == string.Empty || source == null || find == string.Empty || find == null)
                return source;

            while (true)
            {
                i = source.IndexOf(find, iStart);
                if (i < 0) break;

                if (i > 0)
                    source = source.Substring(0, i) + replace + source.Substring(i + find.Length);
                else
                    source = replace + source.Substring(i + find.Length);

                iStart = i + replace.Length;
            }
            return source;
        }
    }
}