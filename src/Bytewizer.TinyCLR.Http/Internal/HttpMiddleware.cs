using System.Diagnostics;

namespace Bytewizer.TinyCLR.Http.Internal
{
    internal class HttpMiddleware : Middleware
    {
        private readonly HttpMessage _httpMessage;

        public HttpMiddleware()
        {
            _httpMessage = new HttpMessage();
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            _httpMessage.Decode(context);

            next(context);

           _httpMessage.Encode(context);

        }
    }
}