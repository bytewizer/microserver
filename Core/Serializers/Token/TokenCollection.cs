using System;
using System.Collections;


namespace MicroServer.Serializers.Token
{
    public class TokenCollection
    {
        private Hashtable _tokens;

        public TokenCollection()
        {
            _tokens = new Hashtable();
        }
        
        public void Add(string token, string replacement)
        {
            _tokens.Add(token, replacement);
        }

        public void Clear()
        {
            _tokens.Clear();
        }

        public bool Contains(string token)
        {
            return _tokens.Contains(token);
        }

        public bool IsFixedSize
        {
            get { return _tokens.IsFixedSize; }
        }

        public bool IsReadOnly
        {
            get { return _tokens.IsReadOnly; }
        }

        public ICollection Keys
        {
            get { return _tokens.Keys; }
        }

        public void Remove(string token)
        {
            _tokens.Remove(token);
        }

        public ICollection Values
        {
            get { return _tokens.Values; }
        }

        public string this[string token]
        {
            get
            {
                return (string)_tokens[token];
            }
            set
            {
                _tokens[token] = value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _tokens.Count; }
        }

        public bool IsSynchronized
        {
            get { return _tokens.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return _tokens.SyncRoot; }
        }

        public IEnumerator GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }
    }
}
