using System;
using System.Text;

using Bytewizer.TinyCLR.Http.Header;
using Bytewizer.TinyCLR.Sockets;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Captures exceptions from the pipeline and generates error responses.
    /// </summary>
    public class DeveloperExceptionPageMiddleware : Middleware
    {
        private readonly DeveloperExceptionPageOptions _options;

        public DeveloperExceptionPageMiddleware()
        {
            _options = new DeveloperExceptionPageOptions();
        }

        public DeveloperExceptionPageMiddleware(DeveloperExceptionPageOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
        }

        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            try 
            {
                next(context);
            }
            catch(Exception ex)
            {
                var sb = new StringBuilder();

                sb.AppendLine("EXCEPTION");
                sb.AppendLine("=========");
                sb.AppendLine(ex.ToString());
                sb.AppendLine();
                if (_options.DisplayStackTrace == true)
                {
                    sb.AppendLine(ex.StackTrace.ToString());
                }
                sb.AppendLine();
                sb.AppendLine("HEADERS");
                sb.AppendLine("=======");
                foreach (HeaderValue pair in context.Request.Headers)
                {
                    sb.AppendLine($"{pair.Key}: {pair.Value}");
                }

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.Write(sb.ToString(), "text/plain");
            }
        }
    }
}