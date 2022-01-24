using System;
using System.Reflection;

using Bytewizer.TinyCLR.Http.Mvc.Filters;
using Bytewizer.TinyCLR.Http.Mvc.ModelBinding;

namespace Bytewizer.TinyCLR.Http.Mvc.Middleware
{
    internal class ControllerDelegateFactory
    {
        private readonly ModelMapper _modelMapper;

        public ControllerDelegateFactory()
        {
            _modelMapper = new ModelMapper();
        }

        public RequestDelegate CreateRequestDelegate(Type controllerType, MethodInfo MethodAction)
        {            
            return (context) =>
            {
                var controller = (Controller)Activator.CreateInstance(controllerType);
                if (controller == null)
                {
                    throw new InvalidOperationException(nameof(controller));
                }

                var descriptor = new ControllerActionDescriptor()
                {
                    ActionName = MethodAction.Name,
                    ControllerName = controllerType.Name,
                    MethodInfo = MethodAction
                };

                var actionContext = new ActionContext(context, descriptor);
                var controllerContext = new ControllerContext(actionContext);
                controller.ControllerContext = controllerContext;

                try
                {
                    // Invoke OnActionExecuting()
                    var actionExecutingContext = new ActionExecutingContext(controllerContext, controllerContext);
                    var actionExecuting = InvokeActionExecuting(controller, actionExecutingContext);
                    if (actionExecuting != null)
                    {
                        return; 
                    }

                    // Invoke controller action ExecutedResult()
                    var args = _modelMapper.Bind(controllerContext);
                    var result = (ActionResult)MethodAction.Invoke(controller, args);
                    result.ExecuteResult(controllerContext);

                    // Invoke OnActionExecuted()
                    var actionExecutedContext = new ActionExecutedContext(actionContext, controllerContext);
                    var actionExecuted = InvokeActionExecuted(controller, actionExecutedContext);
                    if (actionExecuting != null)
                    {
                        return; 
                    }

                    return; 
                }
                catch (Exception ex)
                {
                    // Invoke OnExecption()
                    var exceptionContext = new ExceptionContext(controllerContext, ex);
                    var exception = InvokeException(controller, actionContext, exceptionContext);
                    if (exception != null)
                    {
                        return; 
                    }

                    return; 
                }
            };
        }

        private static IActionResult InvokeActionExecuting(Controller controller, ActionExecutingContext actionExecutingContext)
        {
            controller.OnActionExecuting(actionExecutingContext);

            if (actionExecutingContext.Result == null)
            {
                return null;
            }

            actionExecutingContext.Result.ExecuteResult(actionExecutingContext);
            return actionExecutingContext.Result;
        }

        private static IActionResult InvokeActionExecuted(Controller controller, ActionExecutedContext actionExecutedContext)
        {
            controller.OnActionExecuted(actionExecutedContext);

            if (actionExecutedContext.Result == null)
            {
                return null;
            }

            if (actionExecutedContext.Canceled)
            {
                actionExecutedContext.Result.ExecuteResult(actionExecutedContext);
            }

            if (actionExecutedContext.ExceptionHandled)
            {
                return null;
            }

            if (actionExecutedContext.Exception != null)
            {
                throw actionExecutedContext.Exception;
            }

            return actionExecutedContext.Result;
        }

        private static IActionResult InvokeException(Controller controller, ActionContext actionContext, ExceptionContext exceptionContext)
        {
            controller.OnException(exceptionContext);

            if (exceptionContext.Result == null)
            {
                return null;
            }

            if (exceptionContext.Canceled)
            {
                exceptionContext.Result.ExecuteResult(actionContext);
            }

            if (exceptionContext.ExceptionHandled)
            {
                return null;
            }

            if (exceptionContext.Exception != null)
            {
                throw exceptionContext.Exception;
            }
         
            return exceptionContext.Result;
        }
    }
}