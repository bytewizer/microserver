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

            _httpMessage.Clear();
        }
    }
}

//DebugHeaders(context);
//DebugBody(context);
//private void DebugHeaders(HttpContext context)
//{
//    Debug.WriteLine(string.Empty);
//    foreach (HeaderValue header in context.Request.Headers)
//    {
//        Debug.WriteLine($"{header.Key}: {header.Value}");
//    }
//}

//private void DebugBody(HttpContext context)
//{
//    if (context.Request.Body.Length > 0)
//    {
//        Debug.WriteLine(string.Empty);
//        Debug.WriteLine("---------- Body Content ----------");
//        var reader = new StreamReader(context.Request.Body);
//        while (reader.Peek() != -1)
//        {
//            var line = reader.ReadLine();
//            Debug.WriteLine(line);
//        }
//        context.Request.Body.Position = 0;
//    }
//}