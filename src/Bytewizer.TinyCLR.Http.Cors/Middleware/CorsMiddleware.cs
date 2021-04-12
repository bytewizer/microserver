using System;
using System.Collections;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// A middleware for generating cross-origin resource sharing (CORS) control.
    /// </summary>
    public class CorsMiddleware : Middleware
    {
        private readonly CorsOptions _options;
        private readonly ArrayList _validOrigins;
        private readonly ArrayList _validMethods;

        /// <summary>
        /// A string meaning "All" in CORS headers.
        /// </summary>
        public const string All = "*";

        /// <summary>
        /// Initializes a default instance of the <see cref="CorsMiddleware"/> class.
        /// </summary>
        public CorsMiddleware()
        {
            _options = new CorsOptions();
            _validOrigins = new ArrayList();
            _validMethods = new ArrayList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CorsMiddleware"/> class.
        /// </summary>
        public CorsMiddleware(CorsOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            _options = options;
            _validOrigins = ParsingHelper.SplitByComma(_options.Origins);
            _validMethods = ParsingHelper.SplitByComma(_options.Methods);
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            var isOptionsRequest = context.Request.Method == HttpMethods.Options;

            // If we allow all we don't need to filter
            if (_options.Origins == All && _options.Headers == All && _options.Methods == All)
            {
                context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = All;

                if (isOptionsRequest)
                {
                    ValidateHttpOptions(context);
                    context.Response.StatusCode = StatusCodes.Status204NoContent;
                    return;
                }
            }
            else
            {
                var currentOrigin = context.Request.Headers[HeaderNames.Origin];
                if (string.IsNullOrEmpty(currentOrigin) || _options.Origins == All)
                {
                    // fall through to next(context)
                }
                else
                {
                    if (_validOrigins.Contains(currentOrigin.ToUpper()))
                    {
                        context.Response.Headers[HeaderNames.AccessControlAllowOrigin] = currentOrigin;

                        if (isOptionsRequest)
                        {
                            ValidateHttpOptions(context);
                            context.Response.StatusCode = StatusCodes.Status204NoContent;
                            return;
                        }
                    }
                }
            }

            next(context);
        }

        private void ValidateHttpOptions(HttpContext context)
        {
            var requestHeadersHeader = context.Request.Headers[HeaderNames.AccessControlRequestHeaders];
            if (!string.IsNullOrEmpty(requestHeadersHeader))
            {
                // TODO: Remove unwanted headers from request
                context.Response.Headers[HeaderNames.AccessControlAllowHeaders] = requestHeadersHeader;
            }

            var requestMethodHeader = context.Request.Headers[HeaderNames.AccessControlRequestMethod];
            if (string.IsNullOrEmpty(requestMethodHeader))
            {
                return;
            }

            var currentMethods = ParsingHelper.SplitByComma(requestMethodHeader);
            if (_options.Methods != All && !ValidateMethods(currentMethods))  //!currentMethods.Any(_validMethods.Contains))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            context.Response.Headers[HeaderNames.AccessControlAllowMethods] = requestMethodHeader;
        }

        private bool ValidateMethods(ArrayList currentMethods)
        {
            foreach (var method in currentMethods)
            {
                if (_validMethods.Contains(method))
                {
                    return true;
                }
            }

            return false;
        }
    }
}