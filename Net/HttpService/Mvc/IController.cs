using System;
using System.Collections;
using System.Text;

using MicroServer.Net.Http.Mvc.Controllers;

namespace MicroServer.Net.Http.Mvc
{
    public interface IController
    {
        void OnActionExecuting(ActionExecutingContext context);
        void OnActionExecuted(ActionExecutedContext context);
        void OnException(ExceptionContext context);
    }
}
