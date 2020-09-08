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

        public QueryValue[] GetValues()
        {            
            IEnumerator enumerator = _request.Query.GetEnumerator();
            QueryValue[] list = new QueryValue[_request.Query.Count];
            int x = 0;
            while (enumerator.MoveNext())
            {
                list[x] = (QueryValue)enumerator.Current;
                x++;
            }
            return list;
        }

        /// <summary>
        /// Find all parameters which starts with the specified argument.
        /// </summary>
        /// <param name="prefix">Beginning of the field name</param>
        /// <returns>
        /// All matching parameters.
        /// </returns>
        public IEnumerable Find(string prefix)
        {
            return null;
        }
    }
}
