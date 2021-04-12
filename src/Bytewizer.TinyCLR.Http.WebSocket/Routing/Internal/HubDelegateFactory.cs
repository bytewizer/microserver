using System;
using System.Reflection;

namespace Bytewizer.TinyCLR.Http.WebSockets.Middleware
{
    internal class HubDelegateFactory
    {
        public RequestDelegate CreateRequestDelegate(Type hubType)
        {
            return (context) =>
            {
                var hub = (Hub)Activator.CreateInstance(hubType);
                if (hub == null)
                {
                    throw new InvalidOperationException(nameof(hub));
                }

                //hub.HubCallerContext = new HubCallerContext(context);


                hub.OnConnected();

                //hub.OnMessage();
                //hub.OnDisconnected(new Exception());


                //try
                //{
                    //var websocket = context.GetWebSocket();
                    //if (websocket != null)
                    //{
                    //    //throw new NullReferenceException(nameof(websocket));
                    //}

                    //hub.OnConnected();

                    //var active = true;
                    //var inputStream = context.Channel.InputStream;
                    //while (active)
                    //{
                    //    var frame = WebSocketFrame.ReadFrame(inputStream, true);
                    //    if (frame.IsClose)
                    //    {
                    //        throw new Exception("Client disconnected");
                    //    }

                    //    if (inputStream != null & inputStream.Length > 0)
                    //    {
                    //        hub.OnMessage();
                    //    }
                    //}
                //}
                //catch (Exception ex)
                //{
                //    hub.OnDisconnected(ex);
                //}






                //try
                //{
                //    // Invoke OnActionExecuting()
                //    var actionExecutingContext = new ActionExecutingContext(controllerContext, controllerContext);
                //    var actionExecuting = InvokeActionExecuting(controller, actionExecutingContext);
                //    if (actionExecuting != null)
                //    {
                //        return; 
                //    }

                //    // Invoke controller action ExecutedResult()
                //    var args = _modelMapper.Bind(controllerContext);
                //    var result = (ActionResult)MethodAction.Invoke(controller, args);
                //    result.ExecuteResult(controllerContext);

                //    // Invoke OnActionExecuted()
                //    var actionExecutedContext = new ActionExecutedContext(actionContext, controllerContext);
                //    var actionExecuted = InvokeActionExecuted(controller, actionExecutedContext);
                //    if (actionExecuting != null)
                //    {
                //        return; 
                //    }

                //    return; 
                //}
                //catch (Exception ex)
                //{
                //    // Invoke OnExecption()
                //    var exceptionContext = new ExceptionContext(controllerContext, ex);
                //    var exception = InvokeException(controller, actionContext, exceptionContext);
                //    if (exception != null)
                //    {
                //        return; 
                //    }

                //    return; 
                //}
            };
        }

        //private static IActionResult InvokeActionExecuting(Controller controller, ActionExecutingContext actionExecutingContext)
        //{
        //    controller.OnActionExecuting(actionExecutingContext);

        //    if (actionExecutingContext.Result == null)
        //    {
        //        return null;
        //    }

        //    actionExecutingContext.Result.ExecuteResult(actionExecutingContext);
        //    return actionExecutingContext.Result;
        //}

        //private static IActionResult InvokeActionExecuted(Controller controller, ActionExecutedContext actionExecutedContext)
        //{
        //    controller.OnActionExecuted(actionExecutedContext);

        //    if (actionExecutedContext.Result == null)
        //    {
        //        return null;
        //    }

        //    if (actionExecutedContext.Canceled)
        //    {
        //        actionExecutedContext.Result.ExecuteResult(actionExecutedContext);
        //    }

        //    if (actionExecutedContext.ExceptionHandled)
        //    {
        //        return null;
        //    }

        //    if (actionExecutedContext.Exception != null)
        //    {
        //        throw actionExecutedContext.Exception;
        //    }

        //    return actionExecutedContext.Result;
        //}

        //private static IActionResult InvokeException(Controller controller, ActionContext actionContext, ExceptionContext exceptionContext)
        //{
        //    controller.OnException(exceptionContext);

        //    if (exceptionContext.Result == null)
        //    {
        //        return null;
        //    }

        //    if (exceptionContext.Canceled)
        //    {
        //        exceptionContext.Result.ExecuteResult(actionContext);
        //    }

        //    if (exceptionContext.ExceptionHandled)
        //    {
        //        return null;
        //    }

        //    if (exceptionContext.Exception != null)
        //    {
        //        throw exceptionContext.Exception;
        //    }

        //    return exceptionContext.Result;
        //}
    }
}