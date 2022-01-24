using System.Collections;
using System.Text;

namespace Bytewizer.TinyCLR.Http.Query
{
    /// <summary>
    /// Defines a query string value pair that can be set or retrieved.
    /// </summary>
    public class QueryValue
    {
        /// <summary>
        /// Gets the key in the query/value pair.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets the value in the route/value pair.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryValue"/> structure with the specified key and value.
        /// </summary>
        public QueryValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryValue"/> structure with the specified key and value.
        /// </summary>
        /// <param name="key">The key in the query/value pair.</param>
        /// <param name="value">The value in the query/value pair.</param>
        public QueryValue(string key, string value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Parse a query string into its component key and value parts.
        /// </summary>
        /// <param name="queryString">The raw query string value with or without the leading '?'.</param>
        /// <param name="parsedValues">A collection containing the query string values.</param>
        /// <returns>A collection of parsed keys and values.</returns>
        public static bool TryParseList(string queryString, out ArrayList parsedValues)
        {
            parsedValues = QueryParser.ParseNullableQuery(queryString);

            return parsedValues != null;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            var header = new StringBuilder();

            header.Append(Key);
            header.Append("=");
            header.Append(Value);

            return header.ToString();
        }
    }
}