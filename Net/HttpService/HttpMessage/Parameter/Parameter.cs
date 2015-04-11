using System;
using System.Collections;

namespace MicroServer.Net.Http.Messages
{
    /// <summary>
    ///     A parameter in a HTTP header field.
    /// </summary>
    public class Parameter : IParameter
    {
        private readonly ArrayList _values = new ArrayList();

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentNullException">
        /// name
        /// or
        /// value
        /// </exception>
        public Parameter(string name, string value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (value == null) throw new ArgumentNullException("value");
            Name = name;
            _values.Add(value);
        }

        /// <summary>
        ///     Gets a list of all values.
        /// </summary>
        public IEnumerable Values
        {
            get { return _values; }
        }

        #region IParameter Members

        /// <summary>
        ///     Gets *last* value.
        /// </summary>
        /// <remarks>
        ///     Parameters can have multiple values. This property will always get the last value in the list.
        /// </remarks>
        /// <value>String if any value exist; otherwise <c>null</c>.</value>
        public string Value
        {
            get { return _values.Count == 0 ? null : (string)_values[_values.Count - 1]; }
        }

        /// <summary>
        ///     Gets or sets name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        ///     Get one of the values.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get { return (string)_values[index]; }
        }

        /// <summary>
        ///     Get number of values
        /// </summary>
        public int Count
        {
            get { return _values.Count; }
        }

        /// <summary>
        ///     Add a new parameter value
        /// </summary>
        /// <param name="value">Value to add</param>
        public void Add(string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            _values.Add(value);
        }

        #endregion

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        //public override string ToString()
        //{
        //    //return Name + ": " + string.Join(", ", Values);
        //    return string.Empty;
        //}
    }
}