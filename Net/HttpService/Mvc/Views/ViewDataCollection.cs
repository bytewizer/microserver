using System;
using System.Collections;

namespace MicroServer.Net.Http.Mvc.Views
{
    /// <summary>
    /// Collection containing view data information
    /// </summary>
    public class ViewDataCollection : IEnumerable
    {
        private readonly Hashtable _items = new Hashtable();

        /// <summary>
        /// Gets or sets an item in the view data.
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns>Item if found; otherwise <c>null</c>.</returns>
        public object this[string name]
        {
            get
            {
                if (this.Contains(name))
                {
                    return _items[name];
                }
                else
                {
                    return null;
                }
            }
            set { _items[name] = value; }
        }

        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="value">Item</param>
        /// <remarks>
        /// Another item with the same name must not exist in the collection.
        /// </remarks>
        public void Add(string name, object value)
        {
            _items.Add(name, value);
        }

        /// <summary>
        /// Try get an item
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="value">Item</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>
        public bool TryGetValue(string name, out object value)
        {
            value = null;
            if (this.Contains(name))
            {
                value = _items[name];
                return true;
            }
            return false;
        }

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Remove everything from the dictionary.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// get dictionary count.
        /// </summary>
        public int Count()
        {
            return _items.Count;
        }

        /// <summary>
        /// Checks if view data contains a parameter.
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>        
        public bool Contains(string key)
        {
            return _items.Contains(key);
        }
    }
}