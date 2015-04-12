using System;

using MicroServer.Net.Http.Mvc.ActionResults;

namespace MicroServer.Net.Http.Mvc
{
    public class ExceptionContext
    {
        public ExceptionContext(Exception exception)
        {
            Exception = exception;
        }

        public Exception Exception { get; private set; }
        public bool ExceptionHandled { get; set; }
        public ActionResult Result { get; set; }
    }
}
