using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.Stubble
{
    [Serializable]
    public struct SerializedView
    {
        public ViewData Data;
        public Hashtable Fields; //public IDictionary<string, int[]> Fields;
        public IList Elements; //public List<ViewElement> Elements;
        public IList Partials; //public List<ViewPartial> Partials;
    }
}
