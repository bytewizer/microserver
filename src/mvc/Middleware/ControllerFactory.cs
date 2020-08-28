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
            var descriptor = new ControllerActionDescriptor()
            {
                ActionName = action.Name,
                ControllerName = controllerType.Name,
                MethodInfo = action
            };

            var actionContext = new ActionContext(context, descriptor);
            var controllerContext = new ControllerContext(actionContext);

            var controller = (Controller)ServiceResolver.Current.Resolve(controllerType);
            var newController = controller as IController;

            try
            {

           
                controller.ControllerContext = controllerContext;

                if (newController != null)
                {

                    var executingContext = new ActionExecutingContext(actionContext, new ArrayList(), new Hashtable(), controllerContext);
                    newController.OnActionExecuting(executingContext);
                    if (executingContext.Result != null)
                        return executingContext.Result;
                }

                object[] args = null;
                //object[] args = new object[] { 10 };

                // TODO: ModelBinding
                var parameters = action.GetParameters();
                if (parameters.Length > 0)
                {
                    var httpRequest = controllerContext.HttpContext.Request;
                    args = _modelMapper.Bind(httpRequest, parameters);
                }
                ActionResult result = (ActionResult)action.Invoke(controller, args);
                result.ExecuteResult(controllerContext);

                if (newController != null)
                {
                    var executedContext = new ActionExecutedContext(actionContext, new ArrayList(), controllerContext);
                    newController.OnActionExecuted(executedContext);
                    if (executedContext.Result != null)
                        return executedContext.Result;
                }

                return result;
            }
            catch (Exception ex)
            {
                if (newController != null)
                {
                    var exceptionContext = new ExceptionContext(controllerContext, ex);
                    newController.OnException(exceptionContext);
                    if (exceptionContext.Result != null)
                        return exceptionContext.Result;
                }

                ActionResult result = controller.TriggerOnException(ex);

                return result;
            }
        }
    }
}
