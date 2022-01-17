using System;

using Bytewizer.TinyCLR.Pipeline;

namespace Bytewizer.TinyCLR.Terminal
{
    internal class CommandDelegateFactory
    {
        public CommandDelegateFactory() { }

        public RequestDelegate CreateRequestDelegate(ServerCommand serverCommand, object[] actionParameters, CommandActionDescriptor actionDescriptor)
        {
            return (context) =>
            {
                var ctx = context as TerminalContext;
                
                if (serverCommand == null)
                {
                    throw new InvalidOperationException(nameof(serverCommand));
                }

                var actionContext = new ActionContext(ctx, actionDescriptor);
                var commandContext = new CommandContext(actionContext);
                serverCommand.CommandContext = commandContext;

                try
                {
                    // Invoke OnActionExecuting()
                    var actionExecutingContext = new ActionExecutingContext(commandContext, commandContext);
                    var actionExecuting = InvokeActionExecuting(serverCommand, actionExecutingContext);
                    if (actionExecuting != null)
                    {
                        return;
                    }

                    var parameters = ModelMapper.Bind(actionParameters, ctx.Request.Command.Parameters);
                    var result = (ActionResult)actionDescriptor.MethodInfo.Invoke(serverCommand, parameters);
                    result.ExecuteResult(commandContext);

                    // Invoke OnActionExecuted()
                    var actionExecutedContext = new ActionExecutedContext(actionContext, commandContext);
                    var actionExecuted = InvokeActionExecuted(serverCommand, actionExecutedContext);
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
                    var exception = InvokeException(serverCommand, actionContext, exceptionContext);
                    if (exception != null)
                    {
                        return;
                    }

                    return;
                }
            };
        }

        private static IActionResult InvokeActionExecuting(ServerCommand command, ActionExecutingContext actionExecutingContext)
        {
            command.OnActionExecuting(actionExecutingContext);

            if (actionExecutingContext.Result == null)
            {
                return null;
            }

            actionExecutingContext.Result.ExecuteResult(actionExecutingContext);
            return actionExecutingContext.Result;
        }

        private static IActionResult InvokeActionExecuted(ServerCommand command, ActionExecutedContext actionExecutedContext)
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

        private static IActionResult InvokeException(ServerCommand command, ActionContext actionContext, ExceptionContext exceptionContext)
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