using System.Collections;

using Bytewizer.TinyCLR.Http.Query;

namespace Bytewizer.TinyCLR.Http.Mvc.ModelBinding
{

    /// <summary>
    /// An <see cref="IValueProvider"/> for binding simple data types.
    /// </summary>
    public class RequestValueProvider : IValueProvider // Rename IModelBinderProvider
    {
        private readonly HttpRequest _request;

        /// <summary>
        /// Initializes a new instance of <see cref="RequestValueProvider"/>.
        /// </summary>
        public RequestValueProvider(HttpRequest request)
        {
            _request = request;
        }

        /// <inheritdoc />
        public string Get(string name)
        {
            return _request.Query[name];
        }

        /// <inheritdoc />
        public ICollection GetKeys()
        {
            return _request.Query.Keys;
        }

        /// <inheritdoc />
        public ICollection GetValues()
        {
            return _request.Query.Values;
        }

        /// <inheritdoc />
        public IEnumerable Find(string prefix)
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
    }
}
