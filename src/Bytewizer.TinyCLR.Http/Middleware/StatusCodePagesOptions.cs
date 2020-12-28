using Bytewizer.TinyCLR.Http.Internal;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Options for configuring the <see cref="StatusCodePagesMiddleware"/>.
    /// </summary>
    public class StatusCodePagesOptions
    {
        /// <summary>
        /// Create an instance with the default options settings.
        /// </summary>
        public StatusCodePagesOptions()
        {
            Handle = context =>
            {
                var statusCode = context.Response.StatusCode;

                var body = BuildResponseBody(statusCode);

                context.Response.Write(body);
            };
        }

        private string BuildResponseBody(int httpStatusCode)
        {
            var reasonPhrase = HttpReasonPhrase.Get(httpStatusCode);
            var separator = string.IsNullOrEmpty(reasonPhrase) ? "" : "; ";

            return string.Format($"Status Code: {httpStatusCode}{separator}{reasonPhrase}");
        }

        /// <summary>
        /// The <see cref="RequestDelegate" /> that will handle the exception. 
        /// </summary>
        public RequestDelegate Handle { get; set; } = delegate { };
    }
}