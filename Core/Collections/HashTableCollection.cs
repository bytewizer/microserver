using System;
using System.Collections;

namespace MicroServer.Collections
{
    public class HashTableCollection : ICloneable, IDictionary, ICollection, IEnumerable
    {
        protected Hashtable _hashTable;

        #region ICloneable

        public object Clone()
        {
            return this._hashTable.Clone();
        }

        #endregion ICloneable

        #region IDictionary

        public void Add(object key, object value)
        {
           this._hashTable.Add(key,value);
        }

        public void Clear()
        {
            this._hashTable.Clear();
        }

        public bool Contains(object key)
        {
            return this._hashTable.Contains(key);
        }

        public bool IsFixedSize
        {
            get { return this._hashTable.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return this._hashTable.IsReadOnly; }
        }

        public ICollection Keys
        {
            get { return this._hashTable.Keys; }
        }

        public void Remove(object key)
        {
            this._hashTable.Remove(key);
        }

        public ICollection Values
        {
            get { return this._hashTable.Values; }
        }

        public object this[object key]
        {
            get
            {
                return (object)this._hashTable[key]; 
            }
            set
            {
                this._hashTable.Add(key, value); 
            }
        }

        public void CopyTo(Array array, int index)
        {
            this._hashTable.CopyTo(array, index);
        }

        public int Count
        {
            get { return this._hashTable.Count; }
        }

        public bool IsSynchronized
        {
            get { return this._hashTable.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return this._hashTable.SyncRoot; }
        }

        #endregion IDictionary

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this._hashTable.GetEnumerator();
        }

        #endregion IEnumerable
    }
}
