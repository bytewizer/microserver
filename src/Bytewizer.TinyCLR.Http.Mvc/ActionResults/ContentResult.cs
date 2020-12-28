using System;
using System.Text;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// An action result which sends content to the client. 
    /// </summary>
    public class ContentResult : ActionResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResult"/> class.
        /// </summary>
        public ContentResult(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Gets or set the content representing the body of the response.
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets the Content-Type header for the response.
        /// </summary>
        public string ContentType { get; set; } = "text/html; charset=UTF-8";

        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; } = StatusCodes.Status200OK;

        /// <inheritdoc />
        public override void ExecuteResult(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (ContentType == null)
            {
                throw new ArgumentNullException(nameof(ContentType));
            }

            context.HttpContext.Response.Write(Content, ContentType, StatusCode, Encoding.UTF8);
        }
    }
}