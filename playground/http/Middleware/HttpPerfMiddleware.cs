using System;
using System.Text;
using Bytewizer.TinyCLR.Http;

namespace Bytewizer.Playground.Http
{
    public class HttpPerfMiddleware : Middleware
    {
        private static readonly Random _random = new Random();

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            if (context.Request.Method == HttpMethods.Get
                && context.Request.Path == "/")
            {
                string response = $"<doctype !html><html><head><title>HttpPerf</title></head><body>{GenerateBody()}</body></html>";

                // send the response to browser
                context.Response.Write(response);
            }

            next(context);
        }
        public static string GenerateBody()
        {
            var length = _random.Next(10000);       
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[_random.Next(characters.Length)]);
            }
            return result.ToString();
        }
    }
}