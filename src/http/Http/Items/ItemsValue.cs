using System.Text;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Defines a key value pair that can be set or retrieved.
    /// </summary>
    public class ItemsValue
    {
        /// <summary>
        /// Gets the key in the item/value pair.
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// Gets the value in the item/value pair.
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsValue"/> structure with the specified key and value.
        /// </summary>
        public ItemsValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsValue"/> structure with the specified key and value.
        /// </summary>
        /// <param name="key">The key in the item/value pair.</param>
        /// <param name="value">The value in the item/value pair.</param>
        public ItemsValue(object key, object value)
        {
            Key = key;
            Value = value;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append('[');

            if (Key != null)
            {
                s.Append(Key);
            }

            s.Append(", ");

            if (Value != null)
            {
                s.Append(Value);
            }

            s.Append(']');

            return s.ToString();
        }
    }
}