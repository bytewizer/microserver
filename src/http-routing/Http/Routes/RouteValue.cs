using System.Text;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Defines a route value pair that can be set or retrieved.
    /// </summary>
    public class RouteValue
    {
        /// <summary>
        /// Gets the key in the route/value pair.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Gets the value in the route/value pair.
        /// </summary>
        public string[] Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteValue"/> structure with the specified key and value.
        /// </summary>
        public RouteValue() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteValue"/> structure with the specified key and value.
        /// </summary>
        /// <param name="key">The key in the route/value pair.</param>
        /// <param name="value">The value in the route/value pair.</param>
        public RouteValue(string key, string[] value)
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