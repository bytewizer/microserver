using System;
using System.Reflection;

namespace Bytewizer.TinyCLR.Telnet
{
    internal class CommandDelegateFactory
    {
        public CommandDelegateFactory()
        {
        }

        public CommandDelegate CreateRequestDelegate(Type commandType, MethodInfo MethodAction)
        {
            return (context) =>
            {
                var controller = (Command)Activator.CreateInstance(commandType);
                if (controller == null)
                {
                    throw new InvalidOperationException(nameof(controller));
                }

                var descriptor = new CommandActionDescriptor()
                {
                    MethodInfo = MethodAction,
                    CommandName = commandType.Name,
                    ActionName = MethodAction.Name
                };

                var actionContext = new ActionContext(context, descriptor);
                var commandContext = new CommandContext(actionContext);
                controller.CommandContext = commandContext;

                try
                {
                    // Invoke OnActionExecuting()
                    var actionExecutingContext = new ActionExecutingContext(commandContext, commandContext);
                    var actionExecuting = InvokeActionExecuting(controller, actionExecutingContext);
                    if (actionExecuting != null)
                    {
                        return;
                    }

                    // Invoke controller action ExecutedResult()
                    var result = (ActionResult)MethodAction.Invoke(controller, new object[] { });
                    result.ExecuteResult(commandContext);

                    // Invoke OnActionExecuted()
                    var actionExecutedContext = new ActionExecutedContext(actionContext, commandContext);
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
                    var exceptionContext = new ExceptionContext(commandContext, ex);
                    var exception = InvokeException(controller, actionContext, exceptionContext);
                    if (exception != null)
                    {
                        return;
                    }

                    return;
                }
            };
        }

        private static IActionResult InvokeActionExecuting(Command command, ActionExecutingContext actionExecutingContext)
        {
            command.OnActionExecuting(actionExecutingContext);

            if (actionExecutingContext.Result == null)
            {
                return null;
            }

            actionExecutingContext.Result.ExecuteResult(actionExecutingContext);
            return actionExecutingContext.Result;
        }

        private static IActionResult InvokeActionExecuted(Command command, ActionExecutedContext actionExecutedContext)
        {
            command.OnActionExecuted(actionExecutedContext);

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

        private static IActionResult InvokeException(Command command, ActionContext actionContext, ExceptionContext exceptionContext)
        {
            command.OnException(exceptionContext);

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