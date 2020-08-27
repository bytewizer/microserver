using System;
using System.Collections;
using System.Text;

using Bytewizer.TinyCLR.Http.Mvc.Filters;
using Bytewizer.TinyCLR.Http.Mvc.Middleware;

namespace Bytewizer.TinyCLR.Http.Mvc
{
    public interface IController
    {
        void OnActionExecuting(ActionExecutingContext context);
        void OnActionExecuted(ActionExecutedContext context);
        void OnException(ExceptionContext context);
    }
}
