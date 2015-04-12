using System;
using System.Collections;

using MicroServer.Net.Http.Mvc.Views;

namespace MicroServer.Net.Http.Mvc
{
    /// <summary>
    /// View data implementation.
    /// </summary>
    public class ViewData : IViewData
    {
        private readonly ViewDataCollection _parameters = new ViewDataCollection();

        /// <summary>
        /// Get all parameters for the view.
        /// </summary>
        public ViewDataCollection Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Remove everything from the dictionary.
        /// </summary>
        public void Clear()
        {
            _parameters.Clear();
        }

        /// <summary>
        /// Gets dictionary count.
        /// </summary>
        public int Count()
        {
            return _parameters.Count();
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
            int hashCode = 0;
            foreach (DictionaryEntry parameter in _parameters)
            {
                if (parameter.Value == null)
                    continue;
                hashCode += parameter.Value.GetType().GetHashCode();
            }
            return hashCode;
        }

        #region IViewData Members

        /// <summary>
        /// Checks if view data contains a parameter.
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>
        public bool Contains(string key)
        {
            return _parameters.Contains(key);
        }

        /// <summary>
        /// Gets or sets a view data parameter.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get { return _parameters[name]; }
            set { _parameters[name] = value; }
        }

        /// <summary>
        /// Try get a value from the dictionary.
        /// </summary>
        /// <param name="name">Name of parameter</param>
        /// <param name="value">Value if any.</param>
        /// <returns>
        /// 	<c>true</c> if found; otherwise <c>false</c>.
        /// </returns>
        public bool TryGetValue(string name, out object value)
        {
            return _parameters.TryGetValue(name, out value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator GetEnumerator()
        {
            return _parameters.GetEnumerator();
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
    }
}