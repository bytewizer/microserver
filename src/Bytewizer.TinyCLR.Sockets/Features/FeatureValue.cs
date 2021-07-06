using System;
using System.Text;

namespace Bytewizer.TinyCLR.Features
{
    /// <summary>
    /// Defines a key value pair that can be set or retrieved.
    /// </summary>
    public class FeatureValue
    {
        /// <summary>
        /// Gets the key in the key/value pair.
        /// </summary>
        public Type Key { get; set; }

        /// <summary>
        /// Gets the value in the key/value pair.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureValue"/> structure with the specified key and value.
        /// </summary>
        public FeatureValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FeatureValue"/> structure with the specified key and value.
        /// </summary>
        /// <param name="key">The key in the key/value pair.</param>
        /// <param name="value">The value in the key/value pair.</param>
        public FeatureValue(Type key, object value)
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