using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.Stubble
{
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
}
