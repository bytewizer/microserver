using System;
using System.Collections;
using System.Reflection;

namespace Bytewizer.TinyCLR.Http.Mvc.Stubble
{
    public class ViewData
    {
        private readonly Hashtable _data = new Hashtable();
        
        private Hashtable _children = null; //private Dictionary<string, ViewChild> children = null;

        public Hashtable Fields = new Hashtable(); //public Dictionary<string, int[]> Fields = new Dictionary<string, int[]>();

        public ViewChild Child(string id)
        {
            if (_children == null)
            {
                _children = new Hashtable(); //children = new Dictionary<string, ViewChild>();
            }
            if (!_children.Contains(id))
            {
                _children.Add(id, new ViewChild(this, id));
            }
            return (ViewChild)_children[id];
        }

        public void Bind(object obj, string root = "")
        {
            if (obj != null)
            {
                foreach (MethodInfo method in obj.GetType().GetMethods())
                {
                    if (!method.IsPublic)
                        continue;

                    if (method.Name.IndexOf("get_") == 0)
                    {
                        object val = method.Invoke(obj, null);

                        var name = (root != "" ? root + "." : "") + method.Name.TrimStart(new char[] { 'g', 'e', 't', '_' }).ToLower();
                        if (val == null)
                        {
                            _data[name] = "";
                        }
                        else if (val is string || val is int || val is long || val is double || val is decimal || val is short)
                        {
                            //add property value to dictionary
                            _data[name] = val.ToString();
                        }
                        else if (val is bool)
                        {
                            _data[name] = (bool)val == true ? "1" : "0";
                        }
                        else if (val is DateTime)
                        {
                            _data[name] = ((DateTime)val).ToString() + " " + ((DateTime)val).ToString();
                        }
                        else if (val is object)
                        {
                            //recurse child object for properties
                            Bind(val, name);
                        }
                    }
                }
            }
        }

        public void Show(string blockKey)
        {
            this [blockKey.ToLower(), true] = true;
        }

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
