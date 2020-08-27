using System;

using Bytewizer.TinyCLR.Http.Mvc.Filters;
using Bytewizer.TinyCLR.Http.Mvc.ActionResults;

using GHIElectronics.TinyCLR.Data.Json;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    /// <summary>
    /// MVC controller.
    /// </summary>
    public abstract class Controller : IController
    {
        private ControllerContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        protected Controller() { }
  
        /// <summary>
        /// Gets name of requested action.
        /// </summary>
        public string ActionName { get { return _context.ActionName; } }

        /// <summary>
        /// Gets controller uri
        /// </summary>
        /// <remarks>
        /// Can be "/controllerName/" or "/section/controllerName/" depending on the <see cref="ControllerUriAttribute"/>.
        /// </remarks>
        public string ControllerUri { get { return _context.ControllerUri; } }

        /// <summary>
        /// Gets or sets name of controller.
        /// </summary>
        /// <remarks>
        /// Can be "controllerName" or "section/controllerName" depending on the <see cref="ControllerUriAttribute"/>.
        /// </remarks>
        public string ControllerName { get { return _context.ControllerUri.TrimStart('/').TrimEnd('/'); } }

        /// <summary>
        /// Gets HTTP request
        /// </summary>
        public HttpRequest Request { get { return _context.HttpContext.Request; } }

        /// <summary>
        /// Gets HTTP response.
        /// </summary>
        public HttpResponse Response { get { return _context.HttpContext.Response; } }

        /// <summary>
        /// Sets controller context 
        /// </summary>
        /// <remarks>
        /// Context contains information about the current request.
        /// </remarks>
        internal void SetContext(ControllerContext context)
        {
            _context = context;
        }

        public virtual ContentResult ContentResult(string content, string contentType)
        {
            return new ContentResult
            {
                Content = content,
                ContentType = contentType
            };
        }

        public virtual EmptyResult EmptyResult()
        {
            return new EmptyResult();
        }

        public virtual JsonResult JsonResult(JObject jdom)
        {
            return new JsonResult(jdom);
        }

        public virtual RedirectResult RedirectResult(string url)
        {
            return new RedirectResult(url);
        }

        public virtual RedirectResult RedirectResult(string controllerName, string actionName)
        {
            return new RedirectResult(controllerName, actionName);
        }

        public virtual void OnActionExecuting(ActionExecutingContext context) { }

        public virtual void OnActionExecuted(ActionExecutedContext context) { }
        
        public virtual void OnException(ExceptionContext context) { }

        public virtual ActionResult TriggerOnException(Exception ex)
        {
            return null;
        }
    }
}