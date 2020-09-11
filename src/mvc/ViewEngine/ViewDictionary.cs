using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.ViewEngine
{
    public class ViewDictionary : Hashtable
    {
        private readonly ViewModel _parent;
        private readonly string _id;
        
        public ViewDictionary(ViewModel parent, string id)
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
