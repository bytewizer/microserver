using System.Text;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Defines a form string value pair that can be set or retrieved.
    /// </summary>
    public class CookieValue
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
        /// Initializes a new instance of the <see cref="CookieValue"/> structure with the specified key and value.
        /// </summary>
        public CookieValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieValue"/> structure with the specified key and value.
        /// </summary>
        /// <param name="key">The key in the key/value pair.</param>
        /// <param name="value">The value in the form/value pair.</param>
        public CookieValue(string key, string value)
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