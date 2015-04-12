using System;
using System.Net;

using MicroServer.Utilities;
using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc
{
    /// <summary>
    /// Sends an Http error status to the client.
    /// </summary>
    public class HttpStatusCodeResult : ActionResult
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeResult"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public HttpStatusCodeResult(int statusCode)
            : this(statusCode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeResult"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        public HttpStatusCodeResult(HttpStatusCode statusCode)
            : this(statusCode, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeResult"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="statusDescription">The status description/reason.</param>
        public HttpStatusCodeResult(HttpStatusCode statusCode, string statusDescription)
            : this((int)statusCode, statusDescription)
        {        
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpStatusCodeResult"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="statusDescription">The status description/reason.</param>
        public HttpStatusCodeResult(int statusCode, string statusDescription)
        {
            if (statusCode <= 0)
            {
                throw new ArgumentOutOfRangeException("statusCode");
            } 

            if (StringUtility.IsNullOrEmpty(statusDescription))
            {
                throw new ArgumentException("statusDescription");
            }           

            StatusCode = statusCode;
            StatusDescription = statusDescription;
        }

        /// <summary>
        /// Gets Http status code.
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// Gets Http status description.
        /// </summary>
        public string StatusDescription { get; private set; }

        /// <summary>
        /// Execute the response result.
        /// </summary>
        /// <param name="context">HTTP controller context</param>
        /// <remarks>Invoked by <see cref="ControllerFactory"/> to process the response.</remarks>
        public override void ExecuteResult(IControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            context.HttpContext.Response.StatusCode = StatusCode;
            if (StatusDescription != null)
            {
                context.HttpContext.Response.StatusDescription = StatusDescription;
            }
        }
    }
}