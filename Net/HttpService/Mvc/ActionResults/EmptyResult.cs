using System;

using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc
{
    /// <summary>
    /// Sends a result that doesn't do anything (like a controller action returning null) to the client.
    /// </summary>
    public class EmptyResult : ActionResult
    {
        private static readonly EmptyResult _singleton = new EmptyResult();

        /// <summary>
        /// Gets singleton instance
        /// </summary>
        internal static EmptyResult Instance
        {
            get { return _singleton; }
        }

        /// <summary>
        /// Execute the response result.
        /// </summary>
        /// <param name="context">HTTP controller context</param>
        /// <remarks>Invoked by <see cref="ControllerFactory"/> to process the response.</remarks>
        public override void ExecuteResult(IControllerContext context)
        {
            context.HttpContext.Response.StatusCode = 204;
        }
    }
}
