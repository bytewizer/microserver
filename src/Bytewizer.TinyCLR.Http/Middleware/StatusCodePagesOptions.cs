namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Options for the <see cref="StatusCodePagesMiddleware"/>.
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

                context.Response.ContentType = "text/plain";
                context.Response.Write(body);
            };
        }

        private string BuildResponseBody(int httpStatusCode)
        {
            var reasonPhrase = HttpReasonPhrase.Get(httpStatusCode);
            var separator = string.IsNullOrEmpty(reasonPhrase) ? "" : "; ";

            return string.Format($"Status Code: {httpStatusCode}{separator}{reasonPhrase}");
        }

        public HandleDelegate Handle { get; set; } = delegate { };
    }
}