using System;
using System.Reflection;
using System.Collections;

using Bytewizer.TinyCLR.Http.Mvc.Filters;
using Bytewizer.TinyCLR.Http.Mvc.Resolver;
using Bytewizer.TinyCLR.Http.Mvc.ModelBinding;

namespace Bytewizer.TinyCLR.Http.Mvc.Middleware
{
    class ControllerFactory
    {
        private readonly Hashtable _controllers = new Hashtable();
        private readonly ModelMapper _modelMapper = new ModelMapper();

        public IEnumerable Controllers
        {
            get { return _controllers; }
        }

        public virtual void Register(string uri, ControllerMapping controller)
        {
            if (controller == null) throw new ArgumentNullException("controller");

            _controllers.Add(controller.Uri.ToLower(), controller);
        }

        public virtual void Load()
        {
            ControllerIndexer indexer = new ControllerIndexer();
            indexer.Find();

            foreach (ControllerMapping controller in indexer.Controllers)
            {
                foreach (DictionaryEntry mapping in controller.Mappings)
                {
                    _controllers.Add(controller.Uri.ToLower() + "/" + ((MethodInfo)mapping.Value).Name.ToString().ToLower(), controller);
                }
            }
        }

        public virtual bool TryMapping(string uri, out ControllerMapping mapping)
        {
            mapping = null;
            if (_controllers.Contains(uri))
            {
                mapping = (ControllerMapping)_controllers[uri];
                return true;
            }

            return false;
        }

        public virtual object Invoke(Type controllerType, MethodInfo action, HttpContext context)
        {
            var controller = (Controller)ServiceResolver.Current.Resolve(controllerType);
            if (controller == null)
            {
                throw new InvalidOperationException(nameof(controller));
            }

            var descriptor = new ControllerActionDescriptor()
            {
                ActionName = action.Name,
                ControllerName = controllerType.Name,
                MethodInfo = action
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
                    return actionExecuting;
                }
                
                // Invoke controller action ExecutedResult()
                var args = _modelMapper.Bind(controllerContext);
                var result = (ActionResult)action.Invoke(controller, args);
                result.ExecuteResult(controllerContext);

                // Invoke OnActionExecuted()
                var actionExecutedContext = new ActionExecutedContext(actionContext, controllerContext);
                var actionExecuted = InvokeActionExecuted(controller, actionExecutedContext);
                if (actionExecuting != null)
                {
                    return actionExecuting;
                }

                return result;
            }
            catch (Exception ex)
            {
                // Invoke OnExecption()
                var exceptionContext = new ExceptionContext(controllerContext, ex);
                var exception = InvokeException(controller, actionContext, exceptionContext);
                if (exception != null)
                {
                    return exception;
                }

                return exception;
            }
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