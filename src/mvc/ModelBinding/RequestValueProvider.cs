using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{
    public class RequestValueProvider : IValueProvider
    {
        private readonly HttpRequest _request;

        public RequestValueProvider(HttpRequest request)
        {
            _request = request;
        }

        public string Get(string name)
        {
            return _request.Query[name];
        }

        public DictionaryEntry[] GetValues()
        {
            IEnumerator enumerator = _request.Query.GetEnumerator();
            DictionaryEntry[] list = new DictionaryEntry[_request.Query.Count];
            int x = 0;
            while (enumerator.MoveNext())
            {
                list[x] = (DictionaryEntry)enumerator.Current;
                x++;
            }
            return list;
        }
    }
}
