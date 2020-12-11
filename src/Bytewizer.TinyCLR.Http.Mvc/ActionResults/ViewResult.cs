using System;

using Bytewizer.TinyCLR.Stubble;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// Represents an <see cref="ActionResult"/> that renders a view to the response.
    /// </summary>
    public class ViewResult : ActionResult
    {
        /// <summary>
        /// Gets or sets the name or path of the view that is rendered to the response.
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        /// Gets or sets the section of the view that is rendered to the response.
        /// </summary>
        public string ViewSection { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ViewData"/> for this result.
        /// </summary>
        public ViewData ViewData { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IViewEngine"/> used to locate views.
        /// </summary>
        public ViewEngine ViewEngine { get; set; }

        /// <summary>
        /// Gets or sets the Content-Type header for this result.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            string content = string.Empty;
            
            if (ViewEngine == null)
            {             
                try
                {
                    ViewEngine = new ViewEngine(ViewName, ViewSection);
                    content = ViewEngine.Render(ViewData);
                    context.HttpContext.Response.ContentType = "text/html";
                    context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                }
                catch
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                }
            }
            else
            {
                content = ViewEngine.Render(ViewData);
            }

            context.HttpContext.Response.Write(content, "text/html");
        }
    }
}
