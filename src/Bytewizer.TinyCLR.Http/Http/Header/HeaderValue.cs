using System.Text;

namespace Bytewizer.TinyCLR.Http.Header
{
    /// <summary>
    /// Defines a header value pair that can be set or retrieved.
    /// </summary>
    public class HeaderValue
    {
        /// <summary>
        /// Gets the key in the header/value pair.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets the value in the header/value pair.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderValue"/> structure with the specified key and value.
        /// </summary>
        public HeaderValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderValue"/> structure with the specified key and value.
        /// </summary>
        /// <param name="key">The key in the header/value pair.</param>
        /// <param name="value">The value in the header/value pair.</param>
        public HeaderValue(string key, string value)
        {
            Key = key;
            Value = value;
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