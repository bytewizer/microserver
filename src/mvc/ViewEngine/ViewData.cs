using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.ViewEngine
{
    public class ViewData
    {
        private IDictionary _data = new Hashtable();

        public string this[string key]
        {
            get
            {
                return (string)_data[key];
            }
            set
            {
                _data[key] = value;
            }
        }

        public bool this[string key, bool isBool]
        {
            get
            {
                if ((string)_data[key] == "True")
                {
                    return true;
                }
                return false;
            }

            set
            {
                if (value)
                {
                    _data[key] = "True";
                }
                else
                {
                    _data[key] = "False";
                }
            }
        }

        public ICollection Keys => _data.Keys;

        public ICollection Values => _data.Values;

        public int Count => _data.Count;

        public bool IsReadOnly => false;

        public void Add(string key, string value)
        {
            _data.Add(key, value);
        }

        public void Add(string key, bool value)
        {
            _data.Add(key, value.ToString());
        }

        //public void Add(KeyValuePair<string, string> item)
        //{
        //    _data.Add(item.Key, item.Value);
        //}

        public void Clear()
        {
            _data.Clear();
        }

        public bool Contains(DictionaryEntry item)
        {
            return _data.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return _data.Contains(key);
        }

        public void CopyTo(DictionaryEntry[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            return _data.GetEnumerator();
        }

        public void Remove(string key)
        {
            _data.Remove(key);
        }

        public void Remove(DictionaryEntry item)
        {
            if (_data.Contains(item))
            {
                _data.Remove(item.Key);
            }
        }

        //public bool TryGetValue(string key, out string value)
        //{
        //    return _data.TryGetValue(key, out value);
        //}

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return _data.GetEnumerator();
        //}
    }
}
