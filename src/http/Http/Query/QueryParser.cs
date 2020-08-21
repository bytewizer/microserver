using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Provides methods for parsing and manipulating query strings.
    /// </summary>
    internal class QueryParser
    {
        /// <summary>
        /// Parse a query string into its component key and value parts.
        /// </summary>
        /// <param name="queryString">The raw query string value, with or without the leading '?'.</param>
        /// <returns>A collection of parsed keys and values.</returns>
        public static ArrayList ParseQuery(string queryString)
        {
            var result = ParseNullableQuery(queryString);

            if (result == null)
            {
                return default;
            }

            return result;
        }

        /// <summary>
        /// Parse a query string into its component key and value parts.
        /// </summary>
        /// <param name="queryString">The raw query string value, with or without the leading '?'.</param>
        /// <returns>A collection of parsed keys and values, null if there are no entries.</returns>
        public static ArrayList ParseNullableQuery(string queryString)
        {
            var accumulator = new ArrayList();

            if (string.IsNullOrEmpty(queryString) || queryString == "?")
            {
                return null;
            }

            int scanIndex = 0;
            if (queryString[0] == '?')
            {
                scanIndex = 1;
            }

            int textLength = queryString.Length;
            int equalIndex = queryString.IndexOf('=');
            if (equalIndex == -1)
            {
                equalIndex = textLength;
            }
            while (scanIndex < textLength)
            {
                int delimiterIndex = queryString.IndexOf('&', scanIndex);
                if (delimiterIndex == -1)
                {
                    delimiterIndex = textLength;
                }
                if (equalIndex < delimiterIndex)
                {
                    while (scanIndex != equalIndex && IsWhiteSpace(queryString[scanIndex]))
                    {
                        ++scanIndex;
                    }
                    string name = queryString.Substring(scanIndex, equalIndex - scanIndex);
                    string value = queryString.Substring(equalIndex + 1, delimiterIndex - equalIndex - 1);
                    accumulator.Add(
                        new QueryValue(
                            //Uri.UnescapeDataString(name.Replace("+", " ")),
                            //Uri.UnescapeDataString(value.Replace("+", " ")));
                            name.Replace("+", " "),
                            value.Replace("+", " "))
                        );
                    equalIndex = queryString.IndexOf('=', delimiterIndex);
                    if (equalIndex == -1)
                    {
                        equalIndex = textLength;
                    }
                }
                else
                {
                    if (delimiterIndex > scanIndex)
                    {
                        accumulator.Add(
                            new QueryValue(
                                queryString.Substring(scanIndex, delimiterIndex - scanIndex), string.Empty)
                            );
                    }
                }
                scanIndex = delimiterIndex + 1;
            }

            if (accumulator.Count == 0)
            {
                return default;
            }

            return accumulator;
        }

        private static bool IsWhiteSpace(char source)
        {
            return (source == ' ' || source == '\t' || source == '\n' || source == '\r');
        }
    }
}
