using System;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// Context object for execution of action which has been selected as part of an HTTP request.
    /// </summary>
    public class ActionContext
    {
        /// <summary>
        /// Creates an empty <see cref="ActionContext"/>.
        /// </summary>
        public ActionContext() {}

        /// <summary>
        /// Creates a new <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="actionContext">The <see cref="ActionContext"/> to copy.</param>
        public ActionContext(ActionContext actionContext)
            : this(actionContext?.HttpContext)
        {
        }

        /// <summary>
        /// Creates a new <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
        public ActionContext(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }
            HttpContext = httpContext;
        }

        /// <summary>
        /// Gets or sets the <see cref="HttpContext"/> for the current request.
        /// </summary>
        /// <remarks>
        /// The property setter is provided for unit test purposes only.
        /// </remarks>
        public HttpContext HttpContext
        {
            get; set;
        }
    }
}