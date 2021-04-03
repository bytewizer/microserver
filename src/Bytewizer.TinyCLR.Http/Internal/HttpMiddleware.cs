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
            // TODO: Threading issues? Hangs When pushing 10+ session per second - Rethinking? 
            _httpMessage.Decode(context);

            next(context);

            _httpMessage.Encode(context);
        }
    }
}