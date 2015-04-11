using System;
using System.Collections;

namespace MicroServer.Net.Http
{
    /// <summary>
    /// Uses a Hashtable to store all items
    /// </summary>
    public class MemoryItemStorage : IItemStorage
    {
        private readonly Hashtable _dictionary = new Hashtable();

        #region IItemStorage Members

        /// <summary>
        /// Get or set an item
        /// </summary>
        /// <param name="name">Item name</param>
        /// <returns>Item if found; otherwise <c>null</c>.</returns>
        public object this[string name]
        {
            get
            {
                if (name == null)
                    throw new ArgumentNullException("name");

                if (_dictionary.Contains(name))
                {
                    return (object)_dictionary[name];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (name == null)
                    throw new ArgumentNullException("name");

                if (value == null & !_dictionary.Contains(name))
                {
                    _dictionary.Remove(name);
                }
                else
                {
                
                }
            }
        }

        #endregion
    }
}