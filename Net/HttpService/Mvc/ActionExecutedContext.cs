using System;
using MicroServer.Net.Http.Mvc.ActionResults;

namespace MicroServer.Net.Http.Mvc
{
    public class ActionExecutedContext
    {
        private ActionResult _result;

        public ActionExecutedContext(IControllerContext controllerContext, bool canceled, Exception exception)
        {
            HttpContext = controllerContext;
            Canceled = canceled;
            Exception = exception;
        }

        /// <summary>
        /// Gets the request context.
        /// </summary>
        /// <value>The request context.</value>
        public IControllerContext HttpContext { get; private set; }

        public virtual bool Canceled
        {
            get;
            set;
        }

        public virtual Exception Exception
        {
            get;
            set;
        }

        public bool ExceptionHandled
        {
            get;
            set;
        }

        public ActionResult Result
        {
            get
            {
                return _result ?? EmptyResult.Instance;
            }
            set
            {
                _result = value;
            }
        }
    }
}
