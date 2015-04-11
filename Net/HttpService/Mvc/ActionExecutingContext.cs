using System;

namespace MicroServer.Net.Http.Mvc
{
    public class ActionExecutingContext
    {
        public ActionExecutingContext(IControllerContext context)
        {
            HttpContext = context.HttpContext;
        }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public IHttpContext HttpContext { get; private set;}

        public ActionResult Result { get; set; }
    }
}
