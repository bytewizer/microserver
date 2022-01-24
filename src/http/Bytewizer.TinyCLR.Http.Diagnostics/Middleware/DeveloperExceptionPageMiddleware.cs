using System;
using System.Text;

using Bytewizer.TinyCLR.Logging;
using Bytewizer.TinyCLR.Http.Header;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Captures exceptions from the pipeline and generates error responses.
    /// </summary>
    public class DeveloperExceptionPageMiddleware : Middleware
    {
        private readonly ILogger _logger;
        private readonly DeveloperExceptionPageOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeveloperExceptionPageMiddleware"/> class.
        /// </summary>
        public DeveloperExceptionPageMiddleware()
            : this(NullLoggerFactory.Instance, new DeveloperExceptionPageOptions()) 
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeveloperExceptionPageMiddleware"/> class.
        /// </summary>
        /// <param name="loggerFactory">The factory used to create loggers.</param>
        /// <param name="options">The options for configuring the middleware.</param>
        public DeveloperExceptionPageMiddleware(ILoggerFactory loggerFactory, DeveloperExceptionPageOptions options)
        {
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _logger = loggerFactory.CreateLogger("Bytewizer.TinyCLR.Http");
            _options = options;
        }

        /// <inheritdoc/>
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

                try 
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.Write(sb.ToString());
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}