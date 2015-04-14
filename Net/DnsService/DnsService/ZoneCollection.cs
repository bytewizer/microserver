using System;
using System.Collections;

namespace MicroServer.Net.Dns
{
    /// <summary>
    ///     Collection of parameters in a HTTP header.
    /// </summary>
    public class ZoneCollection : IEnumerable
    {
        private readonly Hashtable _items = new Hashtable();

        /// <summary>
        ///     Add a resource record.
        /// </summary>
        public void Add(ResourceRecord record)
        {
            if (record == null) throw new ArgumentNullException("record");

            string key = string.Concat(record.Domain, "|", record.Type.ToString(), "|", record.Class);
            _items.Add(key, record);
        }

        /// <summary>
        ///     Checks if the specified resource record exists
        /// </summary>
        /// <param name="name">The resource record.</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>;</returns>
        public bool Exists(ResourceRecord record)
        {
            if (record == null) throw new ArgumentNullException("item");

            string key = string.Concat(record.Domain, "|", record.Type.ToString(), "|", record.Class);
            return _items.Contains(key);
        }

        /// <summary>
        ///     Get a resource record.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public ResourceRecord Get(ResourceRecord record)
        {
            string key = string.Concat(record.Domain, "|", record.Type.ToString(), "|", record.Class);
            if (_items.Contains(key))
            {
                return (ResourceRecord)_items[key];
            }
            return null;
        }

        /// <summary>
        ///     Get a resource record
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public Answer Get(Question record)
        {
            string key = string.Concat(record.Domain, "|", record.Type.ToString(), "|", record.Class);
            if (_items.Contains(key))
            {
                return (Answer)_items[key];
            }
            return null;
        }

        /// <summary>
        ///     Gets number of resource records.
        /// </summary>
        public int Count
        {
            get { return _items.Count; }
        }

        /// <summary>
        ///     Remove all resource records.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}