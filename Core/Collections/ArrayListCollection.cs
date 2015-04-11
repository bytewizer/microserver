using System;
using System.Collections;

namespace MicroServer.Collections
{
    public class ArrayListCollection : IList
    {  
        protected ArrayList _arrayList;

        #region IList

        public int Add(object value)
        {
            return this._arrayList.Add(value);
        }

        public void Clear()
        {
            this._arrayList.Clear();
        }

        public bool Contains(object value)
        {
            return this._arrayList.Contains(value);
        }

        public int IndexOf(object value)
        {
            return this._arrayList.IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            this._arrayList.Insert(index, value);
        }

        public bool IsFixedSize
        {
            get { return this._arrayList.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return this._arrayList.IsReadOnly; }
        }

        public void Remove(object value)
        {
            this._arrayList.Remove(value);
        }

        public void RemoveAt(int index)
        {
            this._arrayList.RemoveAt(index);
        }

        public object this[int index]
        {
            get
            {
                return (object)this._arrayList[index];
            }
            set
            {
                this._arrayList.Insert(index, value);
            }
        }

        public void CopyTo(Array array, int index)
        {
            this._arrayList.CopyTo(array, index);
        }

        public int Count
        {
            get { return this._arrayList.Count; }
        }

        public bool IsSynchronized
        {
            get { return this._arrayList.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return this._arrayList.SyncRoot; }
        }

        public IEnumerator GetEnumerator()
        {
            return this._arrayList.GetEnumerator();
        }

        #endregion IList
    }
}
