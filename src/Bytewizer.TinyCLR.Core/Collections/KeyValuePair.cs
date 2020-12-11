using System.Text;

namespace System.Collections
{
    /// <summary>
    /// Defines a key value pair that can be set or retrieved.
    /// </summary>
    public class KeyValuePair
    {
        /// <summary>
        /// Gets the key in the key/value pair.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets the value in the key/value pair.
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuePair"/> structure with the specified key and value.
        /// </summary>
        public KeyValuePair() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyValuePair"/> structure with the specified key and value.
        /// </summary>
        /// <param name="key">The key in the key/value pair.</param>
        /// <param name="value">The value in the key/value pair.</param>
        public KeyValuePair(string key, string value)
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