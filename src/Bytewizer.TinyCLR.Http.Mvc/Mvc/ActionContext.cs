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
        /// <remarks>
        /// The default constructor is provided for unit test purposes only.
        /// </remarks>
        public ActionContext() 
        {
        }

        /// <summary>
        /// Creates a new <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="actionContext">The <see cref="ActionContext"/> to copy.</param>
        public ActionContext(ActionContext actionContext)
            : this(
                actionContext.HttpContext,
                actionContext.ActionDescriptor)
            {
        }

        /// <summary>
        /// Creates a new <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="httpContext">The <see cref="HttpContext"/> for the current request.</param>
        /// /// <param name="actionDescriptor">The <see cref="ActionDescriptor"/> for the selected action.</param>
        public ActionContext(
            HttpContext httpContext, 
            ActionDescriptor actionDescriptor)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (actionDescriptor == null)
            {
                throw new ArgumentNullException(nameof(actionDescriptor));
            }

            HttpContext = httpContext;
            ActionDescriptor = actionDescriptor;
        }

        /// <summary>
        /// Gets or sets the <see cref="ActionDescriptor"/> for the selected action.
        /// </summary>
        /// <remarks>
        /// The property setter is provided for unit test purposes only.
        /// </remarks>
        public ActionDescriptor ActionDescriptor { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Http.HttpContext"/> for the current request.
        /// </summary>
        /// <remarks>
        /// The property setter is provided for unit test purposes only.
        /// </remarks>
        public HttpContext HttpContext { get; set; }
    }
}