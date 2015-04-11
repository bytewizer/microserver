using System;
using System.Reflection;

using MicroServer.Extensions;
using MicroServer.Net.Http.Mvc.Controllers;


namespace MicroServer.Net.Http.Mvc
{
    /// <summary>
    /// MVC controller.
    /// </summary>
    public abstract class Controller : IController
    {
        private IControllerContext _context;


        /// <summary>
        /// Initializes a new instance of the <see cref="Controller"/> class.
        /// </summary>
        protected Controller()
        {

        }

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
        public IHttpRequest Request { get { return _context.HttpContext.Request; } }

        /// <summary>
        /// Gets HTTP response.
        /// </summary>
        public IHttpResponse Response { get { return _context.HttpContext.Response; } }

        /// <summary>
        /// Gets request parameters
        /// </summary>
        /// <remarks>A merged collection of Uri and Form parameters</remarks>
        //public IParameterCollection Parameters
        //{
        //    get { return _context.HttpContext.Request.Parameters; }
        //}

        /// <summary>
        /// Gets form parameters
        /// </summary>
        /// <remarks>Form parameters</remarks>
        public IParameterCollection Form
        {
            get { return _context.HttpContext.Request.Form; }
        }

        /// <summary>
        /// Invoke a method in another controller.
        /// </summary>
        /// <param name="controllerName">Name of controller.</param>
        /// <param name="action">Action to invoke</param>
        /// <param name="arguments">Parameters used by the controller.</param>
        /// <returns></returns>
        //public static object Invoke(string controllerName, string actionName, params object[] arguments)
        //{
        //    return HttpService.Current.Invoke(controllerName, actionName, arguments);

        //}

        /// <summary>
        /// Gets or sets id
        /// </summary>
        public string Id
        {
            get
            {
                var prefixLength = _context.ControllerUri.Length + ActionName.Length + 1; //1= last slash

                if (_context.Uri.AbsolutePath.Length <= prefixLength)
                    return string.Empty;
                
                var myUri = _context.Uri.AbsolutePath.Substring(prefixLength);
                int pos = myUri.IndexOf("/");

                return pos == -1 ? myUri : myUri.Substring(0, pos);
            }
        }

        /// <summary>
        /// Sets controller context 
        /// </summary>
        /// <remarks>
        /// Context contains information about the current request.
        /// </remarks>
        internal void SetContext(IControllerContext context)
        {
            _context = context;
        }

        public virtual void OnToken(string token, ref string replacement)
        {

        }

        public virtual void OnActionExecuting(ActionExecutingContext context)
        {

        }
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public virtual void OnException(ExceptionContext context)
        {

        }

        public virtual ActionResult TriggerOnException(Exception ex)
        {
            return OnException(ex);
        }

        protected virtual ActionResult OnException(Exception ex)
        {
            return null;
        }

    }
}