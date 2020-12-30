using System.Collections;

namespace Bytewizer.TinyCLR.Stubble
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ViewDictionary : Hashtable
    {
        private readonly ViewData _parent;
        private readonly string _id;
        
        public ViewDictionary(ViewData parent, string id)
        {
            _parent = parent;
            _id = id;
        }

        public string this[string key]
        {
            get
            {
                return _parent[_id + "-" + key];
            }
            set
            {
                _parent[_id + "-" + key] = value;
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}