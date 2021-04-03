using System;

namespace Bytewizer.TinyCLR.Http
{
    /// <summary>
    /// Represents a middleware that extracts the specified path base from request path and postpend it to the request path base.
    /// </summary>
    public class UsePathBaseMiddleware : Middleware
    {
        private readonly string _pathBase;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsePathBaseMiddleware"/> class.
        /// </summary>
        /// <param name="pathBase">The path base to extract.</param>
        public UsePathBaseMiddleware(string pathBase)
        {
            if (string.IsNullOrEmpty(pathBase))
            {
                throw new ArgumentException($"{nameof(pathBase)} cannot be null or empty.");
            }

            _pathBase = pathBase;
        }

        /// <inheritdoc/>
        protected override void Invoke(HttpContext context, RequestDelegate next)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            };

            if (context.Request.Path.StartsWith(_pathBase))
            {
                var originalPath = context.Request.Path;
                var originalPathBase = context.Request.PathBase;

                context.Request.Path = context.Request.Path.Substring(_pathBase.Length);
                context.Request.PathBase = _pathBase.TrimStart('/').TrimEnd('/');

                try
                {
                    next(context);
                }
                finally
                {
                    context.Request.Path = originalPath;
                    context.Request.PathBase = originalPathBase;
                }
            }
            else
            {
                next(context);
            }
        }
    }


    //    var originalPath = context.Request.Path;
    //context.Request.Path = $"/{_pathBase}{originalPath}";

    //next(context);
}
